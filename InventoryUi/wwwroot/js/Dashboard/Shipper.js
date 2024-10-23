import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getShipperList();
});
const getShipperList = async () => {
    debugger
    const shippers = await SendRequest({ endpoint: '/Shipper/GetAll' });
    // const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (shippers.status === 200 && shippers.success) {
        await onSuccessUsers(shippers.data);
    }
}

const onSuccessUsers = async (shippers) => {
    debugger
    //const companyMap = dataToMap(companys, 'id');
    const shippersitem = shippers.map((shipper) => {
        if (shipper) {
            debugger
            //const company = companyMap[warehouse.companyId];
            return {
                id: shipper?.id,
                name: shipper?.shipperName ?? "N/A",
                phone: shipper?.phone ?? "N/A",
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
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateShipper' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteShipper' }
                ])
            }
        ];
        if (shippers) {
            await initializeDataTable(shippersitem, userSchema, 'ShipperTable');
        }
    } catch (error) {
        console.error('Error processing Shipper data:', error);
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
const UsrValidae = $('#ShipperForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        ShipperName: {
            required: true,
        },
        Phone: {
            required: true,

        }

    },
    messages: {
        ShipperName: {
            required: "Shipper Name is required.",
        },
        Phone: {
            required: "Phone number is required.",
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
    resetFormValidation('#ShipperForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    //await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#ShipperForm').valid()) {
            const formData = $('#ShipperForm').serialize();
            const result = await SendRequest({ endpoint: '/Shipper/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Shipper Created successfully !", type: "success", title: "Success" });
                await getShipperList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error", time: 0 });
                $('#modelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Shipper Created failed . Please try again. !", type: "error", title: "Error", time: 0 });
    }

});



window.updateShipper = async (id) => {
    resetFormValidation('#ShipperForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    const result = await SendRequest({ endpoint: '/Shipper/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#ShipperName').val(result.data.shipperName);
        $('#Phone').val(result.data.phone);
  
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#ShipperForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#ShipperForm').serialize();
            const result = await SendRequest({ endpoint: '/Shipper/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Shipper Updated successfully !", type: "success", title: "Success" });

                await getShipperList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Shipper Updated failed . Please try again. !", type: "error", title: "Error", time: 0 });
            }

        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteShipper = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Shipper/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Shipper  Deleted successfully !", type: "success", title: "Success" });
            await getShipperList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });

        }
    });
}