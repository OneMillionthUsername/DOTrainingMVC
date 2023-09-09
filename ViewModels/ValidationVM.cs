using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTrainingMVC.ViewModels
{
    public class ValidationVM
    {
        public ValidationVM()
        {
            Results = new();
        }
        public Dictionary<string, string> Results { get; set; }
    }
}
