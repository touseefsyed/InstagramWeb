using InstagramWeb.Models;
using PuppeteerSharp;
using PuppeteerSharp.Mobile;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PuppeteerSharp.Input;


namespace InstagramWeb.Controllers
{
    public class HomeController : PanelController
    {

        
        ScheduleContext _db = new ScheduleContext();
        public async Task<ActionResult> Dashboard()
        {
           
            return View();
        }

        public string ClickButton(string btnName, bool disabled = false)
        {
            string disableScript = "";
            if (disabled)
            {
                disableScript = "btnList[i].disabled = false;";
            }
            string loginBtnScript = $@"
var btnList = document.getElementsByTagName(""button""); 
for (var i = 0; i < btnList.length; i++)
{{
    console.log(""loop working"");
    if (btnList[i].innerText == ""{btnName}"")
    {{
        {disableScript}
        btnList[i].click();
    }}
}}
                    ";
            return loginBtnScript;
        }

        public async void CreateInstagramPost(InstagramWeb.Models.Schedule schedule)
        {
            string Url = "https://www.instagram.com/";
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

            using (var browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                Timeout = 120000
            }).GetAwaiter().GetResult())
            {
                var devices = Puppeteer.Devices;
                var iphone = devices[DeviceDescriptorName.IPhone6];
                var page = browser.NewPageAsync().GetAwaiter().GetResult();
                await page.EmulateAsync(iphone);

                try
                {
                    await page.GoToAsync(Url);
                    string html = await page.GetContentAsync();
                    System.Threading.Thread.Sleep(5000);
                    await page.EvaluateExpressionAsync(ClickButton("Log In"));

                    string usernamePasswordScript = @"
var username = document.getElementsByName(""username"")[0];
username.value=""zawarmehmood.2003@gmail.com"";
var password = document.getElementsByName(""password"")[0];
password.value=""Zawar123!@#"";

";

                    System.Threading.Thread.Sleep(5000);
                    for (int i = 0; i < 4; i++)
                    {
                        await page.Keyboard.DownAsync(key: "Tab");
                        System.Threading.Thread.Sleep(500);
                    }
                    await page.Keyboard.TypeAsync("zawarmehmood.2003@gmail.com", new TypeOptions { Delay = 200 });
                    await page.Keyboard.DownAsync(key: "Tab");
                    await page.Keyboard.TypeAsync("Zawar123!@#", new TypeOptions { Delay = 200 });
                    await page.EvaluateExpressionAsync(ClickButton("Log In", true));
                    System.Threading.Thread.Sleep(5000);
                    await page.EvaluateExpressionAsync(ClickButton("Save Info"));
                    System.Threading.Thread.Sleep(5000);
                    await page.EvaluateExpressionAsync(ClickButton("Cancel"));
                    System.Threading.Thread.Sleep(5000);
                    //Dont add await for this
                    page.EvaluateExpressionAsync(
                        @"document.querySelector(""div[data-testid=new-post-button]"").click()");
                    var fileChooser = await page.WaitForFileChooserAsync();
                    await fileChooser.AcceptAsync(new string[] { "C:\\Users\\Zawar\\Desktop\\New folder (4)\\cover6.jpg" });
                    System.Threading.Thread.Sleep(5000);
                    await page.EvaluateExpressionAsync(ClickButton("Next"));
                    System.Threading.Thread.Sleep(5000);
                    for (int i = 0; i < 4; i++)
                    {
                        await page.Keyboard.DownAsync(key: "Tab");
                        System.Threading.Thread.Sleep(500);
                    }
                    await page.Keyboard.TypeAsync("Automated caption", new TypeOptions { Delay = 200 });
                    await page.EvaluateExpressionAsync(ClickButton("Share"));
                    System.Threading.Thread.Sleep(1600000);

                }
                catch (Exception ex)
                {


                }
            }

        }
    }
}



//                    string loginBtnScript2 = @"
//var btnList = document.getElementsByTagName(""button""); 
//for (var i = 0; i < btnList.length; i++)
//{
//    console.log(""loop working"");
//    if (btnList[i].innerText == ""Log In"")
//    {
//        btnList[i].disabled = false;
//        btnList[i].click();
//    }
//}
//                    ";

//                    string loginBtnScript3 = @"
//var btnList = document.getElementsByTagName(""button""); 
//for (var i = 0; i < btnList.length; i++)
//{
//    console.log(""loop working"");
//    if (btnList[i].innerText == ""Save Info"")
//    {
//        btnList[i].disabled = false;
//        btnList[i].click();
//    }
//}
//                    ";

//                    string loginBtnScript4 = @"
//var btnList = document.getElementsByTagName(""button""); 
//for (var i = 0; i < btnList.length; i++)
//{
//    console.log(""loop working"");
//    if (btnList[i].innerText == ""Cancel"")
//    {
//        btnList[i].disabled = false;
//        btnList[i].click();
//    }
//}
//                    ";

//                    string loginBtnScript5 = @"
//var btnList = document.getElementsByTagName(""button""); 
//for (var i = 0; i < btnList.length; i++)
//{
//    console.log(""loop working"");
//    if (btnList[i].innerText == ""Next"")
//    {
//        btnList[i].disabled = false;
//        btnList[i].click();
//    }
//}
//                    ";


//                    string loginBtnScript6 = @"
//var btnList = document.getElementsByTagName(""button""); 
//for (var i = 0; i < btnList.length; i++)
//{
//    console.log(""loop working"");
//    if (btnList[i].innerText == ""Share"")
//    {
//        btnList[i].disabled = false;
//        btnList[i].click();
//    }
//}
//                    ";
