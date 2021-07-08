using Microsoft.AspNetCore.Mvc;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using System.Linq.Expressions;
using System;

namespace APPartment.Infrastructure.Controllers.Api
{
    [Route("api/[area]/[controller]")]
    public abstract class BaseAPILookupController<T> : BaseAPIController
        where T : LookupPostViewModel, new()
    {
        public BaseAPILookupController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        protected abstract Expression<Func<T, bool>> FilterExpression { get; }

        [HttpGet("{modelID:long}")]
        public virtual ActionResult GetEntity(long modelID)
        {
            try
            {
                var model = BaseCRUDService.GetLookupEntity<T>(modelID);
                NormalizeModel(model);

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
                var models = BaseCRUDService.GetLookupCollection(FilterExpression);

                foreach (var model in models)
                {
                    NormalizeModel(model);
                }

                if (models.Any())
                    return Ok(models);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        protected abstract void NormalizeModel(T model);
    }
}
