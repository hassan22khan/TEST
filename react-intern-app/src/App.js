import './App.css';
import React, { Component } from 'react';
import Header from './components/header';
import {BrowserRouter as Router, Route,Link,Redirect} from 'react-router-dom';
import About from './components/pages/about';
import axios from 'axios';
import LoginForm from './components/loginForm';
import Table from './components/table';
import AddForm from './components/addForm';
import UiCalculator from './components/uiCalculator';
import SideBar from './components/collapsableSidebar';
import CalculatorBootstrap from './components/bootstrapCalculator';
class App extends Component {
  state ={
    studentDataFromApi : [],
    courses : [],
    selectOptions : [],
    userId:0,
    studentDataToEdit : undefined
  }

  componentDidMount = () => {
    let token =  JSON.parse(sessionStorage.getItem("dataPlusToken"));
    if(token != null){
      this.setState({userId : token.data.UserId});
      this.getStudentDataFromApi(token.data.UserId);
    }
    this.getCourses();
  }

  getStudentDataFromApi = (userId) => {
    axios.get(`https://localhost:44380/api/student/${userId}`)
    .then(res => 
      {
        debugger;
        this.setState({studentDataFromApi : res.data})})
      .catch(error => console.log(error));  
  }

  getCourses = () => {
    axios.get('https://localhost:44380/api/course').then(res => {
      debugger;
        const courses = res.data;
        const options = courses.map(course => ({
            "value" : course.id,
            "label" : course.name
        }));
        this.setState({courses : courses,selectOptions : options });
    })
    .catch(error => alert(error)); 
}

addTodo = (title) => {
  axios.post("https://jsonplaceholder.typicode.com/todos",{
    title : title,
    completed : false
  }).then(res => this.setState({todos : [...this.state.todos, res.data]}));
}

  delTodo = (id)=>{
    axios.delete(`https://jsonplaceholder.typicode.com/todos/${id}`)
      .then(res =>this.setState({todos : this.state.todos.filter(todo => todo.id !== id)}));
  }

  markCompleted =(id)=>{
   this.setState({todos : this.state.todos.map(todo => {
    if(todo.id === id) todo.completed = !todo.completed;
    return todo;
    })});
  }

  handleEdit = (studentData) => {
    let courses = this.state.selectOptions;
    let selectedCourses = [];
    studentData.courses.forEach(courseId => 
      {
        let filteredCourses = courses.filter(course => course.value == courseId);
        selectedCourses.push(filteredCourses[0]);
      });
    this.setState({studentDataToEdit : studentData,selectedCourses : selectedCourses }); 
  }

  handleDelete = (studentId) => {
    axios.delete(`https://localhost:44380/api/student/${studentId}`)
    .then(res => {
      let studentsArray = [...this.state.studentDataFromApi];
      let updatedArrayOfStudents = studentsArray.filter(data => data.student.id !== studentId);
      this.setState({studentDataFromApi : updatedArrayOfStudents});
    })
    .catch(error => alert("Failed to delete student."));
  }

  handleRefresh = () =>{
    debugger;
    this.componentDidMount();
  }
  render(){
    debugger;
    return (
      <Router>
        <div className="App">
          <div className="header">
          <Header/>
          </div>
          <div className="content">
            <div className="sidebar">
            <SideBar/>
            </div>
            <div className="content-display">
            <Route path="/table" render={(props) => (
          <React.Fragment>
            <Link to="/add" style={{margin:"10px"}} className="btn btn-primary">Add Student</Link>
          <Table dataArray={this.state.studentDataFromApi} handleEdit={this.handleEdit} handleDelete={this.handleDelete}/>
          </React.Fragment>)}/>
          <Route path="/about" component={About}/>
          <Route path="/login" component={LoginForm}/>
          <Route path="/add" render ={(props) => (<AddForm courses={this.state.selectOptions} studentToEdit={undefined} title={"Add Student"} userId={this.state.userId}/>)}/>
          <Route path="/edit" render ={(props) => (<AddForm  courses={this.state.selectOptions} handleRefresh={this.handleRefresh} selectedCourses={this.state.selectedCourses} studentToEdit={this.state.studentDataToEdit} title={"Edit Student"} userId={this.state.userId}/>)}/>
          <Route path="/calculator" component={CalculatorBootstrap}/>
            </div>
          </div>
        </div>
        </Router>
    );
  } 
}

export default App;
