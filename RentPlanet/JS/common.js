
$(document).ready(function () {
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        console.log('IpAddress', e.ip);
    });

    $("#contact").hide();
    $('#view').click(function () {
        $("#contact").show();
        $("#view").hide();
    });
    //.........For Input Type Number......................
     $(document).on("input", ".data", function (e) {
        this.value = this.value.replace(/[^\d\\-]/g, '');
        })
    //.............For Validation..........................
    $("Sumbit").click(function (e) {
        e.preventDefault();
        })
    //.........Reset Form..........................
    $("#ClearData").click(function () {
        $('#ManageRoom').trigger("reset");
    })
    //...........For Timing........................
    const timeIntervals = [
        ["day", 86400000],
        ["hour", 3600000],
        ["minute", 60000],
        ["second", 1000]
    ];
    const tick = (start) => () => {
        let elapsed = Date.now() - start;
        for (let [unit, ms] of timeIntervals) {
            $("#" + unit).text(Math.floor(elapsed / ms));
            elapsed %= ms;
        }
    };
    let timer = window.setInterval(tick(Date.now()), 1000);

})
   
    //.........Add Data Function..........................
    function Add() {
    var Obj = {
        Description: $('#Description').val(),
        PricesRoom: $('#PricesRoom').val(),
        Contact: $('#Contact').val(),
        Hospital: $('#Hospital').val(),
        Religion: $('#Religion').val(),
        Shop: $('#Shop').val(),
        Metro: $('#Metro').val(),
        RoomLocation: $('#RoomLocation').val(),
        City: $('#City').val(),
        State: $('#State').val(),
        File1: $('#File1').val(),
        File2: $('#File2').val(),
        File3: $('#File3').val(),
              };
       
        var formData = new FormData();
        formData.append("Description", $('#Description').val()),
        formData.append("PricesRoom", $('#PricesRoom').val()),
        formData.append("file", $('#File1')[0].files[0]);
        console.log(formData)
        $.ajax({
            url: "/manageroom/managerooms",
            data: JSON.stringify(Obj),
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            success: function (result) {
                window.location.href = '/manageroom/room';
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
        }
    //.........Function for getting the Data Based on ID.........
    function Getid(id) {
            if (id > 0) {
        $.ajax({
            url: "/manageroom/getid/" + id,
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            success: function (result) {
                $('#ID').val(result.ID),
                    $('#Description').val(result.Description),
                    $('#PricesRoom').val(result.PricesRoom),
                    $('#Contact').val(result.Contact),
                    $('#Hospital').val(result.Hospital),
                    $('#Religion').val(result.Religion),
                    $('#Shop').val(result.Shop),
                    $('#Metro').val(result.Metro),
                    $('#RoomLocation').val(result.RoomLocation),
                    $('#PostelCode').val(result.PostelCode),
                    $('#City').val(result.City),
                    $('#State').val(result.State),
                    $('#Country').val(result.Country)
                $('#btnUpdate').show();
                $('#btnAdd').hide();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
            }
    return false;
        }
    //.........function for updating  record.............
    function UpdateRooms() {
            var Obj = {
        Description: $('#Description').val(),
    PricesRoom: $('#PricesRoom').val(),
    Contact: $('#Contact').val(),
    Hospital: $('#Hospital').val(),
    Religion: $('#Religion').val(),
    Shop: $('#Shop').val(),
    PostelCode: $('#PostelCode').val(),
    Metro: $('#Metro').val(),
    RoomLocation: $('#RoomLocation').val(),
    City: $('#City').val(),
    State: $('#State').val(),
    Country: $('#Country').val(),
    ID: $('#ID').val()
            };
    $.ajax({
        url: "/manageroom/updaterooms",
    data: JSON.stringify(Obj),
    type: "POST",
    contentType: "application/json",
    dataType: "json",
    success: function (result) {
        alert("Success");
    window.location.href = '/manageroom/room';
                },
    error: function (errormessage) {
        alert(errormessage.responseText);
                }
            });
        }
    //.............For Dropdowen..........................
    $.ajax({
        type: "GET",
    dataType: "json",
    contentType: "application/json",
    url: "/manageroom/states",
    success: function (result) {
        console.log(result);
    for (var i = 0; i < result.length; i++) {
        $("#State").append($("<option></option>").val(result[i].id).html(result[i].name));
                }
            },
    error: function (response) {
        console.log('error');
            }
        });
    $('#State').on('change', function () {
        $("#City").empty()
            $("#City").append($("<option value='0'>Select City</option>"))
    id = $(this).val()
    $.ajax({
        type: "GET",
    dataType: "json",
    contentType: "application/json",
    url: `/manageroom/cities?id=${id}`,
    success: function (result) {
        console.log(result);
    for (var i = 0; i < result.length; i++) {
        $("#City").append($("<option></option>").val(result[i].id).html(result[i].city));
                    }
                },
    error: function (response) {
        console.log('error');
                }
            });
        });
