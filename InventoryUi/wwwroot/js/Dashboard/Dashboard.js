import { mackCustomer, mackEmployee } from '../utility/objectmaping.js';
import {  createActionButtons,  initializeDataTable } from '../utility/helpers.js';
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
                    { label: 'Not Approved', btnClass: 'btn-danger', callback: 'isCustomer'},
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
    debugger
    const result = await mackEmployee(id);
    if (result) {
        await getemployeesList();
        notification({ message: "Successfully Approved Employee", type: "success", title: "Success" });
    }
};





window.isCustomer = async (id) => {
    debugger
    const result = await mackCustomer(id);
    if (result) {
        await getemployeesList();
        notification({ message: "SuccessFully Add Customer", type: "success", title: "Success" });
    }
}








