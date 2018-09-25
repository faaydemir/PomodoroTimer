using System.Linq;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PomodoroTimer.Models;
using PomodoroTimer.Views;
using PomodoroTimer.Messaging;
using PomodoroTimer.Services;

namespace PomodoroTimer.ViewModels
{
    public class TasksViewModel : PageViewModel
    {
        private ObservableCollection<UserTask> _userTask;
        private IAppService AppService { get; set; }

        public ObservableCollection<UserTask> UserTasks
        {
            get
            {
                return _userTask;
            }
            set
            {
                SetProperty(ref _userTask, value);
            }
        }
        public Command LoadItemsCommand { get; set; }
        public Command EditItemCommand { get; set; }
        public Command DeleteItemCommand { get; set; }

        public TasksViewModel(IAppService appService) : this()
        {
            AppService = appService;
        }

        public TasksViewModel()
        {
            Title = "Tasks";
            UserTasks = new ObservableCollection<UserTask>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            EditItemCommand = new Command(async (item) => await EditItem(item));
            DeleteItemCommand = new Command((item) => DeleteItem(item));

            MessagingCenter.Subscribe<NewTaskPageViewModel, UserTask>(this, AppMessages.SaveUserTaskMessage, async (obj, item) =>
            {
                var _item = item as UserTask;
                if (item != null)
                {
                    bool result = await AppService.AddNewUserTask(_item);
                    if (result)
                    {
                        UserTasks = new ObservableCollection<UserTask>(AppService.UserTasks);
                    }
                }
            });
        }

        private Task EditItem(object item)
        {
            return Page.Navigation.PushModalAsync(new NavigationPage(new NewItemPage(item as UserTask)));
        }

        private async void DeleteItem(object item)
        {
            var displayAlert = new DialogService(Page);
            var changeTask = await displayAlert.DisplayAlert("Delete Task", "Task will be deleted. Did you want to continue.", "ok", "cancel");
            if (!changeTask)
                return;

            var deletedTask = item as UserTask;
            var result = await AppService.RemoveUserTask(deletedTask);
            if (result)
            {
                UserTasks.Remove(deletedTask);
            }
        }

        private Task ExecuteLoadItemsCommand()
        {
            return Task.Run(() =>
             {
                 if (IsBusy)
                     return;
                 IsBusy = true;
                 UserTasks.Clear();
                 if (AppService == null || AppService.UserTasks == null)
                 {
                     IsBusy = false;
                     return;
                 }
                 foreach (var userTask in AppService.UserTasks)
                 {
                     UserTasks.Add(userTask);
                 }
                 IsBusy = false;
             });
        }
    }
}