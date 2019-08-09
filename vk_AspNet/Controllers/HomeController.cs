using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using vk_AspNet.Models;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using Activity = System.Diagnostics.Activity;


namespace vk_AspNet.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {

            return vk();
        }
        static string vk()
        {
            WebProxy proxy = new WebProxy("51.38.71.101", 8080);
            proxy = new WebProxy(new Uri("http://51.38.71.101:8080"));

            IServiceCollection container = new ServiceCollection();

            container.AddAudioBypass();
            container.AddSingleton<IWebProxy, WebProxy>(provider => proxy);
            FlurlHttp.Configure(settings =>
            {
                settings.HttpClientFactory = new ProxyHttpClientFactory("http://51.38.71.101:8080");
            });


            VkApi vk
              // = new VkApi();
              = new VkApi(container);

            //  (vk.AuthorizationFlow as VkNet.Utils.Browser).Proxy = proxy;

            vk.Authorize(new ApiAuthParams()
            {
                ApplicationId = 123456,
                Login = "seadogs444@gmail.com",
                Password = "bratishkinoff922",
                Settings = Settings.All
            });
            //   var fr = vk.Friends.Get(new FriendsGetParams() { Count = 1 });
            var v = vk.Audio.Get(new AudioGetParams() { AccessKey = "id556153348" });
            Audio audio = v.Last();
            // vk.Audio.Download(audio, @"C:\Projects\VK_API\VK_API\bin\Debug");
            string outStr = "";
            foreach (var audio1 in v)
            {
                outStr += $"{audio1.Title} - {audio1.Url != null}" + Environment.NewLine;
            }
            return outStr;
        }
        public class ProxyHttpClientFactory : DefaultHttpClientFactory
        {
            private string _address;

            public ProxyHttpClientFactory(string address)
            {
                _address = address;
            }

            public override HttpMessageHandler CreateMessageHandler()
            {
                return new HttpClientHandler
                {
                    Proxy = new WebProxy(_address),
                    UseProxy = true
                };
            }
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
