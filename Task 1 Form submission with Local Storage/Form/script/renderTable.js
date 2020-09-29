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
  document.getElementById("student-table").innerHTML = localStorageData.map(
    (element, index) => `<tr><td>${element["name"]}</td>
  <td>${element["phone"]}</td>
  <td>${element["password"]}</td>
  <td>${element["confirmPassword"]}</td>
  <td><a onclick="onEdit(${index})" data-id="${index} href="./AddStudent.html">Edit</a></td>
  <td><a onClick="onDelete(${index})" data-id="${index}">Delete</a></td>
  </tr>`
  );
};

renderTable();
