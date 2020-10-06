let url = "https://localhost:44380/api/student";

function ajaxCall(parameters, objectToSend, method, success, fail) {
  if (parameters) {
    url += parameters;
  }
  $.ajax({
    url: url,
    data: objectToSend,
    method: method,
    success: function (data) {
      typeof success == "function"
        ? success(data)
        : alert("Function not passed in ajax");
    },
  }).fail(function () {
    fail();
  });
}
