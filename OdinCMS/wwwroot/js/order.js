var dataTable;

// Entry point
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id", "width": "15%" },
            {
                "data": "name",
                "width": "15%"
            },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="btn-group" role="group">
                        <a href="/Admin/Order/Detail?orderId=${data}" class="btn btn-primary">Details</a>
                    </div>
                    `
                },
                "width": "15%"
            },

        ]
    });
}