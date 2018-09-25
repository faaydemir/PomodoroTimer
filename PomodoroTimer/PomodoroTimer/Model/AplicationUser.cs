using System;
using StorageHelper;

namespace PomodoroTimer
{
    public class AplicationUser : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set;}
        public string Pass { get; set; }
    }
}