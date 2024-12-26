using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Policy;

namespace BeaverYoink
{
    public static class Program
    {
        public const string VidIDYoinkResourceName = "BeaverYoink.VidIDYoink.js";
        public static readonly string VidIDYoink = LoadVidIDYoink();
        public static string LoadVidIDYoink()
        {
            Assembly assembly = typeof(Program).Assembly;
            Stream VidIDYoinkStream = assembly.GetManifestResourceStream(VidIDYoinkResourceName);
            StreamReader VidIDYoinkReader = new StreamReader(VidIDYoinkStream);
            string output = VidIDYoinkReader.ReadToEnd();
            VidIDYoinkReader.Dispose();
            VidIDYoinkStream.Dispose();
            return output;
        }
        [STAThread]
        public static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                args = new string[] { "76544" };
                //args = new string[] { Console.ReadLine() };
            }
            if (args.Length == 1 && args[0].ToLower() == "js")
            {
                Clipboard.SetText(VidIDYoink);
                Console.WriteLine("VidIDYoink.js has been placed in your clipboard.");
            }
            else if (args.Length == 1)
            {
                string vidID = args[0];
                string vidUrl = $"/content/cenc/A76544_E2_XX_XX_XX/82913722/76544_240p_400kbps_en-US.mp4";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://cdn.swankidc.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.AcceptEncoding.Clear();
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));
                client.DefaultRequestHeaders.Add("DNT", "1");
                client.DefaultRequestHeaders.Add("Origin", "https://digitalcampus.swankmp.net");
                client.DefaultRequestHeaders.Referrer = new Uri("https://digitalcampus.swankmp.net/");
                client.DefaultRequestHeaders.Add("Sec-CH-UA", "\"Google Chrome\";v=\"129\", \"Not=A?Brand\";v=\"8\", \"Chromium\";v=\"129\"");
                client.DefaultRequestHeaders.Add("Sec-CH-UA-Mobile", "?0");
                client.DefaultRequestHeaders.Add("Sec-CH-UA-Platform", "\"Windows\"");
                client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
                client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "cross-site");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36");

                try
                {
                    HttpResponseMessage headResponse = client.SendAsync(new HttpRequestMessage(HttpMethod.Head, vidUrl)).GetAwaiter().GetResult();
                    Console.WriteLine(headResponse.Content.Headers.ContentLength.Value);

                    client.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 1024 * 512);
                    HttpResponseMessage response = client.GetAsync(vidUrl).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    byte[] content = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    File.WriteAllBytes("video.mp4", content);
                    Console.WriteLine("Download successful.");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("To download a video run: BeaverYoink.exe videoID");
                Console.WriteLine("For example: BeaverYoink.exe 12345");
                Console.WriteLine("To get a video ID copy VidIDYoink.js into the chrome developer console while on beaverstreaming.");
                Console.WriteLine("To get VidIDYoink.js in your clipboard run: BeaverYoink.exe js");
            }
            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
        }
    }
}
