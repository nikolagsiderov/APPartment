$(document).ready(function () {
    Dropzone.options.dropzoneForm = {
        maxFilesize: 209715200, // MB
        maxFiles: 3,
        acceptedFiles: "image/*",
        uploadMultiple: true,
        timeout: 180000,
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