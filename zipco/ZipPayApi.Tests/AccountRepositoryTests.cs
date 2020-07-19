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
    public class AccountRepositoryTests
    {
        [Fact]
        public async Task GetAllAccountsAsyncReturnsAccountResponseListAsync()
        {
            var mockLogger = new Mock<ILogger<AccountController>>();
            var accountResponseList = GetAllMockAccounts();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(u => u.GetAllAccountsAsync()).Returns(Task.FromResult(accountResponseList));
            var accountController = new AccountController(mockLogger.Object, accountRepository.Object);
            var controller = await accountController.GetAccountsAsync();
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as List<AccountResponse>;
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetAccountAsyncReturnsAccountResponse()
        {
            var mockLogger = new Mock<ILogger<AccountController>>();
            var accountResponse = GetMockAccount();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(u => u.GetAccountAsync(It.IsAny<int>())).Returns(Task.FromResult(accountResponse));
            var accountController = new AccountController(mockLogger.Object, accountRepository.Object);
            var controller = await accountController.GetAccountAsync(1);
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as AccountResponse;
            Assert.NotNull(result);
            Assert.Equal("Krishna Credit", result.Name);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetAccountAsyncThrowsNotFoundException()
        {
            var mockLogger = new Mock<ILogger<AccountController>>();
            AccountResponse accountResponse = null;
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(u => u.GetAccountAsync(It.IsAny<int>())).Returns(Task.FromResult(accountResponse));
            var accountController = new AccountController(mockLogger.Object, accountRepository.Object);
            var controller = await accountController.GetAccountAsync(2);
            var result = Assert.IsType<NotFoundResult>(controller.Result);
            Assert.Equal(404, result.StatusCode);
        }        

        [Fact]
        public async Task PostAccountAsyncReturnsAccountResponse()
        {
            var mockLogger = new Mock<ILogger<AccountController>>();
            AccountRequest accountRequest = GetNewMockAccount();
            AccountResponse accountResponse = GetNewAccountResponse();
            var userFinanceStatus = new UserFinanceStatus();
            userFinanceStatus.CanCreateAccount = true;
            userFinanceStatus.ErrorMessage = string.Empty;
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(u => u.GetUserFinanceStatusAsync(It.IsAny<int>())).Returns(Task.FromResult(userFinanceStatus));        
            
            var accountController = new AccountController(mockLogger.Object, accountRepository.Object);
            var controller = await accountController.PostAccountAsync(accountRequest);            
            var result = Assert.IsType<CreatedAtActionResult>(controller.Result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task PostAccountAsyncThrowsBadRequestException()
        {
            var mockLogger = new Mock<ILogger<AccountController>>();
            AccountRequest accountRequest = GetNewMockAccountBadRequest();
            AccountResponse accountResponse = GetNewAccountResponse();
            var accountRepository = new Mock<IAccountRepository>();
            var userFinanceStatus = new UserFinanceStatus();
            userFinanceStatus.CanCreateAccount = false;
            userFinanceStatus.ErrorMessage = "test";
            accountRepository.Setup(u => u.GetUserFinanceStatusAsync(It.IsAny<int>())).Returns(Task.FromResult(userFinanceStatus));       
            
            var accountController = new AccountController(mockLogger.Object, accountRepository.Object);
            var controller = await accountController.PostAccountAsync(accountRequest);
            
            var result = Assert.IsType<BadRequestObjectResult>(controller.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task PostAccountAsyncThrowsBadRequestExceptionForRequiredField()
        {
            var mockLogger = new Mock<ILogger<AccountController>>();
            AccountRequest accountRequest = GetNewMockAccountBadRequest();
            AccountResponse accountResponse = GetNewAccountResponse();
            var accountRepository = new Mock<IAccountRepository>();
            var userFinanceStatus = new UserFinanceStatus();
            userFinanceStatus.CanCreateAccount = true;
            userFinanceStatus.ErrorMessage = string.Empty;
            accountRepository.Setup(u => u.GetUserFinanceStatusAsync(It.IsAny<int>())).Returns(Task.FromResult(userFinanceStatus));          
            
            var accountController = new AccountController(mockLogger.Object, accountRepository.Object);
            accountController.ModelState.AddModelError("Name", "Required");
            var controller = await accountController.PostAccountAsync(accountRequest);
            
            var result = Assert.IsType<BadRequestResult>(controller.Result);
            Assert.Equal(400, result.StatusCode);
        }

        private IEnumerable<AccountResponse> GetAllMockAccounts()
        {
            var accountList = new List<AccountResponse>();

            var accountResponseItem1 = new AccountResponse();
            accountResponseItem1.Id = 1;
            accountResponseItem1.Name = "Krishna Credit";
            accountResponseItem1.UserId = 1;

            var accountResponseItem2= new AccountResponse();
            accountResponseItem2.Id = 2;
            accountResponseItem2.Name = "Luke Credit";
            accountResponseItem2.UserId = 2;

            var accountResponseItem3 = new AccountResponse();
            accountResponseItem3.Id = 3;
            accountResponseItem3.Name = "Lucas Credit";
            accountResponseItem3.UserId = 3;

            accountList.Add(accountResponseItem1);
            accountList.Add(accountResponseItem2);
            accountList.Add(accountResponseItem3);

            return accountList;
        }

        private AccountResponse GetMockAccount()
        {
            var accountResponse = new AccountResponse();
            accountResponse.Id = 1;
            accountResponse.Name = "Krishna Credit";
            accountResponse.UserId = 1;

            return accountResponse;
        }

        private AccountRequest GetNewMockAccount()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = "Troy Credit";
            accountRequest.UserId = 4;

            return accountRequest;
        }

        private AccountRequest GetNewMockAccountBadRequest()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = null;
            accountRequest.UserId = 4;

            return accountRequest;
        }

        private AccountResponse GetNewAccountResponse()
        {
            var accountResponse = new AccountResponse();
            accountResponse.Id = 4;
            accountResponse.Name = "Troy Credit";
            accountResponse.UserId = 4;

            return accountResponse;
        }
    }
}
