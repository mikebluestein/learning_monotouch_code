
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.GameKit;

namespace GameKitChat
{
    public class Application
    {
        static void Main (string[] args)
        {
            UIApplication.Main (args);
        }
    }

    public partial class AppDelegate : UIApplicationDelegate
    {
        GKSession _gkSession;
        MyVoiceChatClient _vcClient;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            joinButton.Clicked += delegate { ShowPeerPicker (); };
            
            window.MakeKeyAndVisible ();
            return true;
        }

        void ShowPeerPicker ()
        {
            _gkSession = new GKSession ("com.lmt.gkvoicechat", UIDevice.CurrentDevice.Name, GKSessionMode.Peer);

            _gkSession.ReceiveData += delegate(object sender, GKDataReceivedEventArgs e) { 
                GKVoiceChatService.Default.ReceivedData (e.Data, e.PeerID); 
            };          
            
            _gkSession.ConnectionRequest += delegate(object sender, GKPeerConnectionEventArgs e) {         
                e.Session.AcceptConnection (e.PeerID, IntPtr.Zero);
            };
            
            GKPeerPickerController peerPickerController = new GKPeerPickerController ();
            peerPickerController.Delegate = new PeerPickerDelegate (this);
            peerPickerController.ConnectionTypesMask = GKPeerPickerConnectionType.Nearby;
            peerPickerController.Show ();
        }

        public class MyVoiceChatClient : MonoTouch.GameKit.GKVoiceChatClient
        {
            GKSession _session;
   
            public MyVoiceChatClient (GKSession session)
            {
                _session = session;
            }
            
            public override string ParticipantID ()
            {
                return _session.PeerID;
            }

            public override void SendData (GKVoiceChatService voiceChatService, NSData data, string toParticipant)
            {
                _session.SendData (data, new string[] { toParticipant }, GKSendDataMode.Reliable, IntPtr.Zero);
            }
        }

        class PeerPickerDelegate : GKPeerPickerControllerDelegate
        {
            AppDelegate _controller;

            public PeerPickerDelegate (AppDelegate controller)
            {
                _controller = controller;
            }

            public override GKSession GetSession (GKPeerPickerController picker, GKPeerPickerConnectionType forType)
            {
                return _controller._gkSession;
            }

            public override void PeerConnected (GKPeerPickerController picker, string peerId, GKSession toSession)
            {
                _controller._gkSession = toSession;
                
                picker.Dismiss ();
                picker.Delegate = null;
                
                _controller.joinButton.Title = "Connected";
                _controller.joinButton.Enabled = false;
                   
                AVAudioSession audioSession = AVAudioSession.SharedInstance ();
                NSError error;
                audioSession.SetCategory (AVAudioSession.CategoryPlayAndRecord.ToString (), out error);
                audioSession.SetActive (true, out error);
                _controller._vcClient = new MyVoiceChatClient (_controller._gkSession);
                GKVoiceChatService.Default.Client = _controller._vcClient;
                GKVoiceChatService.Default.StartVoiceChat (peerId, IntPtr.Zero);
            }

            public override void ControllerCancelled (GKPeerPickerController picker)
            {
                picker.Delegate = null;
            }
        }
        
    }
    
}
