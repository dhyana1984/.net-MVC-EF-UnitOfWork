using EF.Core.Entity;
using EF.Data.Conventions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    //[DbConfigurationType(typeof())]
   public class EFDbContext:DbContext
    {
        public EFDbContext():base("name=DbConnectionString")
        {

        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity:BaseEntity
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //添加日期约定
            modelBuilder.Conventions.Add(new DateConvention());

            //反射出所有EF配置类并注册到配置中
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(types => !string.IsNullOrEmpty(types.Namespace))
                              .Where(type => type.BaseType != null
                                     && type.BaseType.IsGenericType
                                     && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }


    }



}
