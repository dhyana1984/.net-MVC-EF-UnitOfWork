using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Entity
{
    public abstract class BaseEntity
    {
        public Int64 ID { get; set; }
        public DateTime CreateTime { get;set; }
        public DateTime ModifyTime { get; set; }
    }
}
