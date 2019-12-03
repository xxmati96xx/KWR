using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace KalendarzWydarzenRodzinnych.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUzytkownikId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("id_uzytkownik");
            
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}