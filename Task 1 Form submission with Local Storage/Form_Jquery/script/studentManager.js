let formData = {};

function getFormData() {
  formData.name = $("#username").val();
  formData.email = $("#email").val();
  formData.phone = $("#phone").val();
  formData.password = $("#password").val();
  formData.confirmPassword = $("#confirm-password").val();
  formData.dob = $("#date").val();
}

function getDataFromLocalStorage() {
  return JSON.parse(localStorage.getItem("USERS"));
}

function editData(id) {
  let localArray = getDataFromLocalStorage();
  let objectToEdit = localArray.find((value) => value.id == id);
  let index = localArray.findIndex((value) => value.id == id);
  console.log("on edit index of found element is" + index);
  $("#username").val(objectToEdit.name);
  $("#email").val(objectToEdit.email);
  $("#phone").val(objectToEdit.phone);
  $("#password").val(objectToEdit.password);
  $("#confirm-password").val(objectToEdit.confirmPassword);
  $("#date").val(objectToEdit.dob);
  $("#id").val(objectToEdit.id);

  //Assigning these values to user data then from submit event we will submit the new values to local storage
}

function clearInputs() {
  $("#username").val(null);
  $("#email").val(null);
  $("#phone").val(null);
  $("#password").val(null);
  $("#confirm-password").val(null);
  $("#dob").val(null);
}

function validation(formData) {
  let validationResult = true;

  if (!isName(formData.name)) {
    $(".username-error-label")
      .text("Name is not valid")
      .css("display", "block")
      .css("margin-top", "3px");

    validationResult = false;
  } else if (!isEmail(formData.email)) {
    $(".email-error-label")
      .text("Email is not valid")
      .css("display", "block")
      .css("margin-top", "3px");

    validationResult = false;
  } else if (!isPhone(formData.phone)) {
    $(".phone-error-label")
      .text("Contact number is not valid")
      .css("display", "block")
      .css("margin-top", "3px");

    validationResult = false;
  } else if (!isPassword(formData.password, formData.confirmPassword)) {
    $(".password-error-label,.confirm-password-error-label")
      .text("Password is not valid")
      .css("display", "block")
      .css("margin-top", "3px");

    validationResult = false;
  }

  return validationResult;
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
    $("#username").val(studentToEdit.name);
    $("#email").val(studentToEdit.email);
    $("#phone").val(studentToEdit.phone);
    $("#password").val(studentToEdit.password);
    $("#confirm-password").val(studentToEdit.confirmPassword);
    $("#date").val(studentToEdit.dob);
  }
}

refillForm();

$("#submit").on("click", function () {
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
    window.location.href = "./StudentsList.html";
  }
  return validInputs;
  //else {
  //   $("#username").val(formData.name);
  //   $("#email").val(formData.email);
  //   $("#phone").val(formData.phone);
  //   $("#password").val(formData.password);
  //   $("#confirm-password").val(formData.confirmPassword);
  //   $("#date").val(formData.dob);
  // }
});
