
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.GameKit;

namespace LMT102
{
    public partial class ChatController : UIViewController
    {
        GKSession _session;
        GKPeerPickerController _peerPickerController;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public ChatController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public ChatController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public ChatController () : base("ChatController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            chatHistory.Editable = false;
            chatText.AutocorrectionType = UITextAutocorrectionType.No;
            chatText.AutocapitalizationType = UITextAutocapitalizationType.None;
            chatText.BecomeFirstResponder ();
            
            chatText.ShouldReturn += delegate {
                
                if (_session != null) {
                    AddToChatHistory (chatText.Text);
                    _session.SendDataToAllPeers (chatText.Text, GKSendDataMode.Reliable, IntPtr.Zero);
                    chatText.Text = "";
                }
                return true;
            };
            
            chatJoin.Clicked += delegate { ShowPeerPicker (); };
        }

        void AddToChatHistory (string text)
        {
            chatHistory.Text += String.Format ("\r\n {0}", text);
            chatHistory.ScrollToBottom ();
        }

        void ShowPeerPicker ()
        {
            _session = new GKSession ("com.lmt.gkchat2", UIDevice.CurrentDevice.Name, GKSessionMode.Peer);
            _session.ReceiveData += (s, e) => { AddToChatHistory (NSString.FromData (e.Data, NSStringEncoding.UTF8).ToString ()); };
            _session.ConnectionRequest += (s, e) => { e.Session.AcceptConnection (e.PeerID, IntPtr.Zero); };
            
            _peerPickerController = new GKPeerPickerController ();
            _peerPickerController.Delegate = new PeerPickerDelegate (this);
            _peerPickerController.ConnectionTypesMask = GKPeerPickerConnectionType.Nearby;
            _peerPickerController.Show ();
        }

        class PeerPickerDelegate : GKPeerPickerControllerDelegate
        {
            ChatController _controller;

            public PeerPickerDelegate (ChatController controller)
            {
                _controller = controller;
            }

            public override GKSession GetSession (GKPeerPickerController picker, GKPeerPickerConnectionType forType)
            {
                return _controller._session;
            }

            public override void PeerConnected (GKPeerPickerController picker, string peerId, GKSession toSession)
            {
                _controller._session = toSession;
                
                picker.Dismiss ();
                picker.Delegate = null;
                
                //just disabling button once connection is made for simplicity
                
                _controller.chatJoin.Enabled = false;
            }

            public override void ControllerCancelled (GKPeerPickerController picker)
            {
                picker.Delegate = null;
            }
        }
        
    }

    public static class UITextViewExtension
    {
        public static void ScrollToBottom (this UITextView textView)
        {
            textView.ScrollRangeToVisible (new NSRange (textView.Text.Length - 1, 1));
        }
    }
}

