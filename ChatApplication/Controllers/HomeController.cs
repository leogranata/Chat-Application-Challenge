using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Util;
using System.Web.Mvc;
using ChatApplication.Models.HelperBll;
using System.Web.UI.WebControls;

namespace ChatApplication.Controllers
{
    public class HomeController : Controller
    {
        private DataLayer dl = new DataLayer();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("login");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult Sendmsg(string message, string room)
        {
            if (Session["userid"] == null)
            {
                return Json(null);
            }
            int currentUserid = Convert.ToInt32(Session["userid"].ToString());
            int currentRoomId = Convert.ToInt32(room);
            RabbitMQBll obj = new RabbitMQBll();
            obj.Send(message, currentRoomId, currentUserid);
            return Json(null);
        }
        [HttpPost]
        public JsonResult SwitchToRoom(string room)
        {
            dl.SwitchToRoom(Convert.ToInt32(Session["userid"]), Convert.ToInt32(room));
            return Json(null);
        }
        [HttpPost]
        [Obsolete]
        public JsonResult Receive(string room)
        {
            try
            {
                int currentUserid = Convert.ToInt32(Session["userid"].ToString());
                int currentRoomId = Convert.ToInt32(room);
                RabbitMQBll obj = new RabbitMQBll();
                string message = obj.Receive(currentRoomId, currentUserid);
                return Json(message);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message);
            }


        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string email = fc["txtemail"].ToString();
            string password = fc["txtpassword"].ToString();
            UserModel user = dl.login(email, password);
            if (user.userid > 0)
            {
                ViewData["status"] = 1;
                ViewData["msg"] = "login Successful...";
                Session["username"] = user.email;
                Session["userid"] = user.userid.ToString();
                return RedirectToAction("Index");
            }
            else
            {

                ViewData["status"] = 2;
                ViewData["msg"] = "invalid Email or Password...";
                return View();
            }

        }

        [HttpPost]
        public JsonResult ConnectedUserList(string room)
        {
            int roomId = Convert.ToInt32(room);
            List<UserModel> users = dl.GetConnectedUsers(roomId);
            List<ListItem> userlist = new List<ListItem>();
            if (!Session.IsNewSession && Session.Count != 0)
            {
                int currentUserid = Convert.ToInt32(Session["userid"].ToString());

                foreach (var item in users)
                {
                    userlist.Add(new ListItem
                    {
                        Value = item.email.ToString(),
                        Text = String.Format("{0} {1}", item.username.ToString(), item.userid == currentUserid ? "(Me)" : "")

                    });
                }
            }
            return Json(userlist);
        }

        [HttpPost]
        public JsonResult RoomList()
        {
            List<RoomModel> rooms = dl.GetAvailableRooms();
            List<ListItem> roomList = new List<ListItem>();
            foreach (var item in rooms)
            {
                roomList.Add(new ListItem
                {
                    Value = item.roomId.ToString(),
                    Text = item.name.ToString()

                });
            }
            return Json(roomList);
        }





    }
}