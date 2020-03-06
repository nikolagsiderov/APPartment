$("#sendButton").click(function () {

    var usernameValue = $('#userInput').val();
    var messageValue = $('#messageInput').val();

    $.ajax({
        type: "POST",
        url: "Home/CreateMessage",
        data: { username: usernameValue, messageText: messageValue },
        dataType: "text",
        success: function () {

        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $("#messageInput").val('');
});

$(document).ready(function () {
    $.ajax({
        url: "Home/GetHomeStatus",
        success: function (data) {
            var dataArr = data.split(";");
            var username = dataArr[1];
            var details = dataArr[2];

            $('#houseStatusWidgetBody').html(`Home status has been set by ${username}` + `<div class="text-white-50 small">${details}</div>`);
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});