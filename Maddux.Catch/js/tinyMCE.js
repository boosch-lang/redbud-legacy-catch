tinymce.init({
    branding: false,
    content_css: '/css/bootstrap.min.css',
    convert_urls: false,
    external_plugins: {
        'moxiemanager': '/js/moxiemanager/plugin.min.js'
    },
    height: 500,
    image_class_list: [
        { title: 'None', value: 'img-fluid' },
        { title: 'Thumbnail', value: 'img-fluid img-thumbnail' },
        { title: 'Rounded', value: 'img-fluid rounded' },
        { title: 'Circle', value: 'img-fluid rounded-circle' }
    ],
    image_dimensions: false,
    removed_menuitems: 'newdocument',
    moxiemanager_insert_filter: function (file) {
        file.url = file.path;
        file.meta.url = file.url;
    },
    moxiemanager_image_template: '<img src="{$url}" class="img-fluid" /></a>',
    moxiemanager_rootpath: '/uploads/files',
    moxiemanager_remember_last_path: true,
    moxiemanager_title: 'File Manager',
    moxiemanager_upload_auto_close: true,
    moxiemanager_view: 'thumbs',
    paste_as_text: true,
    plugins: 'preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template code codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount  imagetools contextmenu colorpicker textpattern help',
    relative_urls: false,
    remove_script_host: true,
    selector: '.tinyMCE',
    statusbar: false,
    style_formats_merge: true,
    table_default_attributes: {
        class: 'table'
    },
    table_cell_class_list: [
        { title: 'None', value: '' },
        { title: 'Active', value: 'table-active' },
        { title: 'Primary', value: 'table-primary' },
        { title: 'Secondary', value: 'table-secondary' },
        { title: 'Success', value: 'table-success' },
        { title: 'Danger', value: 'table-danger' },
        { title: 'Warning', value: 'table-warning' },
        { title: 'Info', value: 'table-info' },
        { title: 'Light', value: 'table-light' },
        { title: 'Dark', value: 'table-dark' }
    ],
    table_class_list: [
        { title: 'None', value: 'table' },
        { title: 'Striped rows', value: 'table table-striped' },
        { title: 'Bordered table', value: 'table table-bordered' },
        { title: 'Hoverable rows', value: 'table table-hover' },
        { title: 'Small table', value: 'table table-sm' }
    ],
    table_row_class_list: [
        { title: 'None', value: '' },
        { title: 'Active', value: 'table-active' },
        { title: 'Primary', value: 'table-primary' },
        { title: 'Secondary', value: 'table-secondary' },
        { title: 'Success', value: 'table-success' },
        { title: 'Danger', value: 'table-danger' },
        { title: 'Warning', value: 'table-warning' },
        { title: 'Info', value: 'table-info' },
        { title: 'Light', value: 'table-light' },
        { title: 'Dark', value: 'table-dark' }
    ],
    theme: 'modern',
    toolbar: 'formatselect fontselect fontsizeselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
    font_formats: 'Nunito Sans="Nunito Sans", sans-serif;Arial=arial,helvetica,sans-serif;Times New Roman=TimesNewRoman, Times New Roman, Times, Baskerville, Georgia, serif;Courier New=courier new,courier,monospace;',
    contextmenu: "link image inserttable | cell row column deletetable",
    importcss_append: true,
    importcss_merge_classes: true,
    textcolor_map: [
        '007bff', 'Primary',
        '868e96', 'Secondary',
        '28a745', 'Success',
        'dc3545', 'Danger',
        'ffc107', 'Warning',
        '17a2b8', 'Info',
        'f8f9fa', 'Light',
        '343a40', 'Dark',
        'ffffff', 'White',
        'ce0f69', 'Pink',
        '3d263a', 'Dark Plum',
        'ff9f72', 'Peach',
        '007770', 'Green'
    ]
});