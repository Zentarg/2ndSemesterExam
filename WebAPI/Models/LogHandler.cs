using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class LogHandler
    {
        public enum RequestTypes {[Description("Update")]PUT = 1, [Description("Create")]POST = 2, [Description("Delete")]DELETE = 3}

        public static void CreateLogEntry(ParknGardenData db, int userId, string logEntry, int requestType)
        {
            Log newLog = new Log(){DateAndTime = DateTime.Now, LogEntry = logEntry, RequestType = requestType, UserID = userId};
            newLog.LogEntry += " at ";
            db.Logs.Add(newLog);
            db.SaveChanges();
        }
    }
}