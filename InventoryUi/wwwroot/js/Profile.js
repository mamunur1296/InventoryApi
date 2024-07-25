import { ajaxCall } from './ajaxutility/ajaxutility.js';

$(document).ready(function () {
    $("#submitBtn").click(async function (event) {
        event.preventDefault();
        debugger;
        const oldPassword = $("#OldPassword").val();
        const newPassword = $("#NewPassword").val();
        const confirmPassword = $("#ConfirmPassword").val();
        const userID = $("#userID").val();

        const data = {
            oldPassword: oldPassword,
            newPassword: newPassword,
            confirmPassword: confirmPassword,
            userID: userID
        };

        // Clear previous validation messages
        $("#newPasswordError").text("");
        $("#confirmPasswordError").text("");
        $("#oldPasswordError").text("");

        // Client-side validation
        if (newPassword !== confirmPassword) {
            $("#confirmPasswordError").text("New password and confirm password do not match.");
            return; // Stop execution if validation fails
        }

        try {
            const response = await ajaxCall({ endpoint: '/User/PasswordChange', method: 'POST', data: data,contentType: 'application/x-www-form-urlencoded'
            });

            if (response.success) {
                // Redirect to login page
                window.location.href = response.redirectUrl;
            } else if (response.detail === 'Incorrect password.') {
                $("#oldPasswordError").text("Incorrect old password.");
            } else if (response.detail.includes("least 6 characters")) {
                $("#newPasswordError").text("Password must be at least 6 characters.");
            } else {
                // Handle other errors
                console.log(response);
                // Optionally show a general error message
                $("#generalError").text("An error occurred while changing the password. Please try again.");
            }
        } catch (error) {
            // Handle the error
            console.log(error);
            $("#generalError").text("An unexpected error occurred. Please try again later.");
        }
    });
});


