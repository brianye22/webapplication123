using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class BotController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Send(string msg = "")
        {
            string botId = Properties.Resources.bot_id;
  
            var x = new System.Collections.Specialized.NameValueCollection();
            x.Add("api_key", Properties.Resources.bot_api_key);
            x.Add("user_id", "Test");
            x.Add("message", msg);

            using (var client = new System.Net.WebClient())
            {
                client.UploadValues(Properties.Resources.bot_url, "POST",  x);
            }

            return Json(x.ToString());
        }

        public IActionResult SendMessage(WebApplication1.Models.BotMessageModel model)
        {
            if (ModelState.IsValid)
            {   //TODO: 
                Send(model.message);
            }

            return View("SendMessage", model);
        }

        //public JsonResult Receive()
        //{
        //    //dynamic json = JsonConvert.DeserializeObject(msg);
        //    //return json;
        //    List<string> l = new List<string>();
        //    l.Add("123");

        //    return Json(l);
        //}

        [HttpPost("")]
        public async Task<IActionResult> Receive()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var txt = await reader.ReadToEndAsync();
                Send("received: " + txt);
                return Ok("It Works!");
            }

            return Unauthorized();
        }


    }
}