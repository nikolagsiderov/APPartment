$("#setHomeStatusButton").click(function () {
    var homeStatusStatus = $('#homeStatusStatus').val().toString();
    var homeStatusDetailsString = $('#homeStatusDetails').val().toString();

    $.ajax({
        url: "/Home/SetHomeStatus",
        data: { homeStatusString: homeStatusStatus, homeStatusDetailsString: homeStatusDetailsString },
        success: function (data) {
        },
        error: function (req, status, error) {
            alert(error);
        },
        complete: function (response) {
        }
    });

    $.ajax({
        url: "/Home/GetHomeStatus",
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