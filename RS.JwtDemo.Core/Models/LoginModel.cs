using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.JwtDemo.Core.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Audience { get; set; }
        public bool IsRefreshToken { get; set; }
    }
}
