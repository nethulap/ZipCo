using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPayApi.Model;

namespace ZipPayApi.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> GetUserAsync(int id);
        Task<UserResponse> CreateUserAsync(UserRequest user);
        Task<bool> UserExistsAsync(string email);
    }
}