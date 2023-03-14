

console.log("Script working");
var checkboxes = document.querySelectorAll(" #state .checkbox");
console.log(checkboxes)


$(document).ready(function () {
    console.log("java workinhh");
    GetCountry();
    GetThemes();



    $('#Country').change(function () {
        console.log("Onchange called")
        //console.log($(this).val());
        var id = $(this).val();

        console.log(id)
        $('#City').empty();
       /* $('#City').append('<Option> City </option>');*/
        $.ajax({
            url: '/Home/City?id=' + id,
            success: function (result) {
                console.log(result);
                $.each(result, function (i, data) {
                    console.log(data.name);
                    $('#City').append('<li style="padding:5px"><input class="form-check-input checkbox" id="' + data.name + '"style="margin-right:10px" type="checkbox" value="' + data.name + '">' + '<label class="form-check-label" for="flexCheckDefault">' + data.name + '</lable></li>');
                    //$('#City').append('<option value=' + data.name + '>' + data.name + '</option>');
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


function GetThemes() {
    $.ajax(

        {
            url: '/Home/Themes',
            success: function (data) {
                console.log(data);
                $.each(data, function (i, result) {
                    //console.log(result.name);
                    $('#Themes').append('<option value=' + result.missionThemeId + '>' + result.title + '</option>');
                    // $('#state').append('<li><div class= "form-check"> <input class="form-check-input" type="checkbox"value="' + result.name + '"id="flexCheckDefault"/><label class="form-check-label" for="flexCheckDefault">' + result.name + '</label </div ></li >');
                });
            }
        }
    );
}