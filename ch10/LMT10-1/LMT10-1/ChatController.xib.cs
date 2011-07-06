
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.GameKit;

namespace LMT101
{
    public partial class ChatController : UIViewController
    {
        GKSession _session;

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
            
            chatText.BecomeFirstResponder ();
            
            chatJoin.Clicked += ChatJoinClicked;
            
            chatSend.Clicked += delegate {
                _session.SendDataToAllPeers (chatText.Text, 
                    GKSendDataMode.Reliable, IntPtr.Zero);
                AddToChatHistory (String.Format ("sent message '{0}'", chatText.Text));
                chatText.Text = "";
            };
        }

        void ChatJoinClicked (object sender, EventArgs e)
        {      
            _session = new GKSession ("com.lmt.gkchat1", 
                UIDevice.CurrentDevice.Name, GKSessionMode.Peer);
            
            _session.PeerChanged += delegate(object s0, 
                GKPeerChangedStateEventArgs peerArgs) {
                
                switch (peerArgs.State) {
                case GKPeerConnectionState.Available:
                    AddToChatHistory (String.Format ("{0} is available", 
                        _session.DisplayNameForPeer (peerArgs.PeerID)));
                    _session.Connect (peerArgs.PeerID, 60);
                    AddToChatHistory (String.Format ("sent connection request to {0}", 
                        _session.DisplayNameForPeer (peerArgs.PeerID)));
                    break;
                case GKPeerConnectionState.Connected:
                    AddToChatHistory (String.Format ("connected to {0}", 
                        _session.DisplayNameForPeer (peerArgs.PeerID)));
                    chatSend.Enabled = true;
                    break;
                case GKPeerConnectionState.Connecting:
                    AddToChatHistory (String.Format ("{0} is connecting", 
                        _session.DisplayNameForPeer (peerArgs.PeerID)));
                    break;
                case GKPeerConnectionState.Disconnected:
                    AddToChatHistory (String.Format ("{0} disconnected", 
                        _session.DisplayNameForPeer (peerArgs.PeerID)));
                    break;
                case GKPeerConnectionState.Unavailable:
                    AddToChatHistory (String.Format ("{0} is unavailable", 
                        _session.DisplayNameForPeer (peerArgs.PeerID)));
                    break;
                }
            };
            
            _session.ConnectionRequest += delegate(object s1, 
                GKPeerConnectionEventArgs connectionArgs) {
                
                AddToChatHistory (String.Format ("received connection request from {0}", 
                    _session.DisplayNameForPeer (connectionArgs.PeerID)));
                
                _session.AcceptConnection (connectionArgs.PeerID, IntPtr.Zero);
                
                AddToChatHistory (String.Format ("accepted connection from {0}", 
                   _session.DisplayNameForPeer (connectionArgs.PeerID)));
            };
            
            _session.ReceiveData += delegate(object s2, GKDataReceivedEventArgs dataArgs) { 
                AddToChatHistory (String.Format("received message '{0}' from {1}", 
                    NSString.FromData (dataArgs.Data, NSStringEncoding.UTF8), 
                    _session.DisplayNameForPeer (dataArgs.PeerID))); 
            };
            
            _session.Available = true;
            
            chatJoin.Enabled = false;
        }

        void AddToChatHistory (string text)
        {
            chatHistory.Text += String.Format ("\r\n{0}", text);
            chatHistory.ScrollRangeToVisible (
                new NSRange (chatHistory.Text.Length - 1, 1));
        }
    }
}
