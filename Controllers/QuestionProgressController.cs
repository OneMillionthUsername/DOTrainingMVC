using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTrainingMVC.Controllers
{
    public class QuestionProgressController : Controller
    {
        static int QuestionNumber { get; set; }
        //public static int QuestionCounter { get; set; }
        static readonly Random Rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        
        //get the count of the views
        static int NumberOfQuestions = Directory.GetFiles("./Views/QuestionProgress").Where(fileName => fileName.Contains("Frage")).ToArray().Length;
        static bool IsRandom = false;

        public IActionResult Welcome()
        {
            QuestionNumber = 0; //reset
            IsRandom = false; //not really necessary, but just in case
            ViewData["ShowSkipButton"] = false; //hide skip button
            ViewBag.NumberOfQuestions = NumberOfQuestions;
            return View();
        }

        public IActionResult SetRandomQuestions()
        {
            IsRandom = true;
            return RedirectToAction(nameof(Frage));
        }

        public IActionResult Frage(bool? israndom, int qNum = 0)
        {
            //jump to question5

            if (qNum > 0)
            {
                QuestionNumber = qNum;
                IsRandom = false;
            }
            //Check if linear progression is requested and go to first question.
            if (israndom.HasValue && israndom == false)
            {
                IsRandom = false;
                QuestionNumber = 0;
            }
            if (!IsRandom && !(qNum > 0)) 
            {
                QuestionNumber++; //increment linear question progression
                if (QuestionNumber > NumberOfQuestions) //if overflow, reset
                {
                    return RedirectToAction(nameof(Welcome));
                }
            }
            else if(IsRandom)
            {
                QuestionNumber = Rnd.Next(1, NumberOfQuestions + 1); //update random question number
            }
            string viewName = $"Frage{QuestionNumber}";
            ViewData["ShowSkipButton"] = true;
            ViewBag.NumberOfQuestions = NumberOfQuestions;
            return View(viewName, QuestionNumber);
        }

        public IActionResult ValidateAnswers(string solutionString)
        {
            ViewData["ShowSkipButton"] = false; //hide skip button
            string[] words = solutionString.Split(' ');
            ViewBag.solutionString = solutionString.ToLower();
            return View();
        }
        [HttpGet]
        public IActionResult QuestionGenerator()
        {
            return View();
        }
        /// <summary>
        /// Erzeugt eine neue Ansicht und übergibt <paramref name="solutionTerms"/> (Lösungsarray), <paramref name="questionDescription"/> (die Fragenbeschreibung) und <paramref name="script"/> (das Lösungsscript) dem DOM.
        /// </summary>
        /// <param name="questionDescription"></param>
        /// <param name="script"></param>
        /// <param name="solutionTerms"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QuestionGenerator(string questionDescription, string script, string solutionTerms)
        {
            string[] solutionArray = solutionTerms.Split(',');
            string[] scriptArray = script.Split(' ', '\r', '\n', '\t'/*, '(', ')'*/);

            //Pfad + Dateiname für file create.
            string path = $"./Views/QuestionProgress/Frage{NumberOfQuestions + 1}.cshtml";

            //solution array trimmen und als string an validateForm übergeben
            string solutionAsParam = "[";
            for (int i = 0; i < solutionArray.Length; i++)
            {
                var temp = solutionArray[i].Trim();
                solutionAsParam += $"'{temp}',";
                solutionArray[i] = temp;
            }
            solutionAsParam = solutionAsParam.Substring(0, solutionAsParam.Length - 1) + "]";

            string fileContent = "";
            int j = 0;

            //header
            fileContent += "@model int\r\n" +
                "@{\r\n" +
                "\tViewData[\"Title\"] = \"Frage \" + Model;\r\n" +
                "}\r\n" +
                $"<h2>\r\n{questionDescription}\r\n" +
                " </h2>\r\n" +
                "@using (Html.BeginForm(\"ValidateAnswers\", \"QuestionProgress\", FormMethod.Get,"+
                $"new {{ onsubmit = \"return validateForm({solutionAsParam})\", autocomplete = \"off\" }}))\r\n" +
                "{<div class=\"form-group\">\r\n" +
                "<div id=\"solutionText\">\r\n";
            //header ende


            //body
            for (int i = 0; i < solutionArray.Length; i++)
            {
                while(j <= scriptArray.Length)
                {
                    //wenn Lösungswort im Script gefunden wurde
                    //erzeuge ein input element dafür. 
                    if (scriptArray[j] == solutionArray[i])
                    {
                        fileContent += $"\r\n<!--{solutionArray[i]}-->\r\n<br /><input type=\"text\" name=\"param{i}\" id=\"param{i}\" />\r\n";
                        j++;
                        break;
                    }
                    else if (string.IsNullOrEmpty(scriptArray[j]))
                    {
                        j++;
                        continue;
                    }
                    else
                    {
                        if (scriptArray[j].Contains('@'))
                        {
                            fileContent += $"<span>@{scriptArray[j]} </span>";
                        }
                        else
                        {
                            fileContent += $"<span>{scriptArray[j]} </span>";
                        }
                        j++;
                    }
                }
            }
            //body ende

            //footer
            fileContent += "</div>\r\n" +
                "<input type=\"hidden\" name=\"solutionString\" id =\"solutionString\"/>\r\n" +
                "<input type=\"submit\" class=\"btn-sm btn-primary\" value=\"Check\"/>\r\n</div>\r\n}";
            //footer ende

            //lösche den Inhalt, vllt um files zu überschreiben, die bereits existieren.
            if (System.IO.File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Truncate))
                {
                    // Der FileStream löscht den Inhalt automatisch.
                } 
            }
            //create view file
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                
                fs.Write(Encoding.ASCII.GetBytes(fileContent));
            }
            NumberOfQuestions++;
            ViewBag.NumberOfQuestions = NumberOfQuestions;

            return RedirectToAction("Frage");
        }
    }
}
