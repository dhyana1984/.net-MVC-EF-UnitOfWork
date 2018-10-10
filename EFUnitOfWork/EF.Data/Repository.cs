using EF.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data
{
   public class Repository<T> where T:BaseEntity
    {

        private readonly EFDbContext context;
        private IDbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(EFDbContext context)
        {
            this.context = context;
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                if(entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Add(entity);
            }
            catch(DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += $"Property:{validationError.PropertyName} Error:{validationError.ErrorMessage}" + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }
        
        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
               
            }
            catch(DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + $"Property:{validationError.PropertyName} Error:{validationError.ErrorMessage}";
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + $"Property:{validationError.PropertyName} Error:{validationError.ErrorMessage}";
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        private IDbSet<T> Entities
        {
            get
            {
                if(entities ==null)
                {
                    entities = context.Set<T>();
                }
                return entities;
            }
        }




    }
}
