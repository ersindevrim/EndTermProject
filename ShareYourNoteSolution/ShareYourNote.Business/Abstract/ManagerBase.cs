using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShareYourNote.Core.DataAccess;
using ShareYourNote.DataAccess.EntityFramework;
using ShareYourNote.Entities;

namespace ShareYourNote.Business.Abstract
{
    public abstract class ManagerBase<T> : IRepo<T> where T : class
    {
        private Repository<T> _repository = new Repository<T>();

        public virtual int Delete(T obj)
        {
            return _repository.Delete(obj);
        }

        public virtual T Find(Expression<Func<T, bool>> where)
        {
            return _repository.Find(where);
        }

        public virtual int Insert(T obj)
        {
            return _repository.Insert(obj);
        }

        public virtual List<T> List()
        {
            return _repository.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            return _repository.List(where);
        }

        public virtual IQueryable<T> ListQueryable()
        {
            return _repository.ListQueryable();
        }

        public virtual int Save()
        {
            return _repository.Save();
        }

        public virtual int Update(T obj)
        {
            return _repository.Update(obj);
        }
    }
}
