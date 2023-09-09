using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DOTrainingMVC.Controllers
{
    public class QuestionProgressController : Controller
    {
        public static int QuestionNumber { get; set; }
        private static Random Rnd = new Random();
        //private static int NumberOfQuestions = Directory.EnumerateFiles("~/Views/QuestionProgress/").Where(fileName => fileName.Contains("Frage")).ToList().Count();
        
        private static int NumberOfQuestions = Directory.GetFiles("./Views/QuestionProgress").Where(fileName => fileName.Contains("Frage")).ToArray().Length;
        public static bool IsRandom = false;

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult SetRandomQuestions()
        {
            IsRandom = true;
            QuestionNumber = Rnd.Next(1, NumberOfQuestions + 1); //random question number
            return RedirectToAction("Frage" + QuestionNumber);
        }

        public IActionResult Frage1()
        {
            if (!IsRandom) //possible bug -> url injection failing IsRandom logic.
            {
                QuestionNumber = 1;
            }
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
            if (IsRandom)
            {
                QuestionNumber = Rnd.Next(1, NumberOfQuestions + 1); //random question number
            }
            else
            {
                QuestionNumber++; //raise question counter
            }
            return View(QuestionNumber);
        }
    }
}
