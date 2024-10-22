import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getUnitMasterList();
    await UnitMasteCreateBtn('#UnitMasterCreateBtn');
});
const getUnitMasterList = async () => {
    debugger
    const UnitMasters = await SendRequest({ endpoint: '/UnitMaster/GetAll' });
    if (UnitMasters.status === 200 && UnitMasters.success) {
        await onSuccessUsers(UnitMasters.data);
    }
}

const onSuccessUsers = async (UnitMasters) => {
    debugger

    const UnitMastersitem = UnitMasters.map((UnitMaster) => {
        if (UnitMaster) {
            debugger

            return {
                id: UnitMaster?.id,
                name: UnitMaster?.name ?? "N/A",
                description: UnitMaster?.unitMasterDescription ?? "N/A",
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
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateUnitMaster' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showUnitMaster', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteUnitMaster' }
                ])
            }
        ];
        if (UnitMasters) {
            await initializeDataTable(UnitMastersitem, userSchema, 'UnitMasterTable');
        }
    } catch (error) {
        console.error('Error processing Category data:', error);
    }
}

// Fatch duplucate file 

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

//$.validator.addMethod("checkDuplicateCatagoryName", createDuplicateCheckValidator(
//    "/Category/CheckDuplicate",
//    "CategoryName",
//    "Message"
//));






// Initialize validation
const UsrValidae = $('#UnitMasterForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        Name: {
            required: true,
        }

    },
    messages: {
        CategoryName: {
            required: " Category Name  is required.",
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

export const UnitMasteCreateBtn = async (createBtnId) => {
    $(createBtnId).off('click').click(async (e) => {
        e.preventDefault();
        resetFormValidation('#UnitMasterForm', UsrValidae);
        clearMessage('successMessage', 'globalErrorMessage');
        debugger
        showCreateModal('UnitMasterModelCreate', 'UnitMasterbtnSave', 'UnitMasterbtnUpdate');
    });
}

// Save Button

$('#UnitMasterbtnSave').off('click').click(async (e) => {
    e.preventDefault();
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#UnitMasterForm').valid()) {
            const formData = $('#UnitMasterForm').serialize();
            const result = await SendRequest({ endpoint: '/UnitMaster/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#UnitMasterModelCreate').modal('hide');
                notification({ message: "Unit Master Created successfully !", type: "success", title: "Success" });
                await getUnitMasterList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error" });
                $('#UnitMasterModelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#UnitMasterModelCreate').modal('hide');
        notification({ message: " Unit Master Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateUnitMaster = async (id) => {
    resetFormValidation('#UnitMasterForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    $('#UnitMasterForm')[0].reset();

    const result = await SendRequest({ endpoint: '/UnitMaster/GetById/' + id });
    if (result.success) {
        $('#UnitMasterbtnSave').hide();
        $('#UnitMasterbtnUpdate').show();
        $('#Name').val(result.data.name);
        $('#UnitMasterDescription').val(result.data.unitMasterDescription);
  
        $('#UnitMasterModelCreate').modal('show');
        resetValidation(UsrValidae, '#UnitMasterForm');
        $('#UnitMasterbtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#UnitMasterForm').serialize();
            const result = await SendRequest({ endpoint: '/UnitMaster/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#UnitMasterModelCreate').modal('hide');
                notification({ message: "UnitMaster Updated successfully !", type: "success", title: "Success" });

                await getUnitMasterList(); // Update the user list
            } else {
                $('#UnitMasterModelCreate').modal('hide');
                notification({ message: " UnitMaster Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteUnitMaster = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/UnitMaster/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Unit Master Deleted successfully !", type: "success", title: "Success" });
            await getUnitMasterList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
