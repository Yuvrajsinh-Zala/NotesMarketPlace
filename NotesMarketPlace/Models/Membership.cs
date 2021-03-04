using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String EmailID { get; set; }
        public string Password { get; set; }

        public string RePassword { get; set; }

    }
}