using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTrainingMVC.Controllers
{
    public class QuestionProgressController : Controller
    {
        public int QuestionNumber { get; set; }
        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Frage()
        {
            ViewBag.QNumber = 1;
            return View();
        }

        [HttpGet]
        public IActionResult Frage2(int questionNumber)
        {
            ViewBag.QNumber = questionNumber;
            return View();
        }

        [HttpGet]
        public IActionResult Frage3(int questionNumber)
        {
            ViewBag.QNumber = questionNumber;
            return View();
        }

        [HttpPost]
        public IActionResult ValidateAnswers(string param1, string param2, int questionNumber, string returnUrl)
        {
            Dictionary<string, string> routeValues = new();
            questionNumber++;
            routeValues.Add("questionNumber", questionNumber.ToString());
            routeValues.Add("nextPage", "Frage" + questionNumber);
            routeValues.Add("result", param1 + " * FROM " + param2);
            routeValues.Add("returnUrl", returnUrl);
            return View(routeValues);
        }
    }
}
