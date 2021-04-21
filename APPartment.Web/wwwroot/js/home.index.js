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

    var inventoryChart = document.getElementById("inventory-pie-chart");
    var inventoryChartBuilt = new Chart(inventoryChart, {
        type: "doughnut",
        data: {
            labels: ["Supplied", "Unsupplied"],
            datasets: [{
                data: [55, 45],
                backgroundColor: [
                    "rgba(0, 172, 105, 1)",
                    "rgba(244, 161, 0)"
                ],
                hoverBackgroundColor: [
                    "rgba(0, 172, 105, 0.9)",
                    "rgba(254, 181, 10)" // +10, +20, +10
                ],
                hoverBorderColor: "rgba(234, 236, 244, 1)"
            }]
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: "#dddfeb",
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10
            },
            legend: {
                display: true
            },
            cutoutPercentage: 80
        }
    });

    var choresChart = document.getElementById("chores-pie-chart");
    var choresChartBuilt = new Chart(choresChart, {
        type: "doughnut",
        data: {
            labels: ["Mine", "Others"],
            datasets: [{
                data: [70, 30],
                backgroundColor: [
                    "rgba(0, 172, 105, 1)",
                    "rgba(0, 97, 242, 1)"
                ],
                hoverBackgroundColor: [
                    "rgba(0, 172, 105, 0.9)",
                    "rgba(0, 97, 242, 0.9)"
                ],
                hoverBorderColor: "rgba(234, 236, 244, 1)"
            }]
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: "#dddfeb",
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10
            },
            legend: {
                display: true
            }
        }
    });

    var issuesChart = document.getElementById("issues-pie-chart");
    var issuesChartBuilt = new Chart(issuesChart, {
        type: "pie",
        data: {
            labels: ["Open", "Closed"],
            datasets: [{
                data: [90, 10],
                backgroundColor: [
                    "rgba(232, 21, 0)",
                    "rgba(0, 172, 105, 1)"
                ],
                hoverBackgroundColor: [
                    "rgba(242, 40, 10)", // +10, +20, +10
                    "rgba(0, 172, 105, 0.9)"
                ],
                hoverBorderColor: "rgba(234, 236, 244, 1)"
            }]
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: "#dddfeb",
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10
            },
            legend: {
                display: true
            }
        }
    });

    $.ajax({
        url: "Home/GetHomeStatus",
        success: function (data) {
            var dataArr = data.split(";");
            var status = parseInt(dataArr[0]);
            var username = dataArr[1];
            var details = dataArr[2];

            $('#homeStatusWidget').removeAttr('class');

            if (status === 1) {
                $('#homeStatus').html('<span class="btn btn-circle btn-success btn-sm"><i class="fas fa-thumbs-up"></i></span>');
                $('#homeStatusWidget').addClass('card bg-success text-white shadow');
            } else if (status === 2) {
                $('#homeStatus').html('<span class="btn btn-circle btn-warning btn-sm"><i class="fas fa-exclamation"></i></span>');
                $('#homeStatusWidget').addClass('card bg-warning text-white shadow');
            } else {
                $('#homeStatus').html('<span class="btn btn-circle btn-danger btn-sm"><i class="fas fa-ban"></i></span>');
                $('#homeStatusWidget').addClass('card bg-danger text-white shadow');
            }

            if (details && details != "") {
                $('#homeStatusDetailsNavItem').html(details);
            }

            $('#homeStatusUserSetNavItem').html(username);

            $('#homeStatusWidgetBody').html(`Home status has been set by ${username}` + `<div class="text-white-50 small">${details}</div>`);
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});