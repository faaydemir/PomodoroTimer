//code from  https://github.com/Microsoft/SmartHotel360-mobile-desktop-apps
namespace PomodoroTimer.Validations
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}
