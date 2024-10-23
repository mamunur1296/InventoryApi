import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getHolidayList();
});
const getHolidayList = async () => {
    debugger
    const holiday = await SendRequest({ endpoint: '/Holiday/GetAll' });
    if (holiday.status === 200 && holiday.success) {
        await onSuccessUsers(holiday.data);
    }
}

const onSuccessUsers = async (holidays) => {
    debugger
  
    const holidaysItem = holidays.map((holiday) => {
        if (holiday) {
            debugger
            return {
                id: holiday?.id,
                name: holiday?.holidayName ?? "Null",
                dis: holiday?.description ?? "Null",
                dat: holiday?.date ?? "Null",

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
                render: (data, type, row) => row?.dis ?? "N/A"
            },
            {
                render: (data, type, row) => row?.dat ?? "N/A"
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateHoliday' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showHoliday', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteHoliday' }
                ])
            }
        ];
        if (holidays) {
            await initializeDataTable(holidaysItem, userSchema, 'HolidayTable');
        }
    } catch (error) {
        console.error('Error processing Holiday data:', error);
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
export const isHolidayValidae = $('#HolidayForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        HolidayName: {
            required: true,
        }
       
        ,
        Date: {
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
$('#CreateHolidayBtn').off('click').click(async () => {
    resetFormValidation('#HolidayForm', isHolidayValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('HolidayModelCreate', 'HolidayBtnSave', 'HolidayBtnUpdate');
    
});

// Save Button

$('#HolidayBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#HolidayForm').valid()) {
            const formData = $('#HolidayForm').serialize();
            const result = await SendRequest({ endpoint: '/Holiday/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#HolidayModelCreate').modal('hide');
                notification({ message: "Holiday Created successfully !", type: "success", title: "Success" });
                await getHolidayList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error", time: 0 });
                $('#HolidayModelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#HolidayModelCreate').modal('hide');
        notification({ message: " Holiday Created failed . Please try again. !", type: "error", title: "Error", time: 0 });
    }

});



window.updateHoliday = async (id) => {
    resetFormValidation('#HolidayForm', isHolidayValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#HolidayForm')[0].reset();


    const result = await SendRequest({ endpoint: '/Holiday/GetById/' + id });
    if (result.success) {
        $('#HolidayBtnSave').hide();
        $('#HolidayBtnUpdate').show();
        //buind item
        $('#HolidayName').val(result.data.holidayName);
        $('#Description').val(result.data.description);
        $('#Date').val(result.data.date);
       

        $('#HolidayModelCreate').modal('show');
        resetValidation(isHolidayValidae, '#HolidayForm');
        $('#HolidayBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#HolidayForm').serialize();
            const result = await SendRequest({ endpoint: '/Holiday/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#HolidayModelCreate').modal('hide');
                notification({ message: "Holiday Updated successfully !", type: "success", title: "Success" });

                await getHolidayList(); // Update the user list
            } else {
                $('#HolidayModelCreate').modal('hide');
                notification({ message: " Holiday Updated failed . Please try again. !", type: "error", title: "Error", time: 0 });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteHoliday = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Holiday/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Holiday Deleted successfully !", type: "success", title: "Success" });
            await getHolidayList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });
        }
    });
}
