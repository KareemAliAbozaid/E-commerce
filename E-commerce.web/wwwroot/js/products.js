$(document).ready(function () {
    $('[data-kt-filter="search"]').on('keyup', function () {
        var input = $(this);
        datatable.search(this.value).draw();
    });
    datatable = $('#Product').DataTable({
        serverSide: true,
        processing: true,
        stateSave: true,
        language: {
            processing: '<div class="d-flex justify-content-center text-primary align-items-center dt-spinner"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div><span class="text-muted ps-2">Loading...</span></div>'
        },
        ajax: {
            url: '/Products/GetProducts',
            type: 'Post'
        },
        columnDefs: [{
            targets: [0],
            visible: false,
            searchable: false
        }],
        columns: [
            { "data": "id", "name": "Id", "className": "d-none" },
            {
                "name": "Name",
                "className": "d-flex align-items-center",
                "render": function (data, type, row) {
                    return `<div class="symbol symbol-50px overflow-hidden me-3">
                                                <a href="/Products/Details/${row.id}">
                                                    <div class="symbol-label h-70px">
                                                         <img src="${(row.imageUrl === null ? '/images/image-placeholder.jpg' : row.imageUrl)}" alt="cover" class="w - 100">
                                                    </div>
                                                </a>
                                            </div>
                                            <div class="d-flex flex-column">
                                              <a href="/Products/Details/${row.id}" class="text-primary fw-bolder mb-1">${row.name}</a>
                                               <a>href="/Products/Details/${row.id}" class="text-primary fw-bolder mb-1">${row.category}</a>
                                            </div>`;
                }
            },
            {
                "name": "publishingDate", "render": function (data, type, row) {
                    return moment(row.publishingDate).format('ll');
                }

            },
        ],
    });
});
