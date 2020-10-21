function ajaxCall(urlToAdd, parameters, objectToSend, method, headers, success, fail) {
  let url = "https://localhost:44380/api/";
  url = url + urlToAdd;
  if (parameters) {
    url += parameters;
  }
  $.ajax({
    url: url,
    data: objectToSend,
    method: method,
    headers :{
      'Authorization' : getAccessToken()
    },
    success: function (data) {
      typeof success == "function"
        ? success(data)
        : alert("Function not passed in ajax");
    },
  }).fail(function () {
    fail();
  });
}

function getAccessToken(){
 return sessionStorage.getItem("accessToken") == null ? null : 'Bearer ' + JSON.parse(sessionStorage.getItem("accessToken"));
}
