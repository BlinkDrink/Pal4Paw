"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

var btn = $("#btn-toast");

connection.on("RefreshDocument", function (msg) {
    //$("#btn-toast").click(function () {
    sessionStorage.reloadAfterPageLoad = true;

    window.location.reload();
    //});

    //$("#btn-toast").click();

    //$(function () {
    //    if (sessionStorage.reloadAfterPageLoad) {
    //        M.toast({html: msg});
    //        sessionStorage.reloadAfterPageLoad = false;
    //    }
    //});
});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});