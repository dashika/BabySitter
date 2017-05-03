using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySitter.DataModel
{
    public class Reminder
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public int id_child { get; set; }
        public int type { get; set; }
        public String stype { get; set; }
        public DateTime dateTime { get; set; }
        public String other { get; set; }
    }

    public class Vaccine
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public String vaccine { get; set; }
        public String cabinet { get; set; }
        public String fio { get; set; }
        public String phone { get; set; }
    }

    public class Doctor
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

        public String doctor { get; set; }
        public String cabinet { get; set; }
        public String fio { get; set; }
        public String phone { get; set; }
    }

    public class Action
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int id { get; set; }

      //  public String action { get; set; }
        public String place { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }

}
