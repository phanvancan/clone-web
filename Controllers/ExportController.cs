using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spire.Xls;
//using Spire.Xls.Core;

namespace downloadconvert.Controllers
{
    public class ExportController : Controller
    {
        public Workbook Workbook;
        public Worksheet Worksheet;

        //public SpireProcess(string fullPathFile)
        //{
          //  Active(fullPathFile);
       // }


        private void Active(string fullPathFile)
        {
            this.Workbook = new Workbook();
            this.Workbook.LoadFromFile(fullPathFile);
            this.Worksheet = Workbook.Worksheets[0];
        }

        // GET: ExportController
        public ActionResult Index()
        {
            string fullPathFile = @"C:\ADM\code\temviet_code_server\server\InvoiceServer.API\Data\Asset\report-Customer.xlsx";
            Active(fullPathFile);
            
            Worksheet sheet = Workbook.Worksheets.Add("AddedSheet");
            sheet.Range["C5"].Text = "This is a new sheet.";


            //Save and Launch
            Workbook.SaveToFile("Output.xlsx", ExcelVersion.Version2010);

            ExcelDocViewer("Output.xlsx");

            return Content(fullPathFile);
        }

        private void ExcelDocViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }
        }

        // GET: ExportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
