$('#createBtn').keypress(function (e) {
    if (e.which == 13) {
        $('#createBtn').focus().click();
        return false;
    }
});