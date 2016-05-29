
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