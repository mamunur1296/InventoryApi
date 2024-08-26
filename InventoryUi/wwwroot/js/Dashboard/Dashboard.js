import { notification } from '../Utility/notification.js';
import { clearMessage, createActionButtons, dataToMap, displayNotification, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal, showExceptionMessage } from '../utility/helpers.js';
import { SendRequest, populateDropdown } from '../utility/sendrequestutility.js';

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
    const users = await SendRequest({ endpoint: '/DashboardUser/GetById/' + id });
    if (users.success) {
        const newEmployee = await mapUserToEmployee(users.data);
        const employee = await SendRequest({ endpoint: '/Employee/Create', method: 'POST', data: newEmployee });
        if (employee.success) {
            const updatedUserData = {
                ...users.data,
                isApprovedByAdmin: true,
                isEmployee: false
            };
            const updateUser = await SendRequest({ endpoint: '/DashboardUser/Update/' + id, method: "PUT", data: updatedUserData })
            if (updateUser.success) {
                await getemployeesList();
                notification({ message: "SuccessFully Approved Employee", type: "success", title: "Success" });
            } else {
                // role back all functions 
            }
        } else {

            notification({ message: "Employee  Create Fauld ! ", type: "error", title: "Error" });
        }

    } else {
        notification({ message: "User Not Found ", type: "error", title: "Error" });
    }
};





window.isCustomer = async (id) => {
    debugger
    const users = await SendRequest({ endpoint: '/DashboardUser/GetById/' + id });
    if (users.success) {
        const newCustomer = await mapUserToCustomer(users.data);
        const employee = await SendRequest({ endpoint: '/Customer/Create', method: 'POST', data: newCustomer });
        if (employee.success) {
            const updatedUserData = {
                ...users.data,
                isApprovedByAdmin: true,
                isEmployee: false
            };
            const updateUser = await SendRequest({ endpoint: '/DashboardUser/Update/' + id, method: "PUT", data: updatedUserData })
            if (updateUser.success) {
                await getemployeesList();
                notification({ message: "SuccessFully Add Customer", type: "success", title: "Success" });
            } else {
                // role back all functions 
            }
        } else {

            notification({ message: "Employee  Create Fauld ! ", type: "error", title: "Error" });
        }

    } else {
        notification({ message: "User Not Found ", type: "error", title: "Error" });
    }
}


const mapUserToEmployee = async (user) => {
    debugger
    const newEmployee = {
        FirstName: user.firstName,
        LastName: user.lastName,
        Title: null,  // Default value or you can set based on your logic
        TitleOfCourtesy: 'No Data',  // Default value or you can set based on your logic
        BirthDate: new Date(),  // Or another date if available in user
        HireDate: new Date(),  // Or another date if applicable
        Address: user.address || 'No Data', // Fallback to empty string if undefined
        City: 'No Data',  // Set based on your logic
        Region: 'No Data',  // Set based on your logic
        PostalCode: 'No Data',  // Set based on your logic
        Country: user.country || 'No Data',  // Use user's country or empty string
        HomePhone: user.phoneNumber || 'No Data', // Use user's phone number or empty string
        Extension: 'No Data',  // Default value or you can set based on your logic
        Notes: user.about || 'No Data',  // Use user's about section or empty string
        ReportsTo: null,  // Default value or set according to your logic
        Photo: null,  // Binary data for photo, set based on your logic
        PhotoPath: user.userImg || 'No Data',  // Use user's image path or empty string
 
    };

    return newEmployee;
};


const mapUserToCustomer = async (user) => {
    debugger;
    const newCustomer = {
        CustomerName: `${user.firstName} ${user.lastName}`, // Combine first and last name
        ContactName: " No Data", // Placeholder or set based on your logic
        ContactTitle: " No Data", // Placeholder or set based on your logic
        Address: user.address || 'Default Address', // Use user's address or default
        City: 'No Data ', // Placeholder or set based on your logic
        Region: 'No Data ', // Placeholder or set based on your logic
        PostalCode: ' No Data', // Placeholder or set based on your logic
        Country: user.country || 'No Data ', // Use user's country or placeholder
        Phone: user.phoneNumber || ' No Data', // Use user's phone number or placeholder
        Fax: 'No Data ', // Placeholder or set based on your logic
        Email: user.email, // Ensure the user's email is provided
        PasswordHash: "No Data", // Assume you have a function to hash passwords
        DateOfBirth: new Date(user.dateOfBirth) || new Date(), // Convert to Date object or use the current date
        MedicalHistory: user.password // Placeholder or set based on your logic
    };

    return newCustomer;
};



