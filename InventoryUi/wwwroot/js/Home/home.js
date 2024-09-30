import { SendRequest } from "../utility/sendrequestutility.js";
document.addEventListener('DOMContentLoaded', function () {
    // First Swiper initialization (Product1)
    var swiper1 = new Swiper('.Product1', {
        slidesPerView: 1,  // Number of slides visible at once
        spaceBetween: 10,  // Space between slides
        loop: true,        // Loop mode enabled
        breakpoints: {
            640: { slidesPerView: 2, spaceBetween: 20 },
            768: { slidesPerView: 3, spaceBetween: 30 },
            1024: { slidesPerView: 4, spaceBetween: 60 },
        },
        pagination: false,
        navigation: { nextEl: '.swiper-button-next', prevEl: '.swiper-button-prev' },
        autoplay: { delay: 2500, disableOnInteraction: true },
    });

    // Second Swiper initialization (Product2)
    var swiper2 = new Swiper('.Product2', {
        slidesPerView: 1,  // Number of slides visible at once
        spaceBetween: 10,  // Space between slides
        loop: true,        // Loop mode enabled
        breakpoints: {
            640: { slidesPerView: 2, spaceBetween: 20 },
            768: { slidesPerView: 3, spaceBetween: 30 },
            1024: { slidesPerView: 4, spaceBetween: 60 },
        },
        pagination: false,
        navigation: { nextEl: '.swiper-button-next', prevEl: '.swiper-button-prev' },
        autoplay: { delay: 2500, disableOnInteraction: true },
    });
});

$(document).ready(async function () {
    await product();         // Fetch products
    await TrendingProduct(); // Fetch trending products
    await popularProduct();  // Fetch popular products
    await RelatedProduct();  // Fetch related products
});

// Product data fetching
const product = async () => {
    const products = await SendRequest({ endpoint: '/Home/Get8Product' });
    if (products !== null) {
        productCare(products, '#ProductSection');
    }
}

// Popular products data fetching
const popularProduct = async () => {
    debugger
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    if (products.status === 200 && products.success) {
        productCareSlider(products.data, '#PopularProductSection');
    }
}

// Trending products data fetching
const TrendingProduct = async () => {
    debugger
    const products = await SendRequest({ endpoint: '/Product/GetAll' });
    if (products.status === 200 && products.success) {
        productCareSlider(products.data, '#TrendingProductSection');
    }
}

// Trending products data fetching
// Fetch related products by category
const RelatedProduct = async () => {
    debugger
    // Get the category ID and product ID from custom attributes
    const categoryId = $('#RelatedProductSection').attr('data-existing-product-category-id');
    const productId = $('#RelatedProductSection').attr('data-existing-product-id');

    // Fetch all products
    const products = await SendRequest({ endpoint: '/Product/GetAll' });

    if (products.status === 200 && products.success) {
        // Filter products by category ID and exclude the current product
        const relatedProducts = products.data.filter(po => po.categoryID == categoryId && po.id != productId);

        // Create a set to keep track of categories already added
        const selectedCategoryIds = new Set();
        selectedCategoryIds.add(categoryId); // Include current category to avoid duplication

        // Find the first product from each other category (different from current category)
        const firstProductFromOtherCategories = products.data.filter(po => {
            if (po.categoryID !== categoryId && !selectedCategoryIds.has(po.categoryID)) {
                selectedCategoryIds.add(po.categoryID); // Mark this category as added
                return true; // Include this product
            }
            return false; // Skip other products from the same category
        });

        // Combine related products and first product from other categories
        const finalProductList = [...relatedProducts, ...firstProductFromOtherCategories];

        // Use the final product list
        productCare(finalProductList, '#RelatedProductSection');
    }
}





const productCare = (paginatedProducts, sectionId) => {
    const $productContainer = $(sectionId);
    $productContainer.empty();  // Clear the container before appending new products

    paginatedProducts.forEach(product => {
        $productContainer.append(`
            <div class="col-md-3 mb-4 d-flex align-items-stretch">
                <div class="card d-flex flex-column" style="width: 100%;">
                    <a href="/product/details/${product.id}">
                        <img src="/images/Product/${product.imageURL || 'default-user.png'}"
                             alt="${product.productName}"
                             class="card-img-top"
                             width="150" height="250"
                             onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';">
                    </a>
                    <div class="card-body d-flex flex-column">
                        <a href="/product/details/${product.id}">
                            ${product.unitsInStock > 0
                ? '<p class="text-success mb-2">In Stock</p>'
                : '<p class="text-danger mb-2">Out of Stock</p>'}
                        </a>
                        <a href="/product/details/${product.id}">
                            <h5 class="card-title">${product.productName || 'Unnamed Product'}</h5>
                        </a>
                        <div class="d-flex align-items-center mb-3">
                            ${generateStars(product.unitPrice)}
                            <span class="ms-2">(${product.unitsInStock})</span>
                        </div>
                        <div class="mt-auto">
                            ${product.discount > 0
                ? `<span class="text-decoration-line-through text-muted me-2">৳${product.unitPrice.toFixed(2)}</span>
                               <span class="fw-bold fs-5 d-block text-success">৳${(product.unitPrice - product.unitPrice * product.discount / 100).toFixed(2)}</span>
                               <span class="badge bg-danger">${product.discount}% Off</span>`
                : `<span class="fw-bold fs-5 d-block">৳${product.unitPrice.toFixed(2)}</span>`}
                        </div>
                    </div>
                </div>
            </div>
        `);
    });
}
const productCareSlider = (paginatedProducts, sectionId) => {
    const $productContainer = $(sectionId);
    $productContainer.empty();  // Clear the container before appending new products
    debugger
    paginatedProducts.forEach(product => {
        $productContainer.append(`
            <div class="swiper-slide">
                <div class="card" style="width: 15rem;">
                    <a href="/product/details/${product.id}">
                        <img src="/images/Product/${product.imageURL || 'default-user.png'}"
                             alt="${product.productName}"
                             class="card-img-top"
                             width="150" height="250"
                             onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';">
                    </a>
                        ${product.unitsInStock > 0
                ? '<p class="text-success mb-2">In Stock</p>'
                : '<p class="text-danger mb-2">Out of Stock</p>'}
                         <a href="/product/details/${product.id}">
                            <h5 class="card-title">${product.productName || 'Unnamed Product'}</h5>
                        </a>
                        <div class="d-flex align-items-center mb-3">
                            ${generateStars(product.unitPrice)}
                            <span class="ms-2">(${product.unitsInStock || 0})</span>
                        </div>
                        <div class="mt-auto">
                            ${product.discount > 0
                ? `<span class="text-decoration-line-through text-muted me-2">৳${product.unitPrice.toFixed(2)}</span>
                               <span class="fw-bold fs-5 d-block text-success">৳${(product.unitPrice - product.unitPrice * product.discount / 100).toFixed(2)}</span>
                               <span class="badge bg-danger">${product.discount}% Off</span>`
                : `<span class="fw-bold fs-5 d-block">৳${product.unitPrice.toFixed(2)}</span>`}
                        </div>
                    </div>
                </div>
            </div>
        `);
    });
}
function generateStars(price) {
    const stars = 5; // Assume 5-star rating for all products
    let starHtml = '';

    for (let i = 0; i < stars; i++) {
        starHtml += '<i class="fas fa-star text-warning"></i>';
    }

    return starHtml;
}

