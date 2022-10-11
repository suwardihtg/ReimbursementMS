using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttachmentsController : BaseController<EmployeeAttachment, EmployeeAttachmentRepository, string>
    {
        private readonly EmployeeAttachmentRepository employeeAttachmentRepository;
        public EmployeeAttachmentsController(EmployeeAttachmentRepository repository) : base(repository)
        {
            this.employeeAttachmentRepository = repository;
        }

        [HttpPost("singleupload")]
        public ActionResult SingleUpload(IFormFile file)
        {
            try
            {
                string newString = "";
                newString = Regex.Replace(file.FileName, @"\s+", "_");
                var filePath = Path.Combine("D:/Project/ProjectRM/Reimburse Management System/API/File/Images/", newString);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        
        }
    }
}
