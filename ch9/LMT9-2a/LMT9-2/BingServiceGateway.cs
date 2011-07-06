using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Json;
using System.IO;
using System.Text;

//iPhone specific - only needed if using NSUrlConnection to make http request
using MonoTouch.Foundation;
using System.Web;

namespace LMT92
{
    public delegate void SynchronizerDelegate (List<SearchResultItem> results);

    public class BingServiceGateway
    {
        const string BING_API_ID = "INSERT_YOUR_API_ID";

        // Let the caller provide the function to sync to the main thread so 
        // that code here can be platform agnostic. Otherwise, you would need 
        // a pointer to a UIViewController here to call InvokeOnMainThread, 
        // which would make this class iOS specific.
        static SynchronizerDelegate _sync;
        
        public BingServiceGateway (SynchronizerDelegate sync)
        {
            _sync = sync;
        }

        void ParseResults (HttpWebResponse httpRes)
        {
            XmlDocument xml = new XmlDocument ();
            xml.Load (httpRes.GetResponseStream ());
            
            XmlNamespaceManager nsm = new XmlNamespaceManager (xml.NameTable);
            nsm.AddNamespace ("web", "http://schemas.microsoft.com/LiveSearch/2008/04/XML/web");
            XmlNodeList resultNodes = xml.SelectNodes ("//web:WebResult/web:Title", nsm);
            
            List<SearchResultItem> results = new List<SearchResultItem> ();
            
            foreach (XmlNode node in resultNodes) {
                results.Add (new SearchResultItem { Title = node.InnerText });
            }
            
            if (_sync != null)
                _sync (results);
        }

        #region option 1 - spin up a thread and make a sync request to the service in that thread

        public void Search1 (string text)
        {
            Thread t = new Thread (Search);
            t.Start (text);
        }

        void Search (object text)
        {
            string bingSearch = String.Format ("http://api.bing.net/xml.aspx?AppId={0}&Query={1}&Sources=web&web.count=10", BING_API_ID, text);
            
            HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (bingSearch));
            
            using (HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse ()) {
                ParseResults (httpRes);
            }
        }

        #endregion

        #region option 2 - use built in async features of HttpWebRequest

        public void Search2 (string text)
        {
            string bingSearch = String.Format ("http://api.bing.net/xml.aspx?AppId={0}&Query={1}&Sources=web&web.count=10", BING_API_ID, text);
            HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (bingSearch));
            httpReq.BeginGetResponse (new AsyncCallback (ResponseCallback), httpReq);
        }

        void ResponseCallback (IAsyncResult ar)
        {
            HttpWebRequest httpReq = (HttpWebRequest)ar.AsyncState;
            using (HttpWebResponse httpRes = (HttpWebResponse)httpReq.EndGetResponse (ar)) {
                ParseResults (httpRes);
            }
        }

        #endregion

        #region option 3 - use Linq to Xml

        public void Search3 (string text)
        {
            Thread t = new Thread (Search3);
            t.Start (text);
        }

        void Search3 (object text)
        {
            string bingSearch = String.Format ("http://api.bing.net/xml.aspx?AppId={0}&Query={1}&Sources=web&web.count=10", BING_API_ID, text);
            
            // need to add ref to System.Xml.Linq
            XDocument x = XDocument.Load (bingSearch);
            XName xWebResult = (XNamespace)"http://schemas.microsoft.com/LiveSearch/2008/04/XML/web" + "WebResult";
            XName xTitle = (XNamespace)"http://schemas.microsoft.com/LiveSearch/2008/04/XML/web" + "Title";
            
            var results = (from result in x.Descendants (xWebResult).Elements (xTitle)
                select new SearchResultItem { Title = result.Value }).ToList ();
            
            if (_sync != null)
                _sync (results);
        }

        #endregion

        #region option 4 - json results

        public void Search4 (string text)
        {
            Thread t = new Thread (Search4);
            t.Start (text);
        }

        void Search4 (object text)
        {
            string bingSearch = String.Format ("http://api.bing.net/json.aspx?AppId={0}&Query={1}&Sources=web&web.count=10", BING_API_ID, text);
            HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (bingSearch));
            using (HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse ()) {
                ParseResultsJson (httpRes);
            }
        }

        void ParseResultsJson (HttpWebResponse httpRes)
        {
            Stream s = httpRes.GetResponseStream ();
            
            // reference System.Json
            JsonObject j = (JsonObject)JsonObject.Load (s);
            
            var results = (from result in (JsonArray)j["SearchResponse"]["Web"]["Results"]
                let jResult = result as JsonObject 
                select new SearchResultItem { Title = jResult["Title"] }).ToList ();
            
            if (_sync != null)
                _sync (results);
        }

        #endregion

        #region option 5 - soap .net 2.0 style
        public void Search5 (string text)
        {
            Bing.BingService bingClient = new Bing.BingService ();
            Bing.SearchRequest request = new Bing.SearchRequest ();
            request.AppId = BING_API_ID;
            request.Sources = new LMT92.Bing.SourceType[] { Bing.SourceType.Web };
            request.Query = text;
            bingClient.BeginSearch (request, new AsyncCallback (BingAsyncCallback), bingClient);
        }

        void BingAsyncCallback (IAsyncResult ar)
        {
            Bing.BingService client = ar.AsyncState as Bing.BingService;
            Bing.SearchResponse response = client.EndSearch (ar);
            var results = response.Web.Results.Select (wr => new SearchResultItem { Title = wr.Title }).ToList ();
            
            if (_sync != null)
                _sync (results);
        }
        #endregion

        #region option 6 - wcf client

        //BUG: Currently doesn't work...pending MonoTouch bug fixes in WCF stack
        public void Search6 (string text)
        {
            // use slsvcutil in windows silverlight sdk to genreate proxy and copy into monodevelop project   
            // c:\temp>"C:\Program Files (x86)\Microsoft SDKs\Silverlight\v3.0\Tools\SlSvcUtil.exe" /n:*,LMT92.Bing.WCF http://api.bing.net/search.wsdl?AppID=InsertYourAppId /noConfig /out:BingServiceWCF.cs
            // manually removed line 25 in the genereated code since SupportsFaults isn't available
            
            string address = "http://api.bing.net:80/soap.asmx";
            
            var bingClient = new Bing.WCF.BingPortTypeClient (new System.ServiceModel.BasicHttpBinding (), new System.ServiceModel.EndpointAddress (address));
            
            var request = new Bing.WCF.SearchRequest { 
                AppId = BING_API_ID, 
                Sources = new Bing.WCF.SourceType[] { Bing.WCF.SourceType.Web }, 
                Query = text };
            
            bingClient.SearchCompleted += delegate(object sender, Bing.WCF.SearchCompletedEventArgs e) {
                var searchResponse = e.Result;
                var results = searchResponse.Web.Results.Select (wr => new SearchResultItem { Title = wr.Title }).ToList ();
                
                if (_sync != null)
                    _sync (results);
            };
            
            bingClient.SearchAsync (request);
        }

        #endregion

        #region option 7 - NSUrlConnection

        BingConnectionDelegate _cnDelegate = new BingConnectionDelegate ();
        
        class BingConnectionDelegate : NSUrlConnectionDelegate
        {
            StringBuilder _sb;

            public BingConnectionDelegate ()
            {
                _sb = new StringBuilder ();
            }

            public override void ReceivedData (NSUrlConnection connection, NSData data)
            {
                string xml = data.ToString ();
                
                _sb.Append (xml);
            }

            public override void FinishedLoading (NSUrlConnection connection)
            {
                //Note: could also use NSXMLParser here from Apple as well if you wish, although we'll
                //stick with Linq to Xml here as it makes the code more concise
                
                string xml = _sb.ToString ();
                XDocument x = XDocument.Load (new StringReader (xml));
                XName xWebResult = (XNamespace)"http://schemas.microsoft.com/LiveSearch/2008/04/XML/web" + "WebResult";
                XName xTitle = (XNamespace)"http://schemas.microsoft.com/LiveSearch/2008/04/XML/web" + "Title";
                
                var results = (from result in x.Descendants (xWebResult).Elements (xTitle)
                    select new SearchResultItem { Title = result.Value }).ToList ();
                
                if (_sync != null)
                    _sync (results);
            }
        }

        public void Search7 (string text)
        {
            //BUG: NSUrl creation fails if space in search text TODO: escape input
            string bingSearch = String.Format ("http://api.bing.net/xml.aspx?AppId={0}&Query={1}&Sources=web&web.count=10", BING_API_ID, text);
            NSUrlRequest bingRequest = new NSUrlRequest (NSUrl.FromString (HttpUtility.UrlPathEncode(bingSearch)));
            _cnDelegate = new BingConnectionDelegate ();
            NSUrlConnection cn = new NSUrlConnection (bingRequest, _cnDelegate);
            cn.Start ();
        }
        
        #endregion
    }
}

