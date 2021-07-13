using System;
using CMS.Service.ConnectCases;
using System.Collections.Generic;
namespace PalmCoastConnect.Models
{
    public class CaseOptions
    {
        public List<Catergory> CaseCategories { get; set; }

        public List<QuickLink> QuickLinks { get; set; }
    }
}
