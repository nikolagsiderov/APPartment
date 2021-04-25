using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Issue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.API.Controllers
{
    [ApiController]
    [Route("api/home/{homeID}/[controller]")]
    public class IssuesController : ControllerBase
    {
        public IssuesController()
        {
        }

        // api/home/7/issues/42
        [HttpGet("{issueID:long}")]
        public ActionResult<IssuePostViewModel> Get(long issueID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).GetEntity<IssuePostViewModel>(issueID);

                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
            
        }

        // api/home/7/issues/issues
        [HttpGet]
        [Route("issues")]
        public ActionResult<List<IssueDisplayViewModel>> GetAll(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).GetCollection<IssueDisplayViewModel>(x => x.HomeID == homeID);

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/opem
        [HttpGet]
        [Route("open")]
        public ActionResult<List<IssueDisplayViewModel>> GetOpen(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).GetCollection<IssueDisplayViewModel>(x => x.HomeID == homeID && x.IsClosed == false);

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/closed
        [HttpGet]
        [Route("closed")]
        public ActionResult<List<IssueDisplayViewModel>> GetClosed(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).GetCollection<IssueDisplayViewModel>(x => x.HomeID == homeID && x.IsClosed == true);

                if (result.Any())
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/createedit
        [HttpPost]
        [Route("createedit")]
        public ActionResult CreateEdit([FromBody] IssuePostViewModel model)
        {
            try
            {
                var currentUserID = 0L;
                var currentHomeID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());

                if (headers.ContainsKey("CurrentHomeID"))
                    currentHomeID = long.Parse(headers.GetCommaSeparatedValues("CurrentHomeID").FirstOrDefault());

                model.HomeID = currentHomeID;
                var result = new BaseCRUDService(currentUserID).Save(model);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/delete/56
        [HttpGet("delete/{issueID:long}")]
        public ActionResult Delete(long issueID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var model = new BaseCRUDService(currentUserID).GetEntity<IssuePostViewModel>(issueID);
                new BaseCRUDService(currentUserID).Delete(model);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/issues/count
        [HttpGet]
        [Route("issues/count")]
        public ActionResult<List<IssueDisplayViewModel>> GetAllCount(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).Count<IssueDisplayViewModel>(x => x.HomeID == homeID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/open/count
        [HttpGet]
        [Route("open/count")]
        public ActionResult<List<IssueDisplayViewModel>> GetOpenCount(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).Count<IssueDisplayViewModel>(x => x.HomeID == homeID && x.IsClosed == false);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/7/issues/closed/count
        [HttpGet]
        [Route("closed/count")]
        public ActionResult<List<IssueDisplayViewModel>> GetClosedCount(long homeID)
        {
            try
            {
                var currentUserID = 0L;
                var re = Request;
                var headers = re.Headers;

                if (headers.ContainsKey("CurrentUserID"))
                {
                    currentUserID = long.Parse(headers.GetCommaSeparatedValues("CurrentUserID").FirstOrDefault());
                }

                var result = new BaseCRUDService(currentUserID).Count<IssueDisplayViewModel>(x => x.HomeID == homeID && x.IsClosed == true);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
