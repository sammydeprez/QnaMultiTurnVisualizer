using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QnaMakerVisualization.Helpers;

namespace QnaMakerVisualization.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public Models.KnowledgeBaseDetails KnowledgeBaseDetails { get; set; }
        public Models.KnowledgeBase KnowledgeBase { get; set; }
        public Models.KnowledgeBaseMetaDataList KnowledgeBases { get; set; } = new Models.KnowledgeBaseMetaDataList() { KnowledgeBases = new List<Models.KnowledgebaseMetaData>() };
        private List<Models.Qna> QnaList { get; set; }
        private List<Models.VisualTreeItem> VisualTreeItems { get; set; }
        public List<SelectListItem> Environments { get; set; }
        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            Environments = new List<SelectListItem>();
            Environments.Add(new SelectListItem("Test", "test"));
            Environments.Add(new SelectListItem("Production", "prod"));
        }
        public async Task OnGet()
        {
            if (HttpContext.Session.Get<Models.KnowledgeBaseDetails>("KnowledgeBaseDetails") != null)
            {
                KnowledgeBaseDetails = HttpContext.Session.Get<Models.KnowledgeBaseDetails>("KnowledgeBaseDetails");
                await GetKbData();
            }

        }
        public async Task OnPostSetKbDetails()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Set<Models.KnowledgeBaseDetails>("KnowledgeBaseDetails", KnowledgeBaseDetails);
            await GetKbData();
        }
        public async Task<IActionResult> OnGetData(int qnaId = -1)
        {
            await GetKbData();
            if (qnaId == -1)
                VisualTreeItems = GetVisualTree(KnowledgeBase.QnaDocuments);
            else
                VisualTreeItems = GetVisualTree(QnaList.Where(q => q.Id == qnaId).ToList());

            return new JsonResult(VisualTreeItems);
        }
        public async Task<IActionResult> OnGetQuestion(int qnaId)
        {
            await GetKbData();

            return new JsonResult(QnaList.Where(m => m.Id == qnaId));
        }
        private async Task GetKnowledgeBase(string endpoint, string environment, string knowledgebaseId, string subscriptionKey)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                String.Format("{0}/qnamaker/v4.0/knowledgebases/{1}/{2}/qna", endpoint, knowledgebaseId, environment));
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                KnowledgeBase = await response.Content
                    .ReadAsAsync<Models.KnowledgeBase>();
            }
            else
            {
                KnowledgeBase = new Models.KnowledgeBase();
            }
            await OnGetKnowledgeBaseMetaData(endpoint, subscriptionKey);
        }
        public async Task<IActionResult> OnGetKnowledgeBaseMetaData(string endpoint, string subscriptionKey)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                String.Format("{0}/qnamaker/v4.0/knowledgebases/", endpoint));
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                KnowledgeBases = await response.Content
                    .ReadAsAsync<Models.KnowledgeBaseMetaDataList>();
            }
            else
            {
                KnowledgeBases = new Models.KnowledgeBaseMetaDataList();
            }
            HttpContext.Session.Set<Models.KnowledgeBaseMetaDataList>("KnowledgeBaseMetaDataList", KnowledgeBases);
            return new JsonResult(KnowledgeBases.KnowledgeBases);
        }
        private void GetQna(List<Models.Qna> qnaList)
        {
            if (QnaList == null)
                QnaList = new List<Models.Qna>();

            if (qnaList != null)
                foreach (var item in qnaList)
                {
                    if (QnaList.Where(it => it.Id == item.Id).FirstOrDefault() == null)
                        QnaList.Add(item);
                    var promptList = new List<Models.Qna>();
                    foreach (var prompt in item.Context.Prompts)
                    {
                        promptList.Add(prompt.Qna);
                    }
                    GetQna(promptList);
                }
        }
        private List<Models.VisualTreeItem> GetVisualTree(List<Models.Qna> qnaList)
        {
            var treeItems = new List<Models.VisualTreeItem>();

            foreach (var item in qnaList)
            {
                var treeItem = new Models.VisualTreeItem()
                {
                    Text = new Models.VisualTreeItemText() { PromptQuestion = item.Questions[0], Answer = item.Answer, QnaId = item.Id }
                };
                treeItem.Children = GetVisualTree(item.Context.Prompts);
                treeItems.Add(treeItem);

            }
            return treeItems;
        }
        private List<Models.VisualTreeItem> GetVisualTree(List<Models.Prompt> promptList)
        {
            var treeItems = new List<Models.VisualTreeItem>();

            foreach (var item in promptList)
            {
                var treeItem = new Models.VisualTreeItem()
                {
                    Text = new Models.VisualTreeItemText() { PromptQuestion = item.DisplayText, Answer = item.Qna.Answer, QnaId = item.Qna.Id }
                };
                treeItem.Children = GetVisualTree(item.Qna.Context.Prompts);
                treeItems.Add(treeItem);

            }
            return treeItems;
        }
        private async Task GetKbData()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("KnowledgeBase")))
            {

                KnowledgeBases = HttpContext.Session.Get<Models.KnowledgeBaseMetaDataList>("KnowledgeBaseMetaDataList");
                KnowledgeBaseDetails = HttpContext.Session.Get<Models.KnowledgeBaseDetails>("KnowledgeBaseDetails");
                await GetKnowledgeBase(KnowledgeBaseDetails.Endpoint, KnowledgeBaseDetails.Environment, KnowledgeBaseDetails.KnowledgeBaseId, KnowledgeBaseDetails.SubscriptionKey);
                GetQna(KnowledgeBase.QnaDocuments);
                HttpContext.Session.Set<Models.KnowledgeBase>("KnowledgeBase", KnowledgeBase);
                HttpContext.Session.Set<List<Models.Qna>>("QnaList", QnaList);
            }
            else
            {
                KnowledgeBases = HttpContext.Session.Get<Models.KnowledgeBaseMetaDataList>("KnowledgeBaseMetaDataList");
                KnowledgeBase = HttpContext.Session.Get<Models.KnowledgeBase>("KnowledgeBase");
                QnaList = HttpContext.Session.Get<List<Models.Qna>>("QnaList");
            }
        }
    }
}
