﻿import { createActionButtons, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getCompanyList();
});
const getCompanyList = async () => {
    const result = await SendRequest({ endpoint: '/Company/GetAll' });
    if (result.status === 200 && result.success) {
        await onSuccessUsers(result.data);
    }
}

const onSuccessUsers = async (companys) => {
    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row.name
            },
            {
                render: (data, type, row) => row.fullName 
            },
            {
                render: (data, type, row) => row.contactPerson 
            },
            {
                render: (data, type, row) => row.address
            },
            {
                render: (data, type, row) => row.phoneNo  
            },
            {
                render: (data, type, row) => row.faxNo 
            },
            {
                render: (data, type, row) => row.emailNo 
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateUser', disabled: true },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteRole' }
                ])
            }
        ];
        await initializeDataTable(companys, userSchema, 'CompanyTable');
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

$.validator.addMethod("checkDuplicateCompanyName", createDuplicateCheckValidator(
    "/Company/CheckDuplicate",
    "Name",
    "Company Name already exists."
));
$.validator.addMethod("checkDuplicateCompanyFullName", createDuplicateCheckValidator(
    "/Company/CheckDuplicate",
    "FullName",
    "Company Full Name already exists."
));





// Initialize validation
const UsrValidae = $('#CompanyForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        Name: {
            required: true,
            checkDuplicateCompanyName: true
        },
        FullName: {
            required: true,
            checkDuplicateCompanyFullName: true
        },
        Address: {
            required: true,
            
        },
        PhoneNo: {
            required: true,
            
        },
        EmailNo: {
            required: true,
        }
       
    },
    messages: {
        Name: {
            required: " Company Name is required.",
            checkDuplicateCompanyName: "This Company Name is already taken."
        },
        FullName: {
            required: " Company Full Name is required.",
            checkDuplicateCompanyFullName: "This Company Full Name is already taken."
        },
        Address: {
            required: " Company Address is required.",

        },
        PhoneNo: {
            required: " Company Phone No is required.",

        },
        EmailNo: {
            required: " Company Email Address is required.",
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
        if ($('#CompanyForm').valid()) {
            const formData = $('#CompanyForm').serialize();
            const result = await SendRequest({ endpoint: '/Company/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#CompanyForm', modalId: '#modelCreate', message: ' Company was successfully Created....' });
                await getCompanyList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#CompanyForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Company Create failed. Please try again.' });
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
        const result = await SendRequest({ endpoint: '/Company/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#CompanyForm', modalId: '#deleteAndDetailsModel', message: ' Company was successfully Delete....' });
            await getCompanyList(); // Update the user list
        }
    });
}