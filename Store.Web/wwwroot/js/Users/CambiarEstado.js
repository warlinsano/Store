$('table tbody').on('click', '.CambiarEstado', function (e) {

    e.preventDefault();

    debugger;

    var id = $(this).attr('data-id');
    var estado = $(this).attr('data-status');
    var estadoName = (estado == 'True') ? 'activar' : 'desactivar'
    var input = $(this);

    Swal.fire({
        title: 'Estado',
        text: '¿Está seguro de que desea ' + estadoName + ' este Usuario?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, Cambiar'
    }).then((result) => {
        if (result.value) {

            $.ajax({
                type: "POST",
                url: "/Account/CambiarEstado",
                data: { id: id, Estado: estado },
                success: function (data) {

                    if (data.data == true) {

                        if (estadoName == 'activar') {
                            input.prop("checked", true);
                            input.attr('data-status', "False");

                        } else if (estadoName == 'desactivar') {
                            input.prop("checked", false);
                            input.attr('data-status', "True");
                        }

                        Swal.fire(
                            'Correcto!',
                            'Se ha cambiado el estado Correctamente.',
                            'success'
                        );
                        //cosyAlert('<strong>Correcto!!</strong><br />Se ha cambiado el estado Correctamente.', 'success', { showTime: 1000, hideTime: 1000, vPos: 'bottom', hPos: 'right' });
                    }
                    else if (data.data == false) {
                        Swal.fire(
                            'Error Registro',
                            'Hubo un error al cambiar el estado.',
                            'warning'
                        )
                    }

                },
                error: function (xhr, status, error) {
                    cosyAlert('<strong>Error!!</strong><br/>Error al conectar con el servidor no se pudo cambiar el estado...', 'error', { showTime: 1000, hideTime: 1000, vPos: 'bottom', hPos: 'right' });
                }
            });

        } else if (result.dismiss === Swal.DismissReason.cancel) {

            Swal.fire(
                'Cancelado',
                'Se ha cancelado',
                'error'
            )
        }

    });

});