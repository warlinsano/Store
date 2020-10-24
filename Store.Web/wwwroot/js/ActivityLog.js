$(document).ready(function () {
    $('.table').DataTable({
        "bLengthChange": false,
        responsive: true,
        language: {
            processing: "Procesando",
            search: "Buscar:",
            lengthMenu: "Ver _MENU_ Filas",
            info: "_START_ - _END_ de _TOTAL_ elementos",
            infoEmpty: "0 - 0 de 0 elementos",
            infoFiltered: "(Filtro de _MAX_ entradas en total)",
            infoPostFix: "",
            loadingRecords: "Cargando datos.",
            zeroRecords: "No se encontraron datos",
            emptyTable: "No hay datos disponibles",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Ultimo"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre décroissant"
            }
        },
        "ajax": {
            "type": "Get",
            "url": "/Account/Datos",
            "datatype": "JSON"
        },
        "columns": [
            {
                "data": "photo", "render": function (data, type, row, meta) {
                    if (row['photo'] != null && row['photo'] != "") {
                        return '<img class="img-circle" src=' + row["photo"].substring(1) + ' height="50" width="50" />';
                    }
                    else {
                        return '<img class="img-circle " src="/img/no-profile.png" height="50" width="50" />';
                    }
                }
            },
            { "data": "userEMail" },
            { "data": "userFullName" },
            { "data": "date" },
            { "data": "controller" },
            { "data": "action" },
            { "data": "description" },
            {
                "data": "status", "render": function (data, type, row, meta) {
                    if (row["status"]) {
                        return '<span class="label label-success">Success</span>';
                    }
                    else {
                        return '<span class="label label-danger">Error</span>';
                    }
                  }
            },
            { "data": "type" },
            { "data": "ip" },
            { "data": "macAddress" } 
        ]
    });
});

//function edite(vas) {
//    document.getElementById("ModalTitle").innerHTML = "Editar Categoria";
//    $(".modal-body").load("/Categorias/Edit/" + vas);
//};

function create() {
    document.getElementById("ModalTitle").innerHTML = "Agregar Empleado";
    $(".modal-body").load("/Employees/create/");
};

//function delet(id) {
//    document.getElementById("ModalTitle").innerHTML = "Eliminar Categoria";
//    $(".modal-body").load("/Categorias/delete/" + id);
//};

function GuardarModal() {
    var valor = document.getElementById("LastName").value;
    if (valor == null || valor.length == 0 || valor.length == "") {
        $("#LastName").parent().parent().attr("class", "form-group has-error");
        $("#LastName").parent().children("span").text("El campo Nombre es obligatorio").show();
        return false;
    } else {
        var oTable = $('.table').DataTable();
        //debugger;   
        //var model = new FormData($('form').get(0));
        var url = $('#AgregarId')[0].action;
        $.ajax({
            type: "POST",
            url: url,
            data: $('#AgregarId').serialize(),
            success: function (data) {
                oTable.ajax.reload();
                if (data) {
                    //$.notification.show('success', 'Datos insertados correctamente.');
                    cosyAlert('<strong>Agregad!!</strong><br />Datos insertados correctamentet...', 'success', { showTime: 1000, hideTime: 1000, vPos: 'bottom', hPos: 'right' });
                }
                else {
                    //toastr.danger(' Hubo un al  insertar registro.', 'Error Registro');
                    cosyAlert('<strong>Error!!</strong><br/>Hubo un al  insertar registro...', 'error', { showTime: 1000, hideTime: 1000, vPos: 'bottom', hPos: 'right' });
                }
            }
        });

        $("#botonGuardar").click();
        return true;
    }
}


//function EditarModal() {
//    var valor = document.getElementById("Nombre").value;
//    if (valor == null || valor.length == 0 || valor.length == "") {
//        $("#Nombre").parent().parent().attr("class", "form-group has-error");
//        $("#Nombre").parent().children("span").text("El campo Nombre es obligatorio").show();
//        return false;
//    } else {
//        var oTable = $('#myDatatable').DataTable();
//        var url = $('#EditModal')[0].action;
//        $.ajax({
//            type: "POST",
//            url: url,
//            data: $('#EditModal').serialize(),
//            success: function (data) {
//                oTable.ajax.reload();
//                if (data) {
//                    swal("Guardando Registro!", "Registro guardado correctamente!", "success")
//                    $.notification.show('info', ' Datos actualizados correctamente.');
//                }
//                else {
//                    $.notification.show('danger', 'Hubo un al  insertar registro');
//                }
//            }
//        });
//        return true;
//    }
//}

//function deleteModal() {
//    var oTable = $('#myDatatable').DataTable();
//    var url = $('#deletes')[0].action;
//    $.ajax({
//        type: "POST",
//        url: url,
//        data: $('#deletes').serialize(),
//        success: function (data) {
//            oTable.ajax.reload();
//            if (data) {
//                $.notification.show('danger', 'Registro eliminado correctamente.');
//            } else {
//                toastr.danger('Registro eliminado correctamente.', 'Elimiando Registro');
//            }

//        }
//    });
//    return true;
//}
