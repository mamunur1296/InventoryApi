﻿import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getCompanyList();
});
const getCompanyList = async () => {
    debugger
    const result = await SendRequest({ endpoint: '/Company/GetAll' });
    if (result.status === 200 && result.success) {
        await onSuccessUsers(result.data);
    }
}

const onSuccessUsers = async (companys) => {
    debugger
    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => `<img src="data:image/jpeg;base64,${row.logo}" alt="Photo" style="width:50px; height:50px;" onerror="this.onerror=null;this.src='/ProjectRootImg/defoltLogo.png';" />`
            },
            {
                render: (data, type, row) => row.name ?? "No Data"
            },
            {
                render: (data, type, row) => row.fullName ?? "No Data"
            },
            { 
                render: (data, type, row) => row.contactPerson ?? "No Data"
            },
            {
                render: (data, type, row) => row.address ?? "No Address"
            },

            {
                render: (data, type, row) => row.phoneNo ?? "No Phone"
            },
            {
                render: (data, type, row) => row.faxNo ?? "No Fax"
            },
            {
                render: (data, type, row) => row.emailNo ?? "No Email"
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateCompany' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteCompany' }
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
$('#CreateUserBtn').off('click').click(async () => {
    resetFormValidation('#CompanyForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#CompanyForm').valid()) {
            //const formData = $('#CompanyForm').serialize();
            const formData = new FormData($('#CompanyForm')[0]);
            const result = await SendRequest({ endpoint: '/Company/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Company Created successfully !", type: "success", title: "Success" });
                await getCompanyList(); // Update the user list
            }
        }
    } catch (error) {
        $('#modelCreate').modal('hide');
        notification({ message: " Company Created failed . Please try again. !", type: "error", title: "Error" });
    }
});



window.updateCompany = async (id) => {
    debugger
    clearMessage('successMessage', 'globalErrorMessage');
    resetFormValidation('#CompanyForm', UsrValidae);
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    const result = await SendRequest({ endpoint: '/Company/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#Name').val(result.data.name);
        $('#FullName').val(result.data.fullName);
        $('#ContactPerson').val(result.data.contactPerson);
        $('#Address').val(result.data.address);
        $('#PhoneNo').val(result.data.phoneNo);
        $('#FaxNo').val(result.data.faxNo);
        $('#EmailNo').val(result.data.emailNo);
        $('#IsActive').val(result.data.isActive);

        // Correctly set the checkbox state based on the isActive value
        $('#IsActiveCheckbox').prop('checked', result.data.isActive);
        
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#CompanyForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            //const formData = $('#CompanyForm').serialize();
            const formData = new FormData($('#CompanyForm')[0]);
            const result = await SendRequest({ endpoint: '/Company/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Company Updated successfully !", type: "success", title: "Success" });
                
                await getCompanyList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Company Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteCompany = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Company/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Company Deleted successfully !", type: "success", title: "Success" });
            await getCompanyList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}