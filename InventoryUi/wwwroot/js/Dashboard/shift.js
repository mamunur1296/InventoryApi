import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getShiftList();
});
const getShiftList = async () => {
    debugger
    const Shift = await SendRequest({ endpoint: '/Shift/GetAll' });
    if (Shift.status === 200 && Shift.success) {
        await onSuccessUsers(Shift.data);
    }
}

const onSuccessUsers = async (Shifts) => {
    debugger
    const ShiftItem = Shifts.map((Shift) => {
        if (Shift) {
            debugger
            return {
                id: Shift?.id,
                name: Shift?.shiftName ?? "N/A",
                start: Shift?.startTime ?? "N/A",
                end: Shift?.endTime ?? "N/A",

            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.name ?? "N/A"
            },
            {
                render: (data, type, row) => row?.start ?? "N/A"
            },
            {
                render: (data, type, row) => row?.end ?? "N/A"
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateShift' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showShift', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteShift' }
                ])
            }
        ];
        if (Shifts) {
            await initializeDataTable(ShiftItem, userSchema, 'ShiftTable');
        }
    } catch (error) {
        console.error('Error processing Shift data:', error);
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
export const isShiftValidae = $('#ShiftForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        ShiftName: {
            required: true,
        }
        ,
        StartTime: {
            required: true,

        }
        ,
        EndTime: {
            required: true,

        }
       
    },
    messages: {
        CategoryName: {
            required: " Branch Name  is required.",
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
$('#CreateShiftBtn').off('click').click(async () => {
    resetFormValidation('#ShiftForm', isShiftValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('ShiftModelCreate', 'ShiftBtnSave', 'ShiftBtnUpdate');
    
});

// Save Button

$('#ShiftBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#ShiftForm').valid()) {
            const formData = $('#ShiftForm').serialize();
            const result = await SendRequest({ endpoint: '/Shift/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#ShiftModelCreate').modal('hide');
                notification({ message: "Shift Created successfully !", type: "success", title: "Success" });
                await getShiftList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#ShiftModelCreate').modal('hide');
        notification({ message: " Shift Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateShift = async (id) => {
    resetFormValidation('#ShiftForm', isShiftValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#ShiftForm')[0].reset();
    

    const result = await SendRequest({ endpoint: '/Shift/GetById/' + id });
    if (result.success) {
        $('#ShiftBtnSave').hide();
        $('#ShiftBtnUpdate').show();
        //buind item
        $('#ShiftName').val(result.data.shiftName);
        $('#StartTime').val(result.data.startTime);
        $('#EndTime').val(result.data.endTime);
        $('#CheckOutTime').val(result.data.checkOutTime);

        $('#ShiftModelCreate').modal('show');
        resetValidation(isShiftValidae, '#ShiftForm');
        $('#ShiftBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#ShiftForm').serialize();
            const result = await SendRequest({ endpoint: '/Shift/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#ShiftModelCreate').modal('hide');
                notification({ message: "Shift Updated successfully !", type: "success", title: "Success" });

                await getShiftList(); // Update the user list
            } else {
                $('#ShiftModelCreate').modal('hide');
                notification({ message: " Shift Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteShift = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Shift/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Shift Deleted successfully !", type: "success", title: "Success" });
            await getShiftList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
