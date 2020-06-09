using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Log
    {
        public Log()
        {

        }

        public Log(int id, int userId, string logEntry, DateTime dateAndTime, int requestType)
        {
            ID = id;
            UserID = userId;
            LogEntry = logEntry;
            DateAndTime = dateAndTime;
            RequestType = requestType;
        }
        public int ID { get; set; }
        public int UserID { get; set; }
        public string LogEntry { get; set; }
        public DateTime DateAndTime { get; set; }
        public int RequestType { get; set; }
    }
}
