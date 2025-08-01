//Custom JS functions for Shop Admins

var BASEPATH = "";

//GENERIC MODAL
$(document).ready(function () {
    OrderNotification();

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

function formatDate(d) {
    let dateToFormat = new Date(d);
    let formatted_date = appendLeadingZeroes(dateToFormat.getDate()) + "/" + appendLeadingZeroes((dateToFormat.getMonth()) + 1) + "/" + dateToFormat.getFullYear();
    return formatted_date;
}

function appendLeadingZeroes(n) {
    if (n <= 9) {
        return "0" + n;
    }
    return n
}

function isIntegerOnly(evt) {
    if (evt.which != 8 && evt.which != 0 && (evt.which < 48 || evt.which > 57) && (evt.which < 96 || evt.which > 105)) {
        return false;
    }
}

function formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

function validateProductImageFiles(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;

    //validate extension type
    var ext = files[0].type;
    if (ext.split('/')[0] === 'image') {

        var uploadURL = "/Home/";
        if (inputId.includes("Icon") || inputId.includes("CategoryImage")) {
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

                        uploadProductImageFiles(inputId, uploadURL);

                        //if (width == 188 && height == 355) {
                        //    //upload image
                        //    uploadShopImageFiles(inputId, uploadURL);
                        //}
                        //else {
                        //    $("#GenericModal").modal('show');
                        //    $("#GenericModalTitle").text('Upload Failed! ');
                        //    $("#GenericModalMsg").text('Required dimension: 355px x 188px');
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
            //    $("#savingGenericLoader").hide();
            //    $("#errorIcon").show();
            //    $("#errorBtn").show();
            //}
        }
    }
    else {
        $("#GenericModal").modal('show');
        $("#GenericModalTitle").text('Upload Failed! ');
        $("#GenericModalMsg").text('Accepts JPEG/JPG or PNG types only.');
        $("#savingGenericLoader").hide();
        $("#errorIcon").show();
        $("#errorBtn").show();
    }
}

function uploadProductImageFiles(inputId, uploadURL) {
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

                if (inputId.includes("ProductCategoryIcon")) {
                    $("#ProductCategoryIcon").val(data.publicUrl);
                }
                else if (inputId.includes("ProductIcon")) {
                    $("#ProductIcon").val(data.publicUrl);
                }
                else if (inputId.includes("AdminIcon")) {
                    $("#ImageUrl").val(data.publicUrl);
                }
                else if (inputId.includes("CategoryImage")) {
                    $("#ProductCategoryImage").val(data.publicUrl);
                }
                else if (inputId.includes("ShopIcon")) {
                    $("#ShopIcon").val(data.publicUrl);
                }
            },
            failed: function () {
                $("#imgPrev" + inputId).hide();
                $("#imageLoading" + inputId).show();
            }
        }
    );
}

function validateDocument(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;

    //validate extension type
    var ext = files[0].type;
    if (ext.split('/')[1] === 'pdf') {

        var uploadURL = "/Home/";
        if (inputId.includes("Document")) {
            uploadURL = BASEPATH + "/Home/UploadDocument";

            //validate size up to 2MB only
            if (files[0].size < 2000000) {

                //get file props
                var convertedSize = formatBytes(files[0].size);

                $("#FileName").val(files[0].name);
                $("#FileType").val(ext.split('/')[1]);
                $("#FileSize").val(convertedSize);

                uploadDocument(inputId, uploadURL);
            }
            else {
                $("#GenericModal").modal('show');
                $("#GenericModalTitle").text('Upload Failed! ');
                $("#GenericModalMsg").text('Exceeded the 2MB maximum size.');
                $("#savingGenericLoader").hide();
                $("#errorIcon").show();
                $("#errorBtn").show();
            }
        }
    }
    else {
        $("#GenericModal").modal('show');
        $("#GenericModalTitle").text('Upload Failed! ');
        $("#GenericModalMsg").text('Accepts PDF types only.');
        $("#savingGenericLoader").hide();
        $("#errorIcon").show();
        $("#errorBtn").show();
    }
}

function uploadDocument(inputId, uploadURL) {
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
                $("#imageLoading" + inputId).hide();

                if (inputId.includes("UploadDocument")) {
                    $("#FilePath").val(data.publicUrl);
                }
            },
            failed: function () {
                $("#imgPrev" + inputId).hide();
                $("#imageLoading" + inputId).show();
            }
        }
    );
}

function OrderNotification() {
    //Admin Notification (list) for new orders
    $.ajax({
        url: "/Shop/GetOrdersNotification",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $('#divOrdersNotification').append('<div id="oNotifContainer"></div>');

            $.each(result.data, function (i, itm) {
                var createdDT = new Date(itm.createdDate);
                var currentDT = new Date();
                var diff = currentDT.getTime() - createdDT.getTime();

                var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                diff -= days * (1000 * 60 * 60 * 24);

                var hours = Math.floor(diff / (1000 * 60 * 60));
                diff -= hours * (1000 * 60 * 60);

                var mins = Math.floor(diff / (1000 * 60));
                diff -= mins * (1000 * 60);

                var timeVal = mins;
                if (days >= 1) {
                    timeVal = days + " il y a un ou plusieurs jours";
                }
                else if (hours >= 1) {
                    timeVal = hours + " il y a une ou plusieurs heures";
                }
                else {
                    timeVal = mins + " min(s) il y a";
                }

                var aItem = $('<a class="dropdown-item" href="/Order/OrderDetails?rid=' + itm.orderID + '"></a>');

                aItem.append("Commande passée par <span class='font-weight-bold'>" + itm.fullName + "</span><br/>" +
                    "<small>" + timeVal + "</small>")

                $('#oNotifContainer').append(aItem);
                $('#oNotifContainer').append('<div class="dropdown-divider"></div>');
            });

            //set 30second timer
            setTimeout(function () {
                $("#oNotifContainer").remove();
                OrderNotification();
            }, 30000);
        },
        failed: function () {
            console.log("error in OrderNotification")
        }
    });
}