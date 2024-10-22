import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getAttendanceList();
});
const getAttendanceList = async () => {
    debugger
    const attendance = await SendRequest({ endpoint: '/Attendance/GetAll' });
    const employee = await SendRequest({ endpoint: '/Employee/GetAll' });
    if (attendance.status === 200 && attendance.success) {
        await onSuccessUsers(attendance.data, employee.data);
    }
}

const onSuccessUsers = async (attendances, employee) => {
    debugger
    const employeeMap = dataToMap(employee, 'id');
    const attendanceItem = attendances.map((attendance) => {
        if (attendance) {
            debugger
            const employee = employeeMap[attendance.employeeId];
            return {
                id: attendance?.id,
                empName: employee?.firstName + " " + employee?.lastName ?? "N/A",
                ispresent: attendance?.isPresent == true ? "Present" : attendance?.isPresent == false ? "Absent" : "N/A",
                checkintime: attendance?.checkInTime ?? "N/A",
                checkOuttime: attendance?.checkOutTime ?? "N/A",
               
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.empName ?? "N/A"
            },
            {
                render: (data, type, row) => {
                    const isPresentText = row?.ispresent ?? "N/A";
                    const colorClass = isPresentText === "Present" ? "text-success" :
                        isPresentText === "Absent" ? "text-danger" : "text-muted";
                    return `<span class="${colorClass}">${isPresentText}</span>`;
                }
            },
            {
                render: (data, type, row) => row?.checkintime ?? "N/A"
            },
            {
                render: (data, type, row) => row?.checkOuttime ?? "N/A"
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateAttendance' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showAttendance', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteAttendance' }
                ])
            }
        ];
        if (attendances) {
            await initializeDataTable(attendanceItem, userSchema, 'AttendanceTable');
        }
    } catch (error) {
        console.error('Error processing Attendance data:', error);
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
export const isAttendanceValidae = $('#AttendanceForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        EmployeeId: {
            required: true,
        }
        ,
        CheckInTime: {
            required: true,

        }
        ,
        CheckOutTime: {
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
$('#CreateAttendanceBtn').off('click').click(async () => {
    resetFormValidation('#AttendanceForm', isAttendanceValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('AttendanceModelCreate', 'AttendanceBtnSave', 'AttendanceBtnUpdate');
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");
});

// Save Button

$('#AttendanceBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#AttendanceForm').valid()) {
            const formData = $('#AttendanceForm').serialize();
            const result = await SendRequest({ endpoint: '/Attendance/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#AttendanceModelCreate').modal('hide');
                notification({ message: "Attendance Created successfully !", type: "success", title: "Success" });
                await getAttendanceList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error" });
                $('#AttendanceModelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#AttendanceModelCreate').modal('hide');
        notification({ message: " Attendance Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateAttendance = async (id) => {
    resetFormValidation('#AttendanceForm', isAttendanceValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#AttendanceForm')[0].reset();
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");

    const result = await SendRequest({ endpoint: '/Attendance/GetById/' + id });
    if (result.success) {
        $('#AttendanceBtnSave').hide();
        $('#AttendanceBtnUpdate').show();
        //buind item
        $('#EmployeeDropdown').val(result.data.employeeId);
        $('#CheckInTime').val(result.data.checkInTime);
        $('#IsPresent').val(result.data.isPresent);
        $('#CheckOutTime').val(result.data.checkOutTime);

        $('#AttendanceModelCreate').modal('show');
        resetValidation(isAttendanceValidae, '#AttendanceForm');
        $('#AttendanceBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#AttendanceForm').serialize();
            const result = await SendRequest({ endpoint: '/Attendance/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#AttendanceModelCreate').modal('hide');
                notification({ message: "Attendance Updated successfully !", type: "success", title: "Success" });

                await getAttendanceList(); // Update the user list
            } else {
                $('#AttendanceModelCreate').modal('hide');
                notification({ message: " Attendance Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteAttendance = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Attendance/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Attendance Deleted successfully !", type: "success", title: "Success" });
            await getAttendanceList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
