using System;

namespace JomMalaysia.Presentation.Models.AppUsers
{
    public class AppUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string PictureUri { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool HasAuthority { get; set; }
        public bool Verified { get; set; }
    }
}