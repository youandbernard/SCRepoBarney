using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaseMix.Entities.Enums
{
    public enum DiagnosticReportStatus
    {
        [Description("Open")]
        Open,

        [Description("Preliminary")]
        Preliminary,

        [Description("Final")]
        Final
    }
}
