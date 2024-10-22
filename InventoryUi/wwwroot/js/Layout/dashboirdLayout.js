import { SendRequest } from "../utility/sendrequestutility.js";

$(document).ready(async function () {
    try {
        // Fetch company data from the API
        const result = await SendRequest({ endpoint: '/Company/GetAll' });

        // Check if there is company data
        if (result.data && result.data.length > 0) {
            const company = result.data[0]; // Get the first company (adjust if needed)

            // Build the base64 image string using the company logo
            const base64Image = `<img src="data:image/jpeg;base64,${company.logo}" alt="Company Logo" style="width:40px; height:40px;"  class="img-circle elevation-2" onerror="this.onerror=null;this.src='/ProjectRootImg/defoltLogo.png';" />`;

            // Set the company name and inventory text
            $('#company-name').text(company.name ?? "Company Name");
            $('#inventory-text').text("Inventory");
            $('#company-logo').html(base64Image);
        } else {
            // No company data, show default values
            const defaultImage = `<img src="/ProjectRootImg/defoltLogo.png" alt="Default Logo" style="width:40px; height:40px;" class="img-circle elevation-2" />`;

            $('#company-name').text("Company Name");
            $('#inventory-text').text("Inventory");
            $('#company-logo').html(defaultImage);
        }
    } catch (error) {
        console.error('Error fetching company information:', error);

        // Handle error case by showing default values
        const defaultImage = `<img src="/ProjectRootImg/defoltLogo.png" alt="Default Logo" style="width:40px; height:40px;" class="img-circle elevation-2" />`;

        $('#company-name').text("Company Name");
        $('#inventory-text').text("Inventory");
        $('#company-logo').html(defaultImage);
    }
});





