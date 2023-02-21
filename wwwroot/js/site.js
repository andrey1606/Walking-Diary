$(function () {
    $(".photo-button").on("click", function () {
        var walkId = $(this).data("id");
        $.get("/Home/GetPhotos", { walkId: walkId }).done(function (result) {
            $("#walk-modal .modal-body").html(result);
        });
    });
});
