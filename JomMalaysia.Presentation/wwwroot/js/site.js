﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function () {

    // hide/show menu
    $('#minimise-menu').click(function () {

        if ($('#custom-menu').hasClass('d-md-block')) {
            $('#custom-menu').removeClass('d-md-block');
            $('#custom-menu').addClass('d-none');
            //$('[role="main"]').removeClass('margin-left-280');
        }
        else {
            $('#custom-menu').addClass('d-md-block');
            $('#custom-menu').removeClass('d-none');
            //$('[role="main"]').addClass('margin-left-280');
        }

    });
    //


    // hide/show dropdown menu
    var dropdown = document.getElementsByClassName("has-sub-menu");
    var i;

    for (i = 0; i < dropdown.length; i++) {
        dropdown[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var dropdownContent = this.nextElementSibling;
            if (dropdownContent.style.display === "block") {
                dropdownContent.style.display = "none";
            } else {
                dropdownContent.style.display = "block";
            }
        });
    }
    //


    // set active menu 
    $(function () {

        var url = window.location.pathname,
            urlRegExp = new RegExp(url.replace(/\/$/, '') + "$"); // create regexp to match current url pathname and remove trailing slash if present as it could collide with the link in navigation in case trailing slash wasn't present there
        // now grab every link from the navigation
        $('a .navlink').each(function () {
            // and test its normalized href against the url pathname regexp
            if (urlRegExp.test(this.href.replace(/\/$/, ''))) {
                $(this).addClass('active');
            }
        });

    });
    //


    $("#action-button").click(function () {

        if ($('.dropdown-menu').is(':visible'))
            $('.dropdown-menu').hide();
        else {
            $('.dropdown-menu').show();
        }
    });

    $(document).click(function (event) {
        $('.dropdown-menu').hide();
    });
});
