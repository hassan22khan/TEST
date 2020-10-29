using DependencyResolver;
using IData;
using IRepository;
using Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ViewModels;

namespace WindowsForm
{
    public partial class StudentForm : Form
    {
        private ICoursesRepository _repository;
        private IStudentsRepository _studentRepository;
        private int _studentId;
        private int _userId;
        public StudentForm(ICoursesRepository repository,IStudentsRepository studentRepository, int userId,int studentId)
        {
            _studentId = studentId;
            _repository = repository;
            _studentRepository = studentRepository;
            _userId = userId;
            InitializeComponent();
        }
        private void StudentForm_Load_1(object sender, EventArgs e)
        {
            CoursesList.DataSource = _repository.GetAll();
            CoursesList.DisplayMember = "Name";
            CoursesList.ValueMember = "Id";
            CoursesList.SelectionMode = SelectionMode.MultiExtended;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var coursesIdList = new List<string>();
            foreach(var course in CoursesList.SelectedItems)
            {
                var courseModel = (Course)course;
                coursesIdList.Add(courseModel.Id.ToString());
            }

            var studentViewModel = new StudentViewModel { 
                Student = new Student 
                {   Id = _studentId,
                    Name = Username.Text,
                    Email = Email.Text,
                    Dob = DateTime.Parse(DateOfBirth.Text),
                    Phone = ContactNumber.Text,
                    Password = Password.Text,
                    ConfirmPassword = ConfirmPassword.Text,
                    UserId = _userId 
                },
                Courses = coursesIdList
            };
            if(_studentId == 0)
            {
                _studentRepository.Post(studentViewModel.Student);
                _studentRepository.AddStudentCourses(studentViewModel);
            }
            else
            {
                var studentInDb = _studentRepository.GetOne(_studentId);
                studentInDb.Id = _studentId;
                studentInDb.Name = Username.Text;
                studentInDb.Email = Email.Text;
                studentInDb.Dob = DateTime.Parse(DateOfBirth.Text);
                studentInDb.Phone = ContactNumber.Text;
                studentInDb.Password = Password.Text;
                studentInDb.ConfirmPassword = ConfirmPassword.Text;
                studentInDb.UserId = _userId;
                _studentRepository.Update(studentInDb);
                _studentRepository.UpdatedStudentCourses(studentViewModel);
            }
            this.Dispose();
            new StudentList(_repository, _studentRepository, _userId).Show();
        }

        public void RefillForm()
        {
           var student = _studentRepository.GetOne(_studentId);
            var studentViewModel = _studentRepository.GetOneStudentWithCourses(student);
            Username.Text = student.Name;
            Email.Text = student.Email;
            DateOfBirth.Text = student.Dob.ToString();
            ContactNumber.Text = student.Phone;
            Password.Text = student.Password;
            ConfirmPassword.Text = student.ConfirmPassword;
            CoursesList.DataSource = _repository.GetAll();
            CoursesList.DisplayMember = "Name";
            CoursesList.ValueMember = "Id";
            CoursesList.SelectionMode = SelectionMode.MultiExtended;
            CoursesList.SelectedItems.Clear();
            foreach (var courseId in studentViewModel.Courses)
            {
                foreach(var course in _repository.GetAll())
                {
                    if (course.Id.ToString() == courseId)
                    {
                        CoursesList.SetSelected(course.Id - 1, true);
                        break;
                    }
                }
            }
        }
    }
}
