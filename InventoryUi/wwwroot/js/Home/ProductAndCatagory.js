import { loger } from "../utility/helpers.js";
import { SendRequest } from "../utility/sendrequestutility.js";

$(document).ready(async function () {
    loger("This is category page");
    await CatagoryList();
    await fetchAllProducts(); // Fetch all products once
});

// Variables to store products data
let allProducts = [];
let filteredProducts = [];
let currentPage = 1;
const pageSize = 10; // Number of products per page
let selectedCategories = []; // Array to store selected categories

// Fetch and display categories
const CatagoryList = async () => {
    try {
        const categoryResponse = await SendRequest({ endpoint: '/Category/GetAll' });

        if (categoryResponse.status === 200 && categoryResponse.success) {
            const $categoryList = $('#category-list'); // Targeting the category list

            categoryResponse.data.forEach(function (category) {
                $categoryList.append(`
                    <div class="form-check">
                        <input class="form-check-input custom-checkbox" type="checkbox" id="cat-${category.id}" data-id="${category.id}">
                        <label class="form-check-label" for="cat-${category.id}">
                            ${category.categoryName}
                        </label>
                    </div>
                `);
            });

            // Add event listener for category filtering
            $('.form-check-input').on('change', function () {
                const categoryId = $(this).data('id');
                if ($(this).is(':checked')) {
                    if (!selectedCategories.includes(categoryId)) {
                        selectedCategories.push(categoryId);
                    }
                } else {
                    selectedCategories = selectedCategories.filter(id => id !== categoryId);
                }
                currentPage = 1; // Reset to first page on filter change
                displayProducts();
            });
        } else {
            console.error('Failed to fetch categories:', categoryResponse.message);
        }
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
};

// Fetch all products once and store them
const fetchAllProducts = async () => {
    try {
        const endpoint = `/Product/GetAll`; // Fetch all products at once
        console.log('Fetching all products from endpoint:', endpoint);
        const productsResponse = await SendRequest({ endpoint });

        if (productsResponse.status === 200 && productsResponse.success) {
            allProducts = productsResponse.data;
            displayProducts(); // Display products after fetching
        } else {
            console.error('Failed to fetch products:', productsResponse.message);
        }
    } catch (error) {
        console.error('Error fetching products:', error);
    }
};

// Display products based on the current page and filter
const displayProducts = () => {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;

    // Filter products based on the selected categories
    filteredProducts = selectedCategories.length > 0
        ? allProducts.filter(product => selectedCategories.includes(product.categoryID))
        : allProducts;

    const paginatedProducts = filteredProducts.slice(startIndex, endIndex);

    const $container = $('#product-container');
    const $pagination = $('#pagination');
    $container.empty(); // Clear previous content

    paginatedProducts.forEach(function (product) {
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
                        <div class="d-flex align-items-center mb-2">
                            ${generateStars(product.unitPrice)}
                            <span class="ms-2">(${product.unitsInStock})</span>
                        </div>
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

    // Update pagination controls
    $pagination.empty(); // Clear previous pagination controls
    const totalPages = Math.ceil(filteredProducts.length / pageSize);
    if (totalPages > 1) {
        for (let i = 1; i <= totalPages; i++) {
            $pagination.append(`
                <button class="btn btn-outline-primary mx-1 ${i === currentPage ? 'active' : ''}" data-page="${i}">
                    ${i}
                </button>
            `);
        }

        $('.btn[data-page]').on('click', function () {
            currentPage = $(this).data('page');
            displayProducts();
        });
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
