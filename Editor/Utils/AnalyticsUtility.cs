using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

namespace Kinetix.Editor
{
    internal static class AnalyticsUtility
    {
        private const string SegmentURL = "https://api.segment.io/v1/track";
        private const string WRITE_KEY  = "81TtulYt7ViNrarGnaL6vGsq2fCwZD0G";

        public static async Task<bool> SendWebRequestEvent(string event_name, string coreBundleType)
        {
            var tcs = new TaskCompletionSource<bool>();

            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            jsonData.Add("userId", AnalyticsSessionInfo.userId.ToString() );
            jsonData.Add("event", GetTrackingName(event_name) );
            jsonData.Add("properties", GetEventProperties( coreBundleType));
            jsonData.Add("context", GetContext() );

            using (UnityWebRequest uwr = new UnityWebRequest(SegmentURL, "POST"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(jsonData));
                uwr.uploadHandler   = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");
                uwr.SetRequestHeader("Authorization", "Basic " + System.Convert.ToBase64String( System.Text.Encoding.ASCII.GetBytes(WRITE_KEY)));

                uwr.SendWebRequest();

                while (!uwr.isDone)
                {
                    await Task.Yield();
                }

                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError ||
                    uwr.result == UnityWebRequest.Result.DataProcessingError)
                {
            
                    tcs.SetResult(false); 
                }
                else
                {
                    tcs.SetResult(true);
                }
            }

            return await tcs.Task;
        }

        private static Dictionary<string, object> GetEventProperties(string coreBundleType)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();

            if(coreBundleType != "")
                properties.Add("coreBundleType", coreBundleType);

            return properties;
        }

        private static Dictionary<string, object> GetContext()
        {
            Dictionary<string, object> context = new Dictionary<string, object>();   

            Dictionary<string, object> app = new Dictionary<string, object>();   
            app.Add("name", Application.productName );
            //app.Add("version", coreBundleVersion);
            context.Add("app", app);

            Dictionary<string, object> device = new Dictionary<string, object>();   
            // device.Add("id", SystemInfo.deviceUniqueIdentifier );
            device.Add("model", SystemInfo.deviceModel );
            device.Add("type", SystemInfo.deviceType.ToString() );
            device.Add("manufacturer", Application.platform.ToString() );
            // device.Add("deviceName", SystemInfo.deviceName );
            // device.Add("graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor );
            context.Add("device", device);

            Dictionary<string, object> os = new Dictionary<string, object>();   
            os.Add("name", SystemInfo.operatingSystemFamily.ToString() );
            os.Add("version", SystemInfo.operatingSystem );
            context.Add("os", os);

            return context;
        }

        private static string GetTrackingName(string event_text="")
        {
            string Environment;

            #if !STAGING_KINETIX && !DEV_KINETIX
                Environment = "Prod";
            #else
                Environment = "PreProd";
            #endif
            
            string Product = "SDK";
            string Tracking = "Unity";

            return Environment+"_"+Product+"_"+Tracking+"_"+event_text;
        }
    }
}
