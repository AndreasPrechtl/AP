// (c) Andreas Prechtl 2010
/// <reference path="AP.jQuery.Tools.js"/>
// ? really - shouldn't be required here ?
/// <reference path="AP.jQuery.ScrollPanel.js"/>
(function ($)
{
    function ImageInfo(title, imageUrl, thumbNailUrl, description)
    {
        /// <summary>
        /// ImageInfo class
        /// </summary>
        this.title = title;
        this.imageUrl = imageUrl;
        this.thumbnailUrl = thumbNailUrl;
        this.description = description;
    };

    $.fn.gallery = function (imageInfos, resizeThumbnails, thumbnailsContainer, imageContainer, descriptionContainer)
    {
        /// <summary>
        /// creates a gallery
        /// </summary>
        ///	<param name="imageInfos" type="ImageInfo[]">
        ///		An array containing all imageInfos
        ///	</param>
        /// <param name="resizeThumbnails" type="Boolean">
        ///     A boolean that tells the gallery to resize the images according to the height of the thumbnails container
        /// </param>
        /// <param name="thumbnailsContainer" type="Object">
        ///     the thumbnailsContainer - any dom element or jQuery element
        /// </param>
        /// <param name="imageContainer">
        ///     the imageContainer - any dom element or jQuery element
        /// </param>
        /// <param name="descriptionContainer">
        ///     the descriptionContainer - any dom element or jQuery element
        /// </param>
        var _container = this;

        // create all required objects
        var _imageContainer = $(imageContainer || this.find(".Gallery_ImageContainer")[0] || $("<div class='Gallery_ImageContainer'></div>").appendTo(this));
        var _descriptionContainer = $(descriptionContainer || this.find(".Gallery_DescriptionContainer")[0] || $("<div class='Gallery_DescriptionContainer'></div>").appendTo(this));
        var _thumbnailsContainer = $(thumbnailsContainer || (this.find(".Gallery_ThumbnailsContainer")[0] || $("<div class='Gallery_ThumbnailsContainer'></div>").appendTo(this)).scrollPanel(100, 100));
        var _image = $(_imageContainer.find(".Gallery_Image")[0] || $("<img class='Gallery_Image' alt='' src='' />").appendTo(_imageContainer));

//        _image.css("margin-top", "auto");
//        _image.css("margin-right", "auto");
//        _image.css("margin-bottom", "auto");
//        _image.css("margin-left", "auto");
        _image.css("position", "absolute");

        // todo - use a proper overlay
        _image.click(function ()
        {
            var url = _image.attr("src");
            window.open(url, _image.attr("title"), "height = 0, width = 0, menubar = 0, toolbar = 0, scrollbars = 1");
        });

        var width = 0;
        var height = _thumbnailsContainer.height();

        var _showImage = function (imageInfo)
        {
            _image.fadeOut("fast", function ()
            {
                _descriptionContainer.text(imageInfo.description);
                _image.width("auto");
                _image.height("auto");
                _image.attr("src", imageInfo.imageUrl);
                _image.attr("title", imageInfo.title);
                _image.scaleSize(_imageContainer.width(), _imageContainer.height());
                _image.fadeIn("slow");
            });
        }

        for (var i = 0; i < imageInfos.length; i++)
        {
            var thumbnail = $("<img class='Gallery_Thumbnail' />");

            var imageInfo = imageInfos[i];

            thumbnail.data("Gallery_ImageInfo", imageInfo);
            thumbnail.attr("src", imageInfo.thumbnailUrl);
            thumbnail.attr("alt", imageInfo.title);
            thumbnail.attr("description", imageInfo.description);
            thumbnail.css("width", "auto");

            thumbnail.click(function ()
            {
                _showImage($(this).data("Gallery_ImageInfo"));
            });

            _thumbnailsContainer.append(thumbnail);

            if (resizeThumbnails)
                thumbnail.resizeTo("auto", _thumbnailsContainer.height());
        }
        _showImage(imageInfos[0]);

        return this;
    };
})($);