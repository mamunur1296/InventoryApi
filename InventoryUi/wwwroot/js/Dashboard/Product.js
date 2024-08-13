import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getProductList();
});
const getProductList = async () => {
    debugger
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    const categorys = await SendRequest({ endpoint: '/Category/GetAll' });
    const suppliers = await SendRequest({ endpoint: '/Supplier/GetAll' });
    if (products.status === 200 && products.success) {
        await onSuccessUsers(products.data, categorys.data, suppliers.data);
    }
}

const onSuccessUsers = async (products, categorys, suppliers) => {
    debugger
    const categorysMap = dataToMap(categorys, 'id');
    const suppliersMap = dataToMap(suppliers, 'id');
    const productsitem = products.map((product) => {
        if (product) {
            debugger
            const category = categorysMap[product.categoryID];
            const supplier = suppliersMap[product.supplierID];
            return {
                id: product?.id,
                name: product?.productName ?? "No Address",
                catagory: category?.categoryName ?? "No Name",
                supplier: supplier?.supplierName ?? "No Address",
                price: product?.unitPrice ?? "No Address",
                stock: product?.unitsInStock ?? "No Address",
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
                render: (data, type, row) => row?.catagory
            },
            {
                render: (data, type, row) => row?.supplier
            },
            {
                render: (data, type, row) => row?.price
            },
            {
                render: (data, type, row) => row?.stock
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateProduct' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteProduct' }
                ])
            }
        ];
        if (products) {
            await initializeDataTable(productsitem, userSchema, 'ProductTable');
        }
    } catch (error) {
        console.error('Error processing Product data:', error);
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
const UsrValidae = $('#ProductForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        ProductName: {
            required: true,
        },
        Description: {
            required: true,

        },
        CategoryID: {
            required: true,

        },
        SupplierID: {
            required: true,

        },
        QuantityPerUnit: {
            required: true,

        },
        UnitPrice: {
            required: true,

        },
        UnitsInStock: {
            required: true,

        },
        ReorderLevel: {
            required: true,

        },
        BatchNumber: {
            required: true,

        },
        ExpirationDate: {
            required: true,

        },
        ImageURL: {
            required: true,

        },
        Weight: {
            required: true,

        },
        Dimensions: {
            required: true,
        }

    },
    messages: {
        WarehouseName: {
            required: " Warehouse Name is required.",

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
    await populateDropdown('/Category/GetAll', '#CategoryDropdown', 'id', 'categoryName', "Select Catagory");
    await populateDropdown('/Supplier/GetAll', '#SupplierDropdown', 'id', 'supplierName', "Select Supplier");
});

// Save Button

$('#btnSave').click(async () => {
    debugger
    try {
        if ($('#ProductForm').valid()) {
            const formData = $('#ProductForm').serialize();
            loger(formData);
            const result = await SendRequest({ endpoint: '/Product/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                displayNotification({ formId: '#ProductForm', modalId: '#modelCreate', message: ' Product was successfully Created....' });
                await getProductList(); // Update the user list
            }
        }
    } catch (error) {
        loger(error);
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#ProductForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Product Create failed. Please try again.' });
    }
});



window.updateProduct = async (id) => {
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Category/GetAll', '#CategoryDropdown', 'id', 'categoryName', "Select Catagory");
    await populateDropdown('/Supplier/GetAll', '#SupplierDropdown', 'id', 'supplierName', "Select Supplier");

    const result = await SendRequest({ endpoint: '/Product/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#ProductName').val(result.data.productName);
        $('#Description').val(result.data.description);
        $('#CategoryDropdown').val(result.data.categoryID);
        $('#SupplierDropdown').val(result.data.supplierID);
        $('#QuantityPerUnit').val(result.data.quantityPerUnit);
        $('#UnitPrice').val(result.data.unitPrice);
        $('#UnitsInStock').val(result.data.unitsInStock);
        $('#ReorderLevel').val(result.data.reorderLevel);
        $('#Discontinued').val(result.data.discontinued);
        $('#BatchNumber').val(result.data.batchNumber);
        $('#ExpirationDate').val(result.data.expirationDate);
        $('#ImageURL').val(result.data.imageURL);
        $('#Weight').val(result.data.weight);
        $('#Dimensions').val(result.data.dimensions);
        $('#Category').val(result.data.category);
        $('#Supplier').val(result.data.supplier);


        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#ProductForm');
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#ProductForm').serialize();
            const result = await SendRequest({ endpoint: '/Product/Update/' + id, method: "PUT", data: formData });
            debugger
            if (result.success) {
                debugger

                displayNotification({ formId: '#ProductForm', modalId: '#modelCreate', message: ' Product was successfully Updated....' });
                await getProductList(); // Update the user list
            }
        });
    }
    loger(result);
}



//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteProduct = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Product/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({ formId: '#ProductForm', modalId: '#deleteAndDetailsModel', message: ' Product was successfully Delete....' });
            await getProductList(); // Update the user list
        }
    });
}