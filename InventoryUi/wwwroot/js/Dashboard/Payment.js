import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getPaymentList();
});
const getPaymentList = async () => {
    debugger
    const payments = await SendRequest({ endpoint: '/Payment/GetAll' });
    const orders = await SendRequest({ endpoint: '/Order/GetAll' });
    if (payments.status === 200 && payments.success) {
        await onSuccessUsers(payments.data, orders.data);
    }
}

const onSuccessUsers = async (payments, orders) => {
    debugger
    const ordersMap = dataToMap(orders, 'id');
    const paymentsitem = payments.map((payment) => {
        if (payment) {
            debugger
            const order = ordersMap[payment.orderID];
            return {
                id: payment?.id,
                orderDate: order?.orderDate ?? "No Name",
                orderDate: payment?.paymentDate ?? "No Address",
                method: payment?.paymentMethod ?? "No Address",
                amount: payment?.amount ?? "No Address",
                status: payment?.paymentStatus ?? "No Address",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.orderDate
            },
            {
                render: (data, type, row) => row?.orderDate
            }, {
                render: (data, type, row) => row?.method
            }, {
                render: (data, type, row) => row?.amount
            }, {
                render: (data, type, row) => row?.status
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updatePayment' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deletePayment' }
                ])
            }
        ];
        if (payments) {
            await initializeDataTable(paymentsitem, userSchema, 'PaymentTable');
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
const UsrValidae = $('#PaymentForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        OrderID: {
            required: true,
        },
        PaymentDate: {
            required: true,

        },
        PaymentMethod: {
            required: true,

        },
        Amount: {
            required: true,

        },
        PaymentStatus: {
            required: true,

        }

    },
    messages: {
        OrderID: {
            required: "Order  is required.",
        },
        PaymentDate: {
            required: "Payment Date is required.",
        },
        PaymentMethod: {
            required: "Payment Method is required.",
        },
        Amount: {
            required: "Amount is required.",
        },
        PaymentStatus: {
            required: "Payment Status is required.",
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
    await populateDropdown('/Order/GetAll', '#Orderdropdown', 'id', 'orderDate', "Select Order Date");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#PaymentForm').valid()) {
            const formData = $('#PaymentForm').serialize();
            const result = await SendRequest({ endpoint: '/Payment/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#PaymentForm', modalId: '#modelCreate', message: ' Payment was successfully Created....' });
                await getPaymentList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#PaymentForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Payment Create failed. Please try again.' });
    }
});



window.updatePayment = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Order/GetAll', '#Orderdropdown', 'id', 'orderDate', "Select Order Date");
    const result = await SendRequest({ endpoint: '/Payment/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#Orderdropdown').val(result.data.orderID);
        $('#PaymentDate').val(result.data.paymentDate);
        $('#Amount').val(result.data.amount);
        $('#PaymentStatus').val(result.data.paymentStatus);
        $('#PaymentMethod').val(result.data.paymentMethod);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#PaymentForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#PaymentForm').serialize();
            const result = await SendRequest({ endpoint: '/Payment/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#PaymentForm', modalId: '#modelCreate', message: ' Payment was successfully Updated....' });
                await getPaymentList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deletePayment = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Payment/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({
                formId: '#PaymentForm',
                modalId: '#deleteAndDetailsModel',
                message: 'Payment was successfully deleted....'
            });
            await getPaymentList(); // Update the category list
        } else {
            // Display the error message in the modal
            $('#DeleteErrorMessage').removeClass('alert-success').addClass('text-danger').text(result.detail).show();
        }
    });
}