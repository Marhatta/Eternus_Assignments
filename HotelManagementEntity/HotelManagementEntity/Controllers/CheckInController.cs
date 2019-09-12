using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagementEntity.Models;

namespace HotelManagementEntity.Controllers
{
    public class CheckInController : Controller
    {
        private HotelEntities7 db = new HotelEntities7();

        // GET: /CheckIn/
        public ActionResult Index(string name)
        {
            if (name == null)
            {
                return View(db.CheckIns.ToList());

            }
            return View(db.CheckIns.Where(c => c.GuestName == name).ToList());

        }

        // GET: /CheckIn/Details/5
        public ActionResult Details(int? id)
        {
            CheckIn checkin = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach(var x in db.CheckIns)
            {
                if(x.GuestId==id)
                {
                    checkin = x;
                }
            }
            if (checkin == null)
            {
                return HttpNotFound();
            }
            return View(checkin);
        }

        // GET: /CheckIn/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CheckIn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CheckIn checkin)
        {
            if (ModelState.IsValid)
            {
                //check if rooms are available , then check in

                //find Room
                try
                {
                    var currentRoom = db.Rooms.Find(checkin.RoomId);
                    ViewBag.RoomsAvailable =  currentRoom.NoOfRooms + " rooms available";
                     
                    //check if room is available
                    if (currentRoom.NoOfRooms >= checkin.Quantity)
                    {
                        try
                        {
                            checkin.Status = "CheckedIn";
                            db.CheckIns.Add(checkin);
                            currentRoom.NoOfRooms -= checkin.Quantity;
                        
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            ViewBag.CannotAddRoom = e;
                        }
                       
                    }
                    return View();
               
                }
                catch(Exception e)
                {
                    ViewBag.CannotFindRoom = e;
                }
               
            }

            return View(checkin);
        }

        // GET: /CheckIn/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkin = db.CheckIns.Find(id);
            if (checkin == null)
            {
                return HttpNotFound();
            }
            return View(checkin);
        }

        // POST: /CheckIn/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RoomId,RoomType,GuestName,Address,Contact,Quantity,CheckInTime,Status")] CheckIn checkin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checkin);
        }

        // GET: /CheckIn/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkin = db.CheckIns.Find(id);
            if (checkin == null)
            {
                return HttpNotFound();
            }
            return View(checkin);
        }

        // POST: /CheckIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                 //find checkin with given id
                CheckIn checkin = db.CheckIns.Find(id);
                List<CheckIn> guestList = new List<CheckIn>();
                guestList = db.CheckIns.ToList();
                int guestroomid = 0;
                foreach(var x in guestList)
                {
                    if(x.GuestId==id)
                    {
                        guestroomid = x.RoomId;
                    }
                }
                //Find current room and add back the released rooms by guest
                Room room = db.Rooms.Find(guestroomid);

                if (checkin.Status.Contains("CheckedIn")) //if checked in , no need to vacant the rooms
                {
                    room.NoOfRooms += checkin.Quantity;
                }
                //remove the guest and release the room
                //db.CheckIns.Remove(checkin);
                checkin.Status = "CheckedOut";
                //save the changes to db
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       
    }
}
