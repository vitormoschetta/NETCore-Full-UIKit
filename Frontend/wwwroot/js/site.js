function PaginationFilter(url, filter, pageNumber) {
    $.post(url, { filter: filter, pageNumber: pageNumber }, function (data) {
        $("#tabela-index").empty();
        $("#tabela-index").html(data);
    })
}

