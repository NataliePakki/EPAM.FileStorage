$(document).ready(function () {
    var visibleText = $('#inputFileVisible');
    var hidden = $('#inputFile');
    var visibleBtn = $('#inputFileBtn');
    visibleBtn.on('click', function () {
        hidden.click();
    })
    hidden.on('change', function () {
        var fileName = $(this).val().split('\\');
        fileName = fileName[fileName.length - 1];
        visibleText.data(fileName);
        visibleText.val(fileName);
    });
});
