using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZipPayApi.Model;
using ZipPayApi.Repository;

namespace ZipPayApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;
        private const string ACCOUNT_CREATION_SUCCESSFULL = "Account successfully created";

        public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AccountResponse>> GetAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountResponse>> PostAccountAsync([FromBody] AccountRequest account)
        {
            if (ModelState.IsValid)
            {
                var userfinanceStatus = await _accountRepository.GetUserFinanceStatusAsync(account.UserId);
                if (userfinanceStatus.CanCreateAccount)
                {
                    var newAccount = await _accountRepository.CreateAccountAsync(account);
                    _logger.LogInformation(ACCOUNT_CREATION_SUCCESSFULL);
                    return CreatedAtAction("GetAccount", new { id = account.Id }, newAccount);
                }
                else
                {
                    _logger.LogError(userfinanceStatus.ErrorMessage + ", {@account}", account);
                    return BadRequest(userfinanceStatus.ErrorMessage);
                }
            }

            return BadRequest();

        }
    }
}
