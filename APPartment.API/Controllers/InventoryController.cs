using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.API.Controllers
{
    [ApiController]
    [Route("api/home/{homeID}/[controller]")]
    public class InventoryController : ControllerBase
    {
        public InventoryController()
        {
        }

        // api/home/3/inventory/67
        [HttpGet("{inventoryID:long}")]
        public ActionResult Get(long inventoryID)
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

                var result = new BaseCRUDService(currentUserID).GetEntity<InventoryPostViewModel>(inventoryID);

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

        // api/home/3/inventory/inventory
        [HttpGet]
        [Route("inventory")]
        public ActionResult<List<InventoryDisplayViewModel>> GetAll(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<InventoryDisplayViewModel>(x => x.HomeID == homeID);

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

        // api/home/3/inventory/supplied
        [HttpGet]
        [Route("supplied")]
        public ActionResult<List<InventoryDisplayViewModel>> GetSupplied(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<InventoryDisplayViewModel>(x => x.HomeID == homeID && x.IsSupplied == true);

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

        // api/home/3/inventory/unsupplied
        [HttpGet]
        [Route("unsupplied")]
        public ActionResult<List<InventoryDisplayViewModel>> GetUnsupplied(long homeID)
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

                var result = new BaseCRUDService(currentUserID).GetCollection<InventoryDisplayViewModel>(x => x.HomeID == homeID && x.IsSupplied == false);

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

        // api/home/3/inventory/createedit
        [HttpPost]
        [Route("createedit")]
        public ActionResult CreateEdit([FromBody] InventoryPostViewModel model)
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

        // api/home/3/inventory/delete/33
        [HttpGet("delete/{inventoryID:long}")]
        public ActionResult Delete(long inventoryID)
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

                var model = new BaseCRUDService(currentUserID).GetEntity<InventoryPostViewModel>(inventoryID);
                new BaseCRUDService(currentUserID).Delete(model);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/3/inventory/inventory/count
        [HttpGet]
        [Route("inventory/count")]
        public ActionResult<List<InventoryDisplayViewModel>> GetAllCount(long homeID)
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

                var result = new BaseCRUDService(currentUserID).Count<InventoryDisplayViewModel>(x => x.HomeID == homeID);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/3/inventory/supplied/count
        [HttpGet]
        [Route("supplied/count")]
        public ActionResult<List<InventoryDisplayViewModel>> GetSuppliedCount(long homeID)
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

                var result = new BaseCRUDService(currentUserID).Count<InventoryDisplayViewModel>(x => x.HomeID == homeID && x.IsSupplied == true);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        // api/home/3/inventory/unsupplied/count
        [HttpGet]
        [Route("unsupplied/count")]
        public ActionResult<List<InventoryDisplayViewModel>> GetUnsuppliedCount(long homeID)
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

                var result = new BaseCRUDService(currentUserID).Count<InventoryDisplayViewModel>(x => x.HomeID == homeID && x.IsSupplied == false);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
    }
}
