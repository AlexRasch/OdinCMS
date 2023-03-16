var dataTable;

// Entry point
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "listPrice", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            /*{ "data": "coverType.name", "width": "15%" },*/
            {
                "data": "coverType",
                "render": function (data) {
                    if (data === null) {
                        return 'None';
                    } else {
                        console.log(data);
                        return data.name;
                    }
                }, 
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="btn-group" role="group">
                        <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary">Edit</a>
                        <a onClick=Delete('/Admin/Product/Delete/'+${data}) class="btn btn-danger">Delete</a>
                    </div>
                    `
                },
                "width": "15%"
            },

        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}