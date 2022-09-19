// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })

    //PlaceHolderElement.on('click', 'button[data-dismiss="modal"]', function (event) {
    //    var form = $(this).parents('.modal').find('form');
    //    var actionUrl = form.attr('action');
    //    var url = actionUrl;
    //    var sendData = form.serialize();
    //    $.post(url, sendData).done(function (data) {
    //        PlaceHolderElement.find('.modal').modal('hide');
    //    })
    //})

    PlaceHolderElement.on('click', 'button[data-dismiss="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var url = form.attr('action');
        $.get(url).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
        })
    })

    //for preson (admin view)
    var PlaceHolderElement1 = $('#modelList');
    $('a[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        PlaceHolderElement1.load(url);
    })
})

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

jQueryAjaxPost = form => {
    $.validator.unobtrusive.parse('form');
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
