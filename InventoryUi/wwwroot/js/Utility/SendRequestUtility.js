export const SendRequest = async ({ endpoint, method = 'GET', data = null, headers = {}, dataType = 'json' }) => {
    // Validate and set default method

    const validMethods = ['GET', 'POST', 'PUT', 'DELETE', 'PATCH'];
    method = validMethods.includes(method.toUpperCase()) ? method.toUpperCase() : 'GET';

    // Validate and set default data type
    const validDataTypes = ['json', 'text', 'html', 'xml', 'script'];
    dataType = validDataTypes.includes(dataType) ? dataType : 'json';

    // Determine content type based on data
    let contentType = null;
    if (data) {
        if (data instanceof FormData) {
            contentType = 'multipart/form-data'; // FormData automatically sets its own content type
        } else if (method === 'POST') {
            contentType = 'application/x-www-form-urlencoded';
        } else if (method === 'PUT') {
            contentType = 'application/x-www-form-urlencoded';
        } 
        else if (typeof data === 'object') {
            contentType = 'application/json;charset=utf-8';
        }
    }

    // Prepare request data
    let requestData = null;
    if (data) {
        switch (contentType) {
            case 'application/json;charset=utf-8':
                requestData = JSON.stringify(data);
                break;
            case 'application/x-www-form-urlencoded':
                requestData = new URLSearchParams(data).toString();
                break;
            case 'multipart/form-data':
                requestData = new FormData();
                Object.keys(data).forEach(key => requestData.append(key, data[key]));
                // FormData handles its own content type, so we do not set it
                delete headers['Content-Type'];
                break;
        }
    }

    // Setup fetch options
    const options = {
        method,
        headers: {
            ...headers,
            ...(contentType && contentType !== 'multipart/form-data' ? { 'Content-Type': contentType } : {})
        },
        ...(requestData ? { body: requestData } : {})
    };

    // Perform fetch request
    try {
        const response = await fetch(endpoint, options);
        const result = dataType === 'json' ? await response.json() : await response.text();

        if (response.ok) {
            return result;
        } else {
            throw new Error(result || 'Network response was not ok.');
        }
    } catch (error) {
        handleError(error.message || 'Unknown error occurred.');
        throw new Error(error.message || 'Network response was not ok.');
    }
};


export function handleError(message) {
    console.error('Error:', message);
}

export const populateDropdown = async (endpoint, dropdownSelector, valueField, textField, defaultOption = '') => {
    debugger
    try {
        const response = await SendRequest({ endpoint: endpoint });
        const data = response.data;
        
        // Clear existing options
        $(dropdownSelector).empty();

        // Add default option
        if (defaultOption !== null) {
            $(dropdownSelector).append(`<option value="">${defaultOption}</option>`);
        }

        // Check if data is null or empty
        if (!data || data.length === 0) {
            $(dropdownSelector).append('<option value="">No data available</option>');
            return;
        }

        // Add options from the fetched data
        $.each(data, function (index, item) {
            $(dropdownSelector).append(`<option value="${item[valueField]}">${item[textField]}</option>`);
        });
    } catch (error) {
        console.error(`Error populating ${dropdownSelector}:`, error);
        // Handle error
        $(dropdownSelector).empty();
        $(dropdownSelector).append('<option value="">Error fetching data</option>');
    }
}
