document.addEventListener("DOMContentLoaded", function () {
    var photoButtons = document.querySelectorAll(".photo-button");

    photoButtons.forEach(function (photoButton) {
        photoButton.addEventListener("click", function () {
            var walkId = this.getAttribute("data-id");
            var xhr = new XMLHttpRequest();
            xhr.open("GET", "/Home/GetPhotos?walkId=" + walkId);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    document.querySelector(
                        "#walk-modal .modal-body"
                    ).innerHTML = xhr.responseText;
                }
            };
            xhr.send();
        });
    });
});
