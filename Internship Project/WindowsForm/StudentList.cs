using IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewModels;

namespace WindowsForm
{
    public partial class StudentList : Form
    {
        private ICoursesRepository _repository;
        private IStudentsRepository _studentRepository;
        private int _userId;
        public StudentList(ICoursesRepository repository,IStudentsRepository studentRepository,int userId)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _userId = userId;
            InitializeComponent();
        }

        private void AddStudent_Click(object sender, EventArgs e)
        {
            this.Dispose();
            var studentForm = new StudentForm(_repository,_studentRepository,_userId,0);
            studentForm.Show();
        }

        private IEnumerable<StudentWithCoursesStringViewModel> GetStudentsWithCourses()
        {
            var studentsPerUser = _studentRepository.GetStudentsForEachUser(_userId.ToString());
            var studentsWithCourses = _studentRepository.GetStudentListWithCourses(studentsPerUser);
            return _studentRepository.GetStudentWithCoursesString(studentsWithCourses);
        }
        private void StudentList_Load(object sender, EventArgs e)
        {
            studentListDataGridView.DataSource = GetStudentsWithCourses();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(studentListDataGridView.Rows[studentListDataGridView.CurrentRow.Index].Cells[0].Value);
            _studentRepository.Delete(id);
            studentListDataGridView.DataSource = GetStudentsWithCourses();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(studentListDataGridView.Rows[studentListDataGridView.CurrentRow.Index].Cells[0].Value);
            string imageName = studentListDataGridView.Rows[studentListDataGridView.CurrentRow.Index].Cells[8].Value.ToString();
            this.Dispose();
            var studentForm = new StudentForm(_repository, _studentRepository, _userId,id);
            studentForm.Show();
            studentForm.RefillForm(imageName);
        }

        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(studentListDataGridView.Rows[studentListDataGridView.CurrentRow.Index].Cells[0].Value);
            //var imgUploadForm = new ImageUploadForm(studentId);
            //imgUploadForm.Show();
        }
    }
}
