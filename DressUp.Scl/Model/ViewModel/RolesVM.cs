using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class RolesVM
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDefault { get; set; }
    }
}