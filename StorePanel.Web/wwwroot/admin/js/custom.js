'use strict';

(function ($) {

    //$(document).on('click', '.layout-builder .layout-builder-toggle', function () {
    //    $('.layout-builder').toggleClass('show');
    //});

    //$(window).on('load', function () {
    //    setTimeout(function () {
    //        $('.layout-builder').removeClass('show');
    //    }, 500);
    //});

    //$('body').append(''+
    //'<div class="layout-builder show">'+
    //    '<div class="layout-builder-toggle shw">'+
    //        '<i class="ti-settings"></i>'+
    //    '</div>'+
    //    '<div class="layout-builder-toggle hdn">'+
    //        '<i class="ti-close"></i>'+
    //    '</div>'+
    //    '<div class="layout-builder-body">'+
    //        '<h5>سفارشی سازی</h5>'+
    //        '<div class="mb-3">'+
    //            '<p>طرح</p>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="horizontal-side-menu" data-layout="horizontal-side-menu">'+
    //              '<label class="custom-control-label" for="horizontal-side-menu">فهرست افقی</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="icon-side-menu" data-layout="icon-side-menu">'+
    //              '<label class="custom-control-label" for="icon-side-menu">فهرست آیکن</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="dark-side-menu" data-layout="dark-side-menu">'+
    //              '<label class="custom-control-label" for="dark-side-menu">فهرست تیره</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="hidden-side-menu" data-layout="hidden-side-menu">'+
    //              '<label class="custom-control-label" for="hidden-side-menu">فهرست پنهان</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="layout-container-1" data-layout="layout-container icon-side-menu">'+
    //              '<label class="custom-control-label" for="layout-container-1">طرح دربرگیرنده 1</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="layout-container-2" data-layout="layout-container horizontal-side-menu">'+
    //              '<label class="custom-control-label" for="layout-container-2">طرح دربرگیرنده 2</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="layout-container-3" data-layout="layout-container hidden-side-menu">'+
    //              '<label class="custom-control-label" for="layout-container-3">طرح دربرگیرنده 3</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="dark-1" data-layout="dark">'+
    //              '<label class="custom-control-label" for="dark-1">طرح تیره 1</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="dark-2" data-layout="layout-container dark icon-side-menu">'+
    //              '<label class="custom-control-label" for="dark-2">طرح تیره 2</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="dark-3" data-layout="layout-container dark horizontal-side-menu">'+
    //              '<label class="custom-control-label" for="dark-3">طرح تیره 3</label>'+
    //            '</div>'+
    //            '<div class="custom-control custom-radio">'+
    //              '<input type="radio" class="custom-control-input" name="layout" id="dark-4" data-layout="layout-container dark hidden-side-menu">'+
    //              '<label class="custom-control-label" for="dark-4">طرح تیره 4</label>'+
    //            '</div>'+
    //        '</div>'+
    //        '<button id="btn-layout-builder-reset" class="btn btn-danger btn-uppercase">بازنشانی</button>'+
    //        '<div class="layout-alert mt-3">'+
    //            '<i class="fa fa-warning m-l-5 text-warning"></i>برخی گزینه های قالب در صورت ترکیب با یکدیگر در صورتی که همخوانی نداشته باشند قابل نمایش نخواهند بود. بنابراین توصیه می شود گزینه های قالب را جدا جدا امتحان کنید.'+
    //        '</div>'+
    //    '</div>'+
    //'</div>');

    var site_layout = localStorage.getItem('site_layout');
    $('body').addClass(site_layout);
    var themeSelector = $("#theme-selector");
    themeSelector.empty();
    if (site_layout == "dark") {
        themeSelector.append("<i class='fa fa-sun-o'></i>")
    } else {
        themeSelector.append("<i class='fas fa-moon'></i>")
    }
    themeSelector.click(function () {
        if (site_layout == "dark") {
            localStorage.removeItem('site_layout');
        } else {
            localStorage.setItem('site_layout', "dark");
        }
        window.location.href = (window.location.href).replace('#', '');
    });
    $('.layout-builder .layout-builder-body input[type="radio"][data-layout="' + $('body').attr('class') + '"]').prop('checked', true);

    $('.layout-builder .layout-builder-body input[type="radio"]').click(function () {
        var class_names = '';

        $('.layout-builder .layout-builder-body input[type="radio"]:checked').each(function () {
            class_names += ' ' + $(this).data('layout');
        });

        localStorage.setItem('site_layout', class_names);

        window.location.href = (window.location.href).replace('#', '');
    });

    $(document).on('click', '#btn-layout-builder', function () {

    });

    $(document).on('click', '#btn-layout-builder-reset', function () {
        localStorage.removeItem('site_layout');
        localStorage.removeItem('site_layout_dark');

        window.location.href = (window.location.href).replace('#', '');
    });

    $(window).on('load', function () {
        if ($('body').hasClass('horizontal-side-menu') && $(window).width() > 768) {
            if ($('body').hasClass('layout-container')) {
                $('.side-menu .side-menu-body').wrap('<div class="container"></div>');
            } else {
                $('.side-menu .side-menu-body').wrap('<div class="container-fluid"></div>');
            }
            setTimeout(function () {
                $('.side-menu .side-menu-body > ul').append('<li><a href="#"><span>سایر</span></a><ul></ul></li>');
            }, 100);
            $('.side-menu .side-menu-body > ul > li').each(function () {
                var index = $(this).index(),
                    $this = $(this);
                if (index > 7) {
                    setTimeout(function () {
                        $('.side-menu .side-menu-body > ul > li:last-child > ul').append($this.clone());
                        $this.addClass('d-none');
                    }, 100);
                }
            });
        }
    });

    $(document).on('click', '[data-attr="layout-builder-toggle"]', function () {
        $('.layout-builder').toggleClass('show');
        return false;
    });

})(jQuery);

var navActive = $('#nav_active').val();
var navItemActive = $('#nav_item_active').val();
if (navActive != null && navActive != "") {
    $('#nav_' + navActive + '').addClass("active");
    if ($('#subnav_' + navActive + '').length) {
        $('#subnav_' + navActive + '').css("display", "block");
        $('#subnav_' + navActive + '').css("overflow", "");
    }
}
if (navItemActive != null && navItemActive != "") {
    $('#nav_item_' + navItemActive + '').addClass("active");
}
function accessDenied() {
    Swal.fire("Error!", "شما دسترسی لازم برای ورود به این بخش را ندارید", "error");
}

$.extend(true, $.fn.dataTable.defaults, {
    serverSide: true,
    processing: true,
    responsive: true,
    language: {
        "sEmptyTable": "هیچ داده ای در جدول وجود ندارد",
        "sInfo": "نمایش _START_ تا _END_ از _TOTAL_ رکورد",
        "sInfoEmpty": "نمایش 0 تا 0 از 0 رکورد",
        "sInfoFiltered": "(فیلتر شده از _MAX_ رکورد)",
        "sInfoPostFix": "",
        "sInfoThousands": ",",
        "sLengthMenu": "نمایش _MENU_ رکورد",
        "sLoadingRecords": "در حال بارگزاری...",
        "sProcessing": "در حال پردازش...",
        "sSearch": "جستجو:",
        "sZeroRecords": "رکوردی با این مشخصات پیدا نشد",
        "oPaginate": {
            "sFirst": "ابتدا",
            "sLast": "انتها",
            "sNext": "بعدی",
            "sPrevious": "قبلی"
        },
        "oAria": {
            "sSortAscending": ": فعال سازی نمایش به صورت صعودی",
            "sSortDescending": ": فعال سازی نمایش به صورت نزولی"
        }
    },
    "initComplete": function (settings, json) {
        $("[name='datatable_length']").css("margin-left", "0.5rem")
    }
});
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

$(document).ajaxError(function (event, xhr, ajaxOptions, thrownError) {
    if (xhr.status == 403 || xhr.status == 401) {
        toastr.error("شما دسترسی لازم برای ورود به این بخش را ندارید.", "خطا");
    }
    else {
        toastr.error(xhr.responseText.split(':')[0], "Error");
    }
});
function openModal(link) {
    $.get(link, function (result) {
        $("#myModal").modal();
        //if (title != null) {
        //    $("#myModalLabel").html(title);
        //}
        $("#myModalBody").html(result);
        var title = $("#title").val();
        if (title != null && title != undefined) {
            $("#myModalLabel").html(title);
        } else {
            $("#myModalLabel").html("");    
        }
    });
}
var tags = document.getElementById('tags');
if (tags != null) {
    var tagInput = new Tagify(tags,
        {
            whitelist: [],
            originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(',')
        });
    tagInput.on('input', onTagInput);
}

var tagRemove = document.querySelector('.tags--removeAllBtn');
if (tagRemove != null) {
    tagRemove.addEventListener('click', tagInput.removeAllTags.bind(tagInput));
}
function onTagInput(e) {
    var value = e.detail.value;
    tagInput.settings.whitelist.length = 0;
    tagInput.loading(true).dropdown.hide.call(tagInput);
    var newWhitelist = [];
    $.get("/Dashboard/Home/GetTags?searchStr=" + value,
        {
        },
        function (data) {
            newWhitelist = data;
            tagInput.settings.whitelist.push(...newWhitelist, ...tagInput.value);
            tagInput.loading(false).dropdown.show.call(tagInput, value);
        });
};
$(".remove-image").click(function () {
    var input = $(this).data("input");
    var container = $(this).data("container");
    if (input != undefined) {
        $("#" + input).val("");
    }
    if (container != undefined) {
        $("#" + container).hide();
        var image = $("#" + container).find("img");
        image.attr("src", "");
    }
});
Dropzone.options.dropzone = {
    url: '/Dashboard/Home/UploadImage',
    init: function () {
        this.on("complete", function (file) {
            this.removeFile(file);
        })
    },
    dictDefaultMessage: "فایل ها را برای ارسال اینجا بکشید",
    dictFallbackMessage: "مرورگر شما از کشیدن و رها سازی برای ارسال فایل پشتیبانی نمی کند.",
    dictFallbackText: "لطفا از فرم زیر برای ارسال فایل های خود مانند گذشته استفاده کنید.",
    dictFileTooBig: "فایل خیلی بزرگ است ({{filesize}}MiB). حداکثر اندازه مجاز: {{maxFilesize}}MiB.",
    dictInvalidFileType: "شما مجاز به ارسال این نوع فایل نیستید.",
    dictResponseError: "سرور با کد {{statusCode}} پاسخ داد.",
    dictCancelUpload: "لغو ارسال",
    dictUploadCanceled: "ارسال لغو شد.",
    dictCancelUploadConfirmation: "آیا از لغو این ارسال اطمینان دارید؟",
    dictRemoveFile: "حذف فایل",
    dictRemoveFileConfirmation: "آیا از حذف این فایل اطمینان دارید؟",
    dictMaxFilesExceeded: "شما نمی توانید فایل دیگری ارسال کنید.",
    success: function (file, response) {
        var currentDropzone = $("#dropzone");
        var input = currentDropzone.data('input');
        var container = currentDropzone.data('container');
        if (input != undefined) {
            $("#" + input).val(response);
        }
        if (container != undefined) {
            $("#" + container).show();
            var image = $("#" + container).find("img");
            image.attr("src", "/UploadedFiles/Images/" + response);
        }
    },
    transformFile: function (file, done) {

        var myDropZone = this;

        // Create the image editor overlay
        var editor = document.createElement('div');
        editor.style.position = 'fixed';
        editor.style.left = 0;
        editor.style.right = 0;
        editor.style.top = 0;
        editor.style.bottom = 0;
        editor.style.zIndex = 9999;
        editor.style.backgroundColor = '#000';

        // Create the confirm button
        var confirm = document.createElement('button');
        confirm.style.position = 'absolute';
        confirm.style.left = '10px';
        confirm.style.top = '10px';
        confirm.style.zIndex = 9999;
        confirm.textContent = 'برش تصویر';
        confirm.classList.add("btn");
        confirm.classList.add("btn-primary");
        confirm.addEventListener('click', function () {

            // Get the canvas with image data from Cropper.js
            var canvas = cropper.getCroppedCanvas({
                minWidth: 256,
                minHeight: 256,
                maxWidth: 4096,
                maxHeight: 4096,
                fillColor: '#fff',
                imageSmoothingEnabled: true,
                imageSmoothingQuality: 'high',
            });

            // Turn the canvas into a Blob (file object without a name)
            canvas.toBlob(function (blob) {

                // Update the image thumbnail with the new image data
                myDropZone.createThumbnail(
                    blob,
                    myDropZone.options.thumbnailWidth,
                    myDropZone.options.thumbnailHeight,
                    myDropZone.options.thumbnailMethod,
                    false,
                    function (dataURL) {

                        // Update the Dropzone file thumbnail
                        myDropZone.emit('thumbnail', file, dataURL);

                        // Return modified file to dropzone
                        done(blob);
                    }
                );

            });

            // Remove the editor from view
            editor.parentNode.removeChild(editor);

        });
        editor.appendChild(confirm);
        // Load the image
        var image = new Image();
        image.src = URL.createObjectURL(file);
        editor.appendChild(image);

        // Append the editor to the page
        document.body.appendChild(editor);
        var currentDropzone = $("#dropzone");
        var xAxis = parseInt(currentDropzone.data("x"));
        var yAxis = parseInt(currentDropzone.data("y"));
        if (xAxis == undefined || isNaN(xAxis)) {
            xAxis = 1;
        }
        if (yAxis == undefined || isNaN(yAxis)) {
            yAxis = 1;
        }
        // Create Cropper.js and pass image
        var cropper = new Cropper(image, {
            aspectRatio: xAxis / yAxis,
            cropBoxResizable: true,
        });

    }
};


