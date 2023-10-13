$(document).ready(function () {

    let masterData = "https://localhost:7140/api/Employees/Employees"

    let activeData = "https://localhost:7140/api/Employees/GetActiveEmpDept"

    let resignData = "https://localhost:7140/api/Employees/GetNonActiveEmpDept"

    $("#employeeData").on('change', () => {
        let employeeData = $('#employeeData').val()

        table.ajax.url(employeeData == 'master' || employeeData == '' ? masterData : (employeeData == 'active' ? activeData : resignData)).load();

        table.ajax.reload()
    });


    var currentUrl = window.location.href;
    $('.nav-treeview a').each(function () {
        if (currentUrl.indexOf($(this).attr('href')) > -1) {
            $(this).addClass('active');
        }
    });
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
                "render": function (data, type, row, meta) {
                    return row.isActive ? '<span class="badge badge-success" >Active</span>' : '<span class="badge badge-danger" >Resign</span>';
                }
            },
            { "data": "departName" },
            {
                "data": null,
                "orderable": false,
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning btn-sm edit-button" data-operation="edit" data-target="#Modal" data-toggle="modal" data-tooltip="tooltip" data-placement="left" onclick="GetByNik(\'' + row.nik + '\');" title="Edit"><i class="fas fa-edit"></i></button>' + ' ' +
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
    $("#firstNameError").text("");
    $("#lastNameError").text("");
    $("#addressError").text("");
    $("#phoneError").text("");
    $("#departmentError").text("");
    $('.form-control').val('');
    $('#save-button').show();
    $('#update-button').hide();
    $('#input-nik').hide()
    $('#input-email').hide()
}

function Save() {
    var firstName = $("#inputFirstName").val();
    var lastName = $("#inputLastName").val();
    var address = $("#inputAddress").val();
    var phoneNumber = $("#inputPhone").val();
    var departID = $("#inputDepart").val();

    $("#firstNameError").text("");
    $("#lastNameError").text("");
    $("#addressError").text("");
    $("#phoneError").text("");
    $("#departmentError").text("");

    var errors = [];

    if (!firstName) {
        errors.push("First Name is required");
        $("#firstNameError").text("First Name is required");
    }
    if (!lastName) {
        errors.push("Last Name is required");
        $("#lastNameError").text("Last Name is required");
    }
    if (!address) {
        errors.push("Address is required");
        $("#addressError").text("Address is required");
    }
    if (!phoneNumber) {
        errors.push("Phone number is required");
        $("#phoneError").text("Phone number is required");
    }
    if (!departID) {
        errors.push("Department is required");
        $("#departmentError").text("Department is required");
    }

    if (errors.length > 0) {
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


//function Save() {
//    var firstName = $("#inputFirstName").val();
//    var lastName = $("#inputLastName").val();
//    var address = $("#inputAddress").val();
//    var phoneNumber = $("#inputPhone").val();
//    var departID = $("#inputDepart").val();

//    $("#firstNameError").text("");
//    $("#lastNameError").text("");
//    $("#addressError").text("");
//    $("#phoneError").text("");
//    $("#departmentError").text("");

//    if (!firstName) {
//        $("#firstNameError").text("First Name is required");
//    }
//    if (!lastName) {
//        $("#lastNameError").text("Last Name is required");
//    }
//    if (!address) {
//        $("#addressError").text("Address is required");
//    }
//    if (!phoneNumber) {
//        $("#phoneError").text("Phone number is required");
//    }
//    if (!departID) {
//        $("#departmentError").text("Department is required");
//    }


//    var Employee = {
//        nik: $("#nik").val(),
//        firstName: firstName,
//        lastName: lastName,
//        email: $("#email").val(),
//        address: address,
//        phoneNumber: phoneNumber,
//        isActive: true,
//        departID: departID
//    };

//    $.ajax({
//        url: 'https://localhost:7140/api/Employees/Employee',
//        type: 'POST',
//        contentType: 'application/json',
//        dataType: 'json',
//        data: JSON.stringify(Employee),
//        success: function () {
//            Swal.fire({
//                title: 'Success',
//                text: 'Employee has been added successfully',
//                icon: 'success'
//            });
//            $('#datatable').DataTable().ajax.reload();
//            $("#Modal").modal("hide");
//        },
//        error: function () {
//            Swal.fire({
//                title: 'Error',
//                text: 'An error occurred while adding the employee',
//                icon: 'error'
//            });
//        }
//    });
//}


function Update() {
    var firstName = $("#inputFirstName").val();
    var lastName = $("#inputLastName").val();
    var address = $("#inputAddress").val();
    var phoneNumber = $("#inputPhone").val();
    var departID = $("#inputDepart").val();

    $("#firstNameError").text("");
    $("#lastNameError").text("");
    $("#addressError").text("");
    $("#phoneError").text("");
    $("#departmentError").text("");

    var errors = [];

    if (!firstName) {
        errors.push("First Name is required");
        $("#firstNameError").text("First Name is required");
    }
    if (!lastName) {
        errors.push("Last Name is required");
        $("#lastNameError").text("Last Name is required");
    }
    if (!address) {
        errors.push("Address is required");
        $("#addressError").text("Address is required");
    }
    if (!phoneNumber) {
        errors.push("Phone number is required");
        $("#phoneError").text("Phone number is required");
    }
    if (!departID) {
        errors.push("Department is required");
        $("#departmentError").text("Department is required");
    }

    if (errors.length > 0) {
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


function Delete(rowNik) {
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
                url: 'https://localhost:7140/api/Employees/' + rowNik,
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

function GetByNik(rowNik) {
    $.ajax({
        url: "https://localhost:7140/api/Employees/" + rowNik,
        type: "GET",
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            var obj = result.data;
            $("#firstNameError").text("");
            $("#lastNameError").text("");
            $("#addressError").text("");
            $("#phoneError").text("");
            $("#departmentError").text("");
            $('#save-button').hide();
            $('#update-button').show();
            $('#input-nik').show();
            $('#input-email').show();
            $('#nik').val(obj.nik);
            $('#email').val(obj.email);
            $('#inputFirstName').val(obj.firstName);
            $('#inputLastName').val(obj.lastName);
            $('#inputAddress').val(obj.address);
            $('#inputPhone').val(obj.phoneNumber);
            $('#inputDepart').val(obj.department_ID);

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

