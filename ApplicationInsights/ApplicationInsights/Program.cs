using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace ApplicationInsights
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = TelemetryConfiguration.CreateDefault();// new TelemetryConfiguration("2e70b2fa-38ac-4dc8-99ac-6ddf5f21038b");
            config.InstrumentationKey = "2e70b2fa-38ac-4dc8-99ac-6ddf5f21038b";
            var client = new TelemetryClient(config);
            client.TrackTrace("test trace");

            client.TrackException(new Exception("test exc"));
            Console.WriteLine("Hello World!");
            /*Telemetry is not sent instantly. 
             * Telemetry items are batched and sent by the ApplicationInsights SDK.
             * In Console apps, which exits right after calling Track() methods, 
             * telemetry may not be sent unless Flush() and Sleep is done before the app exits as shown in full example later in this article.
             * */

            // before exit, flush the remaining data
            client.Flush();

            // flush is not blocking so wait a bit
            Task.Delay(5000).Wait();
        }
    }
}
