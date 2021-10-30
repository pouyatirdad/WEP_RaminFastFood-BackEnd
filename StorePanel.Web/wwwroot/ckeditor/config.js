ClassicEditor
    .create(document.querySelector('.ckEditor'), {

        toolbar: {
            items: [
                'heading',
                '|',
                'alignment',
                'bold',
                'italic',
                'link',
                'bulletedList',
                'numberedList',
                '|',
                //'outdent',
                //'indent',
                //'imageUpload',
                //'blockQuote',
                //'insertTable',
                'mediaEmbed',
                'imageInsert',
                '|',
                //'horizontalLine',
                'undo',
                'redo'
            ]
        },
        ckfinder: {
            uploadUrl: '/Home/CkUploadImage'
        },
        language: 'fa',
        image: {
            toolbar: [
                'imageTextAlternative',
                'imageStyle:full',
                'imageStyle:side'
            ]
        },
        table: {
            contentToolbar: [
                'tableColumn',
                'tableRow',
                'mergeTableCells'
            ]
        },

    })
    .then(editor => {
        window.editor = editor;
    })
    .catch(error => {
        console.error('Oops, something went wrong!');
        console.error(error);
    });