$(document).ready(function () {
    $('#commentButton').prop('disabled', true);
    $('#commentButton').html('<i class="fas fa-comment-slash"></i> &nbsp; Comment');
    $('textarea[type="text"]').keyup(function () {
        if ($(this).val() != '') {
            $('#commentButton').prop('disabled', false);
            $('#commentButton').html('<i class="far fa-comment"></i> &nbsp; Comment');
        }
    });
});