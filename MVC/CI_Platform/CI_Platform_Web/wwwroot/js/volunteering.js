

//For Add to Favourite Mission

function a11(i, j) {
    console.log("A");
    $.ajax({
        url: '/Home/addToFav?id=' + i + '&mid=' + j,
        success: function (data) {
            console.log("success");
            console.log(data);
            if (data == 1) {
                //$('#fafav').style.color="#F88634";
                document.getElementById("favfav").style.color = "#f0022a";
                //document.getElementById("favtext").innerHTML="added";
                $("#favtext").text("Added to Favourite");
            }
            else {
                //$('#fafav').style.color="#F88634";
                document.getElementById("favfav").style.color = "#000000";
                //document.getElementById("favtext").innerHTML="added";
                $("#favtext").text("Add to Favourite");
            }

        }
    });
}

//For the User Rating

function myRate(a, b, c) {
    console.log(c);
    $.ajax({
        url: '/Home/Ratee?uid=' + a + '&mid=' + b + '&rating=' + c,
        success: function (data) {
            console.log(data);
            console.log("rate success");
            //debugger;
            if (data == 1) {
                console.log("RATARATAR")
                for (var i = 1; i < c + 1; i++) {
                    $('#' + i).css('color', 'yellow');
                    //console.log($('#'+i));
                }
            }
            else {
                console.log("alredy rated")
                for (var i = 1; i < 6; i++) {
                    console.log("inside for")
                    if (i < c + 1) {

                        console.log("inside if")
                        console.log(i)
                        console.log(c)
                        $('#' + i).css('color', 'yellow');
                    }
                    else {
                        console.log("inside else")
                        $('#' + i).css('color', 'black');
                    }

                }



            }
        }
    });
}