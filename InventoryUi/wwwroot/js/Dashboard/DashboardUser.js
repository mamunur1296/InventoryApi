import { mackEmployee} from '../Utility/ObjectMaping.js';
import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';
import { isBranchValidae } from './Branch.js';
import { isCompnayValidae } from './company.js';


$(document).ready(async function () {
    await getUserList();
});
const getUserList = async () => {
    debugger
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
                render: (data, type, row) => `<img src="images/User/${row.userImg}" alt="User Avatar" class="rounded-circle" style="width: 50px; height: 50px; object-fit: cover;" onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';" />`
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
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteUser' },
                    { label: 'Add Employee', btnClass: 'btn-primary', callback: 'addEmployee', disabled: row.isApprovedByAdmin  },
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
        "User.FirstName": {
            required: true,
            minlength: 2,
            maxlength: 50
        },
        "User.LastName": {
            required: true,
            minlength: 2,
            maxlength: 50
        },
        "User.UserName": {
            required: true,
            checkDuplicateUsername: true
        },
        "User.Email": {
            required: true,
            checkDuplicateEmail: true
        },
        "User.PhoneNumber": {
            required: true
        },
        "User.Password": {
            required: true,
            minlength: 6
        },
        "User.ConfirmationPassword": {
            required: true,
            equalTo: "#Password"
        },
        "User.RoleName": {
            required: true
        },
        "User.CompanyId": {
            required: true
        },
        "User.BranchId": {
            required: true
        }
    },
    messages: {
        "User.FirstName": {
            required: "First Name is required.",
            minlength: "First Name must be between 2 and 50 characters.",
            maxlength: "First Name must be between 2 and 50 characters."
        },
        "User.LastName": {
            required: "Last Name is required.",
            minlength: "Last Name must be between 2 and 50 characters.",
            maxlength: "Last Name must be between 2 and 50 characters."
        },
        "User.UserName": {
            required: "User Name is required.",
            checkDuplicateUsername: "This username is already taken."
        },
        "User.Email": {
            required: "Email is required.",
            checkDuplicateEmail: "This email is already registered."
        },
        "User.PhoneNumber": {
            required: "Phone Number is required."
        },
        "User.Password": {
            required: "Password is required.",
            minlength: "Password must be at least 6 characters long."
        },
        "User.ConfirmationPassword": {
            required: "Confirmation Password is required.",
            equalTo: "Password and Confirmation Password do not match."
        },
        "User.RoleName": {
            required: "You must select a role."
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
    await populateDropdown('/DashboardRole/GetAll', '#RolesDropdown', 'roleName', 'roleName', " Select Role");
    await populateDropdown('/Branch/GetAll', '#BranchDropdown', 'id', 'name', "Select Branch");
    isEmployee();
    
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#UserForm').valid()) {
            const formData = $('#UserForm').serialize();
            //const formData = new FormData($('#UserForm')[0]);
            //const formData = new FormData(document.querySelector('#UserForm'));
            //for (const [key, value] of formData.entries()) {
            //    console.log(`${key}: ${value}`);
            //}
            const result = await SendRequest({ endpoint: '/DashboardUser/Register', method: 'POST', data: formData });
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
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " User Create failed . Please try again. !", type: "error", title: "Error" });
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Registration failed . Please try again. !", type: "error", title: "Error" });
    }
});

const isEmployee = (isEmp = null) => {
    // Check the initial value and show/hide the employee section accordingly
    if (isEmp) {
        $('#employeeSection').css('display', 'block');
    } else {
        $('#employeeSection').css('display', 'none');
    }
    debugger
    // Set up event listener for future changes
    $('#IsEmployeeCheckbox').off('change').on('change', function () {
        isEmp = $(this).is(':checked'); // Checking if the checkbox is checked
        if (isEmp) {
            $('#employeeSection').css('display', 'block'); // Show the section
        } else {
            $('#employeeSection').css('display', 'none'); // Hide the section
        }
    });
};


window.updateUser = async (id) => {
    resetFormValidation('#UserForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/DashboardRole/GetAll', '#RolesDropdown', 'roleName', 'roleName', null);
    await populateDropdown('/Branch/GetAll', '#BranchDropdown', 'id', 'name', "Select Branch");
    
    const result = await SendRequest({ endpoint: '/DashboardUser/GetById/' + id });
    debugger
    if (result) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#UserId').val(result.user.id);
        $('#FirstName').val(result.user.firstName);
        $('#LastName').val(result.user.lastName);
        $('#UserName').val(result.user.userName);
        $('#Email').val(result.user.email);
        $('#PhoneNumber').val(result.user.phoneNumber);
        $('#RolesDropdown').val(result.user.roles);
        $('#CompanyDropdown').val(result.user.companyId);
        $('#BranchDropdown').val(result.user.branchId);

        debugger
        const isChecked = result.user.isEmployee;
        $('#IsEmployeeCheckbox').prop('checked', isChecked);
        isEmployee(isChecked);
        loger(isChecked);
        $('#modelCreate').modal('show');
        debugger

        // Correctly set the checkbox state based on the isActive value
       // $('#IsEmployeeCheckbox').prop('checked', result.data.isActive);


        resetValidation(UsrValidae, '#UserForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#UserForm').serialize();
            const result = await SendRequest({ endpoint: '/DashboardUser/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "User Updated successfully !", type: "success", title: "Success" });
                await getUserList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " User Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    
}




window.showDetails = async (id) => {
    loger("showDetails id " + id);
}


window.addEmployee = async (id) => {
    loger(id);
    const result = await SendRequest({ endpoint: '/Employee/CreateEmployee/' + id });
    if (result.success) {
        notification({ message: result.detail, type: "success", title: "Success" });
        await getUserList(); // Reload the employees list on success
    } else {
        // Check if the error message contains the specific registration error
        if (result.detail.includes("An error occurred during Employee registration:")) {
            // Display a custom error message for an existing customer
            notification({ message: "This Employee is already registered!", type: "error", title: "Error" });
        } else {
            // Display the default error message from the response if it's not a known case
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    }
};

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
///////////////////////////////////////////////////////


// Initialize the modal for creating a new company
$('#CreateNewCompanyBtn').off('click').click(() => {
    showCreateModal('CompanyModelCreate', 'btnSaveCompany', 'btnUpdateCompany');
    isCompnayValidae();
});

// Save company button handler
$('#btnSaveCompany').off('click').click(async () => {
    await handleCompanyFormSubmit();
});

// Initialize the modal for creating a new branch
$('#btnNewBranch').off('click').click(async () => {
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'name', "Select Company");
    showCreateModal('BranchModelCreate', 'BranchBtnSave', 'BranchBtnUpdate');
    isBranchValidae();
});

// Save branch button handler
$('#BranchBtnSave').off('click').click(async () => {
    await handleBranchFormSubmit();
});

// Function to handle company form submission
async function handleCompanyFormSubmit() {
    clearMessage('successMessage', 'globalErrorMessage');

    try {
        if ($('#CompanyForm').valid()) {
            const formData = new FormData($('#CompanyForm')[0]);
            const result = await SendRequest({ endpoint: '/Company/Create', method: 'POST', data: formData });

            clearPreviousMessages();

            if (result.success && result.status === 201) {
                await reloadPartialView();
                $('#CompanyModelCreate').modal('hide');
                notification({ message: "Company created successfully!", type: "success", title: "Success" });
            }
        }
    } catch (error) {
        handleError('CompanyModelCreate', "Company creation failed. Please try again.");
    }
}

// Function to handle branch form submission
async function handleBranchFormSubmit() {
    clearMessage('successMessage', 'globalErrorMessage');

    try {
        if ($('#BranchForm').valid()) {
            const formData = $('#BranchForm').serialize();
            const result = await SendRequest({ endpoint: '/Branch/Create', method: 'POST', data: formData });

            clearPreviousMessages();

            if (result.success && result.status === 201) {
                await reloadPartialView();
                $('#BranchModelCreate').modal('hide');
                await populateDropdown('/Branch/GetAll', '#BranchDropdown', 'id', 'name', "Select Branch");
                notification({ message: "Branch created successfully!", type: "success", title: "Success" });
            }
        }
    } catch (error) {
        handleError('BranchModelCreate', "Branch creation failed. Please try again.");
    }
}

// Function to reload partial views and reinitialize JS
async function reloadPartialView() {
    $('#EmpPartial').load('/DashboardUser/GetaCompanyForEmpPartial', () => {
        reinitializeJsForPartial();
    });
}

// Reinitialize JavaScript event handlers for partial views
const reinitializeJsForPartial = () => {
    // Rebind the "New Branch" button
    $('#btnNewBranch').off('click').click(async () => {
        await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'name', "Select Company");
        showCreateModal('BranchModelCreate', 'BranchBtnSave', 'BranchBtnUpdate');
        isBranchValidae();
    });

    // Rebind the "Save Branch" button
    $('#BranchBtnSave').off('click').click(async () => {
        await handleBranchFormSubmit();
    });
}

// Utility function to clear previous messages
function clearPreviousMessages() {
    $('#successMessage').hide();
    $('#UserError').hide();
    $('#EmailError').hide();
    $('#PasswordError').hide();
    $('#GeneralError').hide();
}

// Utility function to handle errors
function handleError(modalId, errorMessage) {
    $(`#${modalId}`).modal('hide');
    notification({ message: errorMessage, type: "error", title: "Error" });
}
