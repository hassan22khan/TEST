function logUser(){
    debugger;
    let userCredentials = {
        grant_type : "password",
        username : $("#username").val(),
        password : $("#password").val()
    }
    $.ajax({
        url : "https://localhost:44380/Login",
        method : "POST",
        data : userCredentials,
        success : function(data){
            let tokenFromSession = JSON.parse(sessionStorage.getItem("accessToken"));
            let tokenFromAjax = data.access_token;
            if(tokenFromSession)
            tokenFromSession == tokenFromAjax ? alert("You are already logged in.") : alert("Another user is currently logged in");
            else{
                sessionStorage.setItem("accessToken",JSON.stringify(data.access_token));
                sessionStorage.setItem("dataPlusToken",JSON.stringify(data));
                location.href = "./studentTable.html";
            }
        },
    }).fail(function(){
        alert("ajax failed");
    });
}