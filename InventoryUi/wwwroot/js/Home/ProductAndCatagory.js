import { loger } from "../utility/helpers.js";
import { SendRequest } from "../utility/sendrequestutility.js";

$(document).ready(async function () {
    loger("This is category page");
    await CatagoryList();
    await productList();
});

// Fetch and display categories
const CatagoryList = async () => {
    debugger;
    const categoryResponse = await SendRequest({ endpoint: '/Category/GetAll' });

    if (categoryResponse.status === 200 && categoryResponse.success) {
        var $categoryList = $('#category-list'); // Targeting the ul element in the HTML

        // Loop through the categories and append them to the list
        categoryResponse.data.forEach(function (category) {
            $categoryList.append(`
                <div class="form-check">
                    <input class="form-check-input custom-checkbox" type="checkbox" id="cat-${category.id}">
                    <label class="form-check-label" for="cat-${category.id}">
                        ${category.categoryName}
                    </label>
                </div>
            `);
        });
    }
};

const productList = async () => {
    try {
        const products = await SendRequest({ endpoint: '/Product/GetAll' });

        if (products.status === 200 && products.success) {
            const $container = $('#product-container');
            $container.empty(); // Clear previous content

            products.data.forEach(function (product) {
                $container.append(`
                    <div class="col-md-3 mb-4">
                        <div class="card" style="width: 100%;">
                            <img src="/images/Product/${product.imageURL}"
                                 alt="${product.productName}"
                                 class="card-img-top"
                                 width="150"
                                 height="200"
                                 onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';">
                            <div class="card-body">
                                <p class="text-success">${product.unitsInStock > 0 ? 'In Stock' : 'Out of Stock'}</p>
                                <h5 class="card-title">${product.productName}</h5>
                                <!-- Star Rating -->
                                <div class="d-flex align-items-center mb-2">
                                    ${generateStars(product.unitPrice)} <!-- Assuming unitPrice is used for star rating, adjust as needed -->
                                    <span class="ms-2">(${product.unitsInStock})</span>
                                </div>

                                <!-- Pricing -->
                                <div>
                                    ${product.totalPriceWithoutDiscount > product.unitPrice
                        ? `<span class="text-decoration-line-through text-muted">${formatPrice(product.totalPriceWithoutDiscount)}</span>`
                        : ''}
                                    <span class="fw-bold fs-5 d-block">${formatPrice(product.unitPrice)}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
            });
        }
    } catch (error) {
        console.error('Error fetching products:', error);
    }
};

function generateStars(rating) {
    const totalStars = 5;
    let starsHtml = '';
    for (let i = 1; i <= totalStars; i++) {
        const starClass = i <= rating ? 'fas fa-star text-warning' : 'fas fa-star';
        starsHtml += `<i class="${starClass}"></i>`;
    }
    return starsHtml;
}

function formatPrice(price) {
    return `৳${price.toFixed(2)}`;
}







