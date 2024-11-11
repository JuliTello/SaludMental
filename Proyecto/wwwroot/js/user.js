let datatable;
$(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#table_users').DataTable({
        language: {
            lengthMenu: "Mostrar _MENU_ registros por página",
            zeroRecords: "No hay registros disponibles.",
            info: "Pág. _PAGE_ de _PAGES_ - Mostrando del _START_ al _END_ de _TOTAL_ registros",
            infoEmpty: "No hay registros disponibles.",
            infoFiltered: "(filtrado de un total _MAX_ registros)",
            loadingRecords: "Cargando en curso...",
            emptyTable: "No hay registros disponibles.",
            search: "Buscar",
            paginate: {
                first: "Primero",
                last: "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        ajax: {
            url: "/Users/ListarTodos"
        },
        columns: [
            {
                data: "lockoutEnabled",
                render: function (data) {
                    if (data == true) {
                        return `<span class="btn btn-sm btn-danger"><i class="bi bi-lock-fill">Bloquear</i></span>`;
                    } else {
                        return `<span class="btn btn-sm btn-success"><i class="bi bi-unlock-fill">Desbloquear</i></span>`;
                    }
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            },
            {
                data: {
                    firstName: "firstName", lastName: "lastName"
                },
                render: function (data) { return data.lastName + " " + data.firstName }
            },
            { data: "email" },
            { data: "phoneNumber" },
            { data: "role" },
            {
                data: "lockoutEnabled",
                render: function (data) {
                    if (data == true) {
                        return `<span class="text-success">ACTIVO</span>`;
                    } else {
                        return `<span class="text-danger">INACTIVO</span>`;
                    }
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            },
            {
                data: "id",
                render: function (data) {
                    return `
                        <button class="btn btn-sm btn-info" style="cursor:pointer;" onclick="abrirModalCambioRol('${data}')">
                            <i class="bi bi-person-rolodex">Cambiar Rol</i>
                        </button>
                    `;
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            },
        ]
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de eliminar este estudiante?",
        text: "Este registro no se podrá recuperar",
        html: true,
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url, // Students/Delete/5
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload(); // Recargar el datatable
                    }
                    else {
                        toastr.error(data.message); // Notificaciones
                    }
                }
            });
        }
    });
}

function abrirModalCambioRol(userId) {
    $('#userId').val(userId);
    $('#changeRoleModal').modal('show');
}

function guardarCambiosRol() {
    const userId = $('#userId').val();
    const nuevoRol = $('#roleSelect').val();

    $.ajax({
        type: "POST",
        url: '/Users/CambiarRol',
        data: { userId: userId, nuevoRol: nuevoRol },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                $('#changeRoleModal').modal('hide');
                datatable.ajax.reload();
            } else {
                toastr.error(response.message);
            }
        }
    });
}