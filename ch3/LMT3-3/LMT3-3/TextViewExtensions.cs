using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace LMT33
{
    public static class UITextViewExtension
    {
        public static void AppendTextLine (this UITextView textView, string text)
        {
            textView.Text += String.Format ("\r\n{0}", text);
            textView.ScrollToBottom ();
        }

        public static void ScrollToBottom (this UITextView textView)
        {
            textView.ScrollRangeToVisible (new NSRange (textView.Text.Length - 1, 1));
        }
    }
}

