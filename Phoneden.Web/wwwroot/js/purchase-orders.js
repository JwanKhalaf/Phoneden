$('.line-order-items')
  .on('focus',
  '.poli-product-name',
    function() {
      $(this)
      .autocomplete({
        serviceUrl: '/Product/GetProductsWithName',
        minChars: 3,
        type: 'POST',
        paramName: 'productName',
        transformResult: function (response) {
          return {
            suggestions: $.map(JSON.parse(response),
              function (dataItem) {
                return {
                  value: dataItem.name + ' - ' + getColour(dataItem.colour) + ' - ' + dataItem.associatedQuality,
                  data: dataItem.id
                };
              })
          };
        },
        onSelect: function (selection) {
          $(this).siblings('input[type=hidden]').val(selection.data);
        }
      });
    });

$('.add-line-item')
  .on('click',
    function() {
      $.ajax({
          async: false,
          url: '/PurchaseOrder/LineOrderItem'
      })
        .done(function (partialView) {
          $('.line-order-items tbody').append(partialView);
          updateNameAttributeForAllLineOrderItems();
        });
    });

$('.line-order-items')
  .on('click',
    '.delete-line-order-btn',
    function() {
      $(this).parents(".line-order:first").remove();
      updateNameAttributeForAllLineOrderItems();
      return false;
    });

function getColour(colorCode) {
  if (colorCode === 0) {
    return 'None';
  } else if (colorCode === 1) {
    return 'Clear';
  } else if (colorCode === 2) {
    return 'Black';
  } else if (colorCode === 3) {
    return 'White';
  } else if (colorCode === 4) {
    return 'Grey';
  } else if (colorCode === 5) {
    return 'Silver';
  } else if (colorCode === 6) {
    return 'Gold';
  } else if (colorCode === 7) {
    return 'Rose Gold';
  } else if (colorCode === 8) {
    return 'Blue';
  } else if (colorCode === 9) {
    return 'Green';
  } else if (colorCode === 10) {
    return 'Red';
  } else if (colorCode === 11) {
    return 'Purple';
  } else if (colorCode === 12) {
    return 'Pink';
  } else {
    return 'Brown';
  }
}

function updateNameAttributeForAllLineOrderItems() {
  let lineOrderItems = $('.line-order');
  lineOrderItems.each(function (index) {
    let lineOrder = $(this);
    updateForInputs(lineOrder, index);
    updateForValidationMessages(lineOrder, index);
  });
}

function updateForInputs(lineOrder, index) {
  updateNameAttributeForPurchaseOrderIdField(lineOrder, index);
  updateNameAttributeForLineOrderIdField(lineOrder, index);
  updateNameAttributeForProductIdField(lineOrder, index);
  updateNameAttributeForProductNameField(lineOrder, index);
  updateNameAttributeForProductPriceField(lineOrder, index);
  updateNameAttributeForProductPaymentCurrencyDropdown(lineOrder, index);
  updateNameAttributeForProductPricePaidInGbpField(lineOrder, index);
  updateNameAttributeForProductQuantityField(lineOrder, index);
}

function updateForValidationMessages(lineOrder, lineOrderItemIndex) {
  let validationSpanElements = lineOrder.find('.field-validation-valid');
  validationSpanElements.each(function (index) {
    let span = $(this);
    let text = span.attr('data-valmsg-for');
    let updatedText = text.replace(text.charAt(10), lineOrderItemIndex);
    span.attr('data-valmsg-for', updatedText);
  });
}

function updateNameAttributeForPurchaseOrderIdField(lineOrder, index) {
  let productIdInput = lineOrder.find('.poli-order-id');
  productIdInput.attr('name', getNameAttributeForPurchaseOrderId(index));
}

function updateNameAttributeForLineOrderIdField(lineOrder, index) {
  let productIdInput = lineOrder.find('.poli-id');
  productIdInput.attr('name', getNameAttributeForLineOrderId(index));
}

function updateNameAttributeForProductIdField(lineOrder, index) {
  let productIdInput = lineOrder.find('.poli-product-id');
  productIdInput.attr('name', getNameAttributeForLineOrderProductId(index));
}

function updateNameAttributeForProductNameField(lineOrder, index) {
  let productNameInput = lineOrder.find('.poli-product-name');
  productNameInput.attr('name', getNameAttributeForLineOrderProductName(index));
}

function updateNameAttributeForProductPriceField(lineOrder, index) {
  let productPriceInput = lineOrder.find('.poli-product-price');
  productPriceInput.attr('name', getNameAttributeForLineOrderPrice(index));
}

function updateNameAttributeForProductPaymentCurrencyDropdown(lineOrder, index) {
  let productPriceInput = lineOrder.find('.poli-product-payment-currency');
  productPriceInput.attr('name', getNameAttributeForLineOrderCurrency(index));
}

function updateNameAttributeForProductPricePaidInGbpField(lineOrder, index) {
  let productPriceInput = lineOrder.find('.poli-product-price-paid-in-gbp');
  productPriceInput.attr('name', getNameAttributeForLineOrderPricePaidInGbp(index));
}

function updateNameAttributeForProductQuantityField(lineOrder, index) {
  let productPriceInput = lineOrder.find('.poli-product-quantity');
  productPriceInput.attr('name', getNameAttributeForLineOrderQuantity(index));
}

function getNameAttributeForPurchaseOrderId(index) {
  return 'LineItems[' + index + '].OrderId';
}

function getNameAttributeForLineOrderId(index) {
  return 'LineItems[' + index + '].Id';
}

function getNameAttributeForLineOrderProductId(index) {
  return 'LineItems[' + index + '].ProductId';
}

function getNameAttributeForLineOrderProductName(index) {
  return 'LineItems[' + index + '].Name';
}

function getNameAttributeForLineOrderPrice(index) {
  return 'LineItems[' + index + '].Price';
}

function getNameAttributeForLineOrderCurrency(index) {
  return 'LineItems[' + index + '].Currency';
}

function getNameAttributeForLineOrderPricePaidInGbp(index) {
  return 'LineItems[' + index + '].PricePaidInGbp';
}

function getNameAttributeForLineOrderQuantity(index) {
  return 'LineItems[' + index + '].Quantity';
}
