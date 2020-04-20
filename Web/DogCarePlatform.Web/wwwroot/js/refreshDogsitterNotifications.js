"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

connection.on("RefreshNotifications", function () {
    $.ajax({
        async: true,
        url: '/Main/LoadddlView',
        data: '{model:' + JSON.stringify(check) + '}',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (objOperations) {


            $("#myPartialViewDiv").html(objOperations);

        }
    });
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});