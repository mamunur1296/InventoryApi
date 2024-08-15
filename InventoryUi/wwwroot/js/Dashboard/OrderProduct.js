import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getOrderProductList();
});
const getOrderProductList = async () => {
    debugger
    const orderProducts = await SendRequest({ endpoint: '/OrderProduct/GetAll' });
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    if (orderProducts.status === 200 && orderProducts.success) {
        await onSuccessUsers(orderProducts.data, products.data);
    }
}

const onSuccessUsers = async (orderProducts, products) => {
    debugger
    const productsMap = dataToMap(products, 'id');
    const orderProductsitem = orderProducts.map((orderProduct) => {
        if (orderProduct) {
            debugger
            const product = productsMap[orderProduct.productId];
            return {
                id: orderProduct?.id,
                product: product?.productName ?? "No Name",
               
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.product
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateOrderProduct' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteOrderProduct' }
                ])
            }
        ];
        if (orderProducts) {
            await initializeDataTable(orderProductsitem, userSchema, 'OrderProductTable');
        }
    } catch (error) {
        console.error('Error processing Order Product data:', error);
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
const UsrValidae = $('#OrderProductForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        ProductId: {
            required: true,
          
        }

    },
    messages: {
        ProductId: {
            required: "Product  is required.",
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
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#OrderProductForm').valid()) {
            const formData = $('#OrderProductForm').serialize();
            const result = await SendRequest({ endpoint: '/OrderProduct/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#OrderProductForm', modalId: '#modelCreate', message: ' Order Product was successfully Created....' });
                await getOrderProductList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#OrderProductForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Order Product Create failed. Please try again.' });
    }
});



window.updateOrderProduct = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");
    const result = await SendRequest({ endpoint: '/OrderProduct/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#ProductDropdown').val(result.data.productId);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#OrderProductForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#OrderProductForm').serialize();
            const result = await SendRequest({ endpoint: '/OrderProduct/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#OrderProductForm', modalId: '#modelCreate', message: ' Order Product was successfully Updated....' });
                await getOrderProductList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteOrderProduct = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/OrderProduct/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({
                formId: '#OrderProductForm',
                modalId: '#deleteAndDetailsModel',
                message: 'Order Product was successfully deleted....'
            });
            await getOrderProductList(); // Update the category list
        } else {
            // Display the error message in the modal
            $('#DeleteErrorMessage').removeClass('alert-success').addClass('text-danger').text(result.detail).show();
        }
    });
}