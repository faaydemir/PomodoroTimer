using SmartHotel.Clients.Core.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Validations
{
    public class IntergerBoundValidationController
    {
        public static (bool result, string message) Check(int downLimit, int upLimit, int value, string paramaterName)
        {
            var intergerBoundValidation = new IntergerBoundValidation(upLimit, downLimit, paramaterName);
            if (intergerBoundValidation.Check(value))
            {
                return (result: true, message: "");
            }
            else
            {
                return (result: false, message: intergerBoundValidation.ValidationMessage);
            }
        }
    }
    public class IntergerBoundValidation : IValidationRule<int>
    { 
        private int UpLimit;
        private int DownLimit;

        public IntergerBoundValidation(int upLimit, int downLimit, string paramaterName)
        {
            UpLimit = upLimit;
            DownLimit = downLimit;
            ValidationMessage = $"{paramaterName} should be between {UpLimit} and {DownLimit} ";
        }

        public string ValidationMessage { get; set; }

        public bool Check(int value)
        {
            if (value <= UpLimit && value >= DownLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
