using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Session
    {
        public Session(string sessionKey, int userId)
        {
            SessionKey = sessionKey;
            UserID = userId;
        }

        public string SessionKey { get; set; }
        public int UserID { get; set; }

    }
}
