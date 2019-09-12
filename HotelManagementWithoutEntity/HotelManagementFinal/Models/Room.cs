using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManagementFinal.Models
{
    public class Room
    {
        [Required]
        [DisplayName("Enter Room No.")]
        public int RoomId { get; set; }
        [Required]
        [DisplayName("Enter Room Type")]
        public string RoomType { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "No of must be greater than 0")]
        [DisplayName("Enter No. of Rooms")]
        public int NoOfRooms { get; set; }
        [Required]
        [Range(1,10000000,ErrorMessage="Price must be greater than 0")]
        [DisplayName("Enter Price")]
        public int Price { get; set; }
    }

    public class CheckIn:Room
    {

        [Required]
        public int GuestId { get; set; }
        [Required]
        public string GuestName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "Days must be greater than 0")]
        public int NoOfDays { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

    }
}