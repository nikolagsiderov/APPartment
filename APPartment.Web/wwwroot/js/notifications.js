function GetNotificationsCount() {
    $.ajax({
        url: baseApplicationPath + "Notifications/GetCount",
        success: function (data) {
            if (data === 0) {
                $("#notificationsCount").html(data);
            } else {
                $("#notificationsCount").html(data);
            }
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
}

var notificationsDropdown = document.getElementById('navbarDropdownNotifications');
notificationsDropdown.addEventListener('click', GetNotificationsContent);

function GetNotificationsContent() {
    var isDropdownExpanded = $('#navbarDropdownNotifications').attr('aria-expanded');

    if (isDropdownExpanded === 'false') {
        $.ajax({
            url: baseApplicationPath + "Notifications/GetContents",
            success: function (data) {
                $("#notifications-contents").html(data);
            },
            error: function (req, status, error) {
                alert(error);
            }
        });
    }
}