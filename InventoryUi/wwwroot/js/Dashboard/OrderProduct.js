import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
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
$('#CreateBtn').off('click').click(async () => {
    resetFormValidation('#OrderProductForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
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
                $('#modelCreate').modal('hide');
                notification({ message: "Order Product Created successfully !", type: "success", title: "Success" });
                await getOrderProductList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Order Product Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateOrderProduct = async (id) => {
    resetFormValidation('#OrderProductForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
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
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#OrderProductForm').serialize();
            const result = await SendRequest({ endpoint: '/OrderProduct/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Order Product Updated successfully !", type: "success", title: "Success" });

                await getOrderProductList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Order Product Updated failed . Please try again. !", type: "error", title: "Error" });
            }

        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteOrderProduct = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/OrderProduct/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Order Product  Deleted successfully !", type: "success", title: "Success" });
            await getOrderProductList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });

        }
    });
}