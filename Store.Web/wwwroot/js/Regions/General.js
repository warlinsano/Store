$(document).ready(function () {

    var table = $('.table').DataTable();

    //$('table tbody').on('click', 'td.td_detalles', function () {
    //    //var tr = $(this).closest('tr');
    //    //var row = table.row(tr);
    //    //debugger;
    //    //var id = row.data().RegionId;
    //    //console.log(id); 
    //    $.ajax({
    //        url: "/Regions/Details",
    //        method: 'GET',
    //        data: { id: 1 },
    //        type: "json",
    //        success: function (data) { 
    //            //debugger;
    //            $("#Regiondatails").val(data.descripcion);
    //            //console.log(data.descripcion);
             
    //        }
    //    });
    //});
});

  //---- Detalles
    function Detalles(key){
        console.log(key);
        $.ajax({
            url: "/Regions/Details",
            method: 'GET',
            data: { id: key },
            type: "json",
            success: function (data) {
                //debugger;
                $("#Regiondatails").val(data.descripcion);
                console.log(data.descripcion);
            }
        });
}

  //---- Create
   $('#btn_Create').click(function () {
          debugger;
          var ObjRegion = {
              //"RegionId": null,
                "RegionDescription": $("#RegionDescriptionCreate").val(),
             };

              $.ajax({
                  url: "/Regions/Create",
                  method: 'POST',
                  data: { Region: ObjRegion },
                  type: "json",
                  success: function (data) {
                      console.log(data);
                  }
              });
      });

 //GET
 //---- Edit
function Editar(key) {
 
    $.ajax({
        url: "/Regions/Details",
        method: 'GET',
        data: { id: key },
        type: "json",
        success: function (data) {
            debugger;
            $("#IdRegionEdit").val(data.Id);
            $("#RegionEdit").val(data.descripcion);
            console.log(data.Id);
            console.log(data.descripcion);
        }
    });
}

  ////POST
  ////---- Edit
  //    $('#btn_Create').click(function () {
  //        debugger;
  //        var ObjRegion = {
  //            //"RegionId": null,
  //            "RegionDescription": $("#RegionDescriptionCreate").val(),
  //        };

  //        $.ajax({
  //            url: "/Regions/Create",
  //            method: 'POST',
  //            data: { Region: ObjRegion },
  //            type: "json",
  //            success: function (data) {
  //                console.log(data);
  //            }
  //        });
  //    });

        // Evento que envía una petición Ajax al servidor
        //$('#botonPrueba').click(function (e) {

            //var coche = {
            //    marca: $('#marca').val().trim(),
            //    modelo: $('#modelo').val()
            //};

            //$.ajax({
            //    type: "POST",
            //    url: "/Regions/Details",
            //    content: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: id:1,
            //    success: function (d) {
            //        if (d.success == true)
            //            alert('Has introducido un nuevo coche!!');
            //        else { }
            //    },
            //    error: function (xhr, textStatus, errorThrown) {
            //        alert('Error!!');
            //    }
            //});
        //});




//$('#buttonDemo1').click(function () {
//    debugger;
//    var Region = {
//        "RegionId": 1,
//        "RegionDescription": $("#RegionDescription").val(),
//    };

//    $.ajax({
//        url: "/Regions/Create",
//        method: 'POST',
//        data: { Region: Region },
//        type: "json",
//        success: function (data) {
//            debugger;
//            var descp = data.RegionDescription;
//            console.log(data);
//            alert(data.RegionDescription);
//        }
//    }
//  });


//btn Editar
//$('table tbody').on('click', 'td.td_region', function () {
//    //debugger;
//    var tr = $(this).closest('tr')
//    tr.find("input").css({ "border": "2px solid blue" }).prop('disabled', false).attr('hidden', false);
//    tr.find("a").css({ "border": "2px solid blue" }).attr('hidden', true);
//});

    // tr.children().css({"color": "red", "border": "2px solid red"});
    //tr.children().find("input").css({ "color": "red", "border": "2px solid red" });}
    //tr.find("input").css({ "color": "red", "border": "2px solid blue" }).prop('disabled', false);;

      //$.ajax({
      //      url: "/Imagen/GetImagen",
      //      method: 'GET',
      //      data: { Id_Imagen: imagen },
      //      type: "json",
      //      async: true,
      //      success: function (data) {

      //          $("#preview").parent().parent().show();

      //          $("#preview").attr("src", "" + data[0].ImagenBase64 + "");
      //      }
      //  })