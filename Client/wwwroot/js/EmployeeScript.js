$(document).ready(function () {
    var table = $('#datatable').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "ajax": {
            url: "https://localhost:7140/api/Employees/Employees",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "order": [[1, 'asc']],
        "columns": [
            {
                "data": null,
                "orderable": false,
            },
            { "data": "nik" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return row.firstName + " " + row.lastName;
                } },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "address" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return row.isActive ? 'Aktif' : 'Resign';
                }
            },
            { "data": "departName" },
            {
                "data": null,
                "orderable": false,
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning btn-sm edit-button" data-operation="edit" data-target="#Modal" data-toggle="modal" data-tooltip="tooltip" data-placement="left" onclick="clearUpdate();GetByNik(\'' + row.nik + '\');" title="Edit"><i class="fas fa-edit"></i></button>' + ' ' +
                        '<button type="button" class="btn btn-danger btn-sm remove-button" data-tooltip="tooltip" data-placement="right" onclick="Delete(\'' + row.nik + '\')" title="Delete"><i class="fas fa-trash"></i></button>'
                }
            }
        ]
    })
    table.on('draw.dt', function () {
        var PageInfo = $('#datatable').DataTable().page.info();
        table.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

        function Departments() {
            $.ajax({
                url: 'https://localhost:7140/api/Departments/Department',
                type: 'GET',
                dataType: 'json',
                success: function (response) {
                        var select = $('#inputDepart');
                        select.empty();

                        select.append($('<option>', {
                            value: '',
                            text: 'Choose...'
                        }));

                        response.data.forEach(function (department) {
                            select.append($('<option>', {
                                value: department.deptID,
                                text: department.name
                            }));
                        });
                },
            });
        }

        Departments();

})

function clearSave() {
    /*$('#input-id').hide();*/
    $('#DepartmentName').val('');
    $('#save-button').show();
    $('#update-button').hide();
}

function clearUpdate() {
    /*$('#DepartmentName').val('');*/
    /*$('#input-id').hide();*/
    $('#save-button').hide();
    $('#update-button').show();
}

function Save() {
    var firstName = $("#inputFirstName").val();
    var lastName = $("#inputLastName").val();
    var address = $("#inputAddress").val();
    var phoneNumber = $("#inputPhone").val();
    var departID = $("#inputDepart").val();

    if (!firstName || !lastName || !address || !phoneNumber || !departID) {
        Swal.fire({
            title: 'Validation Error',
            text: 'Please fill out all the required fields',
            icon: 'error'
        });
        return;
    }

    var Employee = {
        nik: $("#nik").val(),
        firstName: firstName,
        lastName: lastName,
        email: $("#email").val(),
        address: address,
        phoneNumber: phoneNumber,
        isActive: true,
        departID: departID
    };

    $.ajax({
        url: 'https://localhost:7140/api/Employees/Employee',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(Employee),
        success: function () {
            Swal.fire({
                title: 'Success',
                text: 'Employee has been added successfully',
                icon: 'success'
            });
            $('#datatable').DataTable().ajax.reload();
            $("#Modal").modal("hide");
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                text: 'An error occurred while adding the employee',
                icon: 'error'
            });
        }
    });
}


function Update() {
    var firstName = $("#inputFirstName").val();
    var lastName = $("#inputLastName").val();
    var address = $("#inputAddress").val();
    var phoneNumber = $("#inputPhone").val();
    var departID = $("#inputDepart").val();

    if (!firstName || !lastName || !address || !phoneNumber || !departID) {
        Swal.fire({
            title: 'Validation Error',
            text: 'Please fill out all the required fields',
            icon: 'error'
        });
        return;
    }

    var Employee = {
        nik: $("#nik").val(),
        firstName: firstName,
        lastName: lastName,
        email: $("#email").val(),
        address: address,
        phoneNumber: phoneNumber,
        isActive: true,
        department_ID: departID
    };

    $.ajax({
        url: 'https://localhost:7140/api/Employees/Employee',
        type: 'PUT',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(Employee),
        success: function () {
            Swal.fire({
                title: 'Success',
                text: 'Employee has been added successfully',
                icon: 'success'
            });
            $('#datatable').DataTable().ajax.reload();
            $("#Modal").modal("hide");
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                text: 'An error occurred while adding the employee',
                icon: 'error'
            });
        }
    });
}


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


//function GetByNik(rownik) {
//    $.ajax({
//        url: "https://localhost:7140/api/Employees/" + rownik,
//        type: "GET",
//        contentType: 'application/json',
//        dataType: 'json',
//        success: function (result) {
//            var obj = result.data;
//            $('#nik').val(obj.nik);
//            $('#inputFirstName').val(obj.firstName);
//            $('#inputLastName').val(obj.lastName);
//            $('#inputAddress').val(obj.address);
//            $('#inputPhone').val(obj.phoneNumber);
//        }

//    })
//    console.log(result);
//}

function GetByNik(rowNik) {
    $.ajax({
        url: "https://localhost:7140/api/Employees/" + rowNik,
        type: "GET",
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            $('#nik').val(obj.nik);
            $('#email').val(obj.email);
            $('#inputFirstName').val(obj.firstName);
            $('#inputLastName').val(obj.lastName);
            $('#inputAddress').val(obj.address);
            $('#inputPhone').val(obj.phoneNumber);
        },
        error: function (error) {
            console.error("Error:", error);
        }
    });
}


$(document).ajaxComplete(function () {
    $('[data-tooltip="tooltip"]').tooltip({
        trigger: 'hover'
    })
});

