using Data.ViewModel;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class StudentContext :DbContext
    {
        public StudentContext() : base("name=StudentDB")
        {
        }
        public  DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        
        public virtual decimal InsertStudent(Student student)
        {
                var name_param = new SqlParameter("@Name", student.Name);
                var email_param = new SqlParameter("@Email", student.Email);
                var password_param = new SqlParameter("@Password", student.Password);
                var confirmPassword_param = new SqlParameter("@ConfirmPassword", student.ConfirmPassword);
                var phone_param = new SqlParameter("@Phone", student.Phone);
                var dob_param = new SqlParameter("@Dob", student.Dob);
                return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<decimal>("InsertNewStudent @Name, @Email, @Password, @ConfirmPassword, @Phone, @Dob", name_param, email_param, password_param, confirmPassword_param, phone_param, dob_param).First();
        }
        
        public virtual void InsertStudentCourses(StudentViewModel studentViewModel,decimal id)
        {
            int studentId = Decimal.ToInt32(id);
            foreach(var course in studentViewModel.Courses)
            {
                var studentId_param = new SqlParameter("@StudentId", studentId);
                var courseId_param = new SqlParameter("@CourseId", Convert.ToInt32(course));

                ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Nullable<decimal>>("InsertStudentCourse @StudentId,@CourseId",studentId_param,courseId_param).First();
            }
        }


    }
}
