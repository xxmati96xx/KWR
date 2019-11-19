$(document).ready(function () {
    $("#files").change(function () {

        var licznik = 0;
        var previewimages = $("#showimage");
        previewimages.html("");
        $($(this)[0].files).each(function () {

            var file = $(this);
            var reader = new FileReader();
            reader.onload = function (e) {
                if (licznik <= 100) {
                    ++licznik;
                    var img = $("<img />");
                    img.attr("style", "height:100px;width:img.width; margin-right:15px;margin-bottom:15px;");
                    img.attr("src", e.target.result);

                    previewimages.append(img);
                }
            }
            reader.readAsDataURL(file[0]);

        });

    });
});