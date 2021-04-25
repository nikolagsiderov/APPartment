using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.API.Controllers
{
    [ApiController]
    [Route("api/home/{homeID}/[controller]")]
    public class SurveysController : ControllerBase
    {
        public SurveysController()
        {
        }

        // api/home/2/surveys/89
        [HttpGet("{surveyID:long}")]
        public ActionResult<SurveyPostViewModel> Get(long surveyID)
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

                var result = new BaseCRUDService(currentUserID).GetEntity<SurveyPostViewModel>(surveyID);
                var surveyParticipants = new BaseCRUDService(currentUserID).GetCollection<SurveyParticipantPostViewModel>(x => x.SurveyID == result.ID);

                foreach (var surveyParticipant in surveyParticipants)
                {
                    result.SurveyParticipantsIDs.Add(surveyParticipant.UserID);
                }

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

        // api/home/2/surveys/types/89
        [HttpGet("{surveyTypeID:long}")]
        [Route("types")]
        public ActionResult<SurveyTypePostViewModel> GetType(long surveyTypeID)
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

                var result = new BaseCRUDService(currentUserID).GetEntity<SurveyTypePostViewModel>(surveyTypeID);

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

        // api/home/2/surveys/surveys
        [HttpGet]
        [Route("surveys")]
        public ActionResult<List<SurveyDisplayViewModel>> GetAll(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<SurveyDisplayViewModel>(x => x.HomeID == homeID);

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

        // api/home/2/surveys/completed
        [HttpGet]
        [Route("completed")]
        public ActionResult<List<SurveyDisplayViewModel>> GetCompleted(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<SurveyDisplayViewModel>(x => x.HomeID == homeID && x.IsCompleted == true);

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

        // api/home/2/surveys/pending
        [HttpGet]
        [Route("pending")]
        public ActionResult<List<SurveyDisplayViewModel>> GetPending(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<SurveyDisplayViewModel>(x => x.HomeID == homeID && x.IsCompleted == false);

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

        // api/home/2/surveys/surveytypes
        [HttpGet]
        [Route("surveytypes")]
        public ActionResult<List<SurveyDisplayViewModel>> GetTypes(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<SurveyTypeDisplayViewModel>(x => x.HomeID == homeID);

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

        // api/home/2/surveys/createdit
        [HttpPost]
        [Route("createedit")]
        public ActionResult CreateEdit([FromBody] SurveyPostViewModel model)
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
                var surveyParticipantsIDs = model.SurveyParticipantsIDs;
                var result = new BaseCRUDService(currentUserID).Save(model);

                foreach (var surveyParticipantID in surveyParticipantsIDs)
                {
                    if (!new BaseCRUDService(currentUserID).Any<SurveyParticipantPostViewModel>(x => x.SurveyID == result.ID && x.UserID == surveyParticipantID))
                    {
                        var surveyParticipant = new SurveyParticipantPostViewModel() { SurveyID = result.ID, UserID = surveyParticipantID };
                        new BaseCRUDService(currentUserID).Save(surveyParticipant);
                    }
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/2/surveys/delete/2
        [HttpGet("delete/{surveyID:long}")]
        public ActionResult Delete(long surveyID)
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

                var model = new BaseCRUDService(currentUserID).GetEntity<SurveyPostViewModel>(surveyID);
                new BaseCRUDService(currentUserID).Delete(model);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/2/surveys/createdittypes
        [HttpPost]
        [Route("createdittypes")]
        public ActionResult CreateEditTypes([FromBody] SurveyTypePostViewModel model)
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

        // api/home/2/surveys/deletetypes/2
        [HttpGet("deletetypes/{surveyTypeID:long}")]
        public ActionResult DeleteTypes(long surveyTypeID)
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

                var model = new BaseCRUDService(currentUserID).GetEntity<SurveyTypePostViewModel>(surveyTypeID);
                new BaseCRUDService(currentUserID).Delete(model);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/2/surveys/surveys/count
        [HttpGet]
        [Route("surveys/count")]
        public ActionResult<List<SurveyDisplayViewModel>> GetAllCount(long homeID)
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

                var result = new BaseCRUDService(currentUserID).Count<SurveyDisplayViewModel>(x => x.HomeID == homeID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/2/surveys/completed/count
        [HttpGet]
        [Route("completed/count")]
        public ActionResult<List<SurveyDisplayViewModel>> GetCompletedCount(long homeID)
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

                var result = new BaseCRUDService(currentUserID).Count<SurveyDisplayViewModel>(x => x.HomeID == homeID && x.IsCompleted == true);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/2/surveys/pending/count
        [HttpGet]
        [Route("pending/count")]
        public ActionResult<List<SurveyDisplayViewModel>> GetPendingCount(long homeID)
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

                var result = new BaseCRUDService(currentUserID).Count<SurveyDisplayViewModel>(x => x.HomeID == homeID && x.IsCompleted == false);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
