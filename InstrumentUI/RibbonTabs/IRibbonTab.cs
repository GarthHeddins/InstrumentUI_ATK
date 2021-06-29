using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstrumentUI_ATK.Controls;

namespace InstrumentUI_ATK.RibbonTabs
{
    public interface IRibbonTab
    {
        RibbonIcon DefaultButton { get; }
        void DeSelectButtons();
        void LocalizeResource();
    }
}
