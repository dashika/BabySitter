using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySitter.DataModel
{
   public class Contact
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public int id_child { get; set; }
        public String name { get; set; }
        public String phone { get; set; }
        public String address { get; set; }
        /// <summary>
        /// кем является (учитель, родитель ...)
        /// </summary>
        public String office { get; set; }
    }
}
