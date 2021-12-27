using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Latian_LKS_2
{
    class Utils
    {
        public static string conn = @"Data Source=DESKTOP-00EPOSJ;Initial Catalog=latian_lks_2;Integrated Security=True";
    }

    class Model
    {
        public static int ID { set; get; }
        public static string Name { set; get; }
        public static string Username { set; get; }
        public static int JobID { set; get; }
    }

    class Encrypt
    {
        public static String encryptPass(String data)
        {
            using(SHA256Managed managed = new SHA256Managed())
            {
                byte[] strData = Encoding.UTF8.GetBytes(data);
                var result = managed.ComputeHash(strData);
                String strResult = Convert.ToBase64String(result);

                return strResult;
            }
        }
    }
    
    class UserSelected
    {
        public static string user_name { set; get; }
        public static int user_id { set; get; }
    }
}
