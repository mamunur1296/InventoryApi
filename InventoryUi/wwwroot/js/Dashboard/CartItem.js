//import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
//import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

//$(document).ready(async function () {
//    await getCartItemList();
//});
//const getCartItemList = async () => {
//    debugger
//    const CartItems = await SendRequest({ endpoint: '/CartItem/GetAll' });
//    const ShoppingCarts = await SendRequest({ endpoint: '/ShoppingCart/GetAll' });
//    const Products = await SendRequest({ endpoint: '/Product/GetAll' });
//    if (CartItems.status === 200 && CartItems.success) {
//        await onSuccessUsers(CartItems.data, ShoppingCarts.data, Products.data);
//    }
//}

//const onSuccessUsers = async (CartItems) => {
//    debugger
//    //const companyMap = dataToMap(companys, 'id');
//    const warehouseitem = warehouses.map((warehouse) => {
//        if (warehouse) {
//            debugger
//            //const company = companyMap[warehouse.companyId];
//            return {
//                id: warehouse?.id,
//                warehouseName: warehouse?.warehouseName ?? "No Name",
//                location: warehouse?.location ?? "No Address",
//            };
//        }
//        return null;
//    }).filter(Boolean);

//    try {
//        debugger
//        const userSchema = [
//            {
//                render: (data, type, row) => row?.warehouseName
//            },
//            {
//                render: (data, type, row) => row?.location
//            },
//            {
//                render: (data, type, row) => createActionButtons(row, [
//                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateCartItem' },
//                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
//                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteCartItem' }
//                ])
//            }
//        ];
//        if (warehouses) {
//            await initializeDataTable(warehouseitem, userSchema, 'CartItemTable');
//        }
//    } catch (error) {
//        console.error('Error processing Cart Item data:', error);
//    }
//}

////// Fatch duplucate file 

////const createDuplicateCheckValidator = (endpoint, key, errorMessage) => {
////    return function (value, element) {
////        let isValid = false;
////        $.ajax({
////            type: "GET",
////            url: endpoint,
////            data: { key: key, val: value },
////            async: false,
////            success: function (response) {
////                isValid = !response;
////            },
////            error: function () {
////                isValid = false;
////            }
////        });
////        return isValid;
////    };
////}

////$.validator.addMethod("checkDuplicateWarehouseName", createDuplicateCheckValidator(
////    "/Warehouse/CheckDuplicate",
////    "WarehouseName"
////));






//// Initialize validation
//const UsrValidae = $('#CartItemForm').validate({
//    onkeyup: function (element) {
//        $(element).valid();
//    },
//    rules: {
//        CartID: {
//            required: true,
//        },
//        ProductID: {
//            required: true,

//        },
//        Quantity: {
//            required: true,

//        }

//    },
//    messages: {
//        WarehouseName: {
//            required: " Select Cart  is required.",
//        },
//        Location: {
//            required: " Address is required.",

//        }
//    },
//    errorElement: 'div',
//    errorPlacement: function (error, element) {
//        error.addClass('invalid-feedback');
//        element.closest('.form-group').append(error);
//    },
//    highlight: function (element, errorClass, validClass) {
//        $(element).addClass('is-invalid');
//    },
//    unhighlight: function (element, errorClass, validClass) {
//        $(element).removeClass('is-invalid');
//    }
//});

////Sow Create Model 
//$('#CreateBtn').click(async () => {
//    debugger
//    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
//    await populateDropdown('/ShoppingCart/GetAll', '#Cartdropdown', 'id', 'userName', "Select Card");
//    await populateDropdown('/Product/GetAll', '#Productdropdown', 'id', 'productName', "Select Product");
//});

//// Save Button

//$('#btnSave').click(async () => {
//    debugger
//    try {
//        if ($('#CartItemForm').valid()) {
//            const formData = $('#CartItemForm').serialize();
//            const result = await SendRequest({ endpoint: '/CartItem/Create', method: 'POST', data: formData });
//            // Clear previous messages
//            $('#successMessage').hide();
//            $('#UserError').hide();
//            $('#EmailError').hide();
//            $('#PasswordError').hide();
//            $('#GeneralError').hide();
//            debugger
//            if (result.success && result.status === 201) {
//                displayNotification({ formId: '#CartItemForm', modalId: '#modelCreate', message: ' Cart Item was successfully Created....' });
//                await getCartItemList(); // Update the user list
//            }
//        }
//    } catch (error) {
//        console.error('Error in click handler:', error);
//        displayNotification({ formId: '#CartItemForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Cart Item Create failed. Please try again.' });
//    }
//});



//window.updateCartItem = async (id) => {
//    debugger
//    $('#myModalLabelUpdateEmployee').show();
//    $('#myModalLabelAddEmployee').hide();
//    await populateDropdown('/ShoppingCart/GetAll', '#Cartdropdown', 'id', 'userName', "Select Card");
//    await populateDropdown('/Product/GetAll', '#Productdropdown', 'id', 'productName', "Select Product");
//    const result = await SendRequest({ endpoint: '/CartItem/GetById/' + id });
//    if (result.success) {
//        $('#btnSave').hide();
//        $('#btnUpdate').show();

//        $('#Cartdropdown').val(result.data.cartID);
//        $('#Productdropdown').val(result.data.productID);
//        $('#Quantity').val(result.data.quantity);

//        $('#modelCreate').modal('show');
//        resetValidation(UsrValidae, '#CartItemForm');
//        $('#btnUpdate').on('click', async () => {
//            debugger
//            const formData = $('#CartItemForm').serialize();
//            const result = await SendRequest({ endpoint: '/CartItem/Update/' + id, method: "PUT", data: formData });
//            if (result.success) {
//                displayNotification({ formId: '#CartItemForm', modalId: '#modelCreate', message: ' Cart Item was successfully Updated....' });
//                await getCartItemList(); // Update the user list
//            }
//        });
//    }
//    loger(result);
//}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


//window.deleteCartItem = async (id) => {
//    debugger
//    $('#deleteAndDetailsModel').modal('show');
//    $('#companyDetails').empty();
//    $('#btnDelete').click(async () => {
//        debugger
//        const result = await SendRequest({ endpoint: '/CartItem/Delete', method: "POST", data: { id: id } });
//        if (result.success) {
//            displayNotification({ formId: '#CartItemForm', modalId: '#deleteAndDetailsModel', message: ' Cart Item was successfully Delete....' });
//            await getCartItemList(); // Update the user list
//        }
//    });
//}