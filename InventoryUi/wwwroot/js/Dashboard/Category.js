import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getCategoryList();
});
const getCategoryList = async () => {
    debugger
    const category = await SendRequest({ endpoint: '/Category/GetAll' });
    // const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (category.status === 200 && category.success) {
        await onSuccessUsers(category.data);
    }
}

const onSuccessUsers = async (categorys) => {
    debugger
    //const companyMap = dataToMap(companys, 'id');
    const categoryitem = categorys.map((category) => {
        if (category) {
            debugger
            //const company = companyMap[warehouse.companyId];
            return {
                id: category?.id,
                name: category?.categoryName ?? "No Name",
                description: category?.description ?? "No Address",
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
                render: (data, type, row) => row?.description
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateCategory' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showCategory', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteCategory' }
                ])
            }
        ];
        if (categorys) {
            await initializeDataTable(categoryitem, userSchema, 'CategoryTable');
        }
    } catch (error) {
        console.error('Error processing Category data:', error);
    }
}

// Fatch duplucate file 

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

$.validator.addMethod("checkDuplicateCatagoryName", createDuplicateCheckValidator(
    "/Category/CheckDuplicate",
    "CategoryName",
    "Message"
));






// Initialize validation
const UsrValidae = $('#CategoryForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        CategoryName: {
            required: true,
            checkDuplicateCatagoryName: true
        },
        Description: {
            required: true,

        }

    },
    messages: {
        CategoryName: {
            required: " Category Name  is required.",
            checkDuplicateCatagoryName: "This Category Name is already taken."
        },
        Description: {
            required: " Description is required.",

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

});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#CategoryForm').valid()) {
            const formData = $('#CategoryForm').serialize();
            const result = await SendRequest({ endpoint: '/Category/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#CategoryForm', modalId: '#modelCreate', message: ' Category was successfully Created....' });
                await getCategoryList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#CategoryForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Category Create failed. Please try again.' });
    }
});



window.updateCategory = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    const result = await SendRequest({ endpoint: '/Category/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#CategoryName').val(result.data.categoryName);
        $('#Description').val(result.data.description);
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#CategoryForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#CategoryForm').serialize();
            const result = await SendRequest({ endpoint: '/Category/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#CategoryForm', modalId: '#modelCreate', message: ' Category was successfully Updated....' });
                await getCategoryList(); // Update the user list
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteCategory = async (id) => {
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Category/Delete', method: "POST", data: { id: id } });

        if (result.success) {
            displayNotification({
                formId: '#CategoryForm',
                modalId: '#deleteAndDetailsModel',
                message: 'Category was successfully deleted....'
            });
            await getCategoryList(); // Update the category list
        } else {
            // Display the error message in the modal
            $('#DeleteErrorMessage').removeClass('alert-success').addClass('text-danger').text(result.detail).show();
        }
    });
}
