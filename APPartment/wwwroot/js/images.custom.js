$(document).ready(function () {
    Dropzone.options.dropzoneForm = {
        maxFilesize: 5, // MB
        maxFiles: 3,
        acceptedFiles: ".png,.jpg,.gif,.bmp,.jpeg",
        uploadMultiple: true,
        init: function () {
            this.on("complete", function (data) {
                //var res = eval('(' + data.xhr.responseText + ')');
                var res = JSON.parse(data.xhr.responseText);
            });
        }
    };
});