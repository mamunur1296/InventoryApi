import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

$(document).ready(async function () {
    await getEmployeeList();
});
const getEmployeeList = async () => {
    debugger
    const employeees = await SendRequest({ endpoint: '/Employee/GetAll' });
    const company = await SendRequest({ endpoint: '/Company/GetAll' });
    const users = await SendRequest({ endpoint: '/DashboardUser/GetAll' });
    const branchs = await SendRequest({ endpoint: '/Branch/GetAll' });
    if (employeees.status === 200 && employeees.success) {
        debugger
        await onSuccessUsers(employeees.data, users.data, company.data, branchs.data);
    }
}
const onSuccessUsers = async (employeees, users, company, branchs) => {
    debugger
    const managerMap = dataToMap(employeees, 'id');
    const usersMap = dataToMap(users, 'id');
    const companyMap = dataToMap(company, 'id');
    const branchsMap = dataToMap(branchs, 'id');
    const employeeesitem = employeees.map((employee) => {
        if (employee) {
            debugger
            const manager = managerMap[employee?.managerId];
            const user = usersMap[employee.userId];
            const company = companyMap[employee.companyId];
            const branch = branchsMap[employee.branchId];
            return {
                id: employee?.id,
                fName: employee?.firstName ?? "Null",
                lName: employee?.lastName ?? "Null",
                title: employee?.title ?? "Null",
                address: employee?.address ?? " "  + employee?.city ?? " " + ", " + employee?.region ?? " " + ", " + employee?.postalCode ?? " " + ", " + employee?.country ?? " " ?? "Null",
                phone: employee?.homePhone ?? "Null",
                company: company?.name ?? " Null",
                branch: branch?.name ?? " Null",
                manager: manager ? (manager?.firstName + " " + manager?.lastName) : " Null",
                name: employee ? (employee?.firstName + " " + employee?.lastName) : " Null",
                photo: employee?.photoPath ?? "Null",
                user: user?.userName ?? "Null",
            };
        }
        return null;
    }).filter(Boolean);

    try {
        debugger
        const userSchema = [
            {
                render: (data, type, row) => `<img src="images/Employee/${row.photo}" alt="User Avatar" class="rounded-circle" style="width: 30px; height: 30px; object-fit: cover;" onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';" />`
            },
            {
                render: (data, type, row) => row?.name
            },
            {
                render: (data, type, row) => row?.title
            },
            {
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => row?.address
            },
            {
                render: (data, type, row) => row?.company
            },
            {
                render: (data, type, row) => row?.branch
            },
            {
                render: (data, type, row) => row?.manager
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Edit', btnClass: 'btn-primary', callback: 'updateEmployee' },
                    { label: 'Details', btnClass: 'btn-info', callback: 'showDetails', disabled: true },
                    { label: 'Delete', btnClass: 'btn-danger', callback: 'deleteEmployee' }
                ])
            }
        ];
        if (employeees) {
            await initializeDataTable(employeeesitem, userSchema, 'EmployeeTable');
        }
    } catch (error) {
        console.error('Error processing Employee data:', error);
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
const UsrValidae = $('#EmployeeForm').validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        LastName: {
            required: true,
        },
        FirstName: {
            required: true,
        },
        BranchId: {
            required: true,
        },
        CompanyId: {
            required: true,
        },
        DepartmentId: {
            required: true,
        },
        UserId: {
            required: true,
        },
        Salary: {
            required: true,
        },
        //TitleOfCourtesy: {
        //    required: true,
        //},
        //BirthDate: {
        //    required: true,
        //},
        //HireDate: {
        //    required: true,
        //},
        //Address: {
        //    required: true,
        //},
        //City: {
        //    required: true,
        //},
        //Region: {
        //    required: true,
        //},
        //PostalCode: {
        //    required: true,
        //},
        //Country: {
        //    required: true,
        //},
        //HomePhone: {
        //    required: true,
        //},
        //Extension: {
        //    required: true,
        //},
        //Notes: {
        //    required: true,
        //},
        //ReportsTo: {
        //    required: true,
        //},
        //Files: {
        //    required: true,
        //}
    },
    messages: {
        LastName: {
            required: "Last Name is required.",
        },
        FirstName: {
            required: "First Name is required.",
        },
        Title: {
            required: "Title is required.",
        },
        TitleOfCourtesy: {
            required: "Title of Courtesy is required.",
        },
        BirthDate: {
            required: "Birth Date is required.",
        },
        HireDate: {
            required: "Hire Date is required.",
        },
        Address: {
            required: "Address is required.",
        },
        City: {
            required: "City is required.",
        },
        Region: {
            required: "Region is required.",
        },
        PostalCode: {
            required: "Postal Code is required.",
        },
        Country: {
            required: "Country is required.",
        },
        HomePhone: {
            required: "Home Phone is required.",
        },
        Extension: {
            required: "Extension is required.",
        },
        Notes: {
            required: "Notes are required.",
        },
        ReportsTo: {
            required: "Reports To is required.",
        },
        Files: {
            required: "Photo Path is required.",
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
    resetFormValidation('#EmployeeForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    showCreateModal('modelCreate', 'btnSave', 'btnUpdate');
    await populateDropdown('/Employee/GetAll', '#ManagerDropdown', 'id', 'firstName', "Select Manager");
    await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'name', "Select Company");
    await populateDropdown('/Branch/GetAll', '#BranchDropdown', 'id', 'name', "Select Branch");
    await populateDropdown('/Department/GetAll', '#DepartmentDropdown', 'id', 'departmentName', "Select Department");
});

// Save Button

$('#btnSave').off('click').click(async () => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    try {
        if ($('#EmployeeForm').valid()) {
            //const formData = $('#EmployeeForm').serialize();
            const formData = new FormData($('#EmployeeForm')[0]);
            const result = await SendRequest({ endpoint: '/Employee/Create', method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $('#modelCreate').modal('hide');
                notification({ message: "Employee Created successfully !", type: "success", title: "Success" });
                await getEmployeeList(); // Update the user list
            } else {
                notification({ message: result.detail, type: "error", title: "Error" });
                $('#modelCreate').modal('hide');
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $('#modelCreate').modal('hide');
        notification({ message: " Employee Created failed . Please try again. !", type: "error", title: "Error" });
    }
});



window.updateEmployee = async (id) => {
    resetFormValidation('#EmployeeForm', UsrValidae);
    clearMessage('successMessage', 'globalErrorMessage');
    loger(id);
    debugger
    $('#myModalLabelUpdateEmployee').show();
    $('#myModalLabelAddEmployee').hide();
    await populateDropdown('/Employee/GetAll', '#ManagerDropdown', 'id', 'firstName', "Select Manager");
    await populateDropdown('/DashboardUser/GetAll', '#UserDropdown', 'id', 'userName', "Select User");
    await populateDropdown('/Company/GetAll', '#CompanyDropdown', 'id', 'name', "Select Company");
    await populateDropdown('/Branch/GetAll', '#BranchDropdown', 'id', 'name', "Select Branch");
    await populateDropdown('/Department/GetAll', '#DepartmentDropdown', 'id', 'departmentName', "Select Department");
    const result = await SendRequest({ endpoint: '/Employee/GetById/' + id });
    if (result.success) {
        $('#btnSave').hide();
        $('#btnUpdate').show();

        debugger
        $('#LastName').val(result.data.lastName);
        $('#FirstName').val(result.data.firstName);
        $('#Title').val(result.data.title);
        $('#TitleOfCourtesy').val(result.data.titleOfCourtesy);
        $('#BirthDate').val(result.data.birthDate);
        $('#HireDate').val(result.data.hireDate);
        $('#Address').val(result.data.address);
        $('#City').val(result.data.city);
        $('#Region').val(result.data.region);
        $('#Country').val(result.data.country);
        $('#HomePhone').val(result.data.homePhone);
        $('#Extension').val(result.data.extension);
        $('#Photo').val(result.data.photo);
        $('#Notes').val(result.data.notes);
        $('#ReportsTo').val(result.data.reportsTo);
        $('#PhotoPath').val(result.data.photoPath);
        $('#Files').val(result.data.files);
        $('#Photo').val(result.data.photo);
        $('#PostalCode').val(result.data.postalCode);
        $('#ManagerDropdown').val(result.data.managerId);
        $('#CompanyDropdown').val(result.data.companyId);
        $('#UserDropdown').val(result.data.userId);
        $('#BranchDropdown').val(result.data.branchId);
        $('#HireDate').val(result.data.hireDate);
        $('#Salary').val(result.data.salary);
        $('#DepartmentDropdown').val(result.data.departmentId);
        


        $('#modelCreate').modal('show');
        resetValidation(UsrValidae, '#EmployeeForm');
        $('#btnUpdate').off('click').on('click', async () => {
            if ($('#EmployeeForm').valid()) {
                debugger
                //const formData = $('#EmployeeForm').serialize();
                const formData = new FormData($('#EmployeeForm')[0]);
                const result = await SendRequest({ endpoint: '/Employee/Update/' + id, method: "PUT", data: formData });
                if (result.success) {
                    $('#modelCreate').modal('hide');
                    notification({ message: "Employee Updated successfully !", type: "success", title: "Success" });
                    await getEmployeeList(); // Update the user list
                } else {
                    $('#modelCreate').modal('hide');
                    notification({ message: " Employee Updated failed . Please try again. !", type: "error", title: "Error" });
                }
            }
            
        });
    }
    loger(result);
}




//window.showDetails = async (id) => {
//    loger("showDetails id " + id);
//}


window.deleteEmployee = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#deleteAndDetailsModel').modal('show');
    $('#companyDetails').empty();
    $('#DeleteErrorMessage').hide();
    $('#btnDelete').off('click').click(async () => {
        debugger
        const result = await SendRequest({ endpoint: '/Employee/Delete', method: "DELETE", data: { id: id } });
        loger(result);
        if (result.success) {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: "Employee Deleted successfully !", type: "success", title: "Success" });
            await getEmployeeList(); // Update the category list
        } else {
            $('#deleteAndDetailsModel').modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}