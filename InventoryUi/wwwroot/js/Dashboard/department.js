import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getDepartmentList();
});
const getDepartmentList = async () => {
    debugger
    const department = await SendRequest({ endpoint: '/Department/GetAll' });
    if (department.status === 200 && department.success) {
        await onSuccessUsers(department.data);
    }
}

const onSuccessUsers = async (departments) => {
    debugger
    const departmentsItem = departments.map((department) => {
        if (department) {
            debugger
            return {
                id: department?.id,
                name: department?.departmentName ?? "Null",
                dis: department?.description ?? "Null",

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
                render: (data, type, row) => row?.dis 
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateDepartment' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDepartment', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteDepartment' }
                ])
            }
        ];
        if (departments) {
            await initializeDataTable(departmentsItem, userSchema, 'DepartmentTable');
        }
    } catch (error) {
        console.error('Error processing Department data:', error);
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
export const isDepartmentValidae = $('#DepartmentForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        DepartmentName: {
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
$('#CreateDepartmentBtn').off('click').click(async () => {
    resetFormValidation('#DepartmentForm', isDepartmentValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('DepartmentModelCreate', 'DepartmentBtnSave', 'DepartmentBtnUpdate');
  
});

// Save Button

$('#DepartmentBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#DepartmentForm').valid()) {
            const formData = $('#DepartmentForm').serialize();
            const result = await SendRequest({ endpoint: '/Department/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#DepartmentModelCreate').modal('hide');
                notification({ message: "Department Created successfully !", type: "success", title: "Success" });
                await getDepartmentList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#DepartmentModelCreate').modal('hide');
        notification({ message: " Department Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateDepartment = async (id) => {
    resetFormValidation('#DepartmentForm', isDepartmentValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#DepartmentForm')[0].reset();
    

    const result = await SendRequest({ endpoint: '/Department/GetById/' + id });
    if (result.success) {
        $('#DepartmentBtnSave').hide();
        $('#DepartmentBtnUpdate').show();
        //buind item
        $('#DepartmentName').val(result.data.departmentName);
        $('#Description').val(result.data.description);
        

        $('#DepartmentModelCreate').modal('show');
        resetValidation(isDepartmentValidae, '#DepartmentForm');
        $('#DepartmentBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#DepartmentForm').serialize();
            const result = await SendRequest({ endpoint: '/Department/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#DepartmentModelCreate').modal('hide');
                notification({ message: "Department Updated successfully !", type: "success", title: "Success" });

                await getDepartmentList(); // Update the user list
            } else {
                $('#DepartmentModelCreate').modal('hide');
                notification({ message: " Department Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteDepartment = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Department/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Department Deleted successfully !", type: "success", title: "Success" });
            await getDepartmentList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
