using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using InstrumentUI_ATK.DataAccess.Model;
using InstrumentUI_ATK.DataAccess;

namespace InstrumentUI_ATK.Common
{
    /// <summary>
    /// A class which stores all info related to a sample
    /// </summary>
    public class Sample
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public int PresentationId { get; set; }
        public string PresentationName { get; set; }

        public List<SampleIdentifier> SampleIdentifiers { get; set; }

        public List<IdNamePair> Traits { get; set; }

        public string SampleTypeName { get; set; }
    }
}
