using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.IO;

namespace EF.Data.Configuration
{
    public class EFDbConfiguration: DbConfiguration
    {
        public EFDbConfiguration()
        {
            //数据库初始化
            SetDatabaseInitializer(new NullDatabaseInitializer<EFDbContext>());

            //指定数据库版本
            // SetManifestTokenResolver(new ManifestTokenResolver());
            this.AddDependencyResolver(new SingletonDependencyResolver<IManifestTokenResolver>(new ManifestTokenResolver()));

            //设置模型缓存
            SetModelStore(new DefaultDbModelStore(Directory.GetCurrentDirectory()));
        }


    }
}
