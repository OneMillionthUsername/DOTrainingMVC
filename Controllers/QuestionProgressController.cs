using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTrainingMVC.Controllers
{
    public class QuestionProgressController : Controller
    {
        public static int QuestionNumber { get; set; }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Frage()
        {
            QuestionNumber = 1;
            return View(QuestionNumber);
        }

        public IActionResult Frage2()
        {
            return View(QuestionNumber);
        }

        public IActionResult Frage3()
        {
            return View(QuestionNumber);
        }

        public IActionResult Frage4()
        {
            return View(QuestionNumber);
        }

        public IActionResult Frage5()
        {
            return View(QuestionNumber);
        }

        public IActionResult Frage6()
        {
            return View(QuestionNumber);
        }

        public IActionResult ValidateAnswers()
        {
            QuestionNumber++; //question counter raised
            return View(QuestionNumber);
        }
    }
}
