using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ShareYourNote.Core.DataAccess;

namespace ShareYourNote.DataAccess.EntityFramework
{
    public class Repository<T> : RepoBase,IRepo<T> where T : class
    {
        private DbSet<T> _dbSet;

        public Repository()
        {   
            _dbSet = dataContext.Set<T>();
        }

        public List<T> List()
        {
            return dataContext.Set<T>().ToList();
        }

        public IEnumerable<T> Search()
        {
            return dataContext.Set<T>().AsQueryable();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _dbSet.AsQueryable<T>();
        }

        public int Save() // Neden int? Save kaç kayıt etkilendiyse onun sayısını döndürür.
        {
            return dataContext.SaveChanges();
        }

        public int Insert(T obj)
        {
            dataContext.Set<T>().Add(obj);
            return Save();
        }

        public int Update(T obj)
        {
            return Save(); // Update diye birşey yok EF'te, SaveChanges var.
        }

        public int Delete(T obj)
        {
            _dbSet.Remove(obj);
            return Save();
        }

        public T Find(Expression<Func<T,bool>> where)
        {
            return _dbSet.FirstOrDefault(where);
        }
    }
}
