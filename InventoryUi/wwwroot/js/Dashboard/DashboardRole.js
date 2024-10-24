﻿import { createActionButtons, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getRoleList();
});
const getRoleList = async () => {
    const roles = await SendRequest({ endpoint: '/DashboardRole/GetAll' });
    if (roles.status === 200 && roles.success) {
        await onSuccessUsers(roles.data);
    }
}

const onSuccessUsers = async (roles) => {
    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row.roleName
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateUser' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteRole' }
                ])
            }
        ];
        await initializeDataTable(roles, userSchema, 'RoleTable');
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

$.validator.addMethod("checkDuplicateRoleName", createDuplicateCheckValidator(
    "/DashboardRole/CheckDuplicate",
    "RoleName",
    "RoleName already exists."
));





// Initialize validation
const UsrValidae = $('#RolesForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        RoleName: {
            required: true,
            checkDuplicateRoleName: true
            
        }
    },
    messages: {
        RoleName: {
            required: " first Name is required.",
            checkDuplicateRoleName:"This Role Name is already taken."
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
$('#CreateUserBtn').click(async () => {
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#RolesForm').valid()) {
            const formData = $('#RolesForm').serialize();
            const result = await SendRequest({ endpoint: '/DashboardRole/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#RolesForm', modalId: '#modelCreate', message: ' Role was successfully Created....' });
                await getRoleList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#RolesForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Role Name Create failed. Please try again.' });
    }
});



//window.updateUser = async (id) => {
//    debugger
//    $('#myModalLabelUpdateEmployee').show();
//    $('#myModalLabelAddEmployee').hide();
//    await populateDropdown('/DashboardRole/GetAll', '#RolesDropdown', 'roleName', 'roleName', null);
//    const result = await SendRequest({ endpoint: '/DashboardUser/GetById/' + id });
//    if (result.success) {
//        $('#btnSave').hide();
//        $('#btnUpdate').show();
//        $('#FirstName').val(result.data.firstName);
//        $('#LastName').val(result.data.lastName);
//        $('#UserName').val(result.data.userName);
//        $('#Email').val(result.data.email);
//        $('#PhoneNumber').val(result.data.phoneNumber);
//        $('#RolesDropdown').val(result.data.roles);
//        $('#modelCreate').modal('show');
//        resetValidation(UsrValidae, '#UserForm');
//        $('#btnUpdate').on('click', async () => {
//            debugger
//            const formData = $('#UserForm').serialize();
//            const result = await SendRequest({ endpoint: '/DashboardUser/Update/' + id, method: "PUT", data: formData });
//            if (result.success) {
//                displayNotification({ formId: '#UserForm', modalId: '#modelCreate', message: ' User was successfully Updated....' });
//                await getUserList(); // Update the user list
//            }
//        });
//    }
//    loger(result);
//}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteRole = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/DashboardRole/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#RolesForm', modalId: '#deleteAndDetailsModel', message: ' Role was successfully Delete....' });
            await getRoleList(); // Update the user list
        }
    });
}