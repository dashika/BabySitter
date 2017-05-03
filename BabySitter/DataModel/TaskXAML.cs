using System;
using System.Collections.Generic;
using System.Text;

namespace BabySitter.DataModel
{
   public class Childs
    {
        public int id { get; set; }
        public String name { get; set; }
        public String birthday { get; set; }
    }
    public class TaskXAML
    {
        public int id { get; set; }

        public DateTime dateTime { get; set; }
        public String other { get; set; }
        public String name { get; set; }   
    }
}
