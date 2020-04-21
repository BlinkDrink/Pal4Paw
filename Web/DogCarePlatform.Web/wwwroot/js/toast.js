"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

connection.on("ReceiveToast", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    $(window).load(function () {
        M.toast({ html: msg });
    });
    //if (sessionStorage.reloadAfterPageLoad) {
      
    //    sessionStorage.reloadAfterPageLoad = false;
    //}
    //var encodedMsg = user + " says " + msg;
    //var li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
    // Hide hangoutButton
    // Assign "click"-event-method to userImage
});


connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});
//"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

////Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});


//"use strict";
//var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

//connection.on("sendToUser", (articleHeading, articleContent) => {
//    var heading = document.createElement("h3");
//    heading.textContent = articleHeading;
//    var p = document.createElement("p");
//    p.innerText = articleContent;

//    var div = document.createElement("div");
//    div.appendChild(heading);
//    div.appendChild(p);

//    document.getElementById("feature-warning").appendChild(div);
//});
//connection.start().catch(function (err) {
//    return console.error(err.toString());
//});

//"use strict";
//var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub?Id=" + Id).build();     
//connection.on("sendToUser", (articleHeading, articleContent) => {
//    var heading = document.createElement("h3");
//    heading.textContent = articleHeading;
//    var p = document.createElement("p");
//    p.innerText = articleContent;
//    var div = document.createElement("div");
//    div.appendChild(heading);
//    div.appendChild(p);

//    document.getElementById("articleList").appendChild(div);
//});
//connection.start().catch(function (err) {
//    return console.error(err.toString());
//}).then(function () {
//    document.getElementById("user").innerHTML = "UserId: " + userId;
//    connection.invoke('GetConnectionId').then(function (connectionId) {
//        document.getElementById('signalRConnectionId').innerHTML = connectionId;
//    })
//});

