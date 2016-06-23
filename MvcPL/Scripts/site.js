$(function () {
    $("#menu li").on("click", function () {
        $("#menu li").removeClass("active");
        $(this).addClass("active");
    });
});
// Pagination
function changePage(userId, page) {
        $.ajax({
            type: "POST",
            url: "File/Index",
            data: {
                "userId": userId,
                "page": page
            },
            cache: false,
            success: function(response) {
                $("#table").html(response);

            }
        });
}

//----UserEdit----
//EditEmail
function editEmail() {
    var form = new FormData($("#form-edit-email").get(0));
    $.ajax({
        type: "POST",
        url: "/User/EditEmail",
        data: form,
        dataType: "json",
        cache: false,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data) {
                $("#info-edit-email").addClass("alert-success").css("display", "inline-block");
                setTimeout(function() {
                    $("#info-edit-email").removeClass("alert-success").css("display", "none");
                    location.reload();
                }, 3000);
            } 
        }
    });
}
//EditName
function editName() {
    var form = new FormData($("#form-edit-name").get(0));
    $.ajax({
        type: "POST",
        url: "/User/EditName",
        data: form,
        dataType: "json",
        cache: false,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data) {
                $("#info-edit-name").addClass("alert-success").css("display", "inline-block");
                $(".profile-usertitle-name").html($("#Name").val());
                setTimeout(function () {
                    $("#info-edit-name").hide();
                }, 3000);
            }
        }
    });
}
//EditPassword
function editPassword() {
    var formData = new FormData($("#form-edit-password").get(0));
    $.ajax({
        type: "POST",
        url: "/User/EditPassword",
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data) {
                $("#success-edit-password").css("display", "inline-block");
                $("#ConfirmPassword").val("");
                $("#Password").val("");
                $("#OldPassword").val("");
                setTimeout(function () {
                    $("#success-edit-password").hide();
                }, 3000);
            }
        }
    });
}

//----Files-------
// Create File Model Window
function createFileModalWindow() {
    var linkCreate = $("#lnkCreate").attr("href");
    $("<div id='dialogContent'></div>")
           .addClass("dialog")
           .appendTo("body")
           .dialog({
               title: "Create",
               width: 800,
               height: 400,
               close: function () { $(this).remove() },
               modal: true,
               buttons: [
                   {
                       text: "Create",
                       click: function () {
                           $("#loadingmessage").show();
                           $("#info-create-file").hide();

                           var formData = new FormData($("#form-create").get(0));
                           $.ajax({
                               type: "POST",
                               url: "/File/Create",
                               data: formData,
                               cache: false,
                               processData: false,
                               contentType: false,
                               success: function (response) {
                                   var alertClass = "";
                                   $("#info-create-file").removeClass("alert-info");
                                   if (response.toString() === "true") {
                                       alertClass = "alert-danger";
                                       $("#info-create-file").addClass(alertClass).html("<strong>Error</strong> Sorry, this file is too large.").show();
                                       
                                   } else {
                                       alertClass = "alert-success";
                                       $("#info-create-file").addClass(alertClass).html("<strong>Success!</strong> File added.").show();
                                       $("#table").html(response);
                                   }
                                   $("#loadingmessage").hide();
                                   setTimeout(function () {
                                       $("#info-create-file").removeClass(alertClass).addClass("alert-info").html("<strong> Info! </strong>File's size < 100 MB");
                                   }, 5000);
                                 }
                           });
                          
                       }
                   }
               ]
           })
           .load(linkCreate);
}
$(".close").on("click", function (e) {
    e.preventDefault();
    $(this).closest(".dialog").dialog("close");
});
//EditFile modal dialog
function editFileModalWindow(id) {
    $("<div id='dialogContent'></div>")
        .addClass("dialog")
        .appendTo("body")
        .dialog({
            title: "Edit",
            width: 800,
            height: 400,
            close: function () { $(this).remove() },
            modal: true,
            buttons: [
                {
                    text: "Save",
                    click: function () {
                        var formData = new FormData($("#form-edit").get(0));
                        $.ajax({
                            type: "POST",
                            url: "/File/Edit",
                            data: formData,
                            cache: false,
                            processData: false,
                            contentType: false,
                            success: function (response) {
                                $("#table").html(response);
                            }
                        });
                        $(this).dialog("close");
                    }
                }
            ]
        })
        .load("/File/Edit/" + id) ;
}

//Delete File
function deleteFile(id, userId) {
    if(confirm("Are you sure?")) {
        $.ajax({
            type: "POST",
            url: "/File/Delete",
            data: {
                "id": id,
                "userId": userId
            },
            cache: false,
            success: function(response) {
                    $("#table").html(response);
            }
        });
    }
}

// ShareLink model dialog
function shareFileModalWindow() {
    $.ajaxSetup({ cache: false });
    var link = $("#lnkShare").attr("href");
    $.get(link, function(data) {
        $("#dialogContent").html(data);
        $("#modDialog").modal("show");
    });
}
// Block User
function block() {
    var userId = $("#Id").val();
    if ($("#IsBlocked").val() === "True") {
        $.ajax({
            type: "Post",
            url: "/Admin/BlockUser",
            data: ({ 'isBlocked': false, 'userId': userId }),
            success: function (result) {
                if (result === "True") {
                    $("#blockSign").removeClass("glyphicon-ban-circle").addClass("glyphicon-ok-circle");
                    $("#blockButton").html("Block user");
                    $("#IsBlocked").val("False");
                }
            }
        });
    } else {
        $.ajax({
            type: "Post",
            url: "/Admin/BlockUser",
            data: ({ 'isBlocked': true, 'userId': userId }),
            success: function (result) {
                if (result === "True") {
                    $("#blockSign").removeClass("glyphicon-ok-circle").addClass("glyphicon glyphicon-ban-circle");
                    $("#blockButton").html("Unblock user");
                    $("#IsBlocked").val("True");
                }
            }
        });

    }
}




