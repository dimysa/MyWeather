﻿using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace MyWeather.iOS.Controls
{
    public class FloatLabeledTextField : UITextField
    {
        private readonly UILabel _errorLabel;
        private readonly UILabel _floatingLabel;
        private readonly CALayer _underline;

        public FloatLabeledTextField()
        {
            _floatingLabel = new UILabel { Alpha = 0.0f };
            _errorLabel = new UILabel { Font = UIFont.SystemFontOfSize(11) };

            _underline = new CALayer
            {
                MasksToBounds = true,
                BorderColor = UIColor.Clear.CGColor,
                BackgroundColor = UIColor.Clear.CGColor,
                BorderWidth = 1f
            };

            AddSubview(_errorLabel);
            AddSubview(_floatingLabel);
            Layer.AddSublayer(_underline);

            BorderStyle = UITextBorderStyle.None;
            ErrorTextColor = UIColor.Red;
            ErrorTextIsVisible = false;
            FloatingLabelTextColor = UIColor.DarkGray;
            FloatingLabelActiveTextColor = UIColor.White;
            FloatingLabelFont = UIFont.BoldSystemFontOfSize(12);            
        }

        public UIColor FloatingLabelTextColor { get; set; }
        public UIColor FloatingLabelActiveTextColor { get; set; }
        public bool FloatingLabelEnabled { get; set; } = true;
        public UIColor ErrorTextColor
        {
            get { return _errorLabel.TextColor; }
            set { _errorLabel.TextColor = value; }
        }

        public bool ErrorTextIsVisible
        {
            get { return !_errorLabel.Hidden; }
            set
            {
                _errorLabel.Hidden = !value;
            }
        }

        public CGColor UnderlineColor
        {
            get { return _underline.BackgroundColor; }
            set { _underline.BackgroundColor = value; }
        }

        public UIFont FloatingLabelFont
        {
            get { return _floatingLabel.Font; }
            set { _floatingLabel.Font = value; }
        }

        public string ErrorText
        {
            get { return _errorLabel.Text; }
            set
            {
                _errorLabel.Text = value;
                _errorLabel.SizeToFit();
                _errorLabel.Frame =
                    new CGRect(
                        0,
                        _errorLabel.Font.LineHeight + 30,
                        _errorLabel.Frame.Size.Width,
                        _errorLabel.Frame.Size.Height);
            }
        }

        public override string Placeholder
        {
            get { return base.Placeholder; }
            set
            {
                base.Placeholder = value;

                _floatingLabel.Text = value;
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame =
                    new CGRect(
                        0,
                        _floatingLabel.Font.LineHeight,
                        _floatingLabel.Frame.Size.Width,
                        _floatingLabel.Frame.Size.Height);
            }
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
            {
                return base.TextRect(forBounds);
            }
            return InsetRect(base.TextRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 22, 0));
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
            {
                return base.EditingRect(forBounds);
            }

            return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 22, 0));
        }

        public override CGRect ClearButtonRect(CGRect forBounds)
        {
            var rect = base.ClearButtonRect(forBounds);

            if (_floatingLabel == null)
            {
                return rect;
            }

            return new CGRect(
                rect.X,
                rect.Y + _floatingLabel.Font.LineHeight / 2.0f,
                rect.Size.Width,
                rect.Size.Height);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _underline.Frame = new CGRect(0f, Frame.Height - 20, Frame.Width, 1f);

            Action updateLabel = () =>
            {
                if (!string.IsNullOrEmpty(Text) && FloatingLabelEnabled)
                {
                    _floatingLabel.Alpha = 1.0f;
                    _floatingLabel.Frame =
                        new CGRect(
                            _floatingLabel.Frame.Location.X,
                            2.0f,
                            _floatingLabel.Frame.Size.Width,
                            _floatingLabel.Frame.Size.Height);
                }
                else
                {
                    _floatingLabel.Alpha = 0.0f;
                    _floatingLabel.Frame =
                        new CGRect(
                            _floatingLabel.Frame.Location.X,
                            _floatingLabel.Font.LineHeight,
                            _floatingLabel.Frame.Size.Width,
                            _floatingLabel.Frame.Size.Height);
                }
            };

            if (IsFirstResponder)
            {
                _floatingLabel.TextColor = UIColor.FromRGB(66, 199, 151); //set binding

                var shouldFloat = !string.IsNullOrEmpty(Text) && FloatingLabelEnabled;
                var isFloating = _floatingLabel.Alpha == 1f;

                if (shouldFloat == isFloating)
                {
                    updateLabel();
                }
                else
                {
                    Animate(
                        0.3f,
                        0.0f,
                        UIViewAnimationOptions.BeginFromCurrentState
                        | UIViewAnimationOptions.CurveEaseOut,
                        () => updateLabel(),
                        () => { });
                }
            }
            else
            {
                _floatingLabel.TextColor = UIColor.FromRGB(66, 199, 151); // set binding

                updateLabel();
            }
        }

        private static CGRect InsetRect(CGRect rect, UIEdgeInsets insets)
        {
            return new CGRect(
                rect.X + insets.Left,
                rect.Y + insets.Top,
                rect.Width - insets.Left - insets.Right,
                rect.Height - insets.Top - insets.Bottom);
        }
    }
}