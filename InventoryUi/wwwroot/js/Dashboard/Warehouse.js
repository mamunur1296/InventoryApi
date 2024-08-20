﻿import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getWarehouseList();
});
const getWarehouseList = async () => {
    debugger
    const warehouse = await SendRequest({ endpoint: '/Warehouse/GetAll' });
    const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (warehouse.status === 200 && warehouse.success) {
        await onSuccessUsers(warehouse.data, company.data);
    }
}

const onSuccessUsers = async (warehouses, companys) => {
    debugger
    const companyMap = dataToMap(companys, 'id');
    const warehouseitem = warehouses.map((warehouse) => {
        if (warehouse) {
            debugger
            const company = companyMap[warehouse.companyId];
            return {
                id: warehouse?.id,
                warehouseName: warehouse?.warehouseName ?? "No Name",
                location: warehouse?.location ?? "No Address",
                company: company?.fullName ?? "No Address",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.warehouseName
            },
            {
                render: (data, type, row) => row?.location
            }, {
                render: (data, type, row) => row?.company
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateWareHouse' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteWareHouse' }
                ])
            }
        ];
        if (warehouses) {
            await initializeDataTable(warehouseitem, userSchema, 'WarehouseTable');
        }
    } catch (error) {
        console.error('Error processing company data:', error);
    }
}

// Fatch duplucate file 

const createDuplicateCheckValidator = (endpoint, key, errorMessage) => {
    return function (value, element) {
        let isValid = false;
        $.ajax({
            type: "GET",
            url: endpoint,
            data: { key: key, val: value },
            async: false,
            success: function (response) {
                isValid = !response;
            },
            error: function () {
                isValid = false;
            }
        });
        return isValid;
    };
}

$.validator.addMethod("checkDuplicateWarehouseName", createDuplicateCheckValidator(
    "/Warehouse/CheckDuplicate",
    "WarehouseName"
));






// Initialize validation
const UsrValidae = $('#WarehouseForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        WarehouseName: {
            required: true,
            checkDuplicateWarehouseName: true
        },
        Location: {
            required: true,

        }
        ,
        CompanyId: {
            required: true,

        }

    },
    messages: {
        WarehouseName: {
            required: " Warehouse Name is required.",
            checkDuplicateWarehouseName: "This Warehouse Name is already taken."
        },
        Location: {
            required: " Address is required.",

        }
        ,
        CompanyId: {
            required: " Company is required.",

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
$('#CreateUserBtn').off('click').click(async () => {
    resetFormValidation('#WarehouseForm', UsrValidae);
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
        if ($('#WarehouseForm').valid()) {
            const formData = $('#WarehouseForm').serialize();
            const result = await SendRequest({ endpoint: '/Warehouse/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger

            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Warehouse Created successfully !", type: "success", title: "Success" });
                await getWarehouseList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Warehouse Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updateWareHouse = async (id) => {
    resetFormValidation('#WarehouseForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'fullName', "Select Company");
    const result = await SendRequest({ endpoint: '/Warehouse/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#WarehouseName').val(result.data.warehouseName);
        $('#Location').val(result.data.location);
        $('#CompanyDropdown').val(result.data.companyId);
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#WarehouseForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#WarehouseForm').serialize();
            const result = await SendRequest({ endpoint: '/Warehouse/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Warehouse Updated successfully !", type: "success", title: "Success" });

                await getWarehouseList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Warehouse Updated failed . Please try again. !", type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteWareHouse = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Warehouse/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Warehouse Deleted successfully !", type: "success", title: "Success" });
            await getWarehouseList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}