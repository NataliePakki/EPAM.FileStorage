$(document).ready(function() {
      $("#menu li").on("click", function() {
          $("#menu li").removeClass("active");
          $(this).addClass("active");
      });
  });
$(function () {
    $("#search").keyup(function () {
        var search = $("#search").val();
        $.ajax({
            type: "POST",
            url: "/File/Index",
            data: { "search": search },
            cache: false,
            success: function (response) {
                $("#table").html(response);
            }
        });
        return false;
    });
});