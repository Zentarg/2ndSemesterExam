using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class UserLevel
    {
        public UserLevel()
        {

        }

        public UserLevel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
