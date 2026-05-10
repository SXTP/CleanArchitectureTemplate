using Noname.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Domain.Entities;

public class Member : BaseEntity
{
    public string IdentityId { get; set; } = string.Empty; // AspNetUsers tablosundaki Id alanına karşılık gelir
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
