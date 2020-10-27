$('.returns-product-selection')
  .on('focus',
  '.return-product-name',
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
