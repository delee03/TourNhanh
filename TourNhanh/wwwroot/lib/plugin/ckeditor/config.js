/// <reference path="../ckfinder/ckfinder.js" />
/// <reference path="../ckfinder/ckfinder.js" />
/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.filebrowserBrowseUrl = "/lib/plugin/ckfinder/ckfinder.html";
    config.filebrowserImageUrl = "/lib/plugin/ckfinder/ckfinder.html?type=Images";
    config.filebrowserFlashUrl = "/lib/plugin/ckfinder/ckfinder.html?type=Flash";
    config.filebrowserUploadUrl = "/lib/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files";
    config.filebrowserImageUploadUrl = "/lib/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images";
    config.filebrowserFlashUploadUrl = "/lib/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash";

    config.extraPlugins = 'youtube';
    config.youtube_responsive = true;
};

