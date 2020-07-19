using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPayApi.Model;

namespace ZipPayApi.Repository
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();
        Task<AccountResponse> GetAccountAsync(int id);
        Task<AccountResponse> CreateAccountAsync(AccountRequest account);
        Task<UserFinanceStatus> GetUserFinanceStatusAsync(int userId);
    }
}