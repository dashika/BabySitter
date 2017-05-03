using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabySitter.DataModel
{
    public class Children
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public String name { get; set; }
        public DateTime birthday { get; set; }

    }
}
