$('button[name="Assign"]').on("click", function () {
    var url = $(this).attr('href');

    $.ajax({
        url: url,
        success: function (data) {
            bootbox.dialog({
                message: data,
                size: 'large',
                centerVertical: true
            })
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});

$('button[name="CreateQuestion"]').on("click", function () {
    var url = $(this).attr('href');

    $.ajax({
        url: url,
        success: function (data) {
            bootbox.dialog({
                message: data,
                size: 'large',
                centerVertical: true
            })
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});

$('button[name="Questions"]').on("click", function () {
    var url = $(this).attr('href');

    $.ajax({
        url: url,
        success: function (data) {
            bootbox.dialog({
                message: data,
                size: 'large',
                centerVertical: true
            })
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});