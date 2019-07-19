using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyInsuranceThird.Models
{
    public class SearchViewModel
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Postcode { get; set; }

        public List<string> Registrations { get; set; }
    }
}