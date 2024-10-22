import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getLeaveList();
});
const getLeaveList = async () => {
    debugger
    const Leave = await SendRequest({ endpoint: '/Leave/GetAll' });
    const employee = await SendRequest({ endpoint: '/Employee/GetAll' });
    if (Leave.status === 200 && Leave.success) {
        await onSuccessUsers(Leave.data, employee.data);
    }
}

const onSuccessUsers = async (Leaves, employee) => {
    debugger
    const employeeMap = dataToMap(employee, 'id');
    const LeaveItem = Leaves.map((Leave) => {
        if (Leave) {
            debugger
            const employee = employeeMap[Leave.employeeId];
            return {
                id: Leave?.id,
                empName: employee?.firstName + " " + employee?.lastName ?? "Null",
                ispresent: Leave?.isApproved == true ? "Approved" : Leave?.isApproved == false ? "Not Approved" : "Null",
                livetype: Leave?.leaveType ?? "Null",
                reson: Leave?.reason ?? "Null",
                edate: Leave?.endDate ?? "Null",
                sdate: Leave?.startDate ?? "Null",

            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.empName 
            },
            {
                render: (data, type, row) => row?.sdate 
            },
            {
                render: (data, type, row) => row?.edate 
            },
            {
                render: (data, type, row) => row?.livetype 
            },
            {
                render: (data, type, row) => row?.reson 
            },
            {
                render: (data, type, row) => {
                    const isPresentText = row?.ispresent ?? "Null";
                    const colorClass = isPresentText === "Approved" ? "text-success" :
                        isPresentText === "Not Approved" ? "text-danger" : "text-muted";
                    return `<span class="${colorClass}">${isPresentText}</span>`;
                }
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateLeave' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showLeave', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteLeave' }
                ])
            }
        ];
        if (Leaves) {
            await initializeDataTable(LeaveItem, userSchema, 'LeaveTable');
        }
    } catch (error) {
        console.error('Error processing Leave data:', error);
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
export const isLeaveValidae = $('#LeaveForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        EmployeeId: {
            required: true,
        }
        ,
        LeaveType: {
            required: true,

        }
        ,
        StartDate: {
            required: true,

        }
        ,
        EndDate: {
            required: true,

        },
        Reason: {
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
$('#CreateLeaveBtn').off('click').click(async () => {
    resetFormValidation('#LeaveForm', isLeaveValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('LeaveModelCreate', 'LeaveBtnSave', 'LeaveBtnUpdate');
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");
});

// Save Button

$('#LeaveBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#LeaveForm').valid()) {
            const formData = $('#LeaveForm').serialize();
            const result = await SendRequest({ endpoint: '/Leave/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#LeaveModelCreate').modal('hide');
                notification({ message: "Leave Created successfully !", type: "success", title: "Success" });
                await getLeaveList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#LeaveModelCreate').modal('hide');
        notification({ message: " Leave Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateLeave = async (id) => {
    resetFormValidation('#LeaveForm', isLeaveValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#LeaveForm')[0].reset();
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");

    const result = await SendRequest({ endpoint: '/Leave/GetById/' + id });
    if (result.success) {
        $('#LeaveBtnSave').hide();
        $('#LeaveBtnUpdate').show();
        //buind item
        $('#EmployeeDropdown').val(result.data.employeeId);
        $('#LeaveType').val(result.data.leaveType);
        $('#StartDate').val(result.data.startDate);
        $('#EndDate').val(result.data.endDate);
        $('#Reason').val(result.data.reason);
        $('#IsApproved').val(result.data.isApproved);

        $('#LeaveModelCreate').modal('show');
        resetValidation(isLeaveValidae, '#LeaveForm');
        $('#LeaveBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#LeaveForm').serialize();
            const result = await SendRequest({ endpoint: '/Leave/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#LeaveModelCreate').modal('hide');
                notification({ message: "Leave Updated successfully !", type: "success", title: "Success" });

                await getLeaveList(); // Update the user list
            } else {
                $('#LeaveModelCreate').modal('hide');
                notification({ message: " Leave Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteLeave = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Leave/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Leave Deleted successfully !", type: "success", title: "Success" });
            await getLeaveList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
