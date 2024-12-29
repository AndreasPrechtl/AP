// (c) Andreas Prechtl 2010
/// <reference path="Javascript/jquery-1.4.2-vsdoc.js" />
/// <reference path="Javascript/AP.jQuery.Tools.js" />

(function ($)
{
    $ = function (selector, context)
    {
        var v = jQuery.apply(this, [selector, context]);

        return v.data("AP.jQuery.ScrollPanel") || v;
    };
    jQuery.extend($, jQuery);

    $.fn.scrollPanel = function (horizontalStepsWidth, verticalStepsHeight)
    {
        /// <summary>
        /// creates a ScrollPanel object using a container, the items and the create method that's invoked for each item
        /// </summary>
       
//        var sp = $(this);
//        sp = sp.data("AP.jQuery.ScrollPanel");

//        if (sp)
//            return sp;

        // else init
        var _innerContainer = this._innerContainer = $("<div class='ScrollPanel_InnerContainer' style='overflow:hidden;position:relative;height:inherit;width:inherit;'></div>");

        var _contentContainer = this._contentContainer = $("<div class='ScrollPanel_ContentContainer' style='position:relative;top:0;left:0;overflow:hidden;border:0;margin:0;padding:0;'></div>");
        var _content = this._content = $("<div class='ScrollPanel_Content' style='height:inherit;width:inherit;overflow:visible;top:0;left:0;position:absolute;'></div>");

        var _horizontalStepsWidth = $.isUndefinedOrNull(horizontalStepsWidth) ? 100 : horizontalStepsWidth;
        var _verticalStepsHeight = $.isUndefinedOrNull(verticalStepsHeight) ? 100 : verticalStepsHeight;

        var controls = "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollUpLeftButton'></div></div>" +      // 0
                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollUpButton'></div></div>" +          // 1         
                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollUpRightButton'></div></div>" +     // 2

                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollLeftButton'></div></div>" +        // 3
                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollRightButton'></div></div>" +       // 4

                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollDownLeftButton'></div></div>" +    // 5
                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollDownButton'></div></div>" +        // 6
                           "<div class='ScrollPanel_ButtonContainer'><div class='ScrollPanel_ScrollDownRightButton'></div></div>";    // 7

        var _controls = this._controls = $(controls);
        delete controls;

        _controls.css("position", "absolute").css("width", "auto").css("height", "auto");

        var w = this.width();
        var h = this.height();

        _innerContainer.append(_contentContainer.append(_content));

        _innerContainer.append(_controls);


        var _append = this.append;
        _append(_innerContainer);

        // now override append and remove
        this.append = function (element)
        {
            //            var sp = this.data("scrollPanel");
            //            if (!sp)
            //                return baseAppend.apply(this, [element]);

            _content.append(element);

            // recalculate the width+heights - fixing browser bugs.. AGAIN !!!!!! GRNNRRAANNARRHGHGH!!!!!!
            var w = _content.width() + element.outerWidth(true) - element.innerWidth();

            var h = _content.outerHeight(true);

            var pWidth = _contentContainer.width();
            var pHeight = _contentContainer.height();

            var ipWidth = _content.innerWidth();
            var ipHeight = _content.innerHeight();

            var offset = _contentContainer.offset();

            return this;
        };

        var _remove = this.remove;

        this.remove = function (element)
        {
            //            var sp = this.data("scrollPanel");
            //            if (!sp)
            //                return baseRemove.apply(this, [element]);
            if (!element)
                _remove.apply(this, arguments);

            _content.remove(element);

            var w = _content.width() - element.outerWidth(true) - element.innerWidth();
            _content.width(w);

            var h = _content.height() - element.outerHeight(true) - element.innerHeight();
            _content.height(h);

            //   return this;
        };


        // resize to fix stupid browser inheritance css bugs.... GAY FAGGOTS
        _contentContainer.resizeTo(w, h);
        _content.resizeTo(w, h);

        // position the controls - according to their calculated measures

        // up + left // rofl need to move it otherwise that stfpd firefox thinks it's a relative position.. - when the contentContainer is also relative positioned .... fu?`plz!
        var c0 = $(_controls[0]);
        c0.moveTo(0, 0).click(function () { _moveButtonClicked(-_horizontalStepsWidth, -_verticalStepsHeight); });

        // up
        var c1 = $(_controls[1]);
        c1.moveTo((w - c1.width()) / 2, 0).click(function () { _moveButtonClicked(0, -_verticalStepsHeight); });

        // up + right
        var c2 = $(_controls[2]);
        c2.moveTo(w - c2.width(), 0).click(function () { _moveButtonClicked(_horizontalStepsWidth, -_verticalStepsHeight); });

        // left
        var c3 = $(_controls[3]);
        c3.moveTo(0, (h - c3.height()) / 2).click(function () { _moveButtonClicked(-_horizontalStepsWidth, 0); });

        // right
        var c4 = $(_controls[4]);
        c4.moveTo(w - c4.width(), (h - c4.height()) / 2).click(function () { if (!_dragInfo) _moveButtonClicked(_horizontalStepsWidth, 0); });

        // down + left
        var c5 = $(_controls[5]);
        c5.moveTo(0, h - c5.height()).click(function () { _moveButtonClicked(-_horizontalStepsWidth, _verticalStepsHeight); });

        // down 
        var c6 = $(_controls[6]);
        c6.moveTo((w - c6.width()) / 2, h - c6.height()).click(function () { _moveButtonClicked(0, _verticalStepsHeight); });

        // down + right
        var c7 = $(_controls[7]);
        c7.moveTo(w - c7.width(), h - c7.height()).click(function () { _moveButtonClicked(_horizontalStepsWidth, _verticalStepsHeight); });


        //        var _currentPosition = 0;
        _dragInfo = null;

        _content.mousedown(function (e)
        {
            _dragInfo = { startX: _content.position().left, startY: _content.position().top, x: e.pageX, y: e.pageY, direction: {} };
        });
        _content.mouseup(function (e)
        {
            _dragInfo = null;
        });
        _content.mousemove(function (e)
        {
            // handle the dragging logic here
            if (!_dragInfo)
                return;

            // calculate the new position
            _moveTo(_dragInfo.startX + e.pageX - _dragInfo.x, _dragInfo.startY + e.pageY - _dragInfo.y);
        });

        var _moveButtonClicked = function (x, y)
        {
            if (_dragInfo)
                return;
            _moveBy(x, y, true);
        };

        var _moveBy = function (x, y, animate)
        {
            var p = _content.position();
            _moveTo(p.left + x, p.top + y, animate);
        };

        var _moveTo = function (x, y, animate)
        {
            var _getSaveCoordinate = function (c, containerSize, contentSize)
            {
                var m = containerSize - contentSize;

                if (m == 0)
                    c = 0;
                else if (m > 0) // case 2: move within the container's boundaries
                {
                    if (c < 0)
                        c = 0;
                    else if (c > m)
                        c = m;
                }
                else // case 3 - limiting the maximum to the opposite border - strange huh? fu! great idea u sucker!
                {
                    // does it move left or right / up or down?
                    if (c < m)
                        c = m;
                    else if (c > 0)
                        c = 0;
                }
                return c;
            };

            var newX = _getSaveCoordinate(x, _container.width(), _content.realWidth(true));
            var newY = _getSaveCoordinate(y, _container.height(), _content.realHeight(true));

            if (animate)
                _content.animate({ left: newX, top: newY }, "slow");
            else
                _content.moveTo(newX, newY);
        };

        this.data("AP.jQuery.ScrollPanel", this);

        return this;
    };
})($);