

console.log("Script working");



$(document).ready(function () {
    console.log("javascript working");
 

    $('#Country').change(function () {
        console.log("Onchange called")
        //console.log($(this).val());
        var id = $(this).val();

        console.log(id)
        $('#City').empty();
        $('#City').append('<Option> Select Your City </option>');
        $.ajax({
            url: '/Home/City?id=' + id,
            success: function (result) {
                console.log(result);
                $.each(result, function (i, data) {
                    console.log(data.name);
                    //$('#City').append('<li style="padding:5px"><input class="form-check-input checkbox" id="' + data.name + '"style="margin-right:10px" type="checkbox" value="' + data.name + '">' + '<label class="form-check-label" for="flexCheckDefault">' + data.name + '</lable></li>');
                    $('#City').append('<option value=' + data.cityId + '>' + data.name + '</option>');
                })
            }

        });
    });

});


function GetCountry() {
    $.ajax(
        {
            url: '/Home/Country',
            success: function (data) {
                console.log(data);
                $.each(data, function (i, result) {
                    //console.log(result.name);
                    $('#Country').append('<option value=' + result.countryId + '>' + result.name + '</option>');
                    //$('#Country').append('<li style="padding:5px"><input class="form-check-input checkbox" id="' + result.name + '"style="margin-right:10px" type="checkbox" value="' + result.countryId + '">' + '<label class="form-check-label" for="flexCheckDefault">' + result.name + '</lable></li>');
                });
            }
        }
    );
}






