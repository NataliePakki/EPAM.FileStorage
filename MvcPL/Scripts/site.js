$(document).ready(function () {
    $("#menu li").on("click", function () {
        $("#menu li").removeClass("active");
        $(this).addClass("active");
    });
});

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
//        var urlInput = $("#myModal #SharedUrl");
//        urlInput.val("http://localhost:56448/File/GetShared/" + fileId);
//            urlInput.show();
    });
});
$(document).ready(function () {
    $("#lnkCreate").on("click", function (e) {
        e.preventDefault();
        $("<div id='dialogContent'></div>")
            .addClass("dialog")
            .appendTo("body")
            .dialog({
                title: "Create",
                width: 500,
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

$(function() {
    $('#blockButton').click(function (event) {
        event.preventDefault();
        debugger;
        var userId = $('#Id').val();
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

$(function () {
    $(".delete").click(function () {
        var id = $(".delete").id();
        $.ajax({
            type: "POST",
            url: "/File/Delete",
            data: { "id": id },
            cache: false,
            success: function (response) {
                $("#table").html(response);
            }
        });
        return false;
    });
});
$(function() {
    $("#searchUserFiles").keyup(function() {
        var search = $("#searchUserFiles").val();
        $.ajax({
            type: "POST",
            url: "/File/UserFiles",
            data: { "search": search },
            cache: false,
            success: function(response) {
                $("#userTable").html(response);
            }
        });
        return false;
    });
});
