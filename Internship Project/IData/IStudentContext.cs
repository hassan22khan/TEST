using Models;
using StudentModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace IData
{
    public interface IStudentContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<StudentCourse> StudentCourses { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<User> Users { get; set; }
        decimal InsertStudent(Student student);
        void InsertStudentCourses(string courseId, decimal id);
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry Entry(object entity);
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}