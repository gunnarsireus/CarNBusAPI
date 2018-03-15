using NServiceBus;
using NServiceBus.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared.Utils
{
    public class Helpers
    {

        static string GetServerFolder()
        {
            string serverFolder;
            string exeFolder = "";
#if (DEBUG)
            exeFolder = "Debug";
#elif (PRODUCTION)
            exeFolder = "Production";
#elif (RELEASE)
            exeFolder = "Release";
#else
#endif

            serverFolder = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + Path.DirectorySeparatorChar + "Server" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + exeFolder + Path.DirectorySeparatorChar + "netcoreapp2.0" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(serverFolder))
            {
                serverFolder = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            }

            return serverFolder;
        }

        public static string GetDbLocation(string appSettingsDbLocation)
        {
            return GetServerFolder() + appSettingsDbLocation + Path.DirectorySeparatorChar;
        }

        public static void RedirectConsoleToTextFile(string fileName)
        {
            var filestream = new FileStream(fileName, FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
        }

        public static EndpointConfiguration CreateEndpoint(string dbLocation, string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.LicensePath(dbLocation + "License.xml");
            endpointConfiguration.Conventions().DefiningMessagesAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    (t.Namespace.EndsWith("Commands")))
                .DefiningEventsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    t.Namespace.EndsWith("Events"));

            endpointConfiguration.PurgeOnStartup(true);
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            return endpointConfiguration;
        }
    }
}
