import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getUserList();
});
const getUserList = async () => {
    const users = await SendRequest({ endpoint: '/DashboardUser/GetAll' });
    if (users.status === 200 && users.success) {
        await onSuccessUsers(users.data);
    } 
}

const onSuccessUsers = async (users) => {
    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row.userImg ? `<img src="images/${row.userImg}" alt="User Image" class="rounded-circle" style="width: 50px; height: 50px; object-fit: cover;" />` : `<img src="/ProjectRootImg/default-user.png" alt="User Image" class="rounded-circle" style="width: 50px; height: 50px; object-fit: cover;" />`
            },
            {
                render: (data, type, row) => `${row.firstName} ${row.lastName}`
            },
            {
                render: (data, type, row) => row.userName
            },
            {
                render: (data, type, row) => row.email
            },
            {
                render: (data, type, row) => row.phoneNumber
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateUser'},
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteUser' }
                ])
            }
        ];
        await initializeDataTable(users, userSchema, 'UsersTable');
    } catch (error) {
        console.error('Error processing company data:', error);
    }
}

// Fatch duplucate file 

const createDuplicateCheckValidator = (endpoint, key, errorMessage) => {
    return function (value, element) {
        let isValid = false;
        $.ajax({
            type: "GET",
            url: endpoint,
            data: { key: key, val: value },
            async: false,
            success: function (response) {
                isValid = !response;
            },
            error: function () {
                isValid = false;
            }
        });
        return isValid;
    };
}

$.validator.addMethod("checkDuplicateUsername", createDuplicateCheckValidator(
    "/DashboardUser/CheckDuplicate",
    "UserName",
    "Username already exists."
));

$.validator.addMethod("checkDuplicateEmail", createDuplicateCheckValidator(
    "/DashboardUser/CheckDuplicate",
    "Email",
    "Email already exists."
));




// Initialize validation
const UsrValidae = $('#UserForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        FirstName: {
            required: true,
            minlength: 2,
            maxlength: 50
        },
        LastName: {
            required: true,
            minlength: 2,
            maxlength: 50
        },
        UserName: {
            required: true,
            checkDuplicateUsername: true
        },
        Email: {
            required: true,
            checkDuplicateEmail: true
        },
        PhoneNumber: {
            required: true
        },
        Password: {
            required: true,
            minlength: 6
        },
        ConfirmationPassword: {
            required: true,
            equalTo: "#Password"
        },
        RoleName: {
            required: true
        }
    },
    messages: {
        FirstName: {
            required: " first Name is required.",
            minlength: "first Name must be between 2 and 50 characters.",
            maxlength: "first Name must be between 2 and 50 characters."
        },
        LastName: {
            required: "Lastst Name  is required.",
            minlength: "Lastst Name  must be between 2 and 50 characters.",
            maxlength: "Lastst Name  must be between 2 and 50 characters."
        },
        UserName: {
            required: "User Name  is required.",
            minlength: "User Name   must be between 2 and 50 characters.",
            maxlength: "User Name   must be between 2 and 50 characters.",
            checkDuplicateUsername: "This username is already taken."
        },
        Email: {
            required: "Email is required.",
            checkDuplicateEmail: "This email is already registered."

        },
        PhoneNumber: {
            required: "Phone Number is required."
        }
        ,
        Password: {
            required: "Password is required.",
            minlength: "Password must be at least 6 characters long."
        }
        ,
        ConfirmationPassword: {
            required: "Confirmation Password is required.",
            equalTo: "Password and Confirmation Password do not match."
        },
        RoleName: {
            required: "Must be select Roles "
        }
    },
    errorElement: 'div',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    }
});

//Sow Create Model 
$('#CreateUserBtn').off('click').click(async () => {
    resetFormValidation('#UserForm', UsrValidae);
     clearMessage('successMessage', 'globalErrorMessage');
     showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
     await populateDropdown('/DashboardRole/GetAll', '#RolesDropdown', 'roleName', 'roleName'," Select Role");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#UserForm').valid()) {
            const formData = $('#UserForm').serialize();
            const result = await SendRequest({ endpoint: '/DashboardUser/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                //displayNotification({ formId: '#UserForm', modalId: '#modelCreate', message: ' User was successfully Created....' });
                $('#modelCreate').modal('hide');
                notification({ message: "User Created successfully !", type: "success", title: "Success" });
                await getUserList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Registration failed . Please try again. !", type: "error", title: "Error" });
    }
});



window.updateUser = async (id) => {
    resetFormValidation('#UserForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/DashboardRole/GetAll', '#RolesDropdown', 'roleName', 'roleName', null);
    const result = await SendRequest({ endpoint: '/DashboardUser/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#FirstName').val(result.data.firstName);
        $('#LastName').val(result.data.lastName);
        $('#UserName').val(result.data.userName);
        $('#Email').val(result.data.email);
        $('#PhoneNumber').val(result.data.phoneNumber);
        $('#RolesDropdown').val(result.data.roles);
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#UserForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#UserForm').serialize();
            const result = await SendRequest({ endpoint: '/DashboardUser/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "User Updated successfully !", type: "success", title: "Success" });
                await getUserList(); // Update the user list
            }
        });
    }
    
}




window.showDetails = async (id) => {
    loger("showDetails id " + id);
}


window.deleteUser = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        const result = await SendRequest({ endpoint: '/DashboardUser/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "User Deleted successfully !", type: "success", title: "Success" });
            await getUserList(); // Update the category list
        } else {
            // Display the error message in the modal
            $('#DeleteErrorMessage').removeClass('alert-success').addClass('text-danger').text(result.detail).show();
        }
    });
}