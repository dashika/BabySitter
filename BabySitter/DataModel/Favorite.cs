using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySitter.DataModel
{
    public class Favorite
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public int id_child { get; set; }
        public String name { get; set; }
        public DateTime dateTime { get; set; }
    }
}
