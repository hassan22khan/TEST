let student = {};

function getStudentData() {
  student.name = $("#username").val();
  student.phone = $("#phone").val();
  student.email = $("#email").val();
  student.dob = $("#dob").val();
  student.password = $("#password").val();
  student.confirmPassword = $("#confirm-password").val();
}

function getStudents() {
  let studentsArray = JSON.parse(localStorage.getItem("Students"));
  if (studentsArray === null) studentsArray = [];
  return studentsArray;
}

function addStudent() {
  let studentsArray = getStudents();
  getStudentData();
  studentsArray.push(student);
  localStorage.setItem("Students", JSON.stringify(studentsArray));
}

function validate(student) {
  let validation = true;
  if (!isName(student.name)) {
    alert("name is not valid");
    validation = false;
  } else if (!isEmail(student.email)) {
    alert("email is not valid");
    validation = false;
  } else if (!isPhone(student.phone)) {
    alert("Contact number is not valid");
    validation = false;
  } else if (!isPassword(student.password, student.confirmPassword)) {
    alert("Password is not valid");
    validation = false;
  }
  return validation;
}

function onSave(index) {
  let studentsArray = getStudents();
  getStudentData();
  if (validate(student)) {
    if (index !== studentsArray.length) {
      studentsArray.splice(index, 1, student);
    } else {
      studentsArray.push(student);
    }

    localStorage.setItem("Students", JSON.stringify(studentsArray));
    $("#add-new-student").attr("disabled", false);
    window.location.reload();
  }
  return false;
}

function onCancel() {
  location.reload();
}

function onDelete(index) {
  let studentsArray = getStudents();
  studentsArray.splice(index, 1);
  localStorage.setItem("Students", JSON.stringify(studentsArray));
  window.location.reload();
}

function refillForm(index) {
  let studentsArray = getStudents();
  $("#username").val(studentsArray[index].name);
  $("#email").val(studentsArray[index].email);
  $("#phone").val(studentsArray[index].phone);
  $("#date").val(studentsArray[index].dob);
  $("#password").val(studentsArray[index].password);
  $("#confirm-password").val(studentsArray[index].confirmPassword);
}

function onEdit(index) {
  let studentsArray = getStudents();
  for (i = 0; i < studentsArray.length; i++) {
    if (i != index) {
      $(`#edit-${i}`).attr("disabled", true);
      $(`#delete-${i}`).attr("disabled", true);
    }
  }
  let row = `<tr id="${index}">
    <td><input type="text" id="username"/></td>
    <td><input type="email" id="email"/></td>
    <td><input type="text" id="phone"/></td>
    <td><input type="date" id="dob"/></td>
    <td><input type="password" id="password"/></td>
    <td><input type="password" id="confirm-password"/></td>
    <td><button onclick="onSave(${index})">Save</button></td>
    <td><button onclick="onCancel()">Cancel</button></td>
    </tr>`;
  if (index === 0) {
    $(`#row-${index}`).remove();
    $("#student-records").prepend(row);
  } else if (index > 0 && index + 1 < studentsArray.length) {
    $(`#row-${index}`).remove();
    $(`#row-${index + 1}`).before(row);
  } else if (index + 1 === studentsArray.length) {
    $(`#row-${index}`).remove();
    $(`#row-${index - 1}`).after(row);
  }
  refillForm(index);
}

function renderFormInTable() {
  let studentsArray = getStudents();
  let index = studentsArray.length;
  let row = `<tr>
<td><input type="text" id="username"/></td>
<td><input type="email" id="email"/></td>
<td><input type="text" id="phone"/></td>
<td><input type="date" id="dob"/></td>
<td><input type="password" id="password"/></td>
<td><input type="password" id="confirm-password"/></td>
<td><button onclick="onSave(${index})">Save</button></td>
<td><button onclick="onCancel()">Cancel</button></td>
</tr>`;
  $("#student-records").append(row);
  $("#add-new-student").attr("disabled", true);
}

function renderTable() {
  let studentsArray = getStudents();
  let table = studentsArray.map(
    (element, index) => `<tr id="row-${index}">
        <td>${element.name}</td>
        <td>${element.email}</td>
        <td>${element.phone}</td>
        <td>${element.dob}</td>
        <td>${element.password}</td>
        <td>${element.confirmPassword}</td>
        <td><button id="edit-${index}" onclick="onEdit(${index})">Edit</button></td>
        <td><button id="delete-${index}" onclick="onDelete(${index})">Delete</button></td>
        </tr>`
  );
  $("#student-records").append(table);
}
renderTable();
