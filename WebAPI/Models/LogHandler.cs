﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class LogHandler
    {
        public enum RequestTypes {PUT = 1, POST = 2, DELETE = 3}

        public static void CreateLogEntry(ParknGardenData db, int userId, string logEntry, int requestType)
        {
            Log newLog = new Log(){DateAndTime = DateTime.Now, LogEntry = logEntry, RequestType = requestType, UserID = userId};
            newLog.LogEntry += " at " + newLog.DateAndTime;
            db.Logs.Add(newLog);
            db.SaveChanges();
        }
    }
}