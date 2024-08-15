import { createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
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
                customer: customer?.customerName ?? "No Name",
                doctor: prescription?.doctorName ?? "No Address",
                date: prescription?.prescriptionDate ?? "No Address",
                details: prescription?.medicationDetails ?? "No Address",
                inst: prescription?.dosageInstructions ?? "No Address",
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
$('#CreateBtn').click(async () => {
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Customer/GetAll', '#CustomerDropdown', 'id', 'customerName', "Select Customer");
});

// Save Button

$('#btnSave').click(async () => {
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
                displayNotification({ formId: '#PrescriptionForm', modalId: '#modelCreate', message: ' Prescription was successfully Created....' });
                await getPrescriptionList(); // Update the user list
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        displayNotification({ formId: '#PrescriptionForm', modalId: '#modelCreate', messageElementId: '#globalErrorMessage', message: 'Prescription Create failed. Please try again.' });
    }
});



window.updatePrescription = async (id) => {
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
        $('#btnUpdate').on('click', async () => {
            debugger
            const formData = $('#PrescriptionForm').serialize();
            const result = await SendRequest({ endpoint: '/Prescription/Update/' + id, method: "PUT", data: formData });
            if (result.success) {
                displayNotification({ formId: '#PrescriptionForm', modalId: '#modelCreate', message: ' Prescription was successfully Updated....' });
                await getPrescriptionList(); // Update the user list
            }
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deletePrescription = async (id) => {
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Prescription/Delete', method: "POST", data: { id: id } });
        if (result.success) {
            displayNotification({
                formId: '#PrescriptionForm',
                modalId: '#deleteAndDetailsModel',
                message: 'Prescription was successfully deleted....'
            });
            await getPrescriptionList(); // Update the category list
        } else {
            // Display the error message in the modal
            $('#DeleteErrorMessage').removeClass('alert-success').addClass('text-danger').text(result.detail).show();
        }
    });
}