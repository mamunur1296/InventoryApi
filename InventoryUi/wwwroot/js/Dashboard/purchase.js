import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getPurchaseList();
});
const getPurchaseList = async () => {
    debugger
    const purchases = await SendRequest({ endpoint: '/Purchase/GetAll' });
    const suppliers = await SendRequest({ endpoint: '/Supplier/GetAll' });
    if (purchases.status === 200 && purchases.success) {
        await onSuccessUsers(purchases.data, suppliers.data);
    }
}

const onSuccessUsers = async (purchases, suppliers) => {
    debugger
    const suppliersMap = dataToMap(suppliers, 'id');
    const purchasesitem = purchases.map((purchase) => {
        if (purchase) {
            debugger
            const suppliers = suppliersMap[purchase.supplierID];
            return {
                id: purchase?.id,
                name: suppliers?.supplierName ?? "N/A",
                date: purchase?.purchaseDate ?? "N/A",
                total: purchase?.totalAmount ?? "N/A",              
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
                render: (data, type, row) => row?.date ?? "N/A"
            },
            {
                render: (data, type, row) => row?.total ?? "N/A"
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updatePurchase', disabled: true },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showPurchase', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deletePurchase' }
                ])
            }
        ];
        if (purchases) {
            await initializeDataTable(purchasesitem, userSchema, 'PurchaseTable');
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
export const isBranchValidae = $('#PurchaseForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        PurchaseDate: {
            required: true,
        }
        ,
        SupplierID: {
            required: true,

        }


    },
    messages: {
        PurchaseDate: {
            required: " Purchase  Date  is required.",
        },
        SupplierID: {
            required: " Supplar is required.",

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
$('#CreatePurchaseBtn').off('click').click(async () => {
    resetFormValidation('#PurchaseForm', isBranchValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('PurchaseModelCreate', 'PurchaseBtnSave', 'PurchaseBtnUpdate');
    await populateDropdown('/Supplier/GetAll', '#SupplierDropdown', 'id', 'supplierName', "Select Supplier");
});

// Save Button

$('#PurchaseBtnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#PurchaseForm').valid()) {
            const formData = $('#PurchaseForm').serialize();
            const result = await SendRequest({ endpoint: '/Purchase/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#PurchaseModelCreate').modal('hide');
                notification({ message: "Purchase Created successfully !", type: "success", title: "Success" });
                await getPurchaseList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error" });
                $('#PurchaseModelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#PurchaseModelCreate').modal('hide');
        notification({ message: " Purchase Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updatePurchase = async (id) => {
    resetFormValidation('#BranchForm', isBranchValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateBranch').show();
    $('#myModalLabelAddBranch').hide();
    $('#BranchForm')[0].reset();
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'name', "Select Company");

    const result = await SendRequest({ endpoint: '/Branch/GetById/' + id });
    if (result.success) {
        $('#BranchBtnSave').hide();
        $('#BranchBtnUpdate').show();

        $('#Name').val(result.data.name);
        $('#FullName').val(result.data.fullName);
        $('#ContactPerson').val(result.data.contactPerson);
        $('#Address').val(result.data.address);
        $('#PhoneNo').val(result.data.phoneNo);
        $('#FaxNo').val(result.data.faxNo);
        $('#EmailNo').val(result.data.emailNo);
        $('#IsActive').val(result.data.isActive);
        $('#CompanyDropdown').val(result.data.companyId);



        $('#BranchModelCreate').modal('show');
        resetValidation(isBranchValidae, '#BranchForm');
        $('#BranchBtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#BranchForm').serialize();
            const result = await SendRequest({ endpoint: '/Branch/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#BranchModelCreate').modal('hide');
                notification({ message: "Branch Updated successfully !", type: "success", title: "Success" });

                await getBranchList(); // Update the user list
            } else {
                $('#BranchModelCreate').modal('hide');
                notification({ message: " Branch Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




////window.showDetails = async (id) => {
////    loger("showDetails id " + id);
////}


window.deletePurchase = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#DeleteErrorMessage').hide(); // Hide error message initially
    $('#btnDelete').off('click').on('click', async () => {
        debugger;
        const result = await SendRequest({ endpoint: '/Purchase/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Purchase Deleted successfully !", type: "success", title: "Success" });
            await getPurchaseList(); // Update the category list

        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
