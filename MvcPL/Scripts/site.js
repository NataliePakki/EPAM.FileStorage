﻿$(function () {
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
$(function () {
    $("#edit-email").on("click", function (e) {
        e.preventDefault();
        var formData = new FormData($("#form-edit-email").get(0));
        $.ajax({
            type: "POST",
            url: "/User/EditEmail",
            data: formData,
            dataType: "json",
            cache: false,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data) {
                    $("#success-edit-email").css("display", "inline-block");
                    $(".profile-usertitle-name").html($("#Email").val());
                    setTimeout(function() {
                        $("#success-edit-email").css("display", "none");
                    }, 3000);
                }
            }
        });
      
    });
});
//EditPassword
$(function () {
    $("#edit-password").on("click", function (e) {
        e.preventDefault();
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
                    setTimeout(function() {
                        $("#success-edit-password").css("display", "none");
                    }, 3000);
                }
            }
        });

    });
});

//----Files-------
// Search
$(function () {
    $("#searchFiles").keyup(function () {
        var search = $("#searchFiles").val();
        var userId = $("#userId").val();
        $.ajax({
            type: "POST",
            url: "/File/Index",
            data: {
                "search": search,
                "userId" : userId
            },
            cache: false,
            success: function (response) {
                $("#table").html(response);
            }
        });
        return false;
    });
});

//CreateFile modal dialog
$(function () {
    $("#lnkCreate").on("click", function (e) {
        e.preventDefault();
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
                            var formData = new FormData($("#form-create").get(0));
                            $.ajax({
                                type: "POST",
                                url: "/File/Create",
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
            .load(this.href);
    });
    $(".close").on("click", function (e) {
        e.preventDefault();
        $(this).closest(".dialog").dialog("close");
    });
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



// ShareLink model dialog
$(function() {
    $(".btn-modal-share").click(function () {
        $.ajaxSetup({ cache: false });
        event.preventDefault();
        debugger;
        var fileId = $(this).data('id');
        $.get(this.href, function (data) {
            $("#dialogContent").html(data);
            $("#modDialog").modal('show');
        });

    });
});

//-----Admin-----
//BlockUser
$(function() {
    $("#blockButton").click(function (event) {
        event.preventDefault();
        debugger;
        var userId = $("#Id").val();
        if ($("#IsBlocked").val() === 'True') {
            $.ajax({
                type: "Post",
                url: '/Admin/BlockUser',
                data: ({ 'isBlocked': false, 'userId': userId }),
                success: function(result) {
                    if (result === 'True') {
                        $('#blockSign').removeClass("glyphicon-ban-circle").addClass("glyphicon-ok-circle");
                        $('#blockButton').html('Block user');
                        $("#IsBlocked").val("False");
                    }
                }
            });
        } else {
            $.ajax({
                type: "Post",
                url: '/Admin/BlockUser',
                data: ({ 'isBlocked': true, 'userId': userId }),
                success: function(result) {
                    if (result === 'True') {
                        $('#blockSign').removeClass("glyphicon-ok-circle").addClass("glyphicon glyphicon-ban-circle");
                        $('#blockButton').html('Unblock user');
                        $("#IsBlocked").val("True");
                    }
                }
            });
        }
    });
});


