
import { CatagoryValidae, ProductValidator, SupplierValidate, validateUnitChildForm, validateUnitMasterForm } from "../utility/allvalidator.js";
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
    debugger
    $(document).off('submit', '#addProductForm').on('submit', '#addProductForm', function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (result, textStatus, jqXHR) {
                debugger
                var contentType = jqXHR.getResponseHeader("Content-Type");
                if (contentType && contentType.includes("text/html")) {
                    debugger
                    $('#ProductListPartial').html(result);
                    ReloadIndexWithPartial();
                } else if (contentType && contentType.includes("application/json")) {
                    if (!result.success) {
                        debugger
                        notification({
                            message: result.message,
                            type: "error",
                            title: "error"
                        });
                    }
                }
               
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
        debugger
        // AJAX request to update product item
        $.ajax({
            url: '/NewOrder/UpdateProductItem', // Update the URL to the correct endpoint
            type: 'POST',
            data: data,
            success: function (result, textStatus, jqXHR) {
                debugger
                var contentType = jqXHR.getResponseHeader("Content-Type");
                if (contentType && contentType.includes("text/html")) {
                    // Update the product list partial view with the response
                    $('#ProductListPartial').html(result);
                    ReloadIndexWithPartial();

                    // Show success notification
                    notification({
                        message: "Product updated successfully!",
                        type: "success",
                        title: "Success"
                    });
                } else if (contentType && contentType.includes("application/json")) {
                    if (!result.success) {
                        notification({
                            message: result.message,
                            type: "error",
                            title: "error"
                        });
                    }
                }
                
            },
            error: function (xhr, status, error) {
                console.error('Error updating item:', error);
                debugger
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
    ProductValidator('#createProductForm');
    $('#createProductForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        if ($('#createProductForm').valid()) {
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
                        $('#createProductModal').modal('hide');
                        form[0].reset();
                        $('#ProductListPartial').load('/Product/GetProductList');
                        notification({
                            message: "Product Create successfully!",
                            type: "success",
                            title: "Success"
                        });
                    }
                    
                },
                error: function (error) {
                    console.log(error);
                    notification({
                        message: "Product Create Faild.",
                        type: "error",
                        title: "error"
                    });
                }
            });
        }

    });


};


const newCategoryModalHandling = () => {
    $('#addNewCatagoryButton').off('click').on('click', function () {
        $('#CategoryModelCreate').modal('show');
    });

    CatagoryValidae('#CreateCategoryForm');
    $('#CreateCategoryForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        if ($('#CreateCategoryForm').valid()) {
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
                        form[0].reset();
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
        }

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
                    form[0].reset();
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
const newSupplierModalHandling = () => {
    // Show the Supplier creation modal when the button is clicked
    $('#addNewSupplierButton').off('click').on('click', function () {
        $('#SupplierModelCreate').modal('show');
    });

    // Initialize validation for the Supplier form
    SupplierValidate('#CreateSupplierForm');

    // Handle form submission
    $('#CreateSupplierForm').off('submit').on('submit', function (e) {
        e.preventDefault();

        // Check if the form is valid
        if ($('#CreateSupplierForm').valid()) {
            var form = $(this);
            var formData = new FormData(form[0]);

            // Perform the AJAX request to submit the form
            $.ajax({
                url: form.attr('action'),
                type: form.attr('method'),
                data: formData,
                processData: false, // Don't process the files
                contentType: false, // Set to false for file uploads
                success: function (response) {
                    if (response.success) {
                        // Hide the modal if the form was successfully submitted
                        $('#SupplierModelCreate').modal('hide');
                        form[0].reset();
                        // Show success notification
                        notification({
                            message: "Supplier created successfully!",
                            type: "success",
                            title: "Success"
                        });

                        // Optionally, reload the supplier list or some other content
                        // $('#SupplierListPartial').load('/Supplier/GetSupplierList');
                    }
                },
                error: function (error) {
                    // Handle errors here
                    console.log(error);
                    notification({
                        message: "Failed to create supplier.",
                        type: "error",
                        title: "Error"
                    });
                }
            });
        }
    });
};
const newUnitMasterModalHandling = () => {
    $('#addNewMasterUnitButton').off('click').on('click', function () {
        $('#UnitMasterModelCreate').modal('show');
    });
    validateUnitMasterForm('#CreateUnitMasterForm');
    $('#CreateUnitMasterForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        if ($('#CreateUnitMasterForm').valid()) {
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
                        form[0].reset();
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
        }
  
    });
};
const newUnitChildModalHandling = () => {
    $('#addNewChildUnitButton').off('click').on('click', function () {
        $('#UnitChildMUodelCreate').modal('show');
    });
    validateUnitChildForm('#CreateUnitChildForm');
    $('#CreateUnitChildForm').off('submit').on('submit', function (e) {
        e.preventDefault();
        if ($('#CreateUnitChildForm').valid()) {
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
                        form[0].reset();

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
        }
        
    });
};
window.processPayment = function () {
    debugger;
    $.ajax({
        url: '/NewOrder/Payment',
        type: 'POST',
        success: function (response, textStatus, jqXHR) {
            debugger
            // Check the content type of the response
            var contentType = jqXHR.getResponseHeader("Content-Type");

            if (contentType && contentType.includes("text/html")) {
                // Handle HTML partial view response
                $('#modalContainer').html(response);
                $('#successModal').modal('show');
            } else if (contentType && contentType.includes("application/json")) {
                // Handle JSON response
                debugger

                if (response.success) {
                    debugger
                    // Show the success modal if JSON response indicates success
                    //$('#modalContainer').html(response); // If you include HTML in the JSON response

                    var orderId = response.message;
                    var isDownload = true;
                    $('#downloadReceiptLink').attr('href', '/PosReport/DownloadInvoice?id=' + orderId + '&isDownload=' + isDownload);
                    $('#successModal').modal('show');
                } else {
                    debugger
                    // Trigger error notification if JSON response indicates failure
                    notification({
                        message: response.message,
                        type: "error",
                        title: "error"
                    });
                }
            } else {
                // Handle unexpected response
                notification({
                    message: "Unexpected response format.",
                    type: "error",
                    title: "error"
                });
            }
        },
        error: function (xhr, status, error) {
            debugger;
            console.log('An error occurred while processing the payment: ', error);

            // Trigger error notification for AJAX errors
            notification({
                message: "An error occurred while processing the payment. Please try again.",
                type: "error",
                title: "error"
            });
        }
    });
};


