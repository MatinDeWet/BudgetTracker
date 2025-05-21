using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.Abstractions;

namespace Domain.Entities;
public class User : Entity<int>
{
    public virtual ApplicationUser IdentityInfo { get; set; } = null!;
}
