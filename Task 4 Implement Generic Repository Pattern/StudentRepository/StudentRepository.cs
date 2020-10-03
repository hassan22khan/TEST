using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Models;
using Repository;

namespace Repository
{
    public class StudentRepository : BaseRepository<Student>
    {
        
        public StudentRepository(StudentContext context) : base(context)
        {
        }
        /*
         public IEnumerable<Student> GetStudents()
         {
             using (var _context = new StudentContext())
             {
                 return _context.Students.ToList();

             }  
         }

         public Student GetStudent(int id)
         {
             using (var _context = new StudentContext())
             {
                 return _context.Students.SingleOrDefault(s => s.Id == id);

             }
         }

         public void AddStudent(Student student)
         {
             using (var _context = new StudentContext())
             {
                 _context.Students.Add(student);
                 _context.SaveChanges();
             }         
         }

         public void DeleteStudent(int id)
         {
             using (var _context = new StudentContext())
             {
                 var studentToDelete = _context.Students.Single(s => s.Id == id);
                 _context.Students.Remove(studentToDelete);
                 _context.SaveChanges();
             }

         }

         public void UpdateStudent(Student student)
         {
             using (var _context = new StudentContext())
             {
                 var students = _context.Students.ToList();
                 var studentToUpdate = students.Single(s => s.Id == student.Id);
                 studentToUpdate.Name = student.Name;
                 studentToUpdate.Email = student.Email;
                 studentToUpdate.Phone = student.Phone;
                 studentToUpdate.Password = student.Password;
                 studentToUpdate.ConfirmPassword = student.ConfirmPassword;

                 _context.SaveChanges();
             }

         }
         */
        
    }
}
