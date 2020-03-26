$(document).ready(function () {
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

    $.ajax({
        url: "/Inventory/GetInventoryCriticalCount",
        success: function (data) {
            if (data === 0) {
                $("#inventoryBadge").attr("style", "display:none");
            } else {
                $("#inventoryBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Hygiene/GetHygieneCriticalCount",
        success: function (data) {
            if (data === 0) {
                $("#hygieneBadge").attr("style", "display:none");
            } else {
                $("#hygieneBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Issues/GetIssuesCriticalCount",
        success: function (data) {
            if (data === 0) {
                $("#issuesBadge").attr("style", "display:none");
            } else {
                $("#issuesBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Surveys/GetPendingSurveysCount",
        success: function (data) {
            if (data === 0) {
                $("#surveysBadge").attr("style", "display:none");
            } else {
                $("#surveysBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Chores/GetMyChoresCount",
        success: function (data) {
            if (data === 0) {
                $("#choresBadge").attr("style", "display:none");
            } else {
                $("#choresBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});