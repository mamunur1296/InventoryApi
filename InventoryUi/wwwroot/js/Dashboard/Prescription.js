import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getPrescriptionList();
});
const getPrescriptionList = async () => {
    debugger
    const prescriptions = await SendRequest({ endpoint: '/Prescription/GetAll' });
    const customers = await SendRequest({ endpoint: '/Customer/GetAll' });
    if (prescriptions.status === 200 && prescriptions.success) {
        await onSuccessUsers(prescriptions.data, customers.data);
    }
}

const onSuccessUsers = async (prescriptions, customers) => {
    debugger
    const customersMap = dataToMap(customers, 'id');
    const prescriptionsitem = prescriptions.map((prescription) => {
        if (prescription) {
            debugger
            const customer = customersMap[prescription.customerID];
            return {
                id: prescription?.id,
                customer: customer?.customerName ?? "N/A",
                doctor: prescription?.doctorName ?? "N/A",
                date: prescription?.prescriptionDate ?? "N/A",
                details: prescription?.medicationDetails ?? "N/A",
                inst: prescription?.dosageInstructions ?? "N/A",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => row?.customer
            },
            {
                render: (data, type, row) => row?.doctor
            },{
                render: (data, type, row) => row?.date
            },{
                render: (data, type, row) => row?.details
            },{
                render: (data, type, row) => row?.inst
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updatePrescription' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deletePrescription' }
                ])
            }
        ];
        if (prescriptions) {
            await initializeDataTable(prescriptionsitem, userSchema, 'PrescriptionTable');
        }
    } catch (error) {
        console.error('Error processing company data:', error);
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

//$.validator.addMethod("checkDuplicateWarehouseName", createDuplicateCheckValidator(
//    "/Warehouse/CheckDuplicate",
//    "WarehouseName"
//));






// Initialize validation
const UsrValidae = $('#PrescriptionForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        CustomerID: {
            required: true,

        },
        DoctorName: {
            required: true,

        },
        PrescriptionDate: {
            required: true,

        },
        MedicationDetails: {
            required: true,

        },
        DosageInstructions: {
            required: true,

        }

    },
    messages: {
        CustomerID: {
            required: "Customer  is required.",
        },
        DoctorName: {
            required: "Doctor Name is required.",
        },
        PrescriptionDate: {
            required: "Prescription Date is required.",
        },
        MedicationDetails: {
            required: "Medication Details are required.",
        },
        DosageInstructions: {
            required: "Dosage Instructions are required.",
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
    resetFormValidation('#PrescriptionForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#PrescriptionForm').valid()) {
            const formData = $('#PrescriptionForm').serialize();
            const result = await SendRequest({ endpoint: '/Prescription/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Prescription Created successfully !", type: "success", title: "Success" });
                await getPrescriptionList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error" });
                $('#modelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Prescription Created failed . Please try again. !", type: "error", title: "Error" });
    }

});



window.updatePrescription = async (id) => {
    resetFormValidation('#PrescriptionForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
    const result = await SendRequest({ endpoint: '/Prescription/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        $('#CustomerDropdown').val(result.data.customerID);
        $('#DoctorName').val(result.data.doctorName);
        $('#PrescriptionDate').val(result.data.prescriptionDate);
        $('#MedicationDetails').val(result.data.medicationDetails);
        $('#DosageInstructions').val(result.data.dosageInstructions);

        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#PrescriptionForm');
        $('#btnUpdate').off('click').on('click', async () => {
            debugger
            const formData = $('#PrescriptionForm').serialize();
            const result = await SendRequest({ endpoint: '/Prescription/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                $('#modelCreate').modal('hide');
                notification({ message: "Prescription Updated successfully !", type: "success", title: "Success" });

                await getPrescriptionList(); // Update the user list
            } else {
                $('#modelCreate').modal('hide');
                notification({ message: " Prescription Updated failed . Please try again. !", type: "error", title: "Error" });
            }

        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deletePrescription = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Prescription/Delete', method: "DELETE", data: { id: id } });

        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Prescription  Deleted successfully !", type: "success", title: "Success" });
            await getPrescriptionList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });

        }

    });
}