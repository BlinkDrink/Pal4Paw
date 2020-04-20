"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

connection.on("RefreshDocument", function (msg) {
    sessionStorage.reloadAfterPageLoad = true;
    window.location.reload();

    $(function () {
        if (sessionStorage.reloadAfterPageLoad) {
            Materialize.toast(msg, 4000);
            sessionStorage.reloadAfterPageLoad = false;
        }
    });
});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});