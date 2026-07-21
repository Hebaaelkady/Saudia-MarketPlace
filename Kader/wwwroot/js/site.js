var Site_Module = (function () {
    function Logout() {

        Cookies.remove('user');
        window.location.href = '/Home/Index'; // Replace 'your-redirect-url' with the URL you
    }
    return {
        Logout: Logout
    };
})();
$(document).ready(function () {
    Cookies.remove('userSession');
    $('#myTable').DataTable();

});    //اضافة عدد المشتريات للسلة من خلال cookie في جميع الصفحات
function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");

    if (parts.length == 2) return parts.pop().split(";").shift();

}
function updateCheckoutIcon() {

    // Retrieve the cart data from cookies
    var cartDataString = getCookie('cartData');

    // Decode the URL-encoded string
    var decodedString = decodeURIComponent(cartDataString);

    // Parse the cart data string into an array of items
    var cartData = decodedString ? JSON.parse(decodedString) : [];

    // Calculate the total quantity of items in the cart
    var totalQuantity = cartData.reduce(function (accumulator, currentItem) {
        return accumulator + Number(currentItem.Quantity);
    }, 0);

    // Update the checkout icon with the total quantity
    var checkoutIcon = $('#checkoutIcon'); // Use jQuery selector

    if (checkoutIcon.length) { // Check if element exists before accessing content
        checkoutIcon.text(totalQuantity); // Set text using jQuery's .text() method
    } else {
        console.error('Element with id "checkoutIcon" not found');
    }
}

// Call this function on page load and after updating the cart
updateCheckoutIcon();
//انتهاء اضافة عدد المشتريات للسلة من خلال cookie في جميع الصفحات


$(document).ready(function () {
    var currentPath = window.location.pathname.split("/").pop();
    $('.sidenav a').each(function () {
        if ($(this).attr('href').includes(currentPath)) {
            $(this).addClass('active');
        }
    });
});

function generateMenu(nodes, baseUrl) {
    let html = `<a class="dropdown-item" href="${baseUrl}">كل المنتجات</a>`;
    nodes.forEach(function (node) {
        const url = `${baseUrl}${node.id}`;
        html += `<a class="dropdown-item" href="${url}">${node.nameTy}</a>`;
    });
    return html;
}
$(document).ready(function () {
    $.ajax({
        url: '@Url.Action("GetRoot", "Home")',
        method: 'GET',
        success: function (data) {
            const menuHtml = generateMenu(data, '@Url.Content("~/Gomla/Index/")');
            $('#menu').html(menuHtml);
        },
        error: function (error) {
            console.error("Error loading Gomla menu:", error);
        }
    });

    $.ajax({
        url: '@Url.Action("GetRootQt2a", "Home")',
        method: 'GET',
        success: function (data) {
            const menuHtml = generateMenu(data, '@Url.Content("~/Qt3/Index/")');
            $('#menuqt3').html(menuHtml);
        },
        error: function (error) {
            console.error("Error loading Qt3 menu:", error);
        }
    });
});
$(document).ready(function () {
    $('#searchBox').on('input', function () {
        let query = $(this).val();

        if (query.length >= 1) {
            // Call the API to fetch products
            // Example: Display search results in a dropdown
            $.ajax({
                url: '/Gomla/SearchGomla',
                type: 'GET',
                data: { searchTerm: query },
                success: function (response) {
                    const products = response.products; // Use the filtered product list
                    const $results = $('#searchResults');

                    $results.empty(); // Clear old results

                    if (products.length > 0) {
                        products.forEach(product => {
                            const catType = product.catTypeId === 1 ? "جملة - " : "قطاعي - ";
                            $results.append(
                                `<li data-id="${product.productId}">${catType}${product.subCatogryTitle}</li>`
                            );
                        });
                        $results.show();
                    } else {
                        $results.hide();
                    }

                },
                error: function () {
                    console.error('Error fetching products');
                }
            });

        } else {
            $('#searchResults').hide();
        }
    });

    // Handle click on dropdown item
    $('#searchResults').on('click', 'li', function () {
        const productId = $(this).data('id'); // Get product ID from the clicked item

        // Redirect to product details page
        window.location.href = `/Gomla/Details/${productId}`;
    });

    // Hide dropdown when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#searchBox, #searchResults').length) {
            $('#searchResults').hide();
        }
    });
});
$(document).ready(function () {
    $('#searchBox1').on('input', function () {
        let query = $(this).val();

        if (query.length >= 1) {
            // Call the API to fetch products
            // Example: Display search results in a dropdown
            $.ajax({
                url: '/Gomla/SearchQt3a',
                type: 'GET',
                data: { searchTerm: query },
                success: function (response) {
                    const products = response.products; // Use the filtered product list
                    const $results = $('#searchResults1');

                    $results.empty(); // Clear old results

                    if (products.length > 0) {
                        products.forEach(product => {
                            $results.append(
                                `<li data-id="${product.productId}">${product.subCatogryTitle}</li>`
                            );
                        });
                        $results.show();
                    } else {
                        $results.hide();
                    }
                },
                error: function () {
                    console.error('Error fetching products');
                }
            });

        } else {
            $('#searchResults1').hide();
        }
    });

    // Handle click on dropdown item
    $('#searchResults1').on('click', 'li', function () {
        const productId = $(this).data('id'); // Get product ID from the clicked item

        // Redirect to product details page
        window.location.href = `/Qt3/Details/${productId}`;
    });

    // Hide dropdown when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#searchBox, #searchResults').length) {
            $('#searchResults1').hide();
        }
    });
});
