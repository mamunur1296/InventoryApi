export async function ajaxCall({ endpoint, method = 'GET', data = null, headers = {}, contentType = 'application/json', dataType = 'json' }) {
    const url = endpoint;
    const options = {
        method: method,
        headers: {
            'Content-Type': contentType,
            ...headers
        }
    };

    if (data) {
        if (contentType === 'application/json') {
            options.body = JSON.stringify(data);
        } else if (contentType === 'application/x-www-form-urlencoded') {
            options.body = new URLSearchParams(data).toString();
        }
    }

    try {
        const response = await fetch(url, options);
        const result = dataType === 'json' ? await response.json() : await response.text();

        if (response.ok) {
            return result;
        } else {
            throw new Error(result || 'Network response was not ok.');
        }
    } catch (error) {
        handleError(error.message);
        throw error;
    }
}

export function handleError(message) {
    console.error('Error:', message);
}
