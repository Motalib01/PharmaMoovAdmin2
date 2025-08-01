//Custom JS functions for Super Admins

var BASEPATH = "";

//GENERIC MODAL
$(document).ready(function () {
    var ShowModal = $("#ShowGenericModal").val();
    if (ShowModal == 'true') {
        $("#GenericModal").modal('show');
        $("#successIcon").show();
        $("#successBtn").show();
    }

    //duplicate entry
    if (ShowModal == 'false') {
        $("#GenericModal").modal('show');
        $("#errorIcon").show();
        $("#errorBtn").show();
    }
});

function limitText(limitField, limitCount, limitNum) {
    if (limitField.value.length > limitNum) {
        limitField.value = limitField.value.substring(0, limitNum);
    } else {
        limitCount.value = limitNum - limitField.value.length;
    }
}

function validateShopImageFiles(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;

    //validate extension type
    var ext = files[0].type;
    if (ext.split('/')[0] === 'image') {

        var uploadURL = "/Home/";
        if (inputId.includes("Banner") ||inputId.includes("Icon") || inputId.includes("ImageUrl")) {
            uploadURL = BASEPATH + "/Home/UploadIcon";

            //validate size 150KB /149000
            //if (files[0].size < 499000) { //temp 500KB

                //validate dimension
                var reader = new FileReader();
                reader.readAsDataURL(files[0]);
                reader.onload = function (frEvent) {
                    var image = new Image();
                    image.src = frEvent.target.result;
                    image.onload = function () {

                        var height = this.height;
                        var width = this.width;

                        uploadShopImageFiles(inputId, uploadURL);

                        //if (width == 300 && height == 300) {
                        //    //upload image
                        //    uploadShopImageFiles(inputId, uploadURL);
                        //}
                        //else {
                        //    $("#GenericModal").modal('show');
                        //    $("#GenericModalTitle").text('Upload Failed! ');
                        //    $("#GenericModalMsg").text('Required dimension: 300px x 300px');
                        //    $("#errorIcon").show();
                        //    $("#errorBtn").show();
                        //}
                    }
                }
            //}
            //else {
            //    $("#GenericModal").modal('show');
            //    $("#GenericModalTitle").text('Upload Failed! ');
            //    $("#GenericModalMsg").text('Exceeded the 150KB maximum size.');
            //    $("#errorIcon").show();
            //    $("#errorBtn").show();
            //}
        }
    }
    else {
        $("#GenericModal").modal('show');
        $("#GenericModalTitle").text('Upload Failed! ');
        $("#GenericModalMsg").text('Accepts JPEG/JPG or PNG types only.');
        $("#errorIcon").show();
        $("#errorBtn").show();
    }
}

function uploadShopImageFiles(inputId, uploadURL) {
    var input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();

    for (var i = 0; i !== files.length; i++) {
        formData.append("files", files[i]);
    }

    $("#imageLoading" + inputId).show();

    $.ajax(
        {
            url: uploadURL,
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
                $("#imgPrev" + inputId).attr("src", data.publicUrl);
                $("#imgPrev" + inputId).show();
                $("#imageLoading" + inputId).hide();

                if (inputId.includes("ShopIcon")) {
                    $("#ShopIcon").val(data.publicUrl);
                }
                else if (inputId.includes("ShopBanner")) {
                    $("#ShopBanner").val(data.publicUrl);
                    $("#ImageUrl").val(data.publicUrl);
                }
                else
                {
                    $("#ImageUrl").val(data.publicUrl);
                }
            },
            failed: function () {
                $("#imgPrev" + inputId).hide();
                $("#imageLoading" + inputId).show();
            }
        }
    );
}
