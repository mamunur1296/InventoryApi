import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getSupplierList();
    await CreateSupplierBtn('#CreateSupplierBtn');
});
const getSupplierList = async () => {
    debugger
    const suppliers = await SendRequest({ endpoint: '/Supplier/GetAll' });
    // const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (suppliers.status === 200 && suppliers.success) {
        await onSuccessUsers(suppliers.data);
    }
}

const onSuccessUsers = async (suppliers) => {
    debugger
    //const companyMap = dataToMap(companys, 'id');
    const suppliersitem = suppliers.map((supplier) => {
        if (supplier) {
            debugger
            //const company = companyMap[warehouse.companyId];
            return {
                id: supplier?.id,
                name: supplier?.supplierName ?? "N/A",
                contact: supplier?.contactName ?? "N/A",
                title: supplier?.contactTitle ?? "N/A",
                address: supplier?.address ?? " " + ", " + supplier?.city ?? " " + ", " + supplier?.region ?? " " + ", " + supplier?.postalCode ?? " " + ", " + supplier?.country ?? " " ?? "N/A",
                phone: supplier?.phone ?? "N/A",
                fax: supplier?.fax ?? "N/A",
                homepage: supplier?.homePage ?? "N/A",
                
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
                render: (data, type, row) => row?.contact
            },
            {
                render: (data, type, row) => row?.title
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
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateSupplier' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteSupplier' }
                ])
            }
        ];
        if (suppliers) {
            await initializeDataTable(suppliersitem, userSchema, 'SupplierTable');
        }
    } catch (error) {
        console.error('Error processing Supplier data:', error);
    }
}

//// Fatch duplucate file 

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

//$.validator.addMethod("checkDuplicateWarehouseName", createDuplicateCheckValidator(
//    "/Warehouse/CheckDuplicate",
//    "WarehouseName"
//));






// Initialize validation
export const SupplierFormValidae = $('#SupplierForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        SupplierName: {
            required: true,

        },
        ContactName: {
            required: true,

        },
        Address: {
            required: true,

        },
        Phone: {
            required: true,

        }

    },
    messages: {
        SupplierName: {
            required: "Supplier Name is required.",
        },
        ContactName: {
            required: "Contact Name is required.",
        },
        ContactTitle: {
            required: "Contact Title is required.",
        },
        Address: {
            required: "Address is required.",
        },
        City: {
            required: "City is required.",
        },
        Region: {
            required: "Region is required.",
        },
        PostalCode: {
            required: "Postal Code is required.",
        },
        Country: {
            required: "Country is required.",
        },
        Phone: {
            required: "Phone is required.",
        },
        Fax: {
            required: "Fax is required.",
        },
        HomePage: {
            required: "Home Page is required.",
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

export const CreateSupplierBtn = async (CreateBtnId) => {
    //Sow Create Model 
    $(CreateBtnId).off('click').click(async (e) => {
        e.preventDefault();
        resetFormValidation('#SupplierForm', SupplierFormValidae);
        clearMessage('successMessage', 'globalErrorMessage');
        debugger
        showCreateModal('SupplierModelCreate', 'SupplierbtnSave', 'SupplierbtnUpdate');
    });
} 


// Save Button

$('#SupplierbtnSave').off('click').click(async (e) => {
    e.preventDefault();
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#SupplierForm').valid()) {
            const formData = $('#SupplierForm').serialize();
            const result = await SendRequest({ endpoint: '/Supplier/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#SupplierModelCreate').modal('hide');
                notification({ message: "Supplier Created successfully !", type: "success", title: "Success" });
                await getSupplierList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#SupplierModelCreate').modal('hide');
        notification({ message: " Supplier Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateSupplier = async (id) => {
    resetFormValidation('#SupplierForm', SupplierFormValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    const result = await SendRequest({ endpoint: '/Supplier/GetById/' + id });
    if (result.success) {
        $('#SupplierbtnSave').hide();
        $('#SupplierbtnUpdate').show();

        $('#SupplierName').val(result.data.supplierName);
        $('#ContactName').val(result.data.contactName);
        $('#ContactTitle').val(result.data.contactTitle);
        $('#Address').val(result.data.address);
        $('#City').val(result.data.city);
        $('#Region').val(result.data.region);
        $('#PostalCode').val(result.data.postalCode);
        $('#Country').val(result.data.country);
        $('#Phone').val(result.data.phone);
        $('#Fax').val(result.data.fax);
        $('#HomePage').val(result.data.homePage);


        $('#SupplierModelCreate').modal('show');
        resetValidation(SupplierFormValidae, '#SupplierForm');
        $('#SupplierbtnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#SupplierForm').serialize();
            const result = await SendRequest({ endpoint: '/Supplier/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#SupplierModelCreate').modal('hide');
                notification({ message: "Supplier Updated successfully !", type: "success", title: "Success" });

                await getSupplierList(); // Update the user list
            } else {
                $('#SupplierModelCreate').modal('hide');
                notification({ message: " Supplier Updated failed . Please try again. !", type: "error", title: "Error" });
            }

        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteSupplier = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Supplier/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Supplier  Deleted successfully !", type: "success", title: "Success" });
            await getSupplierList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });

        }

    });
}