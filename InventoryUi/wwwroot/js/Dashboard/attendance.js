import { notification, notificationErrors } from '../Utility/notification.js';
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
                empName: employee?.firstName + " " + employee?.lastName ?? "null",
                ispresent: attendance?.isPresent == true ? "Present" : attendance?.isPresent == false ? "Absent" : "null",
                checkintime: attendance?.checkInTime ?? "null",
                checkOuttime: attendance?.checkOutTime ?? "null",
                date : attendance?.date ? attendance.date.split("T")[0] : null
               
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
                render: (data, type, row) => row?.date 
            }, 
            {
                render: (data, type, row) => row?.checkintime 
            },
            {
                render: (data, type, row) => row?.checkOuttime 
            },
            {
                render: (data, type, row) => {
                    const isPresentText = row?.ispresent;
                    const colorClass = isPresentText === "Present" ? "text-success" :
                        isPresentText === "Absent" ? "text-danger" : "text-muted";
                    return `<span class="${colorClass}">${isPresentText}</span>`;
                }
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
        },
        CheckInTime: {
            required: true,
            checkTimeOrder: true
           
        },
        CheckOutTime: {
            required: true,
            checkTimeOrder: true
           
        },
        Date: {
            required: true,
            
        }
    },
    messages: {
        EmployeeId: {
            required: "Employee selection is required."
        },
        CheckInTime: {
            required: "Check-in time is required.",
            checkTimeOrder: "Check-in time must be before check-out time."
        },
        CheckOutTime: {
            required: "Check-out time is required.",
            checkTimeOrder: "Check-out time must be after check-in time."
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

$.validator.addMethod("checkTimeOrder", function (value, element) {
    const checkInTime = $('#CheckInTime').val();
    const checkOutTime = $('#CheckOutTime').val();

    // Return true if either field is empty (handled by required rules), or if checkInTime < checkOutTime
    return (checkInTime === "" || checkOutTime === "") || (checkInTime < checkOutTime);
}, "Check-in time must be before check-out time.");


//Sow Create Model 
$('#CreateAttendanceBtn').off('click').click(async () => {
    resetFormValidation('#AttendanceForm', isAttendanceValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    $('#IsPresentCheckbox').prop("checked", true);
    debugger
    showCreateModal('AttendanceModelCreate', 'AttendanceBtnSave', 'AttendanceBtnUpdate');
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName ,lastName', "Select Employee");
});

// Save Button

$('#AttendanceBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#AttendanceForm').valid()) {
            const checkInTime = $('#CheckInTime').val();
            const checkOutTime = $('#CheckOutTime').val();

            // Validate Check-Out time is after Check-In time
            if (checkInTime >= checkOutTime) {
                notification({ message: "Check-Out time must be after Check-In time.", type: "error", title: "Error" });
                return; // Prevent submission if validation fails
            }
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
                notificationErrors({ message: result.detail});
                $('#AttendanceModelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#AttendanceModelCreate').modal('hide');
        notificationErrors({ message: error.message });
    }

});



window.updateAttendance = async (id) => {
    resetFormValidation('#AttendanceForm', isAttendanceValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#AttendanceForm')[0].reset();
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName ,lastName', "Select Employee");

    const result = await SendRequest({ endpoint: '/Attendance/GetById/' + id });
    if (result.success) {
        $('#AttendanceBtnSave').hide();
        $('#AttendanceBtnUpdate').show();

        // Bind item
        $('#EmployeeDropdown').val(result.data.employeeId);
        $('#CheckInTime').val(result.data.checkInTime);
        $('#CheckOutTime').val(result.data.checkOutTime);
        $('#IsPresentCheckbox').prop("checked", result.data.isPresent);

        // Set date safely
        const date = new Date(result.data.date);
        if (!isNaN(date.getTime())) { // Check if the date is valid
            $('#Date').val(date.toISOString().split('T')[0]); // Format date for input
        } else {
            $('#Date').val(''); // Set empty if invalid
        }

        $('#AttendanceModelCreate').modal('show');
        resetValidation(isAttendanceValidae, '#AttendanceForm');

        $('#AttendanceBtnUpdate').off('click').on('click', async () => {
            const checkInTime = $('#CheckInTime').val();
            const checkOutTime = $('#CheckOutTime').val();

            // Validate Check-Out time is after Check-In time
            if (checkInTime >= checkOutTime) {
                notification({ message: "Check-Out time must be after Check-In time.", type: "error", title: "Error" });
                return; // Prevent submission if validation fails
            }

            const formData = $('#AttendanceForm').serialize();
            const updateResult = await SendRequest({ endpoint: '/Attendance/Update/' + id, method: "PUT", data: formData });
            if (updateResult.success) {
                $('#AttendanceModelCreate').modal('hide');
                notification({ message: "Attendance updated successfully!", type: "success", title: "Success" });
                await getAttendanceList(); // Update the attendance list
            } else {
                $('#AttendanceModelCreate').modal('hide');
                notificationErrors({ message: "Attendance update failed. Please try again." });
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
            notificationErrors({ message: result.detail });
        }
    });
}
