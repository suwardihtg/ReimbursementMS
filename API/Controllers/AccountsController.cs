using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Collections;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private AccountRepository accountRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public AccountsController(AccountRepository repository, IConfiguration configuration, MyContext context) : base(repository)
        {
            this.accountRepository = repository;
            this._configuration = configuration;
            this.context = context;
        }
        [HttpPost("Login")]
        public ActionResult Post(LoginVM loginVM)
        {
            var result = accountRepository.Login(loginVM);
            switch (result)
            {
                case 1:
                    var getUserData = (from a in context.Employees
                                       where a.Email == loginVM.Email
                                       join b in context.Accounts on a.Id equals b.Id
                                       join c in context.Jobs on a.JobId equals c.Id
                                       select new
                                       {
                                           Email = a.Email,
                                           Id = a.Id,
                                           Role = c.Name
                                       }).ToList();

                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Email, getUserData[0].Email),
                        new Claim(JwtRegisteredClaimNames.NameId, getUserData[0].Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, getUserData[0].Role),
                        new Claim(ClaimTypes.Role, getUserData[0].Role)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(120),
                        signingCredentials: signIn
                        );

                    var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                    claims.Add(new Claim("TokenSecurity", idtoken.ToString()));

                    return Ok(new {status = 200, Token = idtoken});

                case 2:
                    return BadRequest();
                case 3:
                    return BadRequest();
                default:
                    return BadRequest();
            }
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            var result = accountRepository.Register(registerVM);
            switch (result)
            {
                case 1:
                    return Ok(new { Status = HttpStatusCode.OK, Messages = "Register Sucsses" });
                default:
                    return BadRequest(new { Status = HttpStatusCode.BadRequest, Message = "Register Failed" });
            }

        }

        

    }
}
