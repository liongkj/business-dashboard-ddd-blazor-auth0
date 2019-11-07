// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

$(function () {
    // Set Ajax request always do not cache response.
    $.ajaxSetup({ cache: false });

    /* Inject the href content to the modal in the main layout.
    Use the class 'modal-link' or 'modal-link-large' (for large dialog) for modal tooglers.*/
    $('body').on('click', '.modal-link, .modal-link-large, .modal-link-static', function (e) {
        e.preventDefault();

        // See if the dialog can be closed by clicking outside.
        var IsStatic = $(this).hasClass('modal-link-static');

        // See if large or normal sized dialog need to be used.
        var useLarge = $(this).hasClass('modal-link-large');

        // See to use data-href or href (legacy), with data-href preferred.
        var href = $(this).data('href') || e.currentTarget.href;

        // To generate a new instance modal place after the modal-container
        var createInstance = $(this).hasClass('modal-instance');

        // Get instance modal id
        var instanceId = $(this).data('modal-instanceid');

        ModalHelper.showModal({
            useLargeDialog: useLarge,
            preventClose: IsStatic,
            url: href,
            createInstance: createInstance,
            instanceId: instanceId
        });
    });

    // Initialize the content of the modal when first load.
    $('#modal-container .modal-content').html(ModalHelper.defaultModalContent);
});

var ModalHelper = {
    "showModal": function (setting) {
        var options = $.extend({
            content: '',
            preventClose: false,
            useLargeDialog: false,
            url: '',
            onSuccess: function () { },
            createInstance: false,
            instanceId: '',
            isInstanceExist: false
        }, setting);

        var modal = $('#modal-container');
        var modalDialog = $('#modal-container .modal-dialog');
        var modalContent = $('#modal-container .modal-content');

        if (options.createInstance) {
            var elementId = '#' + options.instanceId;
            options.isInstanceExist = $(elementId).length > 0;

            if (!options.isInstanceExist) {
                modal.clone().prop('id', options.instanceId).appendTo("body");
            }

            modal = $(elementId);
            modalDialog = $(elementId + ' .modal-dialog');
            modalContent = $(elementId + ' .modal-content');
        }

        if (options.useLargeDialog)
            modalDialog.addClass('modal-lg');
        else
            modalDialog.removeClass('modal-lg');

        var successAction = function () {
            var showLoadingScreen = function () {
                modal.css('pointer-events', 'none'); //Disable the whole form while waiting..

                // Inject loading overlay to the modal.
                //var over = $('<div class="overlay"><i class=\'fa fa-refresh fa-spin loading-img\'></i></div>');
                var over = $('<div class="overlay"><i class=\'fa fa-refresh loading-img\'></i></div>');
                over.appendTo(modalContent);
            };

            var initAjaxCallback = function (result) {
                if (result.redirectUrl) {
                    window.location.href = result.redirectUrl;
                }
                else if (result.refreshPage) {
                    window.location.reload();
                }
                else {
                    modal.css('pointer-events', ''); //Enable the form back
                    modal.children('.overlay').remove(); // Remove loading overlay after done

                    modalContent.html(result);
                    initModalFormValidation();
                    initModalFormRichTextControl();
                    initDatePickerControl();
                    initDropDownControl();
                    initModalLinks();
                    initReadOnlyForm();
                    refetchCalendar();
                }
            };

            var ajaxErrorCallback = function (error, errorText) {
                modal.css('pointer-events', ''); //Enable the form back
                modal.children('.overlay').remove(); // Remove loading overlay after done

                modalContent.html(ModalHelper.defaultModalErrorContent.replace('{}', error + " " + errorText));
            };

            var initModalFormValidation = function () {
                if (typeof ValidateHelper !== 'undefined') {
                    // Parse new elements injected into the modal.
                    ValidateHelper.reparseForm();

                    // Check if any new form needs ajax form submission.
                    ValidateHelper.initAjaxForm(showLoadingScreen, initAjaxCallback, ajaxErrorCallback);
                }

                if (typeof InputHelper !== 'undefined') {
                    InputHelper.asterickOnRequiredFields('#modal-container');
                    //InputHelper.checkDirtyFields();
                }
            };

            var initModalFormRichTextControl = function () {
                if (typeof RichTextHelper !== 'undefined') {
                    // Initialize any text area that needs to be equipped with rich text editor.
                    RichTextHelper.initEditorIn('#modal-container');
                }
            };

            var initDatePickerControl = function () {
                if (typeof DateHelper !== 'undefined') {
                    // Initialize any date picker inside the modal.
                    DateHelper.initializeDatePicker('#modal-container');
                }
            };

            var initDropDownControl = function () {
                if (typeof DropDownHelper !== 'undefined') {
                    // Initialize any drop down inside the modal.
                    DropDownHelper.initializeDropDownList('#modal-container');
                }
            };

            var initReadOnlyForm = function () {
                if (typeof InputHelper !== 'undefined') {
                    // Initialize any date picker inside the modal.
                    //InputHelper.setFormReadOnly('#modal-container');
                }
            };

            var initModalLinks = function () {
                // Link with _self target inside the modal
                $('#modal-container a[target="_self"]').click(function (e) {
                    e.preventDefault();
                    showLoadingScreen();
                    $.ajax({
                        url: e.currentTarget.href,
                        method: 'GET',
                        cache: false,
                        success: function (result) {
                            initAjaxCallback(result);
                        },
                        error: function (xhr, status, error) {
                            ajaxErrorCallback(xhr.status, xhr.statusText);
                        }
                    });

                    return false;
                });
            };

            var refetchCalendar = function () {
                var calendar = $('#calendar');
                if (calendar.length > 0) {
                    calendar.fullCalendar('refetchEvents');
                }
            };

            initModalFormValidation();
            initModalFormRichTextControl();
            initDatePickerControl();
            initDropDownControl();
            initModalLinks();
            initReadOnlyForm();
        };

        // Remove previous event listener first before register a new one.
        modal.off('show.bs.modal')
            .on('show.bs.modal', function () {
                if (!options.isInstanceExist) {
                    if (options.content !== '') {
                        modalContent.html(options.content);
                        successAction();
                    }
                    else {
                        modalContent.load(options.url, function (response, status, xhr) {
                            if (status === 'error')
                                modalContent.html(ModalHelper.defaultModalErrorContent.replace('{}', xhr.status + " " + xhr.statusText));

                            successAction();
                        });
                    }
                }
            })
            .off('hidden.bs.modal')
            .on('hidden.bs.modal', function () {
                if (!options.createInstance) {
                    if (typeof RichTextHelper !== 'undefined') {
                        // Destroy any rich text editor in the modal.
                        RichTextHelper.removeEditorIn('#modal-container');
                    }

                    // Reset the content of the modal when it is hidden.
                    $('#modal-container .modal-content').html(ModalHelper.defaultModalContent);
                }
            })
            .modal({
                backdrop: options.preventClose ? "static" : true,
                keyboard: !options.preventClose
            });
    },
    "closeModal": function (elementId) {
        var modal = $('#' + elementId);
        modal.modal('hide');
    },
    "deleteModal": function (elementId) {
        var modal = $('#' + elementId);

        if (modal.data('bs.modal').isShown) {
            modal.on('hidden.bs.modal', function () {
                if (typeof RichTextHelper !== 'undefined') {
                    // Destroy any rich text editor in the modal.
                    RichTextHelper.removeEditorIn('#' + elementId);
                }

                modal.off('hidden.bs.modal');
                modal.remove();
            });
        }
        else {
            modal.remove();
        }
    },

    "defaultModalContent": '<div class=\"modal-header\"><h5 class=\"modal-title\">Loading content...</h5></div><div class=\"modal-body text-center\"><i class=\'fa fa-refresh fa-spin\'"></i></div>',

    "defaultModalErrorContent": '<div class=\"modal-header\"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h5 class=\"modal-title\">Error</h5>' +
        '</div><div class=\"modal-body\">' +
        '<h3 style="color:#ED2E21"> <i class="fa fa-close"></i> Oops! </h3> <h4> <small>An error ({}) occurred while processing your request.</h4><h4>Please try again after a moment. Contact your web administrator if this problem persists.</small> </h4>' +
        '</div>'
};