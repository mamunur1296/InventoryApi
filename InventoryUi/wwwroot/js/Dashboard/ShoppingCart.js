import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getShoppingCartList();
});
const getShoppingCartList = async () => {
    debugger
    const shoppingCarts = await SendRequest({ endpoint: '/ShoppingCart/GetAll' });
    const customers = await SendRequest({ endpoint: '/Customer/GetAll' });
    if (shoppingCarts.status === 200 && shoppingCarts.success) {
        await onSuccessUsers(shoppingCarts.data, customers.data);
    }
}

const onSuccessUsers = async (shoppingCarts, customers) => {
    debugger
    const customersMap = dataToMap(customers, 'id');
    const shoppingCartsitem = shoppingCarts.map((shoppingCart) => {
        if (shoppingCart) {
            debugger
            const customer = customersMap[shoppingCart.customerID];
            return {
                id: shoppingCart?.id,
                customer: customer?.customerName ?? "No Name",
                
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.customer
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateShoppingCart' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteShoppingCart' }
                ])
            }
        ];
        if (shoppingCarts) {
            await initializeDataTable(shoppingCartsitem, userSchema, 'ShoppingCartTable');
        }
    } catch (error) {
        console.error('Error processing Shopping Cart data:', error);
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
const UsrValidae = $('#ShoppingCartForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        CustomerID: {
            required: true,
        }

    },
    messages: {
        CustomerID: {
            required: "Customer ID is required.",
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
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#ShoppingCartForm').valid()) {
            const formData = $('#ShoppingCartForm').serialize();
            const result = await SendRequest({ endpoint: '/ShoppingCart/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#ShoppingCartForm', modalId: '#modelCreate', message: ' Shopping Cart was successfully Created....' });
                await getShoppingCartList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#ShoppingCartForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Shopping Cart Create failed. Please try again.' });
    }
});



window.updateShoppingCart = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
    const result = await SendRequest({ endpoint: '/ShoppingCart/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#CustomerDropdown').val(result.data.customerID);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#ShoppingCartForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#ShoppingCartForm').serialize();
            const result = await SendRequest({ endpoint: '/ShoppingCart/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#ShoppingCartForm', modalId: '#modelCreate', message: ' Shopping Cart was successfully Updated....' });
                await getShoppingCartList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteShoppingCart = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/ShoppingCart/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#ShoppingCartForm', modalId: '#deleteAndDetailsModel', message: ' Shopping Cart was successfully Delete....' });
            await getShoppingCartList(); // Update the user list
        }
    });
}