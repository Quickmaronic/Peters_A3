var Sortable = {
    baseUrl: '',
    sortBy: 0,
    searchTerm: '',
    Search() {
        var searchKey = $('#txtSearch').val();

        $('.sortable tbody tr').each(function () {
            var currentText = ($(this).text());

            if (currentText.toLowerCase().indexOf(searchKey.toLowerCase()) != -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });

        //window.location.href = Sortable.baseUrl + searchKey;
    },
    Sort(sortBy) {
        window.location.href = Sortable.baseUrl + "?sortBy=" + sortBy;
    }
};