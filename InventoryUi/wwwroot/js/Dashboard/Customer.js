import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getCustomerList();
});
const getCustomerList = async () => {
    debugger
    const customers = await SendRequest({ endpoint: '/Customer/GetAll' });
    const users = await SendRequest({ endpoint: '/DashboardUser/GetAll' });
    if (customers.status === 200 && customers.success) {
        await onSuccessUsers(customers.data, users.data);
    }
}

const onSuccessUsers = async (customers, users) => {
    debugger
    const usersMap = dataToMap(users, 'id');
    const customersitem = customers.map((customer) => {
        if (customer) {
            debugger
            const user = usersMap[customer.userId];
            return {
                id: customer?.id,
                name: customer?.customerName ?? "Null",
                contactName: customer?.contactName ?? "Null",
                contactTitle: customer?.contactTitle ?? "Null",
                address: customer?.address ?? " " + customer?.city ?? " " + ", " + customer?.region ?? " " + ", " + customer?.postalCode ?? " " + ", " + customer?.country ?? " " ?? "Null",
                phone: customer?.phone ?? "Null",
                fax: customer?.fax ?? "Null",
                email: customer?.email ?? "Null",
                birthDate: customer?.dateOfBirth ?? "Null",
                medicalHistory: customer?.medicalHistory ?? "Null",
                user: user?.userName ?? "Null", 
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
                render: (data, type, row) => row?.user
            },
            {
                render: (data, type, row) => row?.address
            },
            {
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => row?.email
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
const UsrValidae = $('#CustomerForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        CustomerName: {
            required: true, 
        },
        UserName: {
            required: true,
            checkDuplicateUsername: true
        },
        Email: {
            required: true,
            checkDuplicateEmail: true
        },
        Password: {
            required: true,
            minlength: 6,
            pwcheck: true
        },
        Phone: {
            required: true, 
        }
    },
    messages: {
        CustomerName: {
            required: "Customer name is required.",
            maxlength: "Customer name cannot be longer than 255 characters."
        },
        UserName: {
            required: "User Name is required.",
            checkDuplicateUsername: "This username is already taken."
        },
        Email: {
            required: "Email is required.",
            checkDuplicateEmail: "This email is already registered."
        },
        Password: {
            required: "Password is required.",
            minlength: "Password must be at least 6 characters long.",
            pwcheck: "Password must contain at least one lowercase letter (a-z)."
        },
        Phone: {
            required: "Phone number is required.",
            maxlength: "Phone number cannot be longer than 255 characters."
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

$.validator.addMethod("pwcheck", function (value) {
    return /[a-z]/.test(value); // At least one lowercase letter
});
//Sow Create Model 
$('#CreateBtn').off('click').click(async () => {
    resetFormValidation('#CustomerForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
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
                $('#modelCreate').modal('hide');
                notification({ message: "Customer Created successfully !", type: "success", title: "Success" });
                await getCustomerList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error", time: 0 });
                $('#modelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Customer Created failed . Please try again. !", type: "error", title: "Error", time: 0 });
    }

});



window.updateCustomer = async (id) => {
    resetFormValidation('#CustomerForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
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
        $('#UserDropdown').val(result.data.userId);
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#CustomerForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#CustomerForm').serialize();
            const result = await SendRequest({ endpoint: '/Customer/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Customer Updated successfully !", type: "success", title: "Success" });

                await getCustomerList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Customer Updated failed . Please try again. !", type: "error", title: "Error", time: 0 });
            }

        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteCustomer = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Customer/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Customer Deleted successfully !", type: "success", title: "Success" });
            await getCustomerList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });
        }
    });
}