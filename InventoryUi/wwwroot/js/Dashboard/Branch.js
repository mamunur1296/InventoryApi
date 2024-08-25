import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getBranchList();
});
const getBranchList = async () => {
    debugger
    const branchs = await SendRequest({ endpoint: '/Branch/GetAll' });
    const companys = await SendRequest({ endpoint: '/Company/GetAll' });
    if (branchs.status === 200 && branchs.success) {
        await onSuccessUsers(branchs.data, companys.data);
    }
}

const onSuccessUsers = async (branchs, companys) => {
    debugger
    const companysMap = dataToMap(companys, 'id');
    const branchsitem = branchs.map((branch) => {
        if (branch) {
            debugger
            const company = companysMap[branch.companyId];
            return {
                id: branch?.id,
                name: branch?.name ?? "No Name",
                fullName: branch?.fullName ?? "No Address",
                contactParson: branch?.contactPerson ?? "No Data",
                address: branch?.address ?? "No Data",
                phone: branch?.phoneNo ?? "No Data",
                fax: branch?.faxNo ?? "No Data",
                email: branch?.emailNo ?? "No Data",
                company: company?.fullName ?? "No Data",
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
                render: (data, type, row) => row?.fullName
            },
            {
                render: (data, type, row) => row?.contactParson
            },
            {
                render: (data, type, row) => row?.address
            },
            {
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => row?.fax
            },
            {
                render: (data, type, row) => row?.email
            },
            {
                render: (data, type, row) => row?.company
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateBranch' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showBranch', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteBranch' }
                ])
            }
        ];
        if (branchs) {
            await initializeDataTable(branchsitem, userSchema, 'BranchTable');
        }
    } catch (error) {
        console.error('Error processing Branch data:', error);
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
const UsrValidae = $('#BranchForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        Name: {
            required: true,
        },
        FullName: {
            required: true,

        },
        ContactPerson: {
            required: true,

        }
        ,
        Address: {
            required: true,

        }
        ,
        PhoneNo: {
            required: true,

        }
        ,
        FaxNo: {
            required: true,

        }
        ,
        EmailNo: {
            required: true,

        }
        ,
        IsActive: {
            required: true,

        }
        ,
        CompanyId: {
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
$('#CreateBtn').off('click').click(async () => {
    resetFormValidation('#BranchForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'fullName', "Select Company");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#BranchForm').valid()) {
            const formData = $('#BranchForm').serialize();
            const result = await SendRequest({ endpoint: '/Branch/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Branch Created successfully !", type: "success", title: "Success" });
                await getBranchList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Branch Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateBranch = async (id) => {
    resetFormValidation('#BranchForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    $('#BranchForm')[0].reset();
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'fullName', "Select Company");

    const result = await SendRequest({ endpoint: '/Branch/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#Name').val(result.data.name);
        $('#FullName').val(result.data.fullName);
        $('#ContactPerson').val(result.data.contactPerson);
        $('#Address').val(result.data.address);
        $('#PhoneNo').val(result.data.phoneNo);
        $('#FaxNo').val(result.data.faxNo);
        $('#EmailNo').val(result.data.emailNo);
        $('#IsActive').val(result.data.isActive);
        $('#CompanyDropdown').val(result.data.companyId);

   

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#BranchForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#BranchForm').serialize();
            const result = await SendRequest({ endpoint: '/Branch/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Branch Updated successfully !", type: "success", title: "Success" });

                await getBranchList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Branch Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deleteBranch = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Branch/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Branch Deleted successfully !", type: "success", title: "Success" });
            await getBranchList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
