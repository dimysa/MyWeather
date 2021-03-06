﻿using System;
using System.ComponentModel;
using System.Drawing;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static Xamarin.Forms.Entry;
using Color = Xamarin.Forms.Color;
using Foundation;
using MyWeather.Portable.Controls;
using MyWeather.iOS.Renderers;
using MyWeather.iOS.Controls;
using MyWeather.iOS.Extensions;
using MyWeather.Portable.Controls.Extensions;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace MyWeather.iOS.Renderers
{
    public class ExtendedEntryRenderer: ViewRenderer<ExtendedEntry, FloatLabeledTextField>
    {
        private readonly CGColor _defaultLineColor = Color.FromHex("#4042c797").ToCGColor();
        private readonly CGColor _editingUnderlineColor = Color.FromHex("#42c797").ToCGColor();
        private UIColor _defaultPlaceholderColor;
        private UIColor _defaultTextColor;
        private bool _hasError;
        private bool _hasFocus;

        private IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedEntry> e)
        {
            base.OnElementChanged(e);

            // unsubscribe
            if (e.OldElement != null)
            {
                Control.EditingDidBegin -= OnEditingDidBegin;
                Control.EditingDidEnd -= OnEditingDidEnd;
                Control.EditingChanged -= ViewOnEditingChanged;
            }

            if (e.NewElement != null)
            {
                var ctrl = CreateNativeControl();
                SetNativeControl(ctrl);

                if (!string.IsNullOrWhiteSpace(Element.AutomationId))
                    SetAutomationId(Element.AutomationId);

                _defaultTextColor = Control.FloatingLabelTextColor;
                _defaultPlaceholderColor = Control.FloatingLabelTextColor;

                SetIsPassword();
                SetText();
                SetHintText();
                SetTextColor();
                SetBackgroundColor();
                SetPlaceholderColor();
                SetKeyboard();
                SetHorizontalTextAlignment();
                SetErrorText();
                SetFont();
                SetFloatingHintEnabled();

                Control.ErrorTextIsVisible = true;
                Control.EditingDidBegin += OnEditingDidBegin;
                Control.EditingDidEnd += OnEditingDidEnd;
                Control.EditingChanged += ViewOnEditingChanged;
            }
        }

        protected virtual FloatLabeledTextField CreateNativeControl()
        {
            return new FloatLabeledTextField();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == PlaceholderProperty.PropertyName)
                SetHintText();
            else if (e.PropertyName == ExtendedEntry.ErrorTextProperty.PropertyName)
                SetErrorText();
            else if (e.PropertyName == TextColorProperty.PropertyName)
                SetTextColor();
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                SetBackgroundColor();
            else if (e.PropertyName == IsPasswordProperty.PropertyName)
                SetIsPassword();
            else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                SetIsPassword();
                SetTextColor();
            }
            else if (e.PropertyName == TextProperty.PropertyName)
                SetText();
            else if (e.PropertyName == PlaceholderColorProperty.PropertyName)
                SetPlaceholderColor();
            else if (e.PropertyName == Xamarin.Forms.InputView.KeyboardProperty.PropertyName)
                SetKeyboard();
            else if (e.PropertyName == HorizontalTextAlignmentProperty.PropertyName)
                SetHorizontalTextAlignment();
            else if (e.PropertyName == ExtendedEntry.FloatingHintEnabledProperty.PropertyName)
                SetFloatingHintEnabled();
            else if ((e.PropertyName == FontAttributesProperty.PropertyName) ||
                     (e.PropertyName == FontFamilyProperty.PropertyName) ||
                     (e.PropertyName == FontSizeProperty.PropertyName))
                SetFont();
        }

        private void OnEditingDidEnd(object sender, EventArgs eventArgs)
        {
            var isFocusedPropertyKey = Element.GetInternalField<BindablePropertyKey>("IsFocusedPropertyKey");
            ElementController.SetValueFromRenderer(isFocusedPropertyKey, false);
            _hasFocus = false;
            Control.UnderlineColor = GetUnderlineColorForState();
        }

        private void OnEditingDidBegin(object sender, EventArgs eventArgs)
        {
            var isFocusedPropertyKey = Element.GetInternalField<BindablePropertyKey>("IsFocusedPropertyKey");
            ElementController.SetValueFromRenderer(isFocusedPropertyKey, true);
            _hasFocus = true;
            Control.UnderlineColor = GetUnderlineColorForState();
        }

        private void ViewOnEditingChanged(object sender, EventArgs eventArgs)
        {
            ElementController?.SetValueFromRenderer(TextProperty, Control.Text);
        }

        private void SetFloatingHintEnabled()
        {
            Control.FloatingLabelEnabled = Element.FloatingHintEnabled;
        }

        private void SetErrorText()
        {
            _hasError = !string.IsNullOrEmpty(Element.ErrorText);
            Control.UnderlineColor = GetUnderlineColorForState();
            Control.ErrorTextIsVisible = _hasError;
            Control.ErrorText = Element.ErrorText;
        }

        private CGColor GetUnderlineColorForState()
        {
            if (_hasError) return UIColor.Red.CGColor;
            return _hasFocus ? _editingUnderlineColor : _defaultLineColor;
        }

        private void SetBackgroundColor()
        {
            NativeView.BackgroundColor = Element.BackgroundColor.ToUIColor();
            Control.UnderlineColor = _defaultLineColor;
        }

        private void SetText()
        {
            if (Control.Text != Element.Text)
                Control.Text = Element.Text;
        }

        private void SetIsPassword()
        {
            if (Element.IsPassword && Control.IsFirstResponder)
            {
                Control.Enabled = false;
                Control.SecureTextEntry = true;
                Control.Enabled = Element.IsEnabled;
                Control.BecomeFirstResponder();
            }
            else
            {
                Control.SecureTextEntry = Element.IsPassword;
            }
        }

        private void SetHintText()
        {
            Control.Placeholder = Element.Placeholder;
        }

        private void SetPlaceholderColor()
        {
            NSAttributedString phString = new NSAttributedString(Element.Placeholder, new UIStringAttributes() { ForegroundColor = Element.PlaceholderColor.ToUIColor() });
            Control.AttributedPlaceholder = phString;
        }

        private void SetTextColor()
        {
            if ((Element.TextColor == Color.Default) || !Element.IsEnabled)
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = Element.TextColor.ToUIColor();
        }

        private void SetFont()
        {
            Control.Font = Element.ToUIFont();
        }

        private void SetHorizontalTextAlignment()
        {
            switch (Element.HorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                default:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        private void SetKeyboard()
        {
            var kbd = Element.Keyboard.ToNative();
            Control.KeyboardType = kbd;
            Control.InputAccessoryView = kbd == UIKeyboardType.NumberPad ? NumberpadAccessoryView() : null;
            Control.ShouldReturn = InvokeCompleted;
        }

        private UIToolbar NumberpadAccessoryView()
        {
            return new UIToolbar(new RectangleF(0.0f, 0.0f, (float)Control.Frame.Size.Width, 44.0f))
            {
                Items = new[]
                {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { InvokeCompleted(null); })
                }
            };
        }

        private bool InvokeCompleted(UITextField textField)
        {
            Control.ResignFirstResponder();
            ((IEntryController)Element).SendCompleted();
            return true;
        }
    }
}