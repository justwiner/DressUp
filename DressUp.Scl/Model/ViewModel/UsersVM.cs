using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class UsersVM
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime CreationTime { get; set; }
        public string ContactInfo { get; set; }
    }
}