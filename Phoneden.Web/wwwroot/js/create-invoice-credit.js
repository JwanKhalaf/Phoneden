var creditHelpTextTemplate = 'Remaining Credit is <strong>£';
var creditHelpText = $('#customer-credit-help');
var isSaleOrder = $('#is-sale-order').val();
var orderId = $('#order-id').val();
var remainingCredit = 0;

if (isSaleOrder === 'True') {
  $.ajax({
    url: '/Invoice/GetCustomerRemainingCreditAsync',
    type: 'POST',
    data: { orderId: orderId },
    success: function (data) {
      creditHelpText.html(creditHelpTextTemplate + data + '</strong>');
      remainingCredit = data;
    },
    error: function (err) {
      alert('error');
      console.log(err);
    }
  });

  $('#amount-to-be-paid-on-credit')
    .on('keyup',
      function () {
        let amountToBePaidOnCredit = parseFloat(this.value);
        let remainingAmount = remainingCredit - amountToBePaidOnCredit;
        creditHelpText.html(creditHelpTextTemplate + remainingAmount + '</strong>');
      });

} else {
  $('#amount-to-be-paid-on-credit').prop('disabled', true);
}

if (isSaleOrder === 'True') {
  $('#discount')
    .on('keyup',
      function () {

        let invoiceInput = $('#amount');
        let discountValidation = $('#discount-validation');
        let discountAmount = parseFloat(this.value);
        let invoiceTotal = parseFloat(invoiceInput.val());
        let fivePercentOfInvoiceTotal = invoiceTotal * 0.05;
        if (discountAmount > fivePercentOfInvoiceTotal) {
          discountValidation.html('Discount exceeds allowed amount.');
        } else {
          discountValidation.html('');
        }
      });
}
