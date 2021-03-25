$(document).ready(function () {
    $('.breadcrumb-item > a').addClass('no-underline');

    $.ajax({
        url: baseApplicationPath + "Home/GetHomeStatus",
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
        url: baseApplicationPath + "Inventory/Inventory/GetCount",
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
        url: baseApplicationPath + "Inventory/Supplied/GetCount",
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
        url: baseApplicationPath + "Inventory/Unsupplied/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#unsuppliedInventoryBadge").attr("style", "display:none");
            } else {
                $("#unsuppliedInventoryBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: baseApplicationPath + "Hygiene/Hygiene/GetCount",
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
        url: baseApplicationPath + "Hygiene/Done/GetCount",
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
        url: baseApplicationPath + "Hygiene/InProgress/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#inprogressHygieneBadge").attr("style", "display:none");
            } else {
                $("#inprogressHygieneBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: baseApplicationPath + "Issues/Issues/GetCount",
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
        url: baseApplicationPath + "Issues/Closed/GetCount",
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
        url: baseApplicationPath + "Issues/Open/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#openIssuesBadge").attr("style", "display:none");
            } else {
                $("#openIssuesBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: baseApplicationPath + "Surveys/Surveys/GetCount",
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
        url: baseApplicationPath + "Surveys/Completed/GetCount",
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
        url: baseApplicationPath + "Surveys/Pending/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#pendingSurveysBadge").attr("style", "display:none");
            } else {
                $("#pendingSurveysBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: baseApplicationPath + "Chores/Chores/GetCount",
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
        url: baseApplicationPath + "Chores/Mine/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#mineChoresBadge").attr("style", "display:none");
            } else {
                $("#mineChoresBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    $.ajax({
        url: baseApplicationPath + "Chores/Others/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#othersChoresBadge").attr("style", "display:none");
            } else {
                $("#othersChoresBadge").attr("style", "").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});