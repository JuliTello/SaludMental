﻿let datatable;
$(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#table_tests').DataTable({
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
            url: "/Tests/ListarTodos"
        },
        columns: [
            { data: "testId", width: "10%", className: "text-center", orderable: false },
            { data: "nombreTest", className: "text-center" }, 
            { data: "descripcion", className: "text-center" }, 
            {
                data: "testId",
                render: function (data) {
                    return `
                        <a href="/Tests/Edit/${data}" class="btn btn-sm btn-success text-white" style="cursor:pointer;">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <a onclick=Delete("/Tests/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer;">
                            <i class="bi bi-trash3-fill"></i>
                        </a>
                         <a href="/Tests/Details/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer;">
                           <i class="bi bi-ui-checks"></i> Realizar Test
                        </a>
                    `;
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            },
        ]
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de eliminar este curso?",
        text: "Este registro no se podrá recuperar",
        html: true,
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url, 
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload(); 
                    }
                    else {
                        toastr.error(data.message); 
                    }
                }
            });
        }
    });
}