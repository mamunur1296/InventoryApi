import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getReviewList();
});
const getReviewList = async () => {
    debugger
    const reviews = await SendRequest({ endpoint: '/Review/GetAll' });
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    const customers = await SendRequest({ endpoint: '/Customer/GetAll' });
    if (reviews.status === 200 && reviews.success) {
        await onSuccessUsers(reviews.data, products.data, customers.data);
    }
}

const onSuccessUsers = async (reviews, products, customers) => {
    debugger
    const productsMap = dataToMap(products, 'id');
    const customersMap = dataToMap(customers, 'id');
    const reviewsitem = reviews.map((review) => {
        if (review) {
            debugger
            const product = productsMap[review.productID];
            const customer = customersMap[review.customerID];
            return {
                id: review?.id,
                product: product?.productName ?? "No Name",
                customer: customer?.customerName ?? "No Address",
                rationg: review?.rating ?? "No Address",
                commend: review?.reviewText ?? "No Address",
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
                render: (data, type, row) => row?.customer
            },
            {
                render: (data, type, row) => row?.rationg
            },
            {
                render: (data, type, row) => row?.commend
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateReview' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteReview' }
                ])
            }
        ];
        if (reviews) {
            await initializeDataTable(reviewsitem, userSchema, 'ReviewTable');
        }
    } catch (error) {
        console.error('Error processing Review data:', error);
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
const UsrValidae = $('#ReviewForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        ProductID: {
            required: true,
        },
        CustomerID: {
            required: true,

        },
        Rating: {
            required: true,

        },
        ReviewText: {
            required: true,

        }

    },
    messages: {
        WarehouseName: {
            required: " Warehouse Name is required.",
            checkDuplicateWarehouseName: "This Warehouse Name is already taken."
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
    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select User");
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select User");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#ReviewForm').valid()) {
            const formData = $('#ReviewForm').serialize();
            const result = await SendRequest({ endpoint: '/Review/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#ReviewForm', modalId: '#modelCreate', message: ' Review was successfully Created....' });
                await getReviewList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#ReviewForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Review Create failed. Please try again.' });
    }
});



window.updateReview = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    await populateDropdown('/Product/GetAll', '#ProductDropdown', 'id', 'productName', "Select User");
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select User");

    const result = await SendRequest({ endpoint: '/Review/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#ProductDropdown').val(result.data.productID);
        $('#CustomerDropdown').val(result.data.customerID);
        $('#Rating').val(result.data.rating);
        $('#ReviewText').val(result.data.reviewText);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#ReviewForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#ReviewForm').serialize();
            const result = await SendRequest({ endpoint: '/Review/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#ReviewForm', modalId: '#modelCreate', message: ' Review was successfully Updated....' });
                await getReviewList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteReview = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Review/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#ReviewForm', modalId: '#deleteAndDetailsModel', message: ' Review was successfully Delete....' });
            await getReviewList(); // Update the user list
        }
    });
}