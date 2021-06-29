using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstrumentUI_ATK.DataAccess.Model
{
    /// <summary>
    /// A generic class which can be used for the dropdown and checkbox items
    /// as a Id value pair
    /// </summary>
    public class IdNamePair
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
