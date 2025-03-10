document.addEventListener("DOMContentLoaded", function () {
    var message = document.getElementById("tempDataSuccessMessage")?.value;
    if (message) {
        toastr.success(message);
    }
});
