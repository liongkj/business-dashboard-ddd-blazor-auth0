using System;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.AppUsers;

namespace JomMalaysia.Presentation.Manager
{
    public interface IAuthorizationManagers
    {
       AppUser LoginInfo { get; }
    
    }
}