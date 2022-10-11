using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReimburseHistoriesController : BaseController<ReimburseHistory, ReimburseHistoryRepository, string>
    {
        private ReimburseHistoryRepository reimburseHistoryRepository ;
        public IConfiguration _configuration;
        private readonly MyContext context;
        public ReimburseHistoriesController(ReimburseHistoryRepository repository, IConfiguration configuration, MyContext context) : base(repository)
        {
            this.reimburseHistoryRepository = repository;
            _configuration = configuration;
            this.context = context;
        }
        [HttpGet("History/{reimburseid}")]
        public ActionResult GetReimburseFinances(int reimburseid)
        {
            var result = reimburseHistoryRepository.GetHistory(reimburseid);

            if (result.Count() != 0)
            {
                return Ok(new {status = 200, result = result});
            }
            return NotFound(result);
        }
    }
}
