using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ZipPayApi.Controllers;
using ZipPayApi.Data;
using ZipPayApi.Model;
using ZipPayApi.Repository;

namespace ZipPayApi.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetAllUsersAsyncReturnsUserResponseListAsync()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            var userResponseList = GetAllMockUsers();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetAllUsersAsync()).Returns(Task.FromResult(userResponseList));
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            var controller = await userController.GetUsersAsync();
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as List<UserResponse>;
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetUserAsyncReturnsUserResponse()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            var userResponse = GetMockUser();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(userResponse));
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            var controller = await userController.GetUserAsync(1);
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as UserResponse;
            Assert.NotNull(result);
            Assert.Equal("Krishna", result.Name);
            Assert.Equal("krishna@gmail.com", result.Email);
            Assert.Equal(5675, result.Salary);
            Assert.Equal(2450, result.Expenses);
        }

        [Fact]
        public async Task GetUserAsyncThrowsNotFoundException()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            UserResponse userResponse = null;
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(userResponse));
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            var controller = await userController.GetUserAsync(2);
            var result = Assert.IsType<NotFoundResult>(controller.Result);
            Assert.Equal(404, result.StatusCode);
        }        

        [Fact]
        public async Task PostUserAsyncReturnsUserResponse()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            UserRequest userRequest = GetNewMockUser();
            UserResponse userResponse = GetNewUserResponse();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.CreateUserAsync(It.IsAny<UserRequest>())).Returns(Task.FromResult(userResponse));            
            
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            var controller = await userController.PostUserAsync(userRequest);            
            var result = Assert.IsType<CreatedAtActionResult>(controller.Result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task PostUserAsyncThrowsBadRequestException()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            UserRequest userRequest = GetNewMockUserBadRequest();
            UserResponse userResponse = GetNewUserResponse();
            var userRepository = new Mock<IUserRepository>(); 
            userRepository.Setup(u => u.UserExistsAsync(It.IsAny<string>())).Returns(Task.FromResult(true));         
            
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            //userController.ModelState.AddModelError("Name", "Required");
            var controller = await userController.PostUserAsync(userRequest);
            
            var result = Assert.IsType<BadRequestObjectResult>(controller.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task PostUserAsyncThrowsBadRequestExceptionForRequiredField()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            UserRequest userRequest = GetNewMockUserBadRequest();
            UserResponse userResponse = GetNewUserResponse();
            var userRepository = new Mock<IUserRepository>(); 
            userRepository.Setup(u => u.CreateUserAsync(It.IsAny<UserRequest>())).Returns(Task.FromResult(userResponse));       
            
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            userController.ModelState.AddModelError("Name", "Required");
            var controller = await userController.PostUserAsync(userRequest);
            
            var result = Assert.IsType<BadRequestResult>(controller.Result);
            Assert.Equal(400, result.StatusCode);
        }

        private IEnumerable<UserResponse> GetAllMockUsers()
        {
            var userList = new List<UserResponse>();

            var userResponseItem1 = new UserResponse();
            userResponseItem1.Id = 1;
            userResponseItem1.Name = "Krishna";
            userResponseItem1.Email = "krishna@gmail.com";
            userResponseItem1.Salary = 5675;
            userResponseItem1.Expenses = 2450;

            var userResponseItem2 = new UserResponse();
            userResponseItem1.Id = 2;
            userResponseItem1.Name = "Luke";
            userResponseItem1.Email = "luke@gmail.com";
            userResponseItem1.Salary = 4320;
            userResponseItem1.Expenses = 7140;
            var userResponseItem3 = new UserResponse();

            userResponseItem1.Id = 3;
            userResponseItem1.Name = "Lucas";
            userResponseItem1.Email = "lucas@gmail.com";
            userResponseItem1.Salary = 3625;
            userResponseItem1.Expenses = 1520;

            userList.Add(userResponseItem1);
            userList.Add(userResponseItem2);
            userList.Add(userResponseItem3);

            return userList;
        }

        private UserResponse GetMockUser()
        {
            var userResponse = new UserResponse();
            userResponse.Id = 1;
            userResponse.Name = "Krishna";
            userResponse.Email = "krishna@gmail.com";
            userResponse.Salary = 5675;
            userResponse.Expenses = 2450;

            return userResponse;
        }

        private UserRequest GetNewMockUser()
        {
            var userRequest = new UserRequest();
            userRequest.Name = "Troy";
            userRequest.Email = "troy@gmail.com";
            userRequest.Salary = 6340;
            userRequest.Expenses = 1580;

            return userRequest;
        }

        private UserRequest GetNewMockUserBadRequest()
        {
            var userRequest = new UserRequest();
            userRequest.Name = null;
            userRequest.Email = "troy@gmail.com";
            userRequest.Salary = 6340;
            userRequest.Expenses = 1580;

            return userRequest;
        }

        private UserResponse GetNewUserResponse()
        {
            var userResponse = new UserResponse();
            userResponse.Id = 4;
            userResponse.Name = "Troy";;
            userResponse.Email = "troy@gmail.com";
            userResponse.Salary = 6340;
            userResponse.Expenses = 1580;

            return userResponse;
        }
    }
}
