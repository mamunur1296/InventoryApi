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
const pageSize = 12; // Number of products per page
let selectedCategories = []; // Array to store selected categories

// Fetch and display categories
const CatagoryList = async () => {
    try {
        debugger
        const categoryResponse = await SendRequest({ endpoint: '/Category/GetallSubCatagory' });
        debugger
        if (categoryResponse) {
            const $categoryList = $('#category-list'); // Targeting the category list

            categoryResponse.forEach(function (category) {
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
    const $container = $('#product-container');
    const $pagination = $('#pagination');
    $container.empty(); // Clear previous content
    $pagination.empty(); // Clear previous pagination controls

    // Filter products based on the selected categories
    filteredProducts = selectedCategories.length > 0
        ? allProducts.filter(product => selectedCategories.includes(product.categoryID))
        : allProducts;

    if (filteredProducts.length === 0) {
        // Display a professional message when no products are available
        $container.append(`
            <div class="col-12 text-center">
                <h3 class="text-muted">No Products Found</h3>
                <p class="text-muted">We couldn't find any products matching your criteria. Please try adjusting your filters or check back later.</p>
            </div>
        `);
    } else {
        // Existing code to display products and pagination
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        const paginatedProducts = filteredProducts.slice(startIndex, endIndex);

        paginatedProducts.forEach(function (product) {
            $container.append(`
                <div class="col-md-2 mb-4 d-flex align-items-stretch">
                    <div class="card d-flex flex-column" style="width: 100%;">
                        <!-- Product Image -->
                        <img src="/images/Product/${product.imageURL || 'default-user.png'}"
                             alt="${product.productName}"
                             class="card-img-top"
                             width="150"
                             height="200"
                             onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';">

                        <div class="card-body d-flex flex-column">
                            <!-- Stock Status -->
                            ${product.unitsInStock > 0
                    ? '<p class="text-success mb-2">In Stock</p>'
                    : '<p class="text-danger mb-2">Out of Stock</p>'}

                            <!-- Product Name -->
                            <h5 class="card-title">${product.productName}</h5>

                            <!-- Star Rating -->
                            <div class="d-flex align-items-center mb-3">
                                ${generateStars(product.unitPrice)}
                                <span class="ms-2">(${product.unitsInStock})</span>
                            </div>

                            <!-- Pricing Logic -->
                            <div class="mt-auto">
                                ${product.discount > 0
                    ? `
                                        <span class="text-decoration-line-through text-muted me-2">
                                            ৳${product.unitPrice}
                                        </span>
                                        <span class="fw-bold fs-5 d-block text-success">
                                            ৳${(product.unitPrice - (product.unitPrice * product.discount / 100)).toFixed(2)}
                                        </span>
                                        <span class="badge bg-danger">${product.discount}% Off</span>
                                    `
                    : `<span class="fw-bold fs-5 d-block">৳${product.unitPrice.toFixed(2)}</span>`}
                            </div>
                        </div>
                    </div>
                </div>
            `);
        });

        // Pagination controls
        const totalPages = Math.ceil(filteredProducts.length / pageSize);
        if (totalPages > 1) {
            $pagination.append(`
                <button class="btn btn-outline-primary mx-1 ${currentPage === 1 ? 'disabled' : ''}" id="prevPage">
                    Previous
                </button>
            `);
            for (let i = 1; i <= totalPages; i++) {
                $pagination.append(`
                    <button class="btn btn-outline-primary mx-1 ${i === currentPage ? 'active' : ''}" data-page="${i}">
                        ${i}
                    </button>
                `);
            }
            $pagination.append(`
                <button class="btn btn-outline-primary mx-1 ${currentPage === totalPages ? 'disabled' : ''}" id="nextPage">
                    Next
                </button>
            `);

            $('.btn[data-page]').on('click', function () {
                currentPage = $(this).data('page');
                displayProducts();
            });
            $('#prevPage').on('click', function () {
                if (currentPage > 1) {
                    currentPage--;
                    displayProducts();
                }
            });
            $('#nextPage').on('click', function () {
                if (currentPage < totalPages) {
                    currentPage++;
                    displayProducts();
                }
            });
        }
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
