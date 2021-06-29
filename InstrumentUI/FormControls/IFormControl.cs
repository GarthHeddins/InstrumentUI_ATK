using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstrumentUI_ATK.DataService;

namespace InstrumentUI_ATK.FormControls
{
    public interface IFormControl 
    {
        EnumHelpCode HelpCode { get; }
        void LocalizeResource();
    }
}
