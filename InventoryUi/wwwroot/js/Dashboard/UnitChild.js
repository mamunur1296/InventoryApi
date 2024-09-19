import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getUnitChildList();
});
const getUnitChildList = async () => {
    debugger
    const unitChilds = await SendRequest({ endpoint: '/UnitChild/GetAll' });
    const UnitMasters = await SendRequest({ endpoint: '/UnitMaster/GetAll' });
    if (unitChilds.status === 200 && unitChilds.success) {
        await onSuccessUsers(unitChilds.data, UnitMasters.data);
    }
}

const onSuccessUsers = async (unitChilds, UnitMasters) => {
    debugger
    const UnitMastersMap = dataToMap(UnitMasters, 'id');
    const unitChildsitem = unitChilds.map((unitChild) => {
        if (unitChild) {
            debugger
            const UnitMasters = UnitMastersMap[unitChild.unitMasterId];
            return {
                id: unitChild?.id,
                name: unitChild?.name ?? "N/A",
                unit: UnitMasters?.name ?? "N/A",
                UnitShortCode: unitChild?.unitShortCode ?? "N/A",
                DisplayName: unitChild?.displayName ?? "N/A",
                UnitDescription: unitChild?.unitDescription ?? "N/A",
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
                render: (data, type, row) => row?.unit
            },
            {
                render: (data, type, row) => row?.UnitShortCode
            },
            {
                render: (data, type, row) => row?.DisplayName
            },
            {
                render: (data, type, row) => row?.UnitDescription
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateUnitChild' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showUnitChild', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteUnitChild' }
                ])
            }
        ];
        if (unitChilds) {
            await initializeDataTable(unitChildsitem, userSchema, 'UnitChildTable');
        }
    } catch (error) {
        console.error('Error processing UnitChild data:', error);
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
const UsrValidae = $('#UnitChildForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        Name: {
            required: true,
        },
        UnitMasterId: {
            required: true,

        },
        UnitShortCode: {
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

//Sow Create Model 
$('#CreateBtn').off('click').click(async () => {
    resetFormValidation('#UnitChildForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/UnitMaster/GetAll', '#UnitMasterDropdown', 'id', 'name', "Select Master Unit");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#UnitChildForm').valid()) {
            const formData = $('#UnitChildForm').serialize();
            const result = await SendRequest({ endpoint: '/UnitChild/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "UnitChild Created successfully !", type: "success", title: "Success" });
                await getUnitChildList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " UnitChild Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateUnitChild = async (id) => {
    resetFormValidation('#UnitChildForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    $('#UnitChildForm')[0].reset();

    await populateDropdown('/UnitMaster/GetAll', '#UnitMasterDropdown', 'id', 'name', "Select Master Unit");

    const result = await SendRequest({ endpoint: '/UnitChild/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();


        $('#Name').val(result.data.name);
        $('#UnitMasterDropdown').val(result.data.unitMasterId);
        $('#UnitShortCode').val(result.data.unitShortCode);
        $('#DisplayName').val(result.data.displayName);
        $('#UnitDescription').val(result.data.unitDescription);


        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#UnitChildForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#UnitChildForm').serialize();
            const result = await SendRequest({ endpoint: '/UnitChild/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "UnitChild Updated successfully !", type: "success", title: "Success" });

                await getUnitChildList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " UnitChild Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteUnitChild = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/UnitChild/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "UnitChild Deleted successfully !", type: "success", title: "Success" });
            await getUnitChildList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
