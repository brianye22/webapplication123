using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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


        public static string loop_breaker = "received message";

        public JsonResult Send(string msg = "", string user_id="Test")
        {
            string botId = Properties.Resources.bot_id;

            var x = new System.Collections.Specialized.NameValueCollection();
            x.Add("api_key", Properties.Resources.bot_api_key);
            x.Add("user_id", user_id);
            x.Add("message", msg);

            using (var client = new System.Net.WebClient())
            {
                client.UploadValues(Properties.Resources.bot_url, "POST", x);
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

        public IActionResult Receive()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var txt = reader.ReadToEnd();

                string exchange_type = "";
                string exchange_exchange_id = "";
                string exchange_bot_id = "";
                string exchange_language_code = "";
                string exchange_bot_replies = "";
                string exchange_buttons = "";
                string exchange_edit_url = "";
                string exchange_is_active = "";
                string exchange_chatbubble_hide_input_field = "";
                string exchange_webhook_url = "";

                string message = "";

                try
                {
                    dynamic jsonMsg;
                    jsonMsg = JsonConvert.DeserializeObject(txt);

                    exchange_type = jsonMsg.exchange.type;
                    exchange_exchange_id = jsonMsg.exchange.exchange_id;
                    exchange_bot_id = jsonMsg.exchange.bot_id;
                    exchange_language_code = jsonMsg.exchange.language_code;
                    exchange_bot_replies = jsonMsg.exchange.bot_replies;
                    exchange_buttons = jsonMsg.exchange.buttons.ToString();
                    exchange_edit_url = jsonMsg.exchange.edit_url;
                    exchange_is_active = jsonMsg.exchange.is_active;
                    exchange_chatbubble_hide_input_field = jsonMsg.exchange.chatbubble_hide_input_field;
                    exchange_webhook_url = jsonMsg.exchange.webhook_url;
                    message = jsonMsg.message;
                }
                catch (Exception e) {
                }

                if (exchange_type.Contains("fallback") && !message.Contains(BotController.loop_breaker))
                {   // process and answer
                    //Send("received message: " + message, exchange_exchange_id);
                    Send("received message: " + txt, exchange_exchange_id);
                }

                return Ok(txt);
            }
            return Unauthorized();
        }     
        

    }
}
