using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Entity
{
   public class Book:BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime Published { get; set; }
        public string IP { get; set; }
        public string Url { get; set; }
    }
}
