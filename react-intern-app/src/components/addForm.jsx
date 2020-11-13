import React, { Component } from "react";
import Input from "./input";
import axios from "axios";
import Select from "react-select";
import { Redirect } from "react-router-dom";
class AddForm extends Component {
  state = {
    formData: {
      name: "",
      email: "",
      dob: "",
      phone: "",
      password: "",
      confirmPassword: "",
    },
    imageFile: "",
    selectedCourses: [],
    redirect: undefined,
  };

  componentDidMount() {
    this.setSelectValue();
    this.setFormDataValue();
  }

  setFormDataValue = () => {
    const { studentToEdit } = this.props;
    if (studentToEdit !== undefined) {
      this.setState({ formData: studentToEdit.student });
    }
  };

  refillForm = () => {
    const { studentToEdit, courses } = this.props;
    if (studentToEdit !== null && studentToEdit !== undefined) {
      this.setState({
        formData: studentToEdit.student,
        imageFile: studentToEdit.imageFile,
      });
    }
  };

  handleSubmit = (e) => {
    e.preventDefault();
    let courseIds = [];
    this.state.selectedCourses.forEach((course) => {
      courseIds.push(course.value);
    });
    let formData = { ...this.state.formData, userId: this.props.userId };
    if (this.props.selectedCourses !== undefined) {
      axios({
        method: "PUT",
        url: "https://localhost:44380/api/student",
        data: {
          Student: formData,
          imageFile: this.state.imageFile,
          courses: courseIds,
        },
        crossdomain: true,
        withCredentials: true,
      })
        .then((res) => {
          this.props.handleRefresh();
          this.setState({ redirect: "/table" });
        })
        .catch((error) => alert(error));
    } else {
      axios({
        method: "POST",
        url: "https://localhost:44380/api/student",
        data: {
          Student: formData,
          imageFile: this.state.imageFile,
          courses: courseIds,
        },
        crossdomain: true,
        withCredentials: true,
      })
        .then((res) => {
          this.setState({ redirect: "/table" });
        })
        .catch((error) => alert(error));
    }
  };

  handleChange = (e) => {
    const formData = { ...this.state.formData };
    formData[e.currentTarget.name] = e.currentTarget.value;
    this.setState({ formData: formData });
  };

  encodingImageAsUrl = (e) => {
    let imageFile;
    let files = e.currentTarget.files;
    if (files && files[0]) {
      var fileSelected = files[0];
      var fileReader = new FileReader();
      fileReader.onload = (FileLoadEvent) => {
        var srcData = FileLoadEvent.target.result;
        let baseArray = srcData.split(",");
        imageFile = baseArray;
        this.setState({ formData: this.state.formData, imageFile: imageFile });
      };
      fileReader.readAsDataURL(fileSelected);
    }
  };

  handleSelectChange = (e) => {
    this.setState({ selectedCourses: e });
  };

  setSelectValue = () => {
    let sltCrs = this.props.selectedCourses;
    this.setState({ selectedCourses: sltCrs });
  };

  render() {
    if (this.state.redirect) return <Redirect to={this.state.redirect} />;
    const {
      name,
      email,
      dob,
      phone,
      password,
      confirmPassword,
    } = this.state.formData;
    const { title } = this.props;
    return (
      <React.Fragment>
        <h1 style={{ textAlign: "center", padding: "5px" }}>{title}</h1>
        <form onSubmit={this.handleSubmit}>
          <Input
            name="name"
            value={name}
            label="Username"
            type="text"
            handleChange={this.handleChange}
          />
          <Input
            name="email"
            value={email}
            label="Email"
            type="email"
            handleChange={this.handleChange}
          />
          <Input
            name="dob"
            value={dob}
            label="Date of birth"
            type="date"
            handleChange={this.handleChange}
          />
          <Input
            name="phone"
            value={phone}
            label="Contact Number"
            type="text"
            handleChange={this.handleChange}
          />
          <Input
            name="password"
            value={password}
            label="Password"
            type="password"
            handleChange={this.handleChange}
          />
          <Input
            name="confirmPassword"
            value={confirmPassword}
            label="Confirm Password"
            type="password"
            handleChange={this.handleChange}
          />

          <div className="form-group">
            <label htmlFor="imageFile">Upload Student Image</label>
            <input
              id="imageFile"
              name="imageFile"
              type="file"
              label="Upload student image"
              onChange={this.encodingImageAsUrl}
              className="form-control"
            />
          </div>
          <div className="form-group">
            <label htmlFor="selectCourses">Select Courses</label>
            <Select
              value={this.state.selectedCourses}
              name="selectCourses"
              options={this.props.courses}
              onChange={this.handleSelectChange}
              isMulti
            />
          </div>
          <input type="submit" value="Save" className="btn btn-primary" />
        </form>
      </React.Fragment>
    );
  }
}

export default AddForm;
