using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidBack.BLL.DTO
{
    public class TokensDTO
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
