using IRepository;
using System.Web.Http;
using ViewModels;

namespace InternWebApi.Controllers
{
    [Authorize]
    public class StudentController : ApiController
    {
        private IStudentsRepository _repo;
        public StudentController(IStudentsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IHttpActionResult GetStudents(string id)
        {
            var students = _repo.GetStudentsForEachUser(id);
            if (students == null) return NotFound();
            return Ok(_repo.GetStudentCourses(students));
        }

        [HttpPost]
        public IHttpActionResult PostStudent(StudentViewModel viewModel)
        {
            if (viewModel.Student == null) return BadRequest();
            _repo.Post(viewModel.Student);
            _repo.AddStudentCourses(viewModel);
            return Ok("posted student");
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            _repo.Delete(id);
            _repo.DeleteStudentCourses(id);
            return Ok("Deleted student");
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(StudentViewModel viewModel)
        {
            if (viewModel.Student == null) return BadRequest();
            _repo.Update(viewModel.Student);
            _repo.UpdatedStudentCourses(viewModel);
            return Ok("updated the student");
        }
    }
}
