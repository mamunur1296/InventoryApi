import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getDeliveryAddressList();
});
const getDeliveryAddressList = async () => {
    debugger
    const DeliveryAddress = await SendRequest({ endpoint: '/DeliveryAddress/GetAll' });
    const users = await SendRequest({ endpoint: '/DashboardUser/GetAll' });
    if (DeliveryAddress.status === 200 && DeliveryAddress.success) {
        await onSuccessUsers(DeliveryAddress.data, users.data);
    }
}

const onSuccessUsers = async (DeliveryAddresses, users) => {
    debugger
    const usersMap = dataToMap(users, 'id');
    const DeliveryAddressesitem = DeliveryAddresses.map((DeliveryAddress) => {
        if (DeliveryAddress) {
            debugger
            const user = usersMap[DeliveryAddress.userId];
            return {
                id: DeliveryAddress?.id,
                userName: user?.userName ?? "No Name",
                address: DeliveryAddress?.address ?? "No Address",
                phone: DeliveryAddress?.phone ?? "No Phone",
                mobile: DeliveryAddress?.mobile ?? "No Mobile",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.userName
            },
            {
                render: (data, type, row) => row?.address
            },
            {
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => row?.mobile
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateDeliveryAddress' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteDeliveryAddress' }
                ])
            }
        ];
        if (DeliveryAddresses) {
            await initializeDataTable(DeliveryAddressesitem, userSchema, 'DeliveryAddressTable');
        }
    } catch (error) {
        console.error('Error processing company data:', error);
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
const UsrValidae = $('#DeliveryAddressForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        UserId: {
            required: true,
        },
        Address: {
            required: true,
            maxlength: 255
        },
        Phone: {
            required: true,
            maxlength: 20
        },
        Mobile: {
            required: true,
            maxlength: 20
        },
        DeactivatedDate: {
            required: true,
            date: true
        }
    },
    messages: {
        UserId: {
            required: "User ID is required.",
        },
        Address: {
            required: "Address is required.",
            maxlength: "Address cannot be longer than 255 characters."
        },
        Phone: {
            required: "Phone number is required.",
            maxlength: "Phone number cannot be longer than 20 characters."
        },
        Mobile: {
            required: "Mobile number is required.",
            maxlength: "Mobile number cannot be longer than 20 characters."
        },
        DeactivatedDate: {
            required: "Deactivated date is required.",
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
    await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#DeliveryAddressForm').valid()) {
            const formData = $('#DeliveryAddressForm').serialize();
            const result = await SendRequest({ endpoint: '/DeliveryAddress/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#DeliveryAddressForm', modalId: '#modelCreate', message: ' Delivery Address was successfully Created....' });
                await getDeliveryAddressList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#DeliveryAddressForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Delivery Address Create failed. Please try again.' });
    }
});



window.updateDeliveryAddress = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    const result = await SendRequest({ endpoint: '/DeliveryAddress/GetById/' + id });

    if (result.success) {
        await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName',"select user");
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#UserDropdown').val(result.data.userId);
        $('#Address').val(result.data.address);
        $('#Phone').val(result.data.phone);
        $('#Mobile').val(result.data.mobile);
        $('#DeactivatedDate').val(result.data.deactivatedDate);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#DeliveryAddressForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#DeliveryAddressForm').serialize();
            const result = await SendRequest({ endpoint: '/DeliveryAddress/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#DeliveryAddressForm', modalId: '#modelCreate', message: ' Delivery Address was successfully Updated....' });
                await getDeliveryAddressList(); // Update the user list
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteDeliveryAddress = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/DeliveryAddress/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#DeliveryAddressForm', modalId: '#deleteAndDetailsModel', message: ' Delivery Address was successfully Delete....' });
            await getDeliveryAddressList(); // Update the user list
        }
    });
}