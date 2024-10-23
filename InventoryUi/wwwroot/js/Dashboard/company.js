import { notification } from '../Utility/notification.js';
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
                render: (data, type, row) => `<img src="data:image/jpeg;base64,${row.logo}" alt="Photo" style="width:30px; height:30px;" onerror="this.onerror=null;this.src='/ProjectRootImg/defoltLogo.png';" />`
            },
            {
                render: (data, type, row) => row.name ?? "N/A"
            },
            {
                render: (data, type, row) => row.address ?? "N/A"
            },

            {
                render: (data, type, row) => row.phoneNo ?? "N/A"
            },
            {
                render: (data, type, row) => row.faxNo ?? "N/A"
            },
            {
                render: (data, type, row) => row.emailNo ?? "N/A"
            },
            {
                render: (data, type, row) => row.contactPerson ?? "N/A"
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
export const isCompnayValidae = $('#CompanyForm').validate({
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
$('#CreateCompanyBtn').off('click').click(async () => {
    resetFormValidation('#CompanyForm', isCompnayValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    showCreateModal('CompanyModelCreate', 'btnSaveCompany', 'btnUpdateCompany');
});

// Save Button
$('#btnSaveCompany').off('click').click(async () => {
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
                $('#CompanyModelCreate').modal('hide');
                notification({ message: "Company Created successfully !", type: "success", title: "Success" });
                await getCompanyList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error", time: 0 });
                $('#CompanyModelCreate').modal('hide');
            }
        }
    } catch (error) {
        $('#CompanyModelCreate').modal('hide');
        notification({ message: " Company Created failed . Please try again. !", type: "error", title: "Error", time: 0 });
    }
});



window.updateCompany = async (id) => {
    debugger
    clearMessage('successMessage', 'globalErrorMessage');
    resetFormValidation('#CompanyForm', isCompnayValidae);
    $('#myModalLabelUpdateCompany').show();
    $('#myModalLabelAddCompany').hide();
    const result = await SendRequest({ endpoint: '/Company/GetById/' + id });
    if (result.success) {
        $('#btnSaveCompany').hide();
        $('#btnUpdateCompany').show();

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
        
        $('#CompanyModelCreate').modal('show');
        resetValidation(isCompnayValidae, '#CompanyForm');
        $('#btnUpdateCompany').off('click').on('click', async () => {
            debugger
            //const formData = $('#CompanyForm').serialize();
            const formData = new FormData($('#CompanyForm')[0]);
            const result = await SendRequest({ endpoint: '/Company/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#CompanyModelCreate').modal('hide');
                notification({ message: "Company Updated successfully !", type: "success", title: "Success" });
                
                await getCompanyList(); // Update the user list
            } else {
                $('#CompanyModelCreate').modal('hide');
                notification({ message: " Company Updated failed . Please try again. !", type: "error", title: "Error", time: 0 });
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
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });
        }
    });
}