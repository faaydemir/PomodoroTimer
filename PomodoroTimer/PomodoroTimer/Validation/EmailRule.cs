using PomodoroTimer.Validations;
using System.Text.RegularExpressions;

namespace PomodoroTimer.Validations
{

    public class EmailValidationController
    {
        public static (bool result, string message) Check(string email)
        {
            var emailRule = new EmailRule();
            if (emailRule.Check(email))
            {
                return (result: true, message: "");
            }
            else
            {
                return (result: false, message: emailRule.ValidationMessage);
            }
        }
    }
    //code from  https://github.com/Microsoft/SmartHotel360-mobile-desktop-apps
    public class EmailRule : IValidationRule<string>
    {
        public EmailRule()
        {
            ValidationMessage = "Should be an email address";
        }

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(str);

            return match.Success;
        }
    }
}
