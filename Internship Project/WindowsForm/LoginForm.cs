using IRepository;
using StudentModels;
using System;
using System.Windows.Forms;

namespace WindowsForm
{
    public partial class LoginForm : Form
    {
        private IUserRepository _repository;
        private ICoursesRepository _coursesRepository;
        private IStudentsRepository _studentRepository;
        public LoginForm(IUserRepository repository, ICoursesRepository coursesRepository,IStudentsRepository studentRepository)
        {
            InitializeComponent();
            _repository = repository;
            _coursesRepository = coursesRepository;
            _studentRepository = studentRepository;
        }
        private void username_Click(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            var user = _repository.ValidateUser(usernameText.Text, passwordText.Text);
            if (user != null)
            {
                var studentList = new StudentList(_coursesRepository,_studentRepository,user.Id);
                studentList.Show();
            }
        }
    }
}
