using Microsoft.AspNetCore.Mvc;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace APPartment.Infrastructure.Controllers.Api
{
    [Route("api/[area]/[controller]")]
    public abstract class BaseAPICRUDController<T, U> : BaseAPIController
        where T : GridItemViewModel, new()
        where U : PostViewModel, new()
    {
        public BaseAPICRUDController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected abstract Expression<Func<T, bool>> FilterExpression { get; }

        [HttpGet("{modelID:long}")]
        public virtual ActionResult GetEntity(long modelID)
        {
            try
            {
                var model = BaseCRUDService.GetEntity<U>(modelID);
                NormalizePostModel(model);

                if (model != null)
                    return Ok(model);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpGet]
        public virtual ActionResult GetCollection()
        {
            try
            {
                var models = BaseCRUDService.GetCollection(FilterExpression);

                foreach (var model in models)
                {
                    NormalizeDisplayModel(model);
                }

                if (models.Where(x => x.HideItem == false).Any())
                    return Ok(models.Where(x => x.HideItem == false));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpPost]
        public virtual ActionResult CreateOrEdit([FromBody] U model)
        {
            try
            {
                model = BaseCRUDService.Save(model);
                NormalizePostModel(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpDelete("{modelID:long}")]
        public virtual ActionResult Delete(long modelID)
        {
            try
            {
                var model = BaseCRUDService.GetEntity<U>(modelID);
                BaseCRUDService.Delete(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [HttpGet]
        [Route("count")]
        public virtual ActionResult<List<T>> GetCount()
        {
            try
            {
                var models = BaseCRUDService.GetCollection(FilterExpression);

                foreach (var model in models)
                {
                    NormalizeDisplayModel(model);
                }

                if (models.Where(x => x.HideItem == false).Any())
                    return Ok(models.Where(x => x.HideItem == false).Count());
                else
                    return Ok(0);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        protected abstract void NormalizeDisplayModel(T model);

        protected abstract void NormalizePostModel(U model);
    }
}
