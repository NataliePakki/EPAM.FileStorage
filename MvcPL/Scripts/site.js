$(document).ready(function () {
    $("#menu li").on("click", function () {
        $("#menu li").removeClass("active");
        $(this).addClass("active");
    });
});
$(function () {
    $("#searchAllFiles").keyup(function () {
        var search = $("#searchAllFiles").val();
        $.ajax({
            type: "POST",
            url: "/File/AllPublicFiles",
            data: { "search": search },
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

$(function() {
    $('#blockButton').click(function (event) {
        event.preventDefault();
        debugger;
        var userEmail = $('#Email').val();
        if ($("#IsBlocked").val() === 'True') {
            $.ajax({
                type: "Post",
                url: '/Admin/BlockUser',
                data: ({ 'isBlocked': false, 'userEmail': userEmail }),
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
                data: ({ 'isBlocked': true, 'userEmail': userEmail }),
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
$(function () {
    $("#searchUserFiles").keyup(function () {
        var search = $("#searchUserFiles").val(); 
        $.ajax({
            type: "POST",
            url: "/File/UserFiles",
            data: { "search": search },
            cache: false,
            success: function (response) {
                $("#userTable").html(response);
            }
        });
        return false;
    });
});