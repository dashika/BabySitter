using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySitter.DataModel
{
   public class EveryDay
    {
       [PrimaryKey, AutoIncrement, Unique]
       public int id { get; set; }

       public int id_child { get; set; }
       public Double weight { get; set; }
       public Double heiht { get; set; }
       public Double temperature { get; set; }
       /// <summary>
       /// вздутие животика
       /// </summary>
       public Int16 abdominalDistention { get; set; }   
       /// <summary>
       /// высыпания
       /// </summary>
       public Int16 rashes { get; set; }
       public Int16 crying { get; set; }
       public Int16 playing { get; set; }
       public Int16 locking { get; set; }
       public Int16 gas { get; set; }
       public String description { get; set; }
       public Int16 diareya { get; set; }
       public DateTime date { get; set; }
    }
}
