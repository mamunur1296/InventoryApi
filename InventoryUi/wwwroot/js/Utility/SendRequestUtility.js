﻿export const SendRequest = async ({ endpoint, method = 'GET', data = null, headers = {}, dataType = 'json' }) => {
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
        } else if (typeof data === 'object') {
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
