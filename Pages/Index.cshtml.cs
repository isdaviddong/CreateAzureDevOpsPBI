using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AddWorkItem.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string title { get; set; }
        [BindProperty]
        public string description { get; set; }
        [BindProperty]
        public string site { get; set; }
        [BindProperty]
        public string project { get; set; }
        [BindProperty]
        public string auth { get; set; }

        public string message { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public void OnPostAddWorkItem()
        {
            var body = @"
                    [
                     {
                     ""op"": ""add"",

                    ""path"": ""/fields/System.Title"",
                     ""from"": null,
                     ""value"": ""標題""

                    },
                     {
                     ""op"": ""add"",
                     ""path"": ""/fields/System.Description"",
                     ""from"": null,
                     ""value"": ""說明文字""
                     }
                    ]
";
            body = body.Replace("標題", title);
            body = body.Replace("說明文字", description);

            HttpClient hc = new HttpClient();
            hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", auth);
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json-patch+json"));//ACCEPT header

            var StringContent = new StringContent(body);
            StringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            var response = hc.PostAsync($"https://dev.azure.com/{site}/{project}/_apis/wit/workitems/$Product Backlog Item?api-version=6.0", StringContent);
            //Console.WriteLine(response.Result.Content);
            message = $"done. ({DateTime.Now.ToString()})";
        }

    }
}
