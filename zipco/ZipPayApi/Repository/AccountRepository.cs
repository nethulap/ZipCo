using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZipPayApi.Data;
using ZipPayApi.Model;

namespace ZipPayApi.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ZipPayContext _zipPayContext;
        private readonly IMapper _mapper;
        private const string IN_SUFFICIENT_FUNDS = "User doesn't have sufficient funds to qualify for Zip Pay Credit";
        private const string USER_NOT_FOUND = "User doesn't exist";

        public AccountRepository(ZipPayContext zipPayContext, IMapper mapper)
        {
            _zipPayContext = zipPayContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync()
        {
            var accountEntityList = await _zipPayContext.Account.ToListAsync();
            var users = _mapper.Map<List<AccountResponse>>(accountEntityList);
            return users;
        }

        public async Task<AccountResponse> GetAccountAsync(int id)
        {
            var accountEntity = await _zipPayContext.Account.FirstOrDefaultAsync(u => u.Id == id);
            var user = _mapper.Map<AccountResponse>(accountEntity);
            return user;
        }

        public async Task<AccountResponse> CreateAccountAsync(AccountRequest account)
        {
            var accountEntity = _mapper.Map<Account>(account);
            _zipPayContext.Account.Add(accountEntity);
            await _zipPayContext.SaveChangesAsync();
            return _mapper.Map<AccountResponse>(accountEntity);
        }

        public async Task<UserFinanceStatus> GetUserFinanceStatusAsync(int userId)
        {
            var userEntity = await _zipPayContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            
            var userFinanceStatus = new UserFinanceStatus();

            if (userEntity != null)
            {
                var netSalary = userEntity.Salary - userEntity.Expenses;
                var status = netSalary >= 1000;
                userFinanceStatus.CanCreateAccount = status;
                userFinanceStatus.ErrorMessage = status ? string.Empty : IN_SUFFICIENT_FUNDS;
            }
            else
            {
                userFinanceStatus.CanCreateAccount = false;
                userFinanceStatus.ErrorMessage = USER_NOT_FOUND;
            }

            return userFinanceStatus;
        }
    }
}