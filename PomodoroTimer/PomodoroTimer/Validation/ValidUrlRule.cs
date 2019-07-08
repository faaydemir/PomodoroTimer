using System.ComponentModel.DataAnnotations;
//code from  https://github.com/Microsoft/SmartHotel360-mobile-desktop-apps
namespace PomodoroTimer.Validations
{
    public class ValidUrlRule : IValidationRule<string>
    {
        public ValidUrlRule()
        {
            ValidationMessage = "Should be an URL";
        }
        
        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            return false;
        }
    }
}
