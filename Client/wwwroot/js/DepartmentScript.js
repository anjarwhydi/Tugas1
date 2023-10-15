// Fokuskan input 'editDepartmentName' saat modal edit muncul
$('#editModal').on('shown.bs.modal', function () {
    $('#editDepartmentName').trigger('focus');
});

$(document).ready(function () {
    // Ambil URL saat ini
    var currentUrl = window.location.href;

    // Beri tanda aktif pada tautan menu yang sesuai dengan URL saat ini
    $('.nav-treeview a').each(function () {
        if (currentUrl.indexOf($(this).attr('href')) > -1) {
            $(this).addClass('active');
        }
    });

    // Inisialisasi DataTable
    var table = $('#datatable').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "ajax": {
            url: "https://localhost:7140/api/Departments/Department",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "order": [[1, 'asc']],
        "columns": [
            {
                "data": null,
                "orderable": false
            },
            {
                "data": "name"
            },
            {
                "data": null,
                "orderable": false,
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning btn-sm edit-button" data-operation="edit" data-target="#Modal" data-toggle="modal" data-tooltip="tooltip" data-placement="left" onclick="GetById(\'' + row.deptID + '\');" title="Edit"><i class="fas fa-edit"></i></button>' + ' ' +
                        '<button type="button" class="btn btn-danger btn-sm remove-button" data-tooltip="tooltip" data-placement="right" onclick="Delete(\'' + row.deptID + '\')" title="Delete"><i class="fas fa-trash"></i></button>';
                }
            }
        ]
    });

    // Event saat perubahan pada DataTable
    table.on('draw.dt', function () {
        var PageInfo = $('#datatable').DataTable().page.info();
        table.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });
});

// Fungsi untuk membersihkan form input
function clearSave() {
    $('#input-id').hide();
    $("#departError").text("");
    $('#DepartmentName').val('');
    $('#save-button').show();
    $('#update-button').hide();
}

// Fungsi untuk menyimpan data departemen baru
function Save() {
    var departmentName = $("#DepartmentName").val();
    $("#departError").text("");

    // Validasi input
    if (!departmentName) {
        $("#departError").text("Department name is required");
        Swal.fire({
            title: 'Validation Error',
            text: 'Please enter a department name',
            icon: 'error'
        });
        return;
    }

    // Membuat objek Department untuk dikirim ke server
    var Department = {
        DeptID: '',
        Name: departmentName
    };

    // Mengirim data departemen baru ke server
    $.ajax({
        url: 'https://localhost:7140/api/Departments/Department',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(Department),
        success: function () {
            Swal.fire({
                title: 'Success',
                text: 'Department has been added successfully',
                icon: 'success'
            });
            $('#datatable').DataTable().ajax.reload();
            $('#DepartmentName').val('');
            $("#Modal").modal("hide");
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                text: 'An error occurred while adding the department',
                icon: 'error'
            });
            $('#DepartmentName').val('');
        }
    });
}

// Fungsi untuk mengupdate data departemen
function Update() {
    var departmentName = $("#DepartmentName").val();
    $("#departError").text("");

    // Validasi input
    if (!departmentName) {
        $("#departError").text("Department name is required");
        Swal.fire({
            title: 'Validation Error',
            text: 'Please enter a department name',
            icon: 'error'
        });
        return;
    }

    // Membuat objek Department untuk dikirim ke server
    var Department = {
        DeptID: $("#Id").val(),
        Name: departmentName
    };

    // Mengirim data departemen yang diupdate ke server
    $.ajax({
        url: 'https://localhost:7140/api/Departments/Department',
        type: 'PUT',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(Department),
        success: function () {
            Swal.fire({
                title: 'Success',
                text: 'Department has been updated successfully',
                icon: 'success'
            });
            $('#datatable').DataTable().ajax.reload();
            $("#Modal").modal("hide");
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                text: 'An error occurred while updating the department',
                icon: 'error'
            });
        }
    });
}

// Fungsi untuk menghapus data departemen
function Delete(deptID) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: 'https://localhost:7140/api/Departments/' + deptID,
                type: 'DELETE',
                contentType: 'application/json',
                dataType: 'json',
                success: function () {
                    Swal.fire(
                        'Deleted!',
                        'Your department has been deleted.',
                        'success'
                    );
                    $('#datatable').DataTable().ajax.reload();
                },
                error: function () {
                    Swal.fire(
                        'Error!',
                        'An error occurred while deleting the department.',
                        'error'
                    );
                }
            });
        }
    });
}

// Fungsi untuk mendapatkan data departemen berdasarkan ID
function GetById(deptID) {
    $.ajax({
        url: "https://localhost:7140/api/Departments/" + deptID,
        type: "GET",
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            $("#departError").text("");
            $('#input-id').show();
            $('#save-button').hide();
            $('#update-button').show();
            $('#Id').val(obj.deptID);
            $('#DepartmentName').val(obj.name);
        }
    });
}

$(document).ajaxComplete(function () {
    $('[data-tooltip="tooltip"]').tooltip({
        trigger: 'hover'
    });
});
