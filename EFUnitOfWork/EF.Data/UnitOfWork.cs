using EF.Core.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EFDbContext context;
        private bool disposed;
        private ConcurrentDictionary<string, object> repositories;

        public UnitOfWork(EFDbContext context)
        {
            this.context = context;
        }

        public UnitOfWork()
        {
            context = new EFDbContext();
        }

        public void Commit()
        {
            context.SaveChanges();
        }


        public void RollBack()
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public Repository<T> Repository<T>() where T:BaseEntity
        {
            if(repositories==null)
            {
                repositories = new ConcurrentDictionary<string, object>();
            }
            var type = typeof(T).Name;

            if(!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                //用反射创建类型实例，第二个参数是传入构造函数的参数
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                //把反射得到的实体类型和实体放入并发字典
                repositories.TryAdd(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }
    }
}

