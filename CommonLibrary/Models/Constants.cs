using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommonLibrary
{
    public static class Constants
    {
        public enum RequestTypes { [Description("Update")]PUT = 1, [Description("Create")] POST = 2, [Description("Delete")] DELETE = 3 }
    }
}
