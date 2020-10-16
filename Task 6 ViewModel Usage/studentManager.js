let student = {};
let studentsArray;
let arrayLength;
let coursesFromApi;

function getStudentData() {
  student.name = $("#username").val();
  student.phone = $("#phone").val();
  student.email = $("#email").val();
  student.dob = $("#dob").val();
  student.password = $("#password").val();
  student.confirmPassword = $("#confirm-password").val();
  student.coursesList = $("#select-courses").val();
}

function getStudents() {
  ajaxCall(
    "student",
    null,
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
  console.log(student);
  if (validate(student)) {
    if (index !== studentsArray.length) {
      // ajax call for update
      let studentToEdit = studentsArray[index].student;
      let data = {
        Student: {
          Id: studentToEdit.id,
          Name: student.name,
          Email: student.email,
          Phone: student.phone,
          Dob: student.dob,
          Password: student.password,
          ConfirmPassword: student.confirmPassword,
        },
        Courses: student.coursesList,
      };

      ajaxCall(
        "student",
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
      // ajax call for post
      let data = {
        Student: {
          Name: student.name,
          Email: student.email,
          Phone: student.phone,
          Dob: student.dob,
          Password: student.password,
          ConfirmPassword: student.confirmPassword,
        },
        Courses: student.coursesList,
      };

      ajaxCall(
        "student",
        null,
        data,
        "POST",
        () => {
          $("#add-new-student").attr("disabled", false);
          window.location.reload();
        },
        () => {
          alert("Failed to add student");
          console.log(data);
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
    "student",
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
}

function refillForm(index) {
  // refilling the form with user data when edit is clicked

  $("#username").val(studentsArray[index].student.name);
  $("#email").val(studentsArray[index].student.email);
  $("#phone").val(studentsArray[index].student.phone);
  $("#dob").val(studentsArray[index].student.dob);
  $("#password").val(studentsArray[index].student.password);
  $("#confirm-password").val(studentsArray[index].student.confirmPassword);

  $("#select-courses")
    .val([...studentsArray[index].courses])
    .prop("selected", true);
}

function onEdit(index) {
  // Disabling the edit and delete buttons for all records other than the one being edited

  for (i = 0; i < studentsArray.length; i++) {
    if (i != index) {
      $(`#edit-${i}`).attr("disabled", true);
      $(`#delete-${i}`).attr("disabled", true);
    }
  }

  let coursesDropdown = renderCoursesDropdown();

  let row = `<tr id="${index}">
    <td><input type="text" id="username"/></td>
    <td><input type="email" id="email"/></td>
    <td><input type="text" id="phone"/></td>
    <td><input type="date" id="dob"/></td>
    <td><input type="password" id="password"/></td>
    <td><input type="password" id="confirm-password"/></td>
    <td>${coursesDropdown}</td>
    <td><button onclick="onSave(${index})">Save</button></td>
    <td><button onclick="onCancel()">Cancel</button></td>
    </tr>`;

  // Determined where to render the form in table using index recieved

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

function getCourses() {
  ajaxCall(
    "course",
    null,
    null,
    "GET",
    (data) => {
      coursesFromApi = JSON.parse(JSON.stringify(data));
      console.log(coursesFromApi);
    },
    () => {
      alert("Failed to get courses");
    }
  );
}

function renderCoursesInTable(element) {
  // Filtered the courses array and got a new array with courses in which student has enrolled
  // then returned html markup for rendering it

  let filteredArray = [];
  for (i = 0; i < coursesFromApi.length; i++) {
    let array = coursesFromApi.filter(
      (value) => value.id == parseInt(element.courses[i])
    );
    if (array[0]) filteredArray.push(array[0]);
  }
  console.log(filteredArray);

  return filteredArray.length != 0
    ? `<ul>${filteredArray.map((value) => `<li>${value.name}</li>`)}</ul>`
    : "<ul><li>No Courses</li></ul>";
}

function renderCoursesDropdown() {
  //Rendering select list for courses inside the table

  let coursesDropdown = `<select id="select-courses" multiple>${coursesFromApi.map(
    (element) => `<option value="${element.id}">${element.name}</option>`
  )}</select>`;

  return coursesDropdown;
}

function renderFormInTable() {
  // Form for adding the students

  let index = arrayLength;
  let coursesDropdown = renderCoursesDropdown();

  let row = `<tr id="row-form">
<td><input type="text" id="username" required/></td>
<td><input type="email" id="email" required/></td>
<td><input type="text" id="phone" required/></td>
<td><input type="date" id="dob" required/></td>
<td><input type="password" id="password" required/></td>
<td><input type="password" id="confirm-password" required/></td>
<td>${coursesDropdown}</td>
<td><button onclick="onSave(${index})">Save</button></td>
<td><button onclick="onCancel()">Cancel</button></td>
</tr>`;

  $("#student-records").append(row);
  $("#add-new-student").attr("disabled", true);
}

function renderTable(dataFromStudentsApi) {
  studentsArray = JSON.parse(JSON.stringify(dataFromStudentsApi));
  if (studentsArray === null) studentsArray = [];
  console.log(studentsArray);
  arrayLength = studentsArray.length;
  let table = studentsArray.map(
    (element, index) => `<tr id="row-${index}">
        <td>${element.student.name}</td>
        <td>${element.student.email}</td>
        <td>${element.student.phone}</td>
        <td>${new Date(element.student.dob).toDateString()}</td>
        <td>${element.student.password}</td>
        <td>${element.student.confirmPassword}</td>
        <td>${renderCoursesInTable(element)}</td>
        <td><button id="edit-${index}" onclick="onEdit(${index})">Edit</button></td>
        <td><button id="delete-${index}" onclick="onDelete(${
      element.student.id
    })">Delete</button></td>
        </tr>`
  );
  $("#student-records").append(table);
}

getCourses();
getStudents();
