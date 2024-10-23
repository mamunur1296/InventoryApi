import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getOrderList();
});
const getOrderList = async () => {
    debugger
    const orders = await SendRequest({ endpoint: '/Order/GetAll' });
    const customes = await SendRequest({ endpoint: '/Customer/GetAll' });
    const employees = await SendRequest({ endpoint: '/Employee/GetAll' });
    const prescriptions = await SendRequest({ endpoint: '/Prescription/GetAll' });
    if (orders.status === 200 && orders.success) {
        await onSuccessUsers(orders.data, customes.data, employees.data, prescriptions.data);
    }
}

const onSuccessUsers = async (orders, customes, employees, prescriptions) => {
    debugger
    const customeMap = dataToMap(customes, 'id');
    debugger
    const employeeMap = dataToMap(employees, 'id');
    debugger
    const prescriptionMap = dataToMap(prescriptions, 'id');
    debugger
    const ordersitem = orders.map((order) => {
        if (order) {
            debugger
            const custome = customeMap[order.customerID];
            const employee = employeeMap[order.employeeID];
            const prescription = prescriptionMap[order.prescriptionID];
            return {
                id: order?.id,
                invoice: order?.invoiceNumber ?? "N/A",
                employee: employee?.firstName + employee?.lastName ?? "N/A",
                orderDate: order?.orderDate ?? "N/A",
                requiredDate: order?.requiredDate ?? "N/A",
                shippedDate: order?.shippedDate ?? "N/A",
                address: order?.shipAddress ?? "N/A",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.invoice ?? "N/A"
            },
            {
                render: (data, type, row) => row?.orderDate
            },
            {
                render: (data, type, row) => row?.requiredDate
            },
            {
                render: (data, type, row) => row?.shippedDate
            },
            {
                render: (data, type, row) => row?.address
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateOrder' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails' },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteOrder' }
                ])
            }
        ];
        if (orders) {
            await initializeDataTable(ordersitem, userSchema, 'OrderTable');
        }
    } catch (error) {
        console.error('Error processing Order data:', error);
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
// Initialize validation
const UsrValidae = $('#OrderForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        CustomerID: {
            required: true,
        },
        EmployeeID: {
            required: true,
        },
        OrderDate: {
            required: true,
        },
        RequiredDate: {
            required: true,
        },
        ShippedDate: {
            required: true,
        },
        ShipVia: {
            required: true,
        },
        Shipper: {
            required: true,
        },
        Freight: {
            required: true,
        },
        ShipName: {
            required: true,
        },
        ShipAddress: {
            required: true,
        },
        ShipCity: {
            required: true,
        },
        ShipRegion: {
            required: true,
        },
        ShipPostalCode: {
            required: true,
        },
        ShipCountry: {
            required: true,
        },
        PaymentStatus: {
            required: true,
        },
        OrderStatus: {
            required: true,
        }
    },
    messages: {
        CustomerID: {
            required: "Customer  is required.",
        },
        EmployeeID: {
            required: "Employee  is required.",
        },
        OrderDate: {
            required: "Order Date is required.",
        },
        RequiredDate: {
            required: "Required Date is required.",
        },
        ShippedDate: {
            required: "Shipped Date is required.",
        },
        ShipVia: {
            required: "Ship Via is required.",
        },
        Shipper: {
            required: "Shipper is required.",
        },
        Freight: {
            required: "Freight is required.",
        },
        ShipName: {
            required: "Ship Name is required.",
        },
        ShipAddress: {
            required: "Ship Address is required.",
        },
        ShipCity: {
            required: "Ship City is required.",
        },
        ShipRegion: {
            required: "Ship Region is required.",
        },
        ShipPostalCode: {
            required: "Ship Postal Code is required.",
        },
        ShipCountry: {
            required: "Ship Country is required.",
        },
        PaymentStatus: {
            required: "Payment Status is required.",
        },
        OrderStatus: {
            required: "Order Status is required.",
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
    resetFormValidation('#OrderForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");
    await populateDropdown('/Prescription/GetAll', '#PrescriptionDropdown', 'id', 'doctorName', "Select Prescription");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#OrderForm').valid()) {
            const formData = $('#OrderForm').serialize();
            const result = await SendRequest({ endpoint: '/Order/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Order Created successfully !", type: "success", title: "Success" });
                await getOrderList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error", time: 0 });
                $('#modelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Order Created failed . Please try again. !", type: "error", title: "Error", time: 0 });
    }

});



window.updateOrder = async (id) => {
    resetFormValidation('#OrderForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
    await populateDropdown('/Employee/GetAll', '#EmployeeDropdown', 'id', 'firstName', "Select Employee");
    await populateDropdown('/Prescription/GetAll', '#PrescriptionDropdown', 'id', 'doctorName', "Select Prescription");
    const result = await SendRequest({ endpoint: '/Order/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#CustomerDropdown').val(result.data.customerID);
        $('#EmployeeDropdown').val(result.data.employeeID);
        $('#OrderDate').val(result.data.OrderDate ? result.data.OrderDate.split('T')[0] : '');
        $('#RequiredDate').val(result.data.requiredDate);
        $('#ShippedDate').val(result.data.shippedDate);
        $('#ShipVia').val(result.data.shipVia);
        $('#Freight').val(result.data.freight);
        $('#ShipName').val(result.data.shipName);
        $('#ShipAddress').val(result.data.shipAddress);
        $('#ShipCity').val(result.data.shipCity);
        $('#ShipRegion').val(result.data.shipRegion);
        $('#ShipPostalCode').val(result.data.shipPostalCode);
        $('#ShipCountry').val(result.data.shipCountry);
        $('#PrescriptionDropdown').val(result.data.prescriptionID);
        $('#PaymentStatus').val(result.data.paymentStatus);
        $('#OrderStatus').val(result.data.orderStatus);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#OrderForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#OrderForm').serialize();
            const result = await SendRequest({ endpoint: '/Order/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Order Updated successfully !", type: "success", title: "Success" });

                await getOrderList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Order Updated failed . Please try again. !", type: "error", title: "Error", time: 0 });
            }

        });
    }
    loger(result);
}




window.showDetails = async (id) => {
    const isDownload = false;  // Set this as needed
    const url = '/PosReport/DownloadInvoice?id=' + id + '&isDownload=' + isDownload;
    //window.location.href = url;  
    window.open(url, '_blank');
}


window.deleteOrder = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show'); 
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Order/Delete', method: "DELETE", data: { id: id } });
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Order  Deleted successfully !", type: "success", title: "Success" });
            await getOrderList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });

        }

    });
}