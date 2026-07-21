$(document).ready(function () {
    updateTotalOrderCost();
    getTotalOrderCost();
    initializeMoyasar();
});

// Function to update total transport price
function updateTransportPrice(productId) {
    var quantity = parseInt($('#row-' + productId).find('.quantity-' + productId).val(), 10);
    var transportPrice = 0;

    if (quantity === 0) {
        transportPrice = 0;
    } else if (shippingPriceNavigation && shippingPriceNavigation.length > 0) {
        shippingPriceNavigation.sort((a, b) => a.VarQuantity - b.VarQuantity);

        if (shippingPriceNavigation[0].IsODD_Even) {
            var baseQuantity = shippingPriceNavigation[0].VarQuantity;
            var basePrice = shippingPriceNavigation[0].VarPrice;
            var multiplier = Math.ceil(quantity / baseQuantity);
            transportPrice = multiplier * basePrice;
        } else {
            for (var i = 0; i < shippingPriceNavigation.length; i++) {
                if (quantity <= shippingPriceNavigation[i].VarQuantity) {
                    transportPrice = shippingPriceNavigation[i].VarPrice;
                    break;
                }
            }
            if (quantity > shippingPriceNavigation[shippingPriceNavigation.length - 1].VarQuantity) {
                transportPrice = shippingPriceNavigation[shippingPriceNavigation.length - 1].VarPrice;
            }
        }
    } else {
        $('#transportprice-' + productId).val("N/A");
        return;
    }
    $('#transportprice-' + productId).val(transportPrice.toFixed(2));
}

// Function to update item total
function updateItemTotal(productId) {
    var priceText = $('#row-' + productId).find('.PriceItem').val();
    var price = parseFloat(priceText);
    var quantity = parseInt($('#row-' + productId).find('.quantity-' + productId).val(), 10);

    if (!isNaN(price) && !isNaN(quantity)) {
        var itemTotal = price * quantity;
        $('#item-total-' + productId).text(itemTotal.toFixed(2));
    }
}

// Inactivity Timer
const inactivityTimeout = 6000000000;
const redirectUrl = '/Cart/ViewCart';
let inactivityTimer;

function resetInactivityTimer() {
    clearTimeout(inactivityTimer);
    inactivityTimer = setTimeout(() => {
        window.location.href = redirectUrl;
    }, inactivityTimeout);
}

const activityEvents = ['mousemove', 'keydown', 'mousedown', 'touchstart'];
activityEvents.forEach(event => {
    window.addEventListener(event, resetInactivityTimer);
});
