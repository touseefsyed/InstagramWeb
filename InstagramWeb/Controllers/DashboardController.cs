using InstagramWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InstagramWeb.Controllers
{
    public class InstagramBusinessAccount
    {
        public string id { get; set; }
    }

    public class IGUser
    {
        public InstagramBusinessAccount instagram_business_account { get; set; }
        public string id { get; set; }
    }

    public class DashboardController : PanelController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Token)
        {
            return View();
        }



        public async Task<JsonResult> FacebookPageJson(string authToken)
        {
            using (var _client = new HttpClient())
            {
                string url = $@"https://graph.facebook.com/v7.0/me/accounts?access_token={authToken}";
                var response = await _client.GetAsync(url);
                var result = response.Content.ReadAsStringAsync();
                return Json(result.Result);
            }
        }


        public async Task<ActionResult> UpdatePageId(string pageId, string authToken)
        {
            var currentUser = ScheduleContext.Users.FirstOrDefault(x => x.Id == user.Id);
            
            using (var client = new HttpClient())
            {
                string url = $@"https://graph.facebook.com/v7.0/{pageId}?fields=instagram_business_account&access_token={authToken}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                try
                {
                    var jsonObject = JsonConvert.DeserializeObject<IGUser>(result);

                    if (jsonObject.instagram_business_account == null || string.IsNullOrWhiteSpace(jsonObject.instagram_business_account.id))
                    {
                        return Json("noAccount");
                    }
                    else 
                    {
                        currentUser.InstagramUserId = jsonObject.instagram_business_account.id;
                    }

                }
                catch (Exception)
                {
                    return Json("noAccount");
                }
            }

            currentUser.FacebookPageId = pageId;
            ScheduleContext.SaveChanges();
            Session["User"] = new SessionUser(currentUser);
            return Json(true);
        }

        public async Task<ActionResult> FetchMedia(string authToken)
        {
            var instagramUserId = user.InstagramUserId;
            using (var client = new HttpClient())
            {
                var url = $@"https://graph.facebook.com/v7.0/{instagramUserId}/media?access_token={authToken}";
                var response =await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                return Json(result);
            }
        }


        public async Task<ActionResult> FetchInsights(string authToken)
        {
            var instagramUserId = user.InstagramUserId;
            using (var client = new HttpClient())
            {
                var url = $@"https://graph.facebook.com/v7.0/{instagramUserId}/insights?metric=impressions,reach&period=week&since={DateTime.Now.AddDays(-28).ToString("yyyy-MM-dd")}&until={DateTime.Now.ToString("yyyy-MM-dd")}&access_token={authToken}";
                //var url = $@"https://graph.facebook.com/{instagramUserId}/insights?metric=impressions,reach,profile_views&period=month&access_token={authToken}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                return Json(result);
            }
        }

        public async Task<ActionResult> FetchLifetimeInsights(string authToken)
        {
            var instagramUserId = user.InstagramUserId;
            using (var client = new HttpClient())
            {
                var url = $@"https://graph.facebook.com/v7.0/{instagramUserId}/insights?metric=reach,profile_views&period=lifetime&access_token={authToken}";
              //var url = $@"https://graph.facebook.com/{instagramUserId}/insights?metric=impressions,reach,profile_views&period=month&access_token={authToken}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                return Json(result);
            }
        }


        public async Task<ActionResult> FetchBusiness(string authToken)
        {
            try
            {
                var instagramUserId = user.InstagramUserId;
                var instagramUsername = user.InstagramUsername;
#if DEBUG
                instagramUsername = "zeeforzawar123";
#endif
                using (var client = new HttpClient())
                {
                    var url = @"https://graph.facebook.com/v3.2/" + instagramUserId + "?fields=business_discovery.username("+instagramUsername+"){followers_count,media_count,media{comments_count,like_count}}&access_token=" + authToken;
                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
           
        }



    }
}




//            using (var httpClient = new System.Net.Http.HttpClient())
//            {
//                string url = $@"https://api.instagram.com/oauth/authorize
//?client_id={718129498975401}&redirect_uri={"https://user.getfruity.co/"}&scope=user_profile,user_media&response_type=code";
//                var response = await httpClient.GetAsync(url);
//                var result = response.Content;
//            }


