import React, { Component } from "react";
import Input from "./input";
import axios from "axios";
import $ from "jquery";
import { Redirect } from "react-router-dom";

class LoginForm extends Component {
  state = {
    account: {
      username: "",
      password: "",
    },
    redirect: undefined,
  };

  handleSubmit = (e) => {
    e.preventDefault();
    let userCredentials = {
      grant_type: "password",
      username: this.state.account.username,
      password: this.state.account.password,
    };
    axios({
      method: "post",
      url: "https://localhost:44380/Login",
      withCredentials: true,
      crossdomain: true,
      data: $.param(userCredentials),
    })
      .then((result) => {
        sessionStorage.setItem("accessToken", result.data.access_token);
        sessionStorage.setItem("dataPlusToken", JSON.stringify(result));
        this.setState({ redirect: "/table" });
      })
      .catch(function (error) {
        console.log("Post Error : " + error);
      });
  };

  handleChange = (e) => {
    const account = { ...this.state.account };
    account[e.currentTarget.name] = e.currentTarget.value;
    this.setState({ account: account });
  };

  render() {
    const { username, password } = this.state.account;
    if (this.state.redirect) return <Redirect to={this.state.redirect} />;
    return (
      <React.Fragment>
        <h1>Login</h1>
        <form onSubmit={this.handleSubmit}>
          <Input
            handleChange={this.handleChange}
            name="username"
            type="text"
            label="Username"
            className="input"
            value={username}
          />
          <Input
            handleChange={this.handleChange}
            name="password"
            type="password"
            label="Password"
            className="input"
            value={password}
          />
          <button type="submit">Submit</button>
        </form>
      </React.Fragment>
    );
  }
}

export default LoginForm;
