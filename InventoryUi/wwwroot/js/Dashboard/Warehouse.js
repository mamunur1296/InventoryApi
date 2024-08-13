import {  createActionButtons, dataToMap, displayNotification,  initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getWarehouseList();
});
const getWarehouseList = async () => {
    debugger
    const warehouse = await SendRequest({ endpoint: '/Warehouse/GetAll' });
   // const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (warehouse.status === 200 && warehouse.success) {
        await onSuccessUsers(warehouse.data);
    }
}

const onSuccessUsers = async (warehouses) => {
    debugger
    //const companyMap = dataToMap(companys, 'id');
    const warehouseitem = warehouses.map((warehouse) => {
        if (warehouse) {
            debugger
            //const company = companyMap[warehouse.companyId];
            return {
                id: warehouse?.id,
                warehouseName: warehouse?.warehouseName ?? "No Name",
                location: warehouse?.location ?? "No Address",
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

    },
    messages: {
        WarehouseName: {
            required: " Warehouse Name is required.",
            checkDuplicateWarehouseName: "This Warehouse Name is already taken."
        },
        Location: {
            required: " Address is required.",

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
$('#CreateUserBtn').click(async () => {
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    //await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
});

// Save Button

$('#btnSave').click(async () => {
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
                displayNotification({ formId: '#WarehouseForm', modalId: '#modelCreate', message: ' Warehouse was successfully Created....' });
                await getWarehouseList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#WarehouseForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Warehouse Create failed. Please try again.' });
    }
});



window.updateWareHouse = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    
    const result = await SendRequest({ endpoint: '/Warehouse/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#WarehouseName').val(result.data.warehouseName);
        $('#Location').val(result.data.location);
        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#WarehouseForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#WarehouseForm').serialize();
            const result = await SendRequest({ endpoint: '/Warehouse/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#WarehouseForm', modalId: '#modelCreate', message: ' Warehouse was successfully Updated....' });
                await getWarehouseList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteWareHouse = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Warehouse/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#WarehouseForm', modalId: '#deleteAndDetailsModel', message: ' Warehouse was successfully Delete....' });
            await getWarehouseList(); // Update the user list
        }
    });
}