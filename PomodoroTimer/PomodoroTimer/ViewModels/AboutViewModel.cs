using System;
using System.Windows.Input;
using System.Collections;
using Xamarin.Forms;
using System.Collections.Generic;
using PomodoroTimer.Models;
namespace PomodoroTimer.ViewModels
{



    public class AboutViewModel : PageViewModel
    {
        public string AppDetail { get; set; } = "";
        public string AppName { get; set; } = "Pomodoro Timer";
        public string AppVersion { get; set; } = "0.1";
        public string AppIcon { get; set; } = "icon.png";
        public List<ItemInfo> Contacts { get; set; }
        public List<ItemInfo> Libraries { get; set; }

        private string MailAddres = "mailto:firakti@outlook.com?";
        private string GithubUrl = "https://github.com/firakti";
        private string RedditUrl = "";

        public ICommand OpenWebCommand { get; set; }
        public AboutViewModel()
        {
            Title = "About";
            Contacts = new List<ItemInfo>
            {
                new ItemInfo()
                {
                    Details="",
                    Icon="mail.png",
                    Title="Mail",
                    Url=MailAddres,
                },

                new ItemInfo()
                {
                    Icon="github.png",
                    Title="Github",
                    Url=GithubUrl,
                },
                //new ItemInfo()
                //{
                //    Icon="reddit.png",
                //    Title="Reddit",
                //    Url=RedditUrl,
                //}
            };

            Libraries = new List<ItemInfo>
            {
                new ItemInfo()
                {
                    Title="Microcharts",
                    Details="Create cross-platform (Xamarin, Windows, ...) simple charts.",
                    Url="https://github.com/aloisdeniel/Microcharts",
                },

                new ItemInfo()
                {
                    Title="localnotificationsplugin",
                    Details="Local Notifications Plugin for Xamarin and Windows",
                    Url="https://github.com/edsnider/localnotificationsplugin",
                },
                new ItemInfo()
                {
                    Title="Material Icons",
                    Details="Material icons are delightful, beautifully crafted symbols for common actions and items.",
                    Url="https://material.io/tools/icons/?style=baseline",
                }
                ,
                new ItemInfo()
                {
                    Title="CarouselView",
                    Details="CarouselView control for Xamarin Forms.",
                    Url="https://github.com/alexrainman/CarouselView",
                }
            };
            OpenWebCommand =  new Command(
                execute: (o) =>
                {
                    if (o is string adress)
                        Device.OpenUri(new Uri(adress));
                });
        }

    }
}