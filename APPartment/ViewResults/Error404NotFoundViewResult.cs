using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APPartment.ViewResults
{
    public class Error404NotFoundViewResult : ViewResult
    {
        public Error404NotFoundViewResult()
        {
            ViewName = "Error404NotFound";
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
