using System;
using System.Collections.Generic;

namespace DiplomAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public int? Role { get; set; }

    public int? GroupNumber { get; set; }

    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;


    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Role? RoleNavigation { get; set; }
}
