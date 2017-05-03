using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySitter.DataModel
{
    public class Task
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public int id_child { get; set; }
        public String name { get; set; }
        public DateTime dateTime { get; set; }
        public String other { get; set; }
    }
}
