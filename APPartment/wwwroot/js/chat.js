"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Home/Index").build();
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msgArr = message.split(/(?:\r\n|\r|\n)/g);
    var firstMsg = user + ": " + msgArr[0];
    var div = document.createElement("div");
    var p = document.createElement("span");
    p.textContent = firstMsg;
    div.appendChild(p);

    for (var i = 1; i < msgArr.length; i++) {
        var encodedMsg = msgArr[i];
        var nextP = document.createElement("span");
        nextP.textContent = encodedMsg;
        div.appendChild(document.createElement("br"));
        div.appendChild(nextP);
    }

    document.getElementById("messagesList").appendChild(div);
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

$(document).ready(function () {
    $('#sendButton').attr('disabled', true)

    $('#messageInput').on("keyup", function () {
        var textarea_value = $('#messageInput').val();
        if (textarea_value != '') {
            $('#sendButton').attr('disabled', false)
        }
        else {
            $('#sendButton').attr('disabled', true)
        }
    });
});

$("#messageInput").keypress(function (e) {
    var test = $("#messageInput").val()
    if (e.which == 13 && !e.shiftKey && test.trim() == "") {
        e.preventDefault();
    } else if (e.which == 13 && !e.shiftKey) {
        document.getElementById("sendButton").click();
    }
});

$('#messageInput').on("keydown", function (e) {
    if (e.keyCode == 13 && !e.shiftKey) {
        document.getElementById("sendButton").click();
    }
});