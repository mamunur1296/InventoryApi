import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getMenuList();
});
const getMenuList = async () => {
    debugger
    const menus = await SendRequest({ endpoint: '/Menu/GetAll' });
    // const company = await SendRequest({ endpoint: '/Company/GetAll' });
    if (menus.status === 200 && menus.success) {
        await onSuccessUsers(menus.data);
    }
}

const onSuccessUsers = async (menus) => {
    debugger
    //const companyMap = dataToMap(companys, 'id');
    const menusitem = menus.map((menu) => {
        if (menu) {
            debugger
            //const company = companyMap[warehouse.companyId];
            return {
                id: menu?.id,
                name: menu?.name ?? "No Name",
                url: menu?.url ?? "No Address",
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
                render: (data, type, row) => row?.url
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateMenu' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteMenu' }
                ])
            }
        ];
        if (menus) {
            await initializeDataTable(menusitem, userSchema, 'MenuTable');
        }
    } catch (error) {
        console.error('Error processing Menu data:', error);
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
const UsrValidae = $('#MenuForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        Name: {
            required: true,

        },
        Url: {
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
$('#CreateBtn').click(async () => {
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    //await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#MenuForm').valid()) {
            const formData = $('#MenuForm').serialize();
            const result = await SendRequest({ endpoint: '/Menu/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#MenuForm', modalId: '#modelCreate', message: ' Menu was successfully Created....' });
                await getMenuList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#MenuForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Menu Create failed. Please try again.' });
    }
});



window.updateMenu = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();

    const result = await SendRequest({ endpoint: '/Menu/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#Name').val(result.data.name);
        $('#Url').val(result.data.url);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#MenuForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#MenuForm').serialize();
            const result = await SendRequest({ endpoint: '/Menu/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#MenuForm', modalId: '#modelCreate', message: ' Menu was successfully Updated....' });
                await getMenuList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteMenu = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Menu/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#MenuForm', modalId: '#deleteAndDetailsModel', message: ' Menu was successfully Delete....' });
            await getMenuList(); // Update the user list
        }
    });
}