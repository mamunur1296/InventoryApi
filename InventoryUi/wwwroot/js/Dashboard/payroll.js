import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getPayrollList();
});
const getPayrollList = async () => {
    debugger
    const Payroll = await SendRequest({ endpoint: '/Payroll/GetAll' });
    const employee = await SendRequest({ endpoint: '/Employee/GetAll' });
    if (Payroll.status === 200 && Payroll.success) {
        await onSuccessUsers(Payroll.data, employee.data);
    }
}

const onSuccessUsers = async (Payrolls, employee) => {
    debugger
    const employeeMap = dataToMap(employee, 'id');
    const PayrollItem = Payrolls.map((Payroll) => {
        if (Payroll) {
            debugger
            const employee = employeeMap[Payroll.employeeId];
            return {
                id: Payroll?.id,
                empName: employee?.firstName + " " + employee?.lastName ?? "Null",
                baseSalary: Payroll?.baseSalary ?? "Null",
                bonus: Payroll?.bonus ?? "Null",
                deducation: Payroll?.deductions ?? "Null",
                netSalary: Payroll?.netSalary ?? "Null",
                paymentDate: Payroll?.PaymentDate ?? "Null",
             

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
                render: (data, type, row) => row?.baseSalary ?? "N/A"
            },
            {
                render: (data, type, row) => row?.bonus ?? "N/A"
            },
            {
                render: (data, type, row) => row?.deducation ?? "N/A"
            },
            {
                render: (data, type, row) => row?.netSalary ?? "N/A"
            },
            {
                render: (data, type, row) => row?.paymentDate ?? "N/A"
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updatePayroll' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showPayroll', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deletePayroll' }
                ])
            }
        ];
        if (Payrolls) {
            await initializeDataTable(PayrollItem, userSchema, 'PayrollTable');
        }
    } catch (error) {
        console.error('Error processing Payroll data:', error);
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
export const isPayrollValidae = $('#PayrollForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        EmployeeId: {
            required: true,
        }
        ,
        BaseSalary: {
            required: true,

        }
        ,
        Bonus: {
            required: true,

        }
        ,
        Deductionss: {
            required: true,

        }
        ,
        NetSalary: {
            required: true,

        }
        ,
        PaymentDate: {
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
$('#CreatePayrollBtn').off('click').click(async () => {
    resetFormValidation('#PayrollForm', isPayrollValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('PayrollModelCreate', 'PayrollBtnSave', 'PayrollBtnUpdate');
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");
});

// Save Button

$('#PayrollBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#PayrollForm').valid()) {
            const formData = $('#PayrollForm').serialize();
            const result = await SendRequest({ endpoint: '/Payroll/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#PayrollModelCreate').modal('hide');
                notification({ message: "Payroll Created successfully !", type: "success", title: "Success" });
                await getPayrollList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#PayrollModelCreate').modal('hide');
        notification({ message: " Payroll Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updatePayroll = async (id) => {
    resetFormValidation('#PayrollForm', isPayrollValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#PayrollForm')[0].reset();
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");

    const result = await SendRequest({ endpoint: '/Payroll/GetById/' + id });
    if (result.success) {
        $('#PayrollBtnSave').hide();
        $('#PayrollBtnUpdate').show();
        //buind item
        $('#EmployeeDropdown').val(result.data.employeeId);
        $('#BaseSalary').val(result.data.baseSalary);
        $('#Bonus').val(result.data.bonus);
        $('#Deductions').val(result.data.deductions);
        $('#NetSalary').val(result.data.netSalary);
        $('#PaymentDate').val(result.data.paymentDate);

        $('#PayrollModelCreate').modal('show');
        resetValidation(isPayrollValidae, '#PayrollForm');
        $('#PayrollBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#PayrollForm').serialize();
            const result = await SendRequest({ endpoint: '/Payroll/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#PayrollModelCreate').modal('hide');
                notification({ message: "Payroll Updated successfully !", type: "success", title: "Success" });

                await getPayrollList(); // Update the user list
            } else {
                $('#PayrollModelCreate').modal('hide');
                notification({ message: " Payroll Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deletePayroll = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Payroll/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Payroll Deleted successfully !", type: "success", title: "Success" });
            await getPayrollList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
