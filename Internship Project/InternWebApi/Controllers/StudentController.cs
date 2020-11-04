using IRepository;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Http;
using ViewModels;

namespace InternWebApi.Controllers
{
    [Authorize]
    public class StudentController : ApiController
    {
        private IStudentsRepository _studentRepository;
        public StudentController(IStudentsRepository repo)
        {
            _studentRepository = repo;
        }

        [HttpGet]
        public IHttpActionResult GetStudents(string id)
        {
            var students = _studentRepository.GetStudentsForEachUser(id);
            if (students == null) return NotFound();
            return Ok(_studentRepository.GetStudentCourses(students));
        }

        [HttpPost] 
        public IHttpActionResult PostStudent(StudentViewModel viewModel)
        {
            char[] seperator = { ':', '/', ';' };
            String[] strlist = viewModel.ImageFile[0].Split(seperator);
            byte[] bytes = Convert.FromBase64String(viewModel.ImageFile[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            var subpath = "~/Images/";
            bool exist = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(subpath));
            if (!exist) Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subpath));
            var fileName = viewModel.Student.Name + "." + strlist[2];
            var filepath = HttpContext.Current.Server.MapPath("~/Images/");
            var path = Path.Combine(filepath, fileName);
            image.Save(path, ImageFormat.Png);
            viewModel.Student.ImagePath = fileName;
            _studentRepository.Post(viewModel.Student);
            _studentRepository.AddStudentCourses(viewModel);
            return Ok("posted student");
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            _studentRepository.Delete(id);
            _studentRepository.DeleteStudentCourses(id);
            return Ok("Deleted student");
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(StudentViewModel viewModel)
        {
            if (viewModel.Student == null) return BadRequest();
            char[] seperator = { ':', '/', ';' };
            String[] strlist = viewModel.ImageFile[0].Split(seperator);
            byte[] bytes = Convert.FromBase64String(viewModel.ImageFile[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            var subpath = "~/Images/";
            bool exist = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(subpath));
            if (!exist) Directory.CreateDirectory(HttpContext.Current.Server.MapPath(subpath));
            var studentInDb = _studentRepository.GetOne(viewModel.Student.Id);
            var fileName = studentInDb.ImagePath;
            var filepath = HttpContext.Current.Server.MapPath("~/Images/");
            var path = Path.Combine(filepath, fileName);
            image.Save(path, ImageFormat.Png);
            studentInDb.Name = viewModel.Student.Name;
            studentInDb.Email = viewModel.Student.Email;
            studentInDb.Phone = viewModel.Student.Phone;
            studentInDb.Password = viewModel.Student.Password;
            studentInDb.ConfirmPassword = viewModel.Student.ConfirmPassword;
            studentInDb.Dob = (DateTime)viewModel.Student.Dob;
            studentInDb.ImagePath = fileName;
            studentInDb.UserId = viewModel.Student.UserId;
            _studentRepository.Update(studentInDb);
            _studentRepository.UpdatedStudentCourses(viewModel);
            return Ok("updated the student");
        }
    }
}
