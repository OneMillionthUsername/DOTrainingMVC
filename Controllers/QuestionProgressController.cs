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
        private static int QuestionNumber { get; set; }
        public static int QuestionCounter { get; set; }
        private static Random Rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        
        //get the count of the views
        private static int NumberOfQuestions = Directory.GetFiles("./Views/QuestionProgress").Where(fileName => fileName.Contains("Frage")).ToArray().Length;
        private static bool IsRandom = false;

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult SetRandomQuestions()
        {
            IsRandom = true;
            QuestionNumber = Rnd.Next(1, NumberOfQuestions + 1); //generate first random question number
            return RedirectToAction(nameof(Frage));
        }

        public IActionResult Frage()
        {
            QuestionCounter++;
            if (!IsRandom && QuestionNumber <= NumberOfQuestions) 
            {
                QuestionNumber++; //increment linear question progression
            }
            if (IsRandom)
            {
                QuestionNumber = Rnd.Next(1, NumberOfQuestions + 1); //update random question number
            }
            string viewName = $"Frage{QuestionNumber}";
            return View(viewName, QuestionNumber);
        }

        public IActionResult ValidateAnswers()
        {
            return View();
        }
    }
}
