$("#setHomeStatusButton").click(function () {
    var homeStatusStatus = $('#homeStatusStatus').val().toString();
    var houseStatusDetailsString = $('#homeStatusDetails').val().toString();

    $.ajax({
        url: "/Home/SetHomeStatus",
        data: { houseStatusString: homeStatusStatus, houseStatusDetailsString: houseStatusDetailsString },
        success: function (data) {
        },
        error: function (req, status, error) {
            alert(error);
        },
        complete: function (response) {
            $.ajax({
                url: "/Home/GetHomeStatus",
                success: function (data) {
                    var dataArr = data.split(";");
                    var status = parseInt(dataArr[0]);
                    var username = dataArr[1];
                    var details = dataArr[2];

                    if (status === 1) {
                        $('#homeStatus').html('<span class="btn btn-circle btn-success btn-sm"><i class="fas fa-thumbs-up"></i></span>');
                    } else if (status === 2) {
                        $('#homeStatus').html('<span class="btn btn-circle btn-warning btn-sm"><i class="fas fa-exclamation"></i></span>');
                    } else {
                        $('#homeStatus').html('<span class="btn btn-circle btn-danger btn-sm"><i class="fas fa-ban"></i></span>');
                    }

                    if (details && details != "") {
                        $('#homeStatusDetailsNavItem').html(details);
                    }

                    $('#homeStatusUserSetNavItem').html(username);
                },
                error: function (req, status, error) {
                    alert(error);
                }
            });
        }
    });
});