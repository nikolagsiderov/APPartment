$(document).ready(function () {
    $('.breadcrumb-item > a').addClass('no-underline');

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
        url: "/Inventory/GetCount",
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
        url: "/SuppliedInventory/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#suppliedInventoryBadge").attr("style", "display:none");
            } else {
                $("#suppliedInventoryBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/NotSuppliedInventory/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#notSuppliedInventoryBadge").attr("style", "display:none");
            } else {
                $("#notSuppliedInventoryBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Hygiene/GetCount",
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
        url: "/DoneHygiene/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#doneHygieneBadge").attr("style", "display:none");
            } else {
                $("#doneHygieneBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/NotDoneHygiene/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#notDoneHygieneBadge").attr("style", "display:none");
            } else {
                $("#notDoneHygieneBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Issues/GetCount",
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
        url: "/ClosedIssues/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#closedIssuesBadge").attr("style", "display:none");
            } else {
                $("#closedIssuesBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/NotClosedIssues/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#notClosedIssuesBadge").attr("style", "display:none");
            } else {
                $("#notClosedIssuesBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Surveys/GetCount",
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
        url: "/CompletedSurveys/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#completedSurveysBadge").attr("style", "display:none");
            } else {
                $("#completedSurveysBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/NotCompletedSurveys/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#notCompletedSurveysBadge").attr("style", "display:none");
            } else {
                $("#notCompletedSurveysBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/Chores/GetCount",
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

    $.ajax({
        url: "/DoneChores/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#doneChoresBadge").attr("style", "display:none");
            } else {
                $("#doneChoresBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: "/NotDoneChores/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#notDoneChoresBadge").attr("style", "display:none");
            } else {
                $("#notDoneChoresBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});