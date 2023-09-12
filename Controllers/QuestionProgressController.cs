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
        static int QuestionNumber { get; set; }
        //public static int QuestionCounter { get; set; }
        static readonly Random Rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        
        //get the count of the views
        static readonly int NumberOfQuestions = Directory.GetFiles("./Views/QuestionProgress").Where(fileName => fileName.Contains("Frage")).ToArray().Length;
        static bool IsRandom = false;

        public IActionResult Welcome()
        {
            QuestionNumber = 0; //reset
            IsRandom = false; //not really necessary, but just in case
            ViewData["ShowSkipButton"] = false; //hide skip button
            return View();
        }

        public IActionResult SetRandomQuestions()
        {
            IsRandom = true;
            return RedirectToAction(nameof(Frage));
        }

        public IActionResult Frage(bool? israndom)
        {
            //Check if linear progression is requested.
            if (israndom.HasValue && israndom == false)
            {
                IsRandom = false;
                QuestionNumber = 0;
            }
            if (!IsRandom) 
            {
                QuestionNumber++; //increment linear question progression
                if (QuestionNumber > NumberOfQuestions) //if overflow, reset
                {
                    return RedirectToAction(nameof(Welcome));
                }
            }
            else
            {
                QuestionNumber = Rnd.Next(1, NumberOfQuestions + 1); //update random question number
            }
            string viewName = $"Frage{QuestionNumber}";
            ViewData["ShowSkipButton"] = true;
            return View(viewName, QuestionNumber);
        }

        public IActionResult ValidateAnswers(string solutionString)
        {
            ViewData["ShowSkipButton"] = false; //hide skip button
            string[] words = solutionString.Split(' ');
            //foreach (var item in Enum.GetNames(typeof(SQLBlueKeywords)))
            //{
            //    for (int i = 0; i < words.Length; i++)
            //    {
            //        if (true)
            //        {

            //        }
            //    }
            //}
            ViewBag.solutionString = solutionString.ToLower();
            return View();
        }
        [HttpGet]
        public IActionResult QuestionGenerator()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QuestionGenerator(string questionDescription, string script, string solutionTerms)
        {
            //erstelle eine neue View
            //mit <form>
            return View();
        }
    }
}
