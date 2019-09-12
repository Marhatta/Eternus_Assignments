using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagementFinal.Models;

namespace HotelManagementFinal.Controllers{

    public class Default1Controller : Controller{
        static int RoomId = 0;
        static int guestId = 0;
        Room r = new Room();
        CheckIn c = new CheckIn();
        static List<Room> RoomList = new List<Room>();
        static List<CheckIn> CheckedIn = new List<CheckIn>();
   
        
        // GET: /Default1/

        public ActionResult Index()
        {
            ViewBag.Roomlist = RoomList;
            return View();
        }

        
        // GET: /Default1/Create

        public ActionResult Create()
        {
            return View();
        }

       
        // POST: /Default1/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            //if any modal errors have been set to modal state
            if (ModelState.IsValid)
            {
                RoomId += 1;
                room.RoomId = RoomId;
                RoomList.Add(room);
                
                return RedirectToAction("Index");
            }
            return View(room);

    
        }

        //
        // GET: /Default1/Edit/5

        public ActionResult Edit(int id)
        {
           // Room room = db.Rooms.Find(id);
            var RoomNo = RoomList.Where(room => id == room.RoomId);
            if (RoomNo == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                Room r = RoomList.Single(c => c.RoomId == id);
                r.RoomType = collection["RoomType"];
                r.NoOfRooms = Convert.ToInt32(collection["NoOfRooms"]);
                r.Price = Convert.ToInt32(collection["Price"]);

                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Default1/Delete/5

        public ActionResult Delete(int id )
        {
            return View();
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,FormCollection collection)
        {
                Room r = RoomList.Single(c => c.RoomId == id);
                foreach (var guest in CheckedIn)
                {
                    if (guest.Status == "CheckedIn" && guest.RoomId == id)
                    {
                        TempData["CannotDeleteRoom"] = "SomeOne is checked in to the Room.Cannot Delete.....";
                        return RedirectToAction("Index");
                    }
                }
                 RoomList.Remove(r);
                 return RedirectToAction("Index");

            }


        //========================Check In Actions====================================
        public ActionResult CheckIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                //1. standard 2. Deluxe 3.supreme
                var currentRoom = RoomList.Single(c => c.RoomId == checkIn.RoomId);
                if (Convert.ToInt32(currentRoom.NoOfRooms) >= checkIn.NoOfRooms)
                {
                    guestId += 1;
                    checkIn.GuestId = guestId;
                    checkIn.Status = "CheckedIn";
                    CheckedIn.Add(checkIn);
                    currentRoom.NoOfRooms -= checkIn.NoOfRooms;
                }
                return RedirectToAction("CheckInDetails");
            }
            return View();
          
        }

        public ActionResult Checkout(Int32 id)
        {
           var guest = CheckedIn.Single(c => c.GuestId == id);
           guest.Status = "CheckedOut";
           var room = RoomList.Single(r => r.RoomId == guest.RoomId);
           room.NoOfRooms += guest.NoOfRooms;
           
            return RedirectToAction("CheckInDetails");
        }

        public ActionResult CheckInDetails()
        {
            ViewBag.CheckedIn = CheckedIn;
            return View();
        }

        public ActionResult SearchCheckedInUsers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchCheckedInUsers(FormCollection collection)
        {
            var foundUsers = CheckedIn.FindAll(c => c.GuestName.Contains(collection["nameVal"]));
            ViewBag.foundUsers = foundUsers.ToList();
            return View("FoundUsers");
        }

       
           
        
    }
}