const gbpCurrencyValue = 0;
const gbpPromptText = 'Enter amount paid in GBP';

const shippingCostInput = $('.shipping-cost');
const shippingCurrencySelect = $('.shipping-currency');
const shippingCostInGbpInput = $('.shipping-cost-in-gbp');
const shippingConversionRateText = $('.shipping-conversion-rate-read p');

const lineItemPaymentCurrencySelect = $('.poli-product-payment-currency');
const lineItemPriceInGbpInput = $('.poli-product-price-paid-in-gbp');

$(document).ready(function () {
  let shippingSelectedCurrencyValue = parseInt(shippingCurrencySelect.val());
  let shippingCostInGbp = parseFloat(shippingCostInGbpInput.val());
  let selectedCurrencyValue = parseInt(lineItemPaymentCurrencySelect.val());

  disableOrEnableLineItemGbpPriceInput(lineItemPriceInGbpInput, selectedCurrencyValue);
  disableOrEnableShippingGbpPriceInput(shippingSelectedCurrencyValue);

  if (shippingSelectedCurrencyValue !== gbpCurrencyValue) {
    let selectedCurrencyText = $(".shipping-currency option:selected").text();
    updateConversionRateText(shippingConversionRateText, selectedCurrencyText, shippingCostInput.val(), shippingCostInGbpInput.val());
    if (shippingCostInGbp === 0) {
      shippingConversionRateText.text(gbpPromptText);
    }
  }

  let lineOrders = $('.line-order');

  lineOrders.each(function (index, element) {
    let lineOrder = $(element);

    let lineOrderCurrencySelect = lineOrder.find('.poli-product-payment-currency');
    let lineOrderPricePaidInGbpInput = lineOrder.find('.poli-product-price-paid-in-gbp');

    let lineOrderSelectedCurrencyValue = parseInt(lineOrderCurrencySelect.val());

    if (lineOrderSelectedCurrencyValue !== 0) {
      let lineOrderSelectedCurrencyText = lineOrderCurrencySelect.children(':selected').text();
      let lineOrderPriceValue = parseFloat(lineOrder.find('.poli-product-price').val());

      lineOrderPricePaidInGbpInput.prop('disabled', false);

      let lineOrderPricePaidInGbpValue = parseFloat(lineOrderPricePaidInGbpInput.val());
      let conversionRateText = lineOrder.find('.poli-product-conversion-rate-read p');
      let formattedRate = parseFloat(lineOrderPriceValue / lineOrderPricePaidInGbpValue).toFixed(5);
      conversionRateText.text('1 GBP = ' + formattedRate + ' ' + lineOrderSelectedCurrencyText);
    } else {
      lineOrderPricePaidInGbpInput.prop('disabled', true);
    }
  });
});

$(document).on('change', '.shipping-cost, .shipping-currency, .shipping-cost-in-gbp', function () {
  let shippingSelectedCurrencyValue = parseInt(shippingCurrencySelect.val());
  let shippingCostInGbp = parseFloat(shippingCostInGbpInput.val());

  if (shippingSelectedCurrencyValue === gbpCurrencyValue) {
    shippingCostInGbpInput.val('0.00');
    shippingConversionRateText.text('Not Applicable');
  } else {
    let selectedCurrencyText = $(".shipping-currency option:selected").text();
    updateConversionRateText(shippingConversionRateText, selectedCurrencyText, shippingCostInput.val(), shippingCostInGbpInput.val());
    if (shippingCostInGbp === 0) {
      shippingConversionRateText.text(gbpPromptText);
    }
  }
});

$(document).on('change', '.shipping-currency', function () {
  let shippingSelectedCurrencyValue = parseInt(shippingCurrencySelect.val());
  disableOrEnableShippingGbpPriceInput(shippingSelectedCurrencyValue);
});

$(document).on('change', '.poli-product-payment-currency', function () {
  let lineItemCurrencySelect = $(this);
  let rootTableCell = lineItemCurrencySelect.parents('td');
  let gbpPriceInputTableCell = rootTableCell.next();
  let lineItemConversionRateTableCell = gbpPriceInputTableCell.next();
  let gbpPriceInput = gbpPriceInputTableCell.find('.poli-product-price-paid-in-gbp');
  let conversionRateText = lineItemConversionRateTableCell.find('.poli-product-conversion-rate-read p');
  let selectedCurrencyOnLineItemValue = parseInt(lineItemCurrencySelect.val());
  disableOrEnableLineItemGbpPriceInput(gbpPriceInput, selectedCurrencyOnLineItemValue);

  if (parseInt(lineItemCurrencySelect.val()) === gbpCurrencyValue) {
    gbpPriceInput.val('0.00');
    conversionRateText.text('Not Applicable');
  } else {
    if (parseInt(gbpPriceInput.val()) === 0) {
      conversionRateText.text(gbpPromptText);
    } else {
      let conversionRateTextWithoutCurrency = conversionRateText.text().slice(0, -3);
      conversionRateText.text(conversionRateTextWithoutCurrency + lineItemCurrencySelect.children(':selected').text());
    }
  }
});

$(document).on('change', '.poli-product-price-paid-in-gbp', function () {
  let lineItemGbpPriceInput = $(this);
  let rootTableCell = lineItemGbpPriceInput.parents('td');
  let lineItemCurrencySelect = rootTableCell.prev().find('.poli-product-payment-currency');
  let itemPriceInput = rootTableCell.prev().prev().find('.poli-product-price');
  let lineItemConversionRateTableCell = rootTableCell.next();
  let conversionRateText = lineItemConversionRateTableCell.find('.poli-product-conversion-rate-read p');
 
  let selectedCurrencyText = lineItemCurrencySelect.children(':selected').text();

  updateConversionRateText(
    conversionRateText,
    selectedCurrencyText,
    itemPriceInput.val(),
    lineItemGbpPriceInput.val());

  if (parseInt(lineItemGbpPriceInput.val()) === 0) {
    conversionRateText.text(gbpPromptText);
  }
});

function updateConversionRateText(selector, currency, value, valueInGbp) {
  let valueAsFloat = parseFloat(value);
  let gbpValueAsFloat = parseFloat(valueInGbp);
  let formattedRate = parseFloat(valueAsFloat / gbpValueAsFloat).toFixed(5);
  selector.text('1 GBP = ' + formattedRate + ' ' + currency);
}

function disableOrEnableLineItemGbpPriceInput(input, selectedCurrencyOnLineItem) {
  if (selectedCurrencyOnLineItem === gbpCurrencyValue) {
    input.prop('disabled', true);
  } else {
    input.prop('disabled', false);
  }
}

function disableOrEnableShippingGbpPriceInput(shippingSelectedCurrency) {
  if (shippingSelectedCurrency === gbpCurrencyValue) {
    shippingCostInGbpInput.prop('disabled', true);
  } else {
    shippingCostInGbpInput.prop('disabled', false);
  }
}
