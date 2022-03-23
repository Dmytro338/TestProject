using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Announcement;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace Announcements.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        public static List<Annoncement> announcements = new List<Annoncement>
        {
            new Annoncement {
              Id = 1,
              Title = "Online Skin, Hair and Nutrition Store",
              Description = "The Largest Skin, Hair  Nutrition Online Store in India with over 900 Brands. Shop online for Cosmeceuticals, Dermacare Nutraceutical Products.",
              DateAdded = new DateTime(2015, 7, 20)
             },
            new Annoncement {
              Id = 2,
              Title = "Online Skin, Hair and Nutrition Store",
              Description = "The Largest Skin, Hair  Nutrition Online Store in India with over 900 Brands. Shop online for Cosmeceuticals, Dermacare Nutraceutical Products.",
              DateAdded = new DateTime(2015, 7, 20)
             },
            new Annoncement {
              Id = 3,
              Title = "Online Skin",
              Description = "The werwer erwerwe",
              DateAdded = new DateTime(2015, 7, 20)
             },
            new Annoncement {
              Id = 4,
              Title = "Online Skin, Hair and Nutrition Store",
              Description = "The Largest Skin, Hair  Nutrition Online Store in India with over 900 Brands. Shop online for Cosmeceuticals, Dermacare Nutraceutical Products.",
              DateAdded = new DateTime(2015, 7, 20)
             },
            new Annoncement {
              Id = 5,
              Title = "Online Skin, Hair and Nutrition Store",
              Description = "The Largest Skin, Hair  Nutrition Online Store in India with over 900 Brands. Shop online for Cosmeceuticals, Dermacare Nutraceutical Products.",
              DateAdded = new DateTime(2015, 7, 20)
             }
        };

        [HttpGet("Announcements")]
        public ActionResult<List<Annoncement>> GetAnnouncements()
        {
            return Ok(announcements);
        }

        [HttpGet("Announcement/{id}")]
        public ActionResult<List<Annoncement>> GetAnnouncement(int id)
        {
            var announcment = announcements.Find(a => a.Id == id);
            if (announcment == null)
                return BadRequest("Announcement not found");
            return Ok(announcment);
        }

        [HttpPost("AddAnnouncement")]
        public ActionResult<List<Annoncement>> AddAnnouncement(Annoncement announcementModel)
        {
            if (announcementModel != null)
                announcements.Add(announcementModel);

            return Ok(announcements);
        }

        [HttpPut]
        [Route("UpdateAnnouncement")]
        public ActionResult<List<Annoncement>> UpdateAnnouncement(Annoncement announcementModel)
        {
            var announcment = announcements.Find(a => a.Id == announcementModel.Id);
            if (announcment == null)
                return BadRequest("Announcement not found");
            announcment.Title = announcementModel.Title;
            announcment.Description = announcementModel.Description;
            announcment.DateAdded = announcementModel.DateAdded;

            return Ok(announcment);
        }



        [HttpDelete]
        [Route("Announcement/{id}")]
        public ActionResult<List<Annoncement>> DeleteAnnouncement(int id)
        {
            var announcment = announcements.Find(a => a.Id == id);
            if (announcment == null)
                return BadRequest("Announcement not found");

            announcements.Remove(announcment);
            return Ok(announcements);
        }


        [HttpGet]
        [Route("SimilarAnnouncements")]
        public ActionResult<List<Annoncement>> SimilarAnnouncements(int id)
        {
            List<AnnouncementToWords> topAnnouncement = new List<AnnouncementToWords>();
            var announcmentToCompare = announcements.Find(a => a.Id == id);
            if (announcmentToCompare == null)
                return BadRequest("Announcement not found");

            foreach (Annoncement announcement in announcements)
            {
                topAnnouncement.Add(new AnnouncementToWords
                {
                    WordsNum = CountSame(GetWords(ConcatText(announcement)), GetWords(ConcatText(announcmentToCompare))),
                    announcementModel = announcement
                });
            }

            return topAnnouncement.Where(a => a.announcementModel.Id != id)
                .OrderByDescending(t => t.WordsNum)
                .Take(3).Select(t => t.announcementModel)
                .ToList();
        }

        private static string ConcatText(Annoncement anounc)
        {
            return anounc.Title + " " + anounc.Description;
        }

        private static int CountSame(List<string> w1, List<string> w2)
        {
            return w1.Intersect(w2).Count();
        }

        private static List<string> GetWords(string str)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(str, "").ToLower().Split(" ").ToList();
        }
    }
}
