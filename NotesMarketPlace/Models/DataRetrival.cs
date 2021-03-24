using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class DataRetrival
    {
        public Database.NoteCategory NoteCategory { get; set; }
        public Database.User User { get; set; }
        public Database.NoteType NoteType { get; set; }
        public Database.Country Country { get; set; }
    }
}