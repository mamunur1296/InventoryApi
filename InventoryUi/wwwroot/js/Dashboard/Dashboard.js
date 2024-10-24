import { mackCustomer, mackEmployee } from '../utility/objectmaping.js';
import {  createActionButtons,  initializeDataTable, loger } from '../utility/helpers.js';
import { SendRequest } from '../utility/sendrequestutility.js';
import { notification } from '../utility/notification.js';

$(document).ready(async function () {
    await getemployeesList();
});
const getemployeesList = async () => {
    debugger
    const employees = await SendRequest({ endpoint: '/Dashboard/GetNotApprovedEmployees' });
    if (employees !==null ) {
        await onSuccessUsers(employees);
    }
}

const onSuccessUsers = async (employees) => {
    debugger
    const employeesitem = employees.map((employee) => {
        if (employee) {
            debugger
            return {
                id: employee?.id,
                name: `${employee.firstName} ${employee.lastName}` ,
                userName: employee?.userName ?? "No Address",
                email: employee?.email ?? "No Data",
                phone: employee?.phoneNumber ?? "No Data",
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
                render: (data, type, row) => row?.userName
            },
            {
                render: (data, type, row) => row?.email
            },
            {
                render: (data, type, row) => row?.phone
            },
            {
                render: (data, type, row) => createActionButtons(row, [
                    { label: 'Approved', btnClass: 'btn-primary', callback: 'isEmployee' },
                    { label: 'Add Cutomer', btnClass: 'btn-primary', callback: 'isCustomer'},
                    { label: 'Not Approved', btnClass: 'btn-danger', callback: 'isNotApproved'},
                ])
            }
        ];
        if (employees) {
            await initializeDataTable(employeesitem, userSchema, 'EmployeeTable');
        }
    } catch (error) {
        console.error('Error processing Category data:', error);
    }
}

window.isEmployee = async (id) => {
    loger(id);
    const result = await SendRequest({ endpoint: '/Employee/CreateEmployee/' + id });
    if (result.success) {
        notification({ message: result.detail, type: "success", title: "Success" });
        await getemployeesList(); // Reload the employees list on success
    } else {
        // Check if the error message contains the specific registration error
        if (result.detail.includes("An error occurred during Employee registration:")) {
            // Display a custom error message for an existing customer
            notification({ message: "This Employee is already registered!", type: "error", title: "Error", time: 0 });
        } else {
            // Display the default error message from the response if it's not a known case
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });
        }
    }
};


window.isNotApproved = async (id) => {
    debugger;
    const result = await SendRequest({ endpoint: '/Employee/NotApprovedEmployee/' + id });
    loger(result);

    if (result.success) {
        notification({ message: result.detail, type: "success", title: "Success" });
        await getemployeesList(); // Reload the employees list on success
    } else {
        // Check if the error message contains the specific registration error
        if (result.detail.includes("An error occurred during customer registration:")) {
            // Display a custom error message for an existing customer
            notification({ message: "This customer is already registered!", type: "error", title: "Error", time: 0 });
        } else {
            // Display the default error message from the response if it's not a known case
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });
        }
    }
}
window.isCustomer = async (id) => {
    debugger;
    const result = await SendRequest({ endpoint: '/Customer/CreateCustomer/' + id });
    loger(result);

    if (result.success) {
        notification({ message: result.detail, type: "success", title: "Success" });
        await getemployeesList(); // Reload the employees list on success
    } else {
        // Check if the error message contains the specific registration error
        if (result.detail.includes("An error occurred during customer registration:")) {
            // Display a custom error message for an existing customer
            notification({ message: "This customer is already registered!", type: "error", title: "Error", time: 0 });
        } else {
            // Display the default error message from the response if it's not a known case
            notification({ message: result.detail, type: "error", title: "Error", time: 0 });
        }
    }
}



//window.isCustomer = async (id) => {
//    debugger
//    const result = await mackCustomer(id);
//    if (result) {
//        await getemployeesList();
//        notification({ message: "SuccessFully Add Customer", type: "success", title: "Success" });
//    }
//}


//window.isEmployee = async (id) => {
//    debugger
//    const result = await mackEmployee(id);
//    if (result) {
//        await getemployeesList();
//        notification({ message: "Successfully Approved Employee", type: "success", title: "Success" });
//    }
//};





