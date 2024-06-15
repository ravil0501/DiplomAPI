using System;
using System.Collections.Generic;

namespace DiplomAPI.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public int? GroupTeacher { get; set; }

    public virtual User? GroupTeacherNavigation { get; set; }
}
