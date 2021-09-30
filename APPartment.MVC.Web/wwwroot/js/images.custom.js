document.addEventListener("DOMContentLoaded", function () {
    Dropzone.options.imagesDropzone = {
        maxFilesize: 15, // MB
        acceptedFiles: "image/*",
        uploadMultiple: true,
        parallelUploads: 100,
        maxFiles: 100,
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

    Dropzone.options.imagesDropzone.init();
});