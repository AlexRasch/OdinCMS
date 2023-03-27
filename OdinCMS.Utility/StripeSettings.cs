using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.Utility
{

    public class StripeSettings
    {
        //private readonly IConfiguration _config;

        public string Publishable_key { get; set; }
        public string Secret_key { get; set; }


        /*
        public StripeSettings(IConfiguration config)
        {

            // This reads value from "secrets.json"
            _config = config;
            Publishable_key = _config["Publishable key"];
            Secret_key = _config["Secret key"];

        }*/

    }
}
