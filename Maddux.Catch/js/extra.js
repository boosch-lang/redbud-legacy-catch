/**
 * Extra js
 */
$(document).ready(function () {
    //The tab the user is currently on
    var currentTab = $("#cphBody_txtActiveTab").val();

    //Handles tab changes when adding or removing
    if (currentTab == "tab-item-details") {
        $("#tab-item-details").addClass("active");
        $("#details").addClass("active in");
    } else if (currentTab == "tab-item-catalogs") {
        $("#tab-item-catalogs").addClass("active");
        $("#catalogs").addClass("active in");
    } else if (currentTab == "tab-item-provinces") {
        $("#tab-item-provinces").addClass("active");
        $("#provinces").addClass("active in");
    } else if (currentTab == "tab-item-catalogues") {
        $("#tab-item-catalogues").addClass("active");
        $("#catalogues").addClass("active in");
    } else if (currentTab == "tab-item-associations") {
        $("#tab-item-associations").addClass("active");
        $("#associations").addClass("active in");
    }
});


 