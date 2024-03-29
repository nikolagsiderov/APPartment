﻿using Microsoft.AspNetCore.Mvc;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using System.Linq;
using APPartment.Infrastructure.Services.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using APPartment.Infrastructure.UI.Web.Html;

namespace APPartment.Infrastructure.Controllers.Api
{
    [Route("api/[controller]")]
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
        public virtual async Task<ActionResult> GetEntity(long modelID)
        {
            try
            {
                var model = BaseCRUDService.GetEntity<U>(modelID);
                NormalizePostModel(model);
                await SetObjectActions(model);

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
        public virtual async Task<ActionResult> GetCollection()
        {
            try
            {
                var models = BaseCRUDService.GetCollection(FilterExpression);

                foreach (var model in models)
                {
                    NormalizeDisplayModel(model);
                    await SetGridItemActions(model);
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

        #region Web UI settings and actions (HTML)
        public virtual async Task SetGridItemActions(T model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildDetailsAction(CurrentArea, model.ID));

                model.ActionsHtml.Add(GridItemActionBuilder.BuildEditAction(CurrentArea, model.ID));
                model.ActionsHtml.Add(GridItemActionBuilder.BuildDeleteAction(CurrentArea, model.ID));
        }

        public virtual async Task SetObjectActions(U model)
        {
            model.ActionsHtml.Add(ObjectActionBuilder.BuildDetailsAction(CurrentArea, model.ID));

                model.ActionsHtml.Add(ObjectActionBuilder.BuildEditAction(CurrentArea, model.ID));
                model.ActionsHtml.Add(ObjectActionBuilder.BuildDeleteAction(CurrentArea, model.ID));
        }
        #endregion
    }
}
