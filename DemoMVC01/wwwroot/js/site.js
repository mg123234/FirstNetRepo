// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery.validator.addMethod("name", function (value, element, param) {

    if (value == "aaaa") {
        return false;
    }
    else {
        return true;
    }
});

jQuery.validator.unobtrusive.adapters.addBool("name");