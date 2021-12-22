using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hospital.Models;
using System.Web.Mvc;
using System.Globalization;

namespace hospital.Controllers
{

    public class HomeController : Controller
    {
        
        hdata context = new hdata();
       
        // GET: Home
        public ActionResult Index()
        {
            int docs = context.tbl_Doctors.Count();
            int dep = context.tbl_Skills.Count();
            ViewBag.docs = docs;
            ViewBag.dep = dep;
            return View();
        }

        public ActionResult getdep()
        {
            var dep = context.tbl_Skills.Select(x => new { x.pkID, x.Skill }).ToList();


            return Json(dep, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getdocs(int dep)
        {

            var docs = context.tbl_Doctors.Where(x => x.fkSkill == dep).Select(x => new { x.pkID, x.Name, x.Family }).ToList();
            return Json(docs, JsonRequestBehavior.AllowGet);


        }


        public ActionResult getvisit(int doc)
        {
            var visit = context.View_Visit.Where(x => x.fkDocID == doc && x.fkPID == null).Select(x => new { x.pkID, x.PDate }).ToList();
            return Json(visit, JsonRequestBehavior.AllowGet);
        }


        public ActionResult setvisit(int vn, string namee, string phone, string family)
        {
            var c = context.tbl_Visit.Where(x => x.pkID == vn).SingleOrDefault();
            int statee = 0;
            if (c.fkPID == null)

            {
                int pid = 0;
                var p = context.tbl_Patient.Where(x => x.Mobile == phone).SingleOrDefault();

                if (p == null)
                {
                    tbl_Patient newp = new tbl_Patient();
                    newp.Name = namee;
                    newp.Family = family;
                    newp.Mobile = phone;

                    context.tbl_Patient.Add(newp);
                    context.SaveChanges();
                    var np = context.tbl_Patient.Where(x => x.Mobile == phone).SingleOrDefault();
                    pid = np.pkID;
                }
                else
                {
                    pid = p.pkID;
                }
                var v = context.tbl_Visit.Where(x => x.pkID == vn).SingleOrDefault();
                v.fkPID = pid;
                v.fkVTID = 1;
                v.EDate = v.SDate.AddMinutes(20);

                context.SaveChanges();
                statee = 2; //نوبت دهی انجام شد
            }
            else
            {
                statee = 1; //نوبت تمام شده
            }
            return Json(statee, JsonRequestBehavior.AllowGet);
        }



        public ActionResult recept()
        {
            ViewBag.title = "داشبرد";
            return View();
        }
        public ActionResult visit_management()
        {
            ViewBag.title = "مدیریت نوبتها";
            return View();
          }

        
    }

}

        

            




            