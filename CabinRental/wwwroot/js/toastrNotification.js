document.addEventListener("DOMContentLoaded", function () {
    let message = document.getElementById("tempDataSuccessMessage")?.value;
    let errorMessage = document.getElementById("tempDataFailureMessage")?.value;
    
    if (message) {
        toastr.success(message);
    }
    if (errorMessage) {
        toastr.error(errorMessage);
    }
    
});
