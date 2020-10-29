using Data.Model;
using IData;
using IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private IStudentContext _context;
        private DbSet<T> _dbSet;
        public BaseRepository(IStudentContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.ToList();
            }catch(Exception e)
            {
                throw new Exception("Unable to get entities");
            }
        }

        public T GetOne(int id)
        {
            return _dbSet.Find(id);
        }

        public void Post(T tObject)
        {
            try
            {
                _dbSet.Add(tObject);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't post entities");
            }
        }

        public void Update(T tObject)
        {
            try
            {
                _context.Entry(tObject).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var objectToDelete = _context.Set<T>().Find(id);
                _context.Set<T>().Remove(objectToDelete);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't delete entities");
            }
        }

        public void AddRange(List<T> list)
        {
            _context.Set<T>().AddRange(list);
        }

        public void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
        }

        public void DeleteRange(List<T> list)
        {
            _context.Set<T>().RemoveRange(list);
        }
    }
}
