using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string email, IList<string> roles);
}
