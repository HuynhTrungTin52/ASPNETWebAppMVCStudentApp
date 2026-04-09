using Microsoft.Reporting.WebForms;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly SchoolDBEntities _context = new SchoolDBEntities();

        public ActionResult Index()
        {
            var model = new CourseViewModel
            {
                Courses = new SelectList(_context.Courses, "CourseID", "Title")
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Generate(int selectedCourseID, string action)
        {
            var reportPath = Path.Combine(HostingEnvironment.MapPath("~/Reports"), "StudentReport.rdlc");
            var localReport = new LocalReport { ReportPath = reportPath };
            var students = _context.Enrollments
                .Where(e => e.CourseID == selectedCourseID)
                .Select(e => new
                {
                    e.Student.StudentID,
                    e.Student.FirstName,
                    e.Student.LastName,
                    e.Grade
                }).ToList();

            localReport.DataSources.Add(new ReportDataSource("CourseStudentDataSet", students));
            
            var reportBytes = localReport.Render("PDF");
            return File(reportBytes, "application/pdf", "StudentReport.pdf");
        }
    }
}

namespace ASPNETWebAppMVCStudentApp.Models
{
    public class CourseViewModel
    {
        public int SelectedCourseID { get; set; }
        public SelectList Courses { get; set; }
    }
}