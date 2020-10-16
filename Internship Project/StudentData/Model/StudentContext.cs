using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Model
{
    public class StudentContext : IdentityDbContext<ApplicationUser>
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

        public static StudentContext Create()
        {
            return new StudentContext();
        }

        public virtual void InsertStudentCourses(string courseId,decimal id)
        {
                var studentId_param = new SqlParameter("@StudentId", Decimal.ToInt32(id));
                var courseId_param = new SqlParameter("@CourseId", Convert.ToInt32(courseId));
                ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<Nullable<decimal>>("InsertStudentCourse @StudentId,@CourseId",studentId_param,courseId_param).First();
         }
    }
}
