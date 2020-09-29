let formData = {};

function getFormData() {
  formData.name = document.getElementById("username").value;
  formData.phone = document.getElementById("phone").value;
  formData.password = document.getElementById("password").value;
  formData.confirmPassword = document.getElementById("confirm-password").value;
  formData.dob = document.getElementById("date").value;
}

function getDataFromLocalStorage() {
  return JSON.parse(localStorage.getItem("USERS"));
}

function editData(id) {
  let localArray = getDataFromLocalStorage();
  let objectToEdit = localArray.find((value) => value.id == id);
  let index = localArray.findIndex((value) => value.id == id);
  console.log("on edit index of found element is" + index);
  document.getElementById("username").value = objectToEdit.name;
  document.getElementById("phone").value = objectToEdit.phone;
  document.getElementById("password").value = objectToEdit.password;
  document.getElementById("confirm-password").value =
    objectToEdit.confirmPassword;
  document.getElementById("date").value = objectToEdit.dob;
  document.getElementById("id").value = objectToEdit.id;

  //Assigning these values to user data then from submit event we will submit the new values to local storage
}

function clearInputs() {
  document.getElementById("username").value = null;
  document.getElementById("phone").value = null;
  document.getElementById("password").value = null;
  document.getElementById("confirm-password").value = null;
  document.getElementById("dob").value = null;
}

function validation(formData) {
  let validationResult = true;

  if (!isName(formData.name)) {
    alert("Name is not valid");
    validationResult = false;
  }

  if (!isPhone(formData.phone)) {
    alert("Phone number is not valid");
    validationResult = false;
  }

  if (!isPassword(formData.password, formData.confirmPassword)) {
    alert("Password is not valid");
    validationResult = false;
  }

  if (validationResult) return true;
}

function addDataToLocalStorage(formData) {
  let localStorageData = getDataFromLocalStorage();
  if (localStorageData == null) localStorageData = [];
  localStorageData.push(formData);
  localStorage.setItem("USERS", JSON.stringify(localStorageData));
}

function refillForm(index) {
  let studentId = JSON.parse(localStorage.getItem("StudentId"));
  if (studentId != null) {
    let localStorageData = getDataFromLocalStorage();
    let studentToEdit = localStorageData[studentId];
    console.log(studentToEdit);
    document.getElementById("username").value = studentToEdit.name;
    document.getElementById("phone").value = studentToEdit.phone;
    document.getElementById("password").value = studentToEdit.password;
    document.getElementById("confirm-password").value =
      studentToEdit.confirmPassword;
  }
}

refillForm();

document.getElementById("submit").addEventListener("click", function () {
  let studentId = localStorage.getItem("StudentId");
  getFormData();
  let validInputs = validation(formData);
  if (validInputs) {
    if (studentId != null) {
      let localStorageData = getDataFromLocalStorage();
      localStorageData.splice(studentId, 1, { ...formData });
      localStorage.setItem("USERS", JSON.stringify(localStorageData));
      localStorage.removeItem("StudentId");
    } else {
      addDataToLocalStorage(formData);
    }
  }
});
