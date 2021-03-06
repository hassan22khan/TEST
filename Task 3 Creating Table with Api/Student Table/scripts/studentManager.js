let student = {};
let studentsArray;
let arrayLength;
function getStudentData() {
  student.name = $("#username").val();
  student.phone = $("#phone").val();
  student.email = $("#email").val();
  student.dob = $("#dob").val();
  student.password = $("#password").val();
  student.confirmPassword = $("#confirm-password").val();
}

function getStudents() {
  ajaxCall(
    "",
    null,
    "GET",
    (data) => {
      renderTable(data);
    },
    () => {
      alert("Failed to get students");
    }
  );
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
  getStudentData();
  if (validate(student)) {
    if (index !== studentsArray.length) {
      let studentToEdit = studentsArray[index];
      let data = {
        Id: studentToEdit.id,
        Name: student.name,
        Email: student.email,
        Phone: student.phone,
        Dob: student.dob,
        Password: student.password,
        ConfirmPassword: student.confirmPassword,
      };

      ajaxCall(
        null,
        data,
        "PUT",
        () => {
          location.reload();
        },
        () => {
          alert("Failed to update student");
        }
      );
    } else {
      let data = {
        Name: student.name,
        Email: student.email,
        Phone: student.phone,
        Dob: student.dob,
        Password: student.password,
        ConfirmPassword: student.confirmPassword,
      };

      ajaxCall(
        null,
        data,
        "POST",
        () => {
          $("#add-new-student").attr("disabled", false);
          window.location.reload();
        },
        () => {
          alert("Failed to add student");
        }
      );
    }
  }
}

function onCancel() {
  location.reload();
}

function onDelete(studentId) {
  ajaxCall(
    `?Id=${studentId}`,
    null,
    "DELETE",
    () => {
      location.reload();
    },
    () => {
      alert("Failed to delete");
    }
  );

  // $.ajax({
  //   url: "https://localhost:44380/api/student?Id=" + studentId,
  //   method: "DELETE",
  //   success: function () {
  //     window.location.reload();
  //   },
  // }).fail(function () {
  //   alert("Failed to delete student");
  // });
}

function refillForm(index) {
  $("#username").val(studentsArray[index].name);
  $("#email").val(studentsArray[index].email);
  $("#phone").val(studentsArray[index].phone);
  $("#dob").val(studentsArray[index].dob);
  $("#password").val(studentsArray[index].password);
  $("#confirm-password").val(studentsArray[index].confirmPassword);
}

function onEdit(index) {
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
  let index = arrayLength;
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

function renderTable(dataFromApi) {
  studentsArray = JSON.parse(JSON.stringify(dataFromApi));
  if (studentsArray === null) studentsArray = [];
  console.log(studentsArray);
  arrayLength = studentsArray.length;
  let table = studentsArray.map(
    (element, index) => `<tr id="row-${index}">
        <td>${element.name}</td>
        <td>${element.email}</td>
        <td>${element.phone}</td>
        <td>${new Date(element.dob).toDateString()}</td>
        <td>${element.password}</td>
        <td>${element.confirmPassword}</td>
        <td><button id="edit-${index}" onclick="onEdit(${index})">Edit</button></td>
        <td><button id="delete-${index}" onclick="onDelete(${
      element.id
    })">Delete</button></td>
        </tr>`
  );
  $("#student-records").append(table);
}

getStudents();
