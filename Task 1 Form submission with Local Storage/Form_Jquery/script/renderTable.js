onEdit = (index) => {
  localStorage.setItem("StudentId", JSON.stringify(index));
  window.location.href = "./AddStudent.html";
};

onDelete = (index) => {
  let localStorageData = getDataFromLocalStorage();
  localStorageData.splice(index, 1);
  localStorage.setItem("USERS", JSON.stringify(localStorageData));
  window.location.reload();
};

renderTable = () => {
  let localStorageData = getDataFromLocalStorage();
  let tableRow = localStorageData.map(
    (element, index) => `<tr><td>${element["name"]}</td>
  <td>${element["email"]}</td>
  <td>${element["phone"]}</td>
  <td>${element["password"]}</td>
  <td>${element["confirmPassword"]}</td>
  <td><button><a onclick="onEdit(${index})" data-id="${index} href="./AddStudent.html">Edit</a></button></td>
  <td><button><a onClick="onDelete(${index})" data-id="${index}">Delete</a></button></td>
  </tr>`
  );
  $("#student-table").append(tableRow);
};

renderTable();
