using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class Constants
    {
        public enum VerifyUserErrors {OK = 0, SESSION_NOT_FOUND = 1, INCORRECT_SESSION_KEY = 2, }
    }
}