function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function formatCurrency(value)
{
    return '$' + value.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

function isDate(dateValue) {
    var currVal = dateValue;
    if (currVal == '')
        return false;

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function numberKeysOnly(e) {
    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    specialKeys.push(9); //Tab
    specialKeys.push(46); //Delete
    specialKeys.push(37); //Left arrow
    specialKeys.push(39); //right arrow

    var keyCode = e.which ? e.which : e.keyCode;
    return ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105) || specialKeys.indexOf(keyCode) != -1);
}

function numberKeysAndDecimalOnly(e) {
    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    specialKeys.push(9); //Tab
    specialKeys.push(46); //Delete
    specialKeys.push(37); //Left arrow
    specialKeys.push(39); //right arrow

    var keyCode = e.which ? e.which : e.keyCode;
    return (e.keyCode == 109 || e.keyCode == 189 || e.keyCode == 110 || e.keyCode == 190 || (e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 96 && e.keyCode <= 105) || specialKeys.indexOf(keyCode) != -1);
}

function validateNumberInput(txtBox) {
    txtBox.value = txtBox.value.replace(' ', '');
    if (isNaN(txtBox.value) || txtBox.value.length == 0) { txtBox.value = 0; }
}

function validateNumberInputBlank(txtBox) {
    txtBox.value = txtBox.value.replace(' ', '');
    if (isNaN(txtBox.value)) { txtBox.value = ''; }
}