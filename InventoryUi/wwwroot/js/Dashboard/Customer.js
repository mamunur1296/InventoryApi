import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getCustomerList();
});
const getCustomerList = async () => {
    debugger
    const customers = await SendRequest({ endpoint: '/Customer/GetAll' });
    // const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (customers.status === 200 && customers.success) {
        await onSuccessUsers(customers.data);
    }
}

const onSuccessUsers = async (customers) => {
    debugger
    //const companyMap = dataToMap(companys, 'id');
    const customersitem = customers.map((customer) => {
        if (customer) {
            debugger
            //const company = companyMap[warehouse.companyId];
            return {
                id: customer?.id,
                name: customer?.customerName ?? "No Name",
                contactName: customer?.contactName ?? "No Name",
                contactTitle: customer?.contactTitle ?? "No Title",
                address: customer?.address + customer?.City + customer?.region + customer?.postalCode + customer?.country  ?? "No Address",
                phone: customer?.phone ?? "No Phone",
                fax: customer?.fax ?? "No Fax",
                email: customer?.email ?? "No Email",
                birthDate: customer?.dateOfBirth ?? "No Birth Date",
                medicalHistory: customer?.medicalHistory ?? "No Medical History",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.name
            },
            {
                render: (data, type, row) => row?.contactName
            },
            {
                render: (data, type, row) => row?.contactTitle
            },
            {
                render: (data, type, row) => row?.address
            },
            {
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => row?.fax
            },
            {
                render: (data, type, row) => row?.email
            },
            {
                render: (data, type, row) => row?.birthDate
            },
            {
                render: (data, type, row) => row?.medicalHistory
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateCustomer' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showCustomer', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteCustomer' }
                ])
            }
        ];
        if (customers) {
            await initializeDataTable(customersitem, userSchema, 'CustomerTable');
        }
    } catch (error) {
        console.error('Error processing Customer data:', error);
    }
}

//// Fatch duplucate file 

//const createDuplicateCheckValidator = (endpoint, key, errorMessage) => {
//    return function (value, element) {
//        let isValid = false;
//        $.ajax({
//            type: "GET",
//            url: endpoint,
//            data: { key: key, val: value },
//            async: false,
//            success: function (response) {
//                isValid = !response;
//            },
//            error: function () {
//                isValid = false;
//            }
//        });
//        return isValid;
//    };
//}

//$.validator.addMethod("checkDuplicateWarehouseName", createDuplicateCheckValidator(
//    "/Warehouse/CheckDuplicate",
//    "WarehouseName"
//));






// Initialize validation
const UsrValidae = $('#CustomerForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        CustomerName: {
            required: true,
            maxlength: 255
        },
        ContactName: {
            maxlength: 255
        },
        ContactTitle: {
            maxlength: 255
        },
        Address: {
            maxlength: 255
        },
        City: {
            maxlength: 255
        },
        Region: {
            maxlength: 255
        },
        PostalCode: {
            maxlength: 255
        },
        Country: {
            maxlength: 255
        },
        Phone: {
            maxlength: 255
        },
        Fax: {
            maxlength: 255
        },
        Email: {
            required: true,
            email: true,
            maxlength: 255
        },
        PasswordHash: {
            required: true,
            maxlength: 255
        },
        MedicalHistory: {
            required: true
        },
        DateOfBirth: {
            required: true,
            date: true
        }
    },
    messages: {
        CustomerName: {
            required: "Customer name is required.",
            maxlength: "Customer name cannot be longer than 255 characters."
        },
        ContactName: {
            maxlength: "Contact name cannot be longer than 255 characters."
        },
        ContactTitle: {
            maxlength: "Contact title cannot be longer than 255 characters."
        },
        Address: {
            maxlength: "Address cannot be longer than 255 characters."
        },
        City: {
            maxlength: "City cannot be longer than 255 characters."
        },
        Region: {
            maxlength: "Region cannot be longer than 255 characters."
        },
        PostalCode: {
            maxlength: "Postal code cannot be longer than 255 characters."
        },
        Country: {
            maxlength: "Country cannot be longer than 255 characters."
        },
        Phone: {
            maxlength: "Phone number cannot be longer than 255 characters."
        },
        Fax: {
            maxlength: "Fax number cannot be longer than 255 characters.",
            required: true
        },
        Email: {
            required: "Email is required.",
            email: "Invalid email format.",
            maxlength: "Email cannot be longer than 255 characters."
        },
        PasswordHash: {
            required: "Password is required.",
            maxlength: "Password cannot be longer than 255 characters."
        },
        MedicalHistory: {
            required: "Medical history is required."
        },
        DateOfBirth: {
            required: "Birth date is required.",
            date: "Please enter a valid date."
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
$('#CreateBtn').click(async () => {
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');

});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#CustomerForm').valid()) {
            const formData = $('#CustomerForm').serialize();
            const result = await SendRequest({ endpoint: '/Customer/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#CustomerForm', modalId: '#modelCreate', message: ' Customer was successfully Created....' });
                await getCustomerList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#CustomerForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Customer Create failed. Please try again.' });
    }
});



window.updateCustomer = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    const result = await SendRequest({ endpoint: '/Customer/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#CustomerName').val(result.data.customerName);
        $('#ContactName').val(result.data.contactName);
        $('#ContactTitle').val(result.data.contactTitle);
        $('#Address').val(result.data.address);
        $('#City').val(result.data.city);
        $('#Region').val(result.data.region);
        $('#PostalCode').val(result.data.postalCode);
        $('#Country').val(result.data.country);
        $('#Phone').val(result.data.phone);
        $('#Fax').val(result.data.fax);
        $('#Email').val(result.data.email);
        $('#PasswordHash').val(result.data.passwordHash);
        $('#DateOfBirth').val(result.data.dateOfBirth);
        $('#MedicalHistory').val(result.data.medicalHistory);
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#CustomerForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#CustomerForm').serialize();
            const result = await SendRequest({ endpoint: '/Customer/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#CustomerForm', modalId: '#modelCreate', message: ' Customer was successfully Updated....' });
                await getCustomerList(); // Update the user list
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteCustomer = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Customer/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#CustomerForm', modalId: '#deleteAndDetailsModel', message: ' Customer was successfully Delete....' });
            await getCustomerList(); // Update the user list
        }
    });
}