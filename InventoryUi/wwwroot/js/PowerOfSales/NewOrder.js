import { notification } from "../Utility/notification.js";

$(document).ready(function () {
    initializeFunctions();
});

const initializeFunctions = () => {
    SearchProduct();
    SearchCustomer();
    AddCustomer();
    AddToCart();
    DeleteCartItem();
    UpdateCartItem();
    newProductModalHandling();
    newCustomerModalHandling();
    newCategoryModalHandling();
    newSupplierModalHandling();
    newUnitMasterModalHandling();
    newUnitChildModalHandling();
   
};

const SearchProduct = () => {
    $("#tags").autocomplete({
        source: '/NewOrder/SarchProduct', // Endpoint to fetch products
        select: function (event, ui) {
            // Set the selected product name to the search input
            $("#tags").val(ui.item.label);
            // Store the selected productId in the hidden input field
            $("#selectedProductId").val(ui.item.productid);
            // Prevent the default form submission
            event.preventDefault();
            // Clear the input field before triggering the form submission
            $("#tags").val('');
            // Manually trigger the form's submit handler to make an AJAX call
            $('#addProductForm').trigger('submit');
        }
    });
};

const SearchCustomer = () => {
    $("#phoneNumber").autocomplete({
        source: '/NewOrder/SearchCustomer',
        select: function (event, ui) {
            $("#phoneNumber").val(ui.item.label);
            $("#selectedCustomerId").val(ui.item.customerId);
            event.preventDefault();
            $("#phoneNumber").val('');
            $('#addCustomerForm').trigger('submit');
        }
    });
};

const AddCustomer = () => {
    debugger
    $(document).off('submit', '#addCustomerForm').on('submit', '#addCustomerForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                ReloadIndexWithPartial();
                debugger
                notification({
                    message: "Product Add successfully!",
                    type: "success",
                    title: "Success"
                });
            },
            error: function (xhr, status, error) {
                console.error('Error adding product:', error);
                alert('An error occurred while adding the product to the cart.');
            }
        });
    });
};

const AddToCart = () => {
    $(document).off('submit', '#addProductForm').on('submit', '#addProductForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                $('#ProductListPartial').html(result);
                ReloadIndexWithPartial();
            },
            error: function (xhr, status, error) {
                console.error('Error adding product:', error);
                alert('An error occurred while adding the product to the cart.');
            }
        });
    });
};

const ReloadIndexWithPartial = () => {
    $.ajax({
        url: '/NewOrder/Index?isPartial=true',
        type: 'GET',
        success: function (result) {
            $('#ProductVawser').html(result);
            initializeFunctions(); // Reinitialize functions after partial view load
        },
        error: function (xhr, status, error) {
            console.error('Error reloading index:', error);
            alert('An error occurred while updating the customer and payment section.');
        }
    });
};

const DeleteCartItem = () => {
    $(document).off('submit', '#deleteItem').on('submit', '#deleteItem', function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                $('#ProductListPartial').html(result);
                ReloadIndexWithPartial();
            },
            error: function (xhr, status, error) {
                console.error('Error deleting item:', error);
                alert('An error occurred while deleting the product from the cart.');
            }
        });
    });
};


const UpdateCartItem = () => {
    $(document).off('click', '.update-item-btn').on('click', '.update-item-btn', async function () {
        // Get the product ID from the button's data attribute
        const productId = $(this).data('product-id');

        // Find the corresponding row inputs for quantity and discount using product ID
        const quantity = $(`input.product-quantity[data-product-id="${productId}"]`).val();
        const discount = $(`input.product-discount[data-product-id="${productId}"]`).val();

        // Debugging outputs
        console.log(`Updating product ${productId} with quantity ${quantity} and discount ${discount}`);

        // Preparing data to be sent in the AJAX request
        const data = {
            productId: productId,
            quantity: quantity,
            discount: discount
        };

        // AJAX request to update product item
        $.ajax({
            url: '/NewOrder/UpdateProductItem', // Update the URL to the correct endpoint
            type: 'POST',
            data: data,
            success: function (result) {
                // Update the product list partial view with the response
                $('#ProductListPartial').html(result);
                ReloadIndexWithPartial();

                // Show success notification
                notification({
                    message: "Product updated successfully!",
                    type: "success",
                    title: "Success"
                });
            },
            error: function (xhr, status, error) {
                console.error('Error updating item:', error);

                // Show error notification
                notification({
                    message: "An error occurred while updating the product in the cart. Please try again!",
                    type: "error",
                    title: "Error"
                });
            }
        });
    });
};



const newProductModalHandling = () => {
    $('#addNewProductButton').off('click').on('click', function () {
        $('#createProductModal').modal('show');
    });

    $('#createProductForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                $('#createProductModal').modal('hide');
                $('#ProductListPartial').load('/Product/GetProductList');
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
};
const newCustomerModalHandling = () => {
    $('#addNewCustomerButton').off('click').on('click', function () {
        $('#createCustomerModal').modal('show');
    });

    $('#CreatCustomerForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#createCustomerModal').modal('hide');
                    notification({
                        message: "Customer Create successfully!",
                        type: "success",
                        title: "Success"
                    });
                }
                //$('#ProductListPartial').load('/Product/GetProductList');
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
};

const newCategoryModalHandling = () => {
    $('#addNewCatagoryButton').off('click').on('click', function () {
        $('#CategoryModelCreate').modal('show');
    });

    $('#CreateCategoryForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#CategoryModelCreate').modal('hide');
                    notification({
                        message: "Category Create successfully!",
                        type: "success",
                        title: "Success"
                    });
                }
                //$('#ProductListPartial').load('/Product/GetProductList');
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
};

const newSupplierModalHandling = () => {
    $('#addNewSupplierButton').off('click').on('click', function () {
        $('#SupplierModelCreate').modal('show');
    });

    $('#CreateSupplierForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#SupplierModelCreate').modal('hide');
                    notification({
                        message: "Supplier Create successfully!",
                        type: "success",
                        title: "Success"
                    });
                }
                //$('#ProductListPartial').load('/Product/GetProductList');
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
};
const newUnitMasterModalHandling = () => {
    $('#addNewMasterUnitButton').off('click').on('click', function () {
        $('#UnitMasterModelCreate').modal('show');
    });

    $('#CreateUnitMasterForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = new FormData(form[0]);

        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#UnitMasterModelCreate').modal('hide');
                    notification({
                        message: "Master Unit Create successfully!",
                        type: "success",
                        title: "Success"
                    });
                }
                //$('#ProductListPartial').load('/Product/GetProductList');
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
};

const newUnitChildModalHandling = () => {
    $('#addNewChildUnitButton').off('click').on('click', function () {
        $('#UnitChildMUodelCreate').modal('show');
    });

    $('#CreateUnitChildForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var formData = new FormData(form[0]);
        debugger
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#UnitChildMUodelCreate').modal('hide');
                
                    notification({
                        message: "Child Unit Create successfully!",
                        type: "success",
                        title: "Success"
                    });
                }
                
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
};
window.processPayment = function () {
    debugger
    $.ajax({
        url: '/NewOrder/Payment',
        type: 'POST',
        success: function (response) {
            debugger
            // Load the partial view response into the modal container
            $('#modalContainer').html(response);
            // Show the modal using Bootstrap's modal method
            $('#successModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.log('An error occurred while processing the payment: ', error);
        }
    });
};
