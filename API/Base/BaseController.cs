using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

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
            if (result.Count() != 0)
            {
                return Ok(result);

            }
            return NotFound();
        }


        [HttpGet("{Id}")]
        public ActionResult Get(Key Key)
        {
            var result = repository.Get(Key);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpDelete("{Id}")]
        public ActionResult Delete(Key key)
        {
            var result = repository.Delete(key);
            try
            {
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPut("{Id}")]
        public ActionResult Update(Entity entity, Key key)
        {
            var result = repository.Update(entity, key);
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /*[HttpPost]
        public ActionResult Post(Entity entity)
        {
            try
            {
                var result = repository.Insert(entity);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }*/

    }
}
