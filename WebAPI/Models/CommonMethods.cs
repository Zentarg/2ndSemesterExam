using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;

namespace WebAPI.Models
{
    public static class CommonMethods
    {
        public static HttpStatusCode StatusCodeReturn(Constants.VerifyUserErrors error)
        {
            if (error == Constants.VerifyUserErrors.INCORRECT_SESSION_KEY)
                return HttpStatusCode.Unauthorized;
            if (error == Constants.VerifyUserErrors.SESSION_NOT_FOUND)
                return HttpStatusCode.NotFound;
            return HttpStatusCode.InternalServerError;
        }
    }
}