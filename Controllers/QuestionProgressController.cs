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
            else if (IsRandom)
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
        /// Erzeugt eine neue Ansicht mit den gewünschten <paramref name="solutionTerms"/> (Liste der Lösungswörter),
        /// der <paramref name="questionDescription"/> (Frage) und dem <paramref name="script"/> (das vorgefertigte Script, worin die Lösungswörter zu Eingabefeldern werden).
        /// </summary>
        /// <param name="questionDescription"></param>
        /// <param name="script"></param>
        /// <param name="solutionTerms"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QuestionGenerator(string questionDescription, string script, string solutionTerms)
        {
            //init
            string[] solutionTermsArray = solutionTerms.Split(',');
            script = script.Replace("\r\n", "<br />");
            string[] scriptLines = script.Split("<br />");
            string solution = "";
            string fileContent = "";
            string path = $"./Views/QuestionProgress/Frage{NumberOfQuestions + 1}.cshtml";

            //solution array trimmen und als list-string an validateForm übergeben
            string solutionAsParamsList = GetParamsListForJS(solutionTermsArray);

            //generate header HTML
            fileContent += "@model int\r\n" +
                "@{\r\n" +
                "\tViewData[\"Title\"] = \"Frage \" + Model;\r\n" +
                "}\r\n" +
                $"<h2>\r\n{questionDescription.Trim()}\r\n</h2>\r\n" +
                "@using (Html.BeginForm(\"ValidateAnswers\", \"QuestionProgress\", FormMethod.Get," +
                $"new {{ onsubmit = \"return validateForm({solutionAsParamsList})\", autocomplete = \"off\" }}))\r\n" +
                "{\r\n<div class=\"form-group\">\r\n<div id=\"solutionText\">\r\n";
            //header ende

            //generate body HTML
            int solutionTermNumber = 0;
            for (int i = 0; i < solutionTermsArray.Length; i++)
            {
                solutionTermsArray[i] = solutionTermsArray[i].Trim().ToLower();
            }

            bool setSpan = false;
            
            //main loop - loop all scriptlines and manipulate them, create the HTML DOM.
            for (int i = 0; i < scriptLines.Length; i++)
            {
                char[] specialChars = { '(', ')', ';', ',', '.' };
                //check every script line for special characters
                for (int j = 0; j < specialChars.Length; j++)
                {
                    if (string.IsNullOrEmpty(scriptLines[i])) continue;

                    if (scriptLines[i].Contains(specialChars[j]))
                    {
                        int specialCharsCount = scriptLines[i].Count(c => c == specialChars[j]);
                        int count = 0;
                        int charLoop = 0;

                        //check every single character to catch multiple occurences.
                        //maybe use recursion instead?
                        while (count < specialCharsCount)
                        {
                            if (scriptLines[i][charLoop] == specialChars[j])
                            {
                                scriptLines[i] = scriptLines[i].Remove(charLoop) + ' ' + scriptLines[i].Substring(charLoop, 1) + ' ' + scriptLines[i].Substring(charLoop + 1);
                                count++;
                                charLoop++; //weil derIndex des Chars um 1 nach rechts verschoben wird, muss danach +2 vom gegenwärtigen Index aus gesucht werden.
                            }
                            charLoop++;
                        }
                    }
                }

                //gehe jedes Wort in einer Zeile durch und bestimme seinen synatktischen Platz im HTML DOM.
                string[] scriptLine = scriptLines[i].Split(' ');
                int wordNumber = 0;
                while (wordNumber < scriptLine.Length)
                {
                    if (solutionTermNumber < solutionTermsArray.Length && scriptLine[wordNumber].ToLower().Trim() == solutionTermsArray[solutionTermNumber])
                    {
                        if (setSpan)
                        {
                            solution += "</span>";
                            setSpan = false;
                        }
                        if (solutionTermsArray[solutionTermNumber] == ">=")
                        {
                            solutionTermsArray[solutionTermNumber] = "&gt=";
                        }
                        if (solutionTermsArray[solutionTermNumber] == "<=")
                        {
                            solutionTermsArray[solutionTermNumber] = "&lt=";
                        }
                        solution += $"\r\n<!--{solutionTermsArray[solutionTermNumber]}-->\r\n<input type=\"text\" name=\"param{solutionTermNumber}\" id=\"param{solutionTermNumber}\" />\r\n";
                        wordNumber++;
                        solutionTermNumber++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(scriptLine[wordNumber]))
                    {
                        wordNumber++;
                        continue;
                    }
                    else
                    {
                        if (scriptLine[wordNumber].Contains('@'))
                        {
                            var index = scriptLine[wordNumber].IndexOf('@');
                            var doubleAtString = scriptLine[wordNumber].Remove(index) + '@' + scriptLine[wordNumber].Substring(index);
                            if (setSpan)
                            {
                                solution += $"{doubleAtString}"; 
                            }
                            else
                            {
                                solution += $"<span> {doubleAtString}"; 
                                setSpan = true;
                            }
                        }
                        else
                        {
                            if (setSpan)
                            {
                                solution += $" {scriptLine[wordNumber]} ";
                            }
                            else
                            {
                                solution += $"<span> {scriptLine[wordNumber]} ";
                                setSpan = true;
                            }
                            //if keyword, color!
                        }
                        wordNumber++;
                    }
                }

                //line endings
                if (setSpan)
                {
                    solution += "</span><br />";
                    setSpan = false;
                }
                else
                {
                    solution += "<br />";
                }
            }
            fileContent += solution;
            //body ende

            //generate footer HTML
            fileContent += "\r\n</div>\r\n" +
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

        private static string GetParamsListForJS(string[] solutionArray)
        {
            string solutionAsParamsList = "[";
            for (int i = 0; i < solutionArray.Length; i++)
            {
                var temp = solutionArray[i].Trim();
                solutionAsParamsList += $"'{temp}',";
                solutionArray[i] = temp;
            }
            solutionAsParamsList = solutionAsParamsList.Substring(0, solutionAsParamsList.Length - 1) + "]";
            return solutionAsParamsList;
        }
    }
}
