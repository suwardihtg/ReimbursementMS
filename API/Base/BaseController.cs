using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Base
{
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {

        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var result = repository.Get();
            if (result != null)
            {
                return Ok(new { status = 200, result = result }) ;

            }
            return NotFound();
        }


        [HttpGet]
        [Route("Id")]
        public ActionResult Get(Key Key)
        {
           
            var result = repository.Get(Key);
            if (result != null)
            {
                return Ok(new {status = 200, result = result});
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("Id")]
        public ActionResult Delete(Key key)
        {
           
            var result = repository.Delete(key);
            try
            {
                return Ok(new { status = 200, data = "Delete data Success" });
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("Id")]
        public ActionResult Update(Entity entity, Key key)
        {
            
            var result = repository.Update(entity, key);
            try
            {
                return Ok(new {status = 200, data = "Update data Success"});
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult Post(Entity entity)
        {
            try
            {
                var result = repository.Insert(entity);
                return Ok(new {status = 200, data = "Insert data Success"});
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
