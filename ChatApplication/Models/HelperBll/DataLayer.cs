using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
namespace ChatApplication.Models.HelperBll
{
    public class DataLayer
    {
        public DataLayer()
        {
        }
        public UserModel login(string email,string password)
        {
            UserModel user = new UserModel();
            ChatDataEntities dbContext = new ChatDataEntities();
            User userFromDB = dbContext.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
            if (userFromDB != null)
            { 
                user.userid = userFromDB.UserId;
                user.email = userFromDB.Email;
                user.mobile = userFromDB.Mobile;
                user.password = userFromDB.Password;
                user.username = userFromDB.Username;
            }
            return user;
        }
        public List<UserModel> GetConnectedUsers(int roomId)
        {
            List<UserModel> userlist = new List<UserModel>();
            ChatDataEntities dbContext = new ChatDataEntities();
            List<User> usersFromDB = dbContext.Users.Where(x => x.ChatRoom == roomId).ToList();
            foreach(User userFromDB in usersFromDB)
            {
                UserModel user = MapUserToModel(userFromDB);
                userlist.Add(user);
            }
            return userlist;
        }

        public void SwitchToRoom(int userId, int roomId)
        {
            if (userId == 0) return;
            ChatDataEntities dbContext = new ChatDataEntities();
            dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault().ChatRoom = roomId;
            dbContext.SaveChanges();
        }

        public List<RoomModel> GetAvailableRooms()
        {
            List<RoomModel> roomList = new List<RoomModel>();
            ChatDataEntities dbContext = new ChatDataEntities();
            List<ChatRoom> roomsFromDB = dbContext.ChatRooms.ToList();
            foreach (ChatRoom roomFromDB in roomsFromDB)
            {
                RoomModel room = new RoomModel();
                room.roomId = roomFromDB.Id;
                room.name = roomFromDB.Name;
                roomList.Add(room);

            }
            return roomList;
        }

        internal UserModel GetUserById(int userId)
        {
            List<UserModel> userlist = new List<UserModel>();
            ChatDataEntities dbContext = new ChatDataEntities();
            User userFromDB = dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
            return MapUserToModel(userFromDB);
        }

        private UserModel MapUserToModel (User userFromDB)
        {
            UserModel user = new UserModel();
            user.userid = userFromDB.UserId;
            user.email = userFromDB.Email;
            user.mobile = userFromDB.Mobile;
            user.password = userFromDB.Password;
            user.dob = userFromDB.DOB;
            user.username = userFromDB.Username;
            return user;
        }
    }
}