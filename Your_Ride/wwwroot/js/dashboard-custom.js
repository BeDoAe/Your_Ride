$(document).ready(function () {
    $(".dashboard-nav-dropdown-toggle").click(function () {
        $(this).closest(".dashboard-nav-dropdown")
            .toggleClass("show")
            .find(".dashboard-nav-dropdown")
            .removeClass("show");
        $(this).parent()
            .siblings()
            .removeClass("show");
    });

    $("#sidebarToggle").change(function () {
        if (this.checked) {
            $(".dashboard-nav").removeClass("hide-sidebar");
            $(".dashboard-nav a").fadeIn(); // Show all links when sidebar appears
            $(".main-content").removeClass("expanded");
        } else {
            $(".dashboard-nav").addClass("hide-sidebar");
            $(".dashboard-nav a").fadeOut(); // Hide all links when sidebar is hidden
            $(".main-content").addClass("expanded");
        }
    });
});
