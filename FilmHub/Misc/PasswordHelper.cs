using crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FilmHub.Misc
{
    public class PasswordHelper
    {
        public static string IzracunajHash(string passsword)
        {
            var sBytes = new UTF8Encoding().GetBytes(passsword);
            byte[] hBytes;
            using (var alg = new System.Security.Cryptography.SHA256Managed())
            {
                hBytes = alg.ComputeHash(sBytes);
            }
            return Convert.ToBase64String(hBytes);
        }
    }
}