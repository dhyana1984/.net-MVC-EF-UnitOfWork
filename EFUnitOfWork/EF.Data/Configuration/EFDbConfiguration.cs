using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Configuration
{
   public class EFDbConfiguration: DbConfiguration
    {
        public EFDbConfiguration()
        {
            //数据库初始化
            SetDatabaseInitializer(new NullDatabaseInitializer<EFDbContext>());

            //指定数据库版本
            SetManifestTokenResolver(new ManifestTokenResolver());

            //设置模型缓存
            SetModelStore(new DefaultDbModelStore(Directory.GetCurrentDirectory()));
        }


    }
}
