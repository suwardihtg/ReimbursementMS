using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestFormController : BaseController<RequestForm, RequestFormRepository, int>
    {
        private RequestFormRepository formRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;
        public RequestFormController(RequestFormRepository repository, IConfiguration configuration, MyContext context) : base(repository)
        {
            this.formRepository = repository;
            this._configuration = configuration;
            this.context = context;
        }

        [HttpPost("InsertForm")]
        public ActionResult NewForm(ReqFormVM fromVM)
        {
            var result = formRepository.NewForm(fromVM);
            switch (result)
            {
                case 1:
                    return Ok();
                default:
                    return BadRequest();
            }
        }

        [HttpGet("FormData/{reimburseid}")]
        public ActionResult GetForm(int reimburseid)
        {
            var result = formRepository.GetForm(reimburseid);

            if (result.Count() != 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("TotalReimburse/{reimburseid}")]
        public ActionResult TotalExpenseForm(int reimburseid)
        {
            var result = formRepository.TotalExpenseForm(reimburseid);

            if (result != null)
            {
                return Ok(result);
            }
            return Ok(result);
        }

        [HttpPut("FormUpdate")]
        public ActionResult FormUpdate(ReqFormVM fromVM)
        {
            var result = formRepository.FormUpdate(fromVM);
            switch (result)
            {
                case 1:
                    return Ok();
                default:
                    return BadRequest();
            }
        }

        [HttpGet("Getatc/{imgid}")]
        public ActionResult Getatc(int imgid)
        {
            var result = formRepository.Getatc(imgid);

            if (result != null)
            {
                return Ok(result);
            }
            return Ok(result);
        }
    }
}
