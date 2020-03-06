$(document).ready(function () {
    Dropzone.options.dropzoneForm = {
        maxFilesize: 5, // MB
        maxFiles: 3,
        acceptedFiles: ".png,.jpg,.gif,.bmp,.jpeg",
        uploadMultiple: true,
        init: function () {
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    $.get(location.href).then(function (page) {
                        $("#images-main-div").html($(page).find("#images-main-div").html())
                    })
                }
                this.removeFile(file);
            });
        }
    };
});