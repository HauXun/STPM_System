using Microsoft.AspNetCore.Identity;
using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public class AppUserRole : IdentityRole<int>, IEntity<int>
{
    public string Description { get; set; }

    public AppUserRole() : base()
    {

    }

    public AppUserRole(string roleName) : base(roleName)
    {

    }
}
