function isName(name) {
  return /^[a-zA-Z ]{2,30}$/.test(name);
}

function isEmail(email) {
  return /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/.test(email);
}

function isPhone(phoneNumber) {
  return /^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$/.test(phoneNumber);
}

function isPassword(password, confirmPassword) {
  if (password === confirmPassword) {
    let passwordResult = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$/.test(
      password
    );
    if (passwordResult) {
      return true;
    }
  }
}
