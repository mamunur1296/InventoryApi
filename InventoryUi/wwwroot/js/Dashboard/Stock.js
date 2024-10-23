import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getStockList();
});
const getStockList = async () => {
    debugger
    const stocks = await SendRequest({ endpoint: '/Stock/GetAll' });
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    const warehouses = await SendRequest({ endpoint: '/Warehouse/GetAll' });
    if (stocks.status === 200 && stocks.success) {
        await onSuccessUsers(stocks.data, products.data, warehouses.data);
    }
}

const onSuccessUsers = async (stocks, products, warehouses) => {
    debugger
    const productsMap = dataToMap(products, 'id');
    const warehousesMap = dataToMap(warehouses, 'id');
    const stocksitem = stocks.map((stock) => {
        if (stock) {
            debugger
            const product = productsMap[stock.productID];
            const warehouse = warehousesMap[stock.warehouseID];
            return {
                id: stock?.id,
                product: product?.productName ?? "N/A",
                warehouse: warehouse?.warehouseName ?? "N/A",
                quentity: stock?.quantity ?? "N/A"
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
                render: (data, type, row) => row?.warehouse
            },
            {
                render: (data, type, row) => row?.quentity
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateStock' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteStock' }
                ])
            }
        ];
        if (stocks) {
            await initializeDataTable(stocksitem, userSchema, 'StockTable');
        }
    } catch (error) {
        console.error('Error processing Stock data:', error);
    }
}

///* Fatch duplucate file */

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
const UsrValidae = $('#StockForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        ProductID: {
            required: true,
        },
        WarehouseID: {
            required: true,
        },
        Quantity: {
            required: true,
        }

    },
    messages: {
        ProductID: {
            required: "Product  is required.",
        },
        WarehouseID: {
            required: "Warehouse  is required.",
        },
        Quantity: {
            required: "Quantity is required.",
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
    resetFormValidation('#StockForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");
    await populateDropdown('/Warehouse/GetAll', '#WarehouseDropdown', 'id', 'warehouseName', "Select Warehouse");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#StockForm').valid()) {
            const formData = $('#StockForm').serialize();
            const result = await SendRequest({ endpoint: '/Stock/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Stock Created successfully !", type: "success", title: "Success" });
                await getStockList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error", time: 0 });
                $('#modelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Stock Created failed . Please try again. !", type: "error", title: "Error", time: 0 });
    }

});



window.updateStock = async (id) => {
    resetFormValidation('#StockForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select Product");
    await populateDropdown('/Warehouse/GetAll', '#WarehouseDropdown', 'id', 'warehouseName', "Select Warehouse");

    const result = await SendRequest({ endpoint: '/Stock/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#ProductDropdown').val(result.data.productID);
        $('#WarehouseDropdown').val(result.data.warehouseID);
        $('#Quantity').val(result.data.quantity);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#StockForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#StockForm').serialize();
            const result = await SendRequest({ endpoint: '/Stock/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Stock Updated successfully !", type: "success", title: "Success" });

                await getStockList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Stock Updated failed . Please try again. !", type: "error", title: "Error", time: 0 });
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteStock = async (id) => {

    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Stock/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Stock  Deleted successfully !", type: "success", title: "Success" });
            await getStockList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });

        }
    });
}