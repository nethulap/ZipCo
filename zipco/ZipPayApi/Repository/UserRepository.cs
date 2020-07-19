using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZipPayApi.Data;
using ZipPayApi.Model;

namespace ZipPayApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ZipPayContext _zipPayContext;
        private readonly IMapper _mapper;

        public UserRepository(ZipPayContext zipPayContext, IMapper mapper)
        {
            _zipPayContext = zipPayContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var userEntityList = await _zipPayContext.User.ToListAsync();
            var users = _mapper.Map<List<UserResponse>>(userEntityList);
            return users;
        }

        public async Task<UserResponse> GetUserAsync(int id)
        {
            var userEntity = await _zipPayContext.User.FirstOrDefaultAsync(u => u.Id == id);
            var user = _mapper.Map<UserResponse>(userEntity);
            return user;
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest user)
        {
            var userEntity = _mapper.Map<User>(user);
            _zipPayContext.User.Add(userEntity);
            await _zipPayContext.SaveChangesAsync();
            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var userEntity = await _zipPayContext.User.FirstOrDefaultAsync(u => u.Email == email);
            return userEntity != null;
        }
    }
}