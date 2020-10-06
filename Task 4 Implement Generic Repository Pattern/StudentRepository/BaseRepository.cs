using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public StudentContext _context;

        //public DbSet<T> _dbset;

        public BaseRepository(StudentContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't retrieve entities");
            }
        }

        public void Post(T tObject)
        {
            try
            {
                _context.Set<T>().Add(tObject);
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
            catch (Exception)
            {
                throw new Exception("Couldn't update entities");
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
    }
}
