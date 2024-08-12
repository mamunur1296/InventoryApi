import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getOrderDetailList();
});
const getOrderDetailList = async () => {
    debugger
    const orderDetails = await SendRequest({ endpoint: '/OrderDetail/GetAll' });
    const orders = await SendRequest({ endpoint: '/Order/GetAll' });
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    if (orderDetails.status === 200 && orderDetails.success) {
        await onSuccessUsers(orderDetails.data, orders.data, products.data);
    }
}

const onSuccessUsers = async (orderDetails, orders, products) => {
    debugger
    const ordersMap = dataToMap(orders, 'id');
    const productsMap = dataToMap(products, 'id');
    const orderDetailsitem = orderDetails.map((orderDetail) => {
        if (orderDetail) {
            debugger
            const order = ordersMap[orderDetail.orderID];
            const product = productsMap[orderDetail.productID];
            return {
                id: orderDetail?.id,
                product: product?.productName ?? "No Address",
                price: orderDetail?.unitPrice ?? "No Address",
                quentity: orderDetail?.quantity ?? "No Address",
                discount: orderDetail?.discount ?? "No Address",
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
                render: (data, type, row) => row?.price
            },
            {
                render: (data, type, row) => row?.quentity
            },
            {
                render: (data, type, row) => row?.discount
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateOrderDetail' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteOrderDetail' }
                ])
            }
        ];
        if (orderDetails) {
            await initializeDataTable(orderDetailsitem, userSchema, 'OrderDetailTable');
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
const UsrValidae = $('#OrderDetailForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        OrderID: {
            required: true,
        },
        ProductID: {
            required: true,

        },
        UnitPrice: {
            required: true,

        },
        Quantity: {
            required: true,

        },
        Discount: {
            required: true,

        }

    },
    messages: {
        WarehouseName: {
            required: " Warehouse Name is required.",
        },
        Location: {
            required: " Address is required.",

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
    await populateDropdown('/Order/GetAll', '#OrderDropdown', 'id', 'orderDate', "Select Order Date");
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#OrderDetailForm').valid()) {
            const formData = $('#OrderDetailForm').serialize();
            const result = await SendRequest({ endpoint: '/OrderDetail/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#OrderDetailForm', modalId: '#modelCreate', message: ' Order Detail was successfully Created....' });
                await getOrderDetailList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#OrderDetailForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Order Detail Create failed. Please try again.' });
    }
});



window.updateOrderDetail = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    await populateDropdown('/Order/GetAll', '#OrderDropdown', 'id', 'orderDate', "Select Order Date");
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");

    const result = await SendRequest({ endpoint: '/OrderDetail/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#OrderDropdown').val(result.data.orderID);
        $('#ProductDropdown').val(result.data.productID);
        $('#UnitPrice').val(result.data.unitPrice);
        $('#Quantity').val(result.data.quantity);
        $('#Discount').val(result.data.discount);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#OrderDetailForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#OrderDetailForm').serialize();
            const result = await SendRequest({ endpoint: '/OrderDetail/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#OrderDetailForm', modalId: '#modelCreate', message: ' Order Detail was successfully Updated....' });
                await getOrderDetailList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteOrderDetail = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/OrderDetail/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#OrderDetailForm', modalId: '#deleteAndDetailsModel', message: ' Order Detail was successfully Delete....' });
            await getOrderDetailList(); // Update the user list
        }
    });
}