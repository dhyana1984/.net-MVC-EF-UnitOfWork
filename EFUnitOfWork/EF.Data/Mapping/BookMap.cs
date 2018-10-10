using EF.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Mapping
{
  public  class BookMap:EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            ToTable("Books");
            HasKey(t => t.ID);

            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Author).IsRequired();
            Property(t => t.ISBN).IsRequired();
            Property(t => t.Published).IsRequired();
            Property(t => t.Url).IsRequired();
            

        }
    }
}
