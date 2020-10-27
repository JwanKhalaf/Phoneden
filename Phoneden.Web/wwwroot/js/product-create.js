$(document).ready(function () {
  const validClass = "field-validation-valid";
  const warningAlert = '<div class="col-md-12"><div class="alert alert-warning">Your selling price is lower than your cost price!</div></div>';
  const unitCostInput = $('#UnitCostPrice');
  const unitSellingInput = $('#UnitSellingPrice');
  const unitCostInputValidation = unitCostInput.next();
  const unitSellingInputValidation = unitSellingInput.next();

  unitSellingInput.on('input', function () {
    let unitCostInputIsValid = unitCostInputValidation.hasClass(validClass);
    let unitSellingInputIsValid = unitSellingInputValidation.hasClass(validClass);

    if (unitCostInputIsValid && unitSellingInputIsValid) {
      let costPrice = parseInt(unitCostInput.val());
      let sellingPrice = parseInt(unitSellingInput.val());

      if (costPrice > sellingPrice) {
        unitSellingInput.closest('.row').append(warningAlert);
      } else {
        unitSellingInput.closest('.row').children('.col-md-12').last().remove();
      }
    }
  });
});
