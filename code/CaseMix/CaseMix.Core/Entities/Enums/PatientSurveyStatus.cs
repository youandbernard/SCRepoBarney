using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaseMix.Entities.Enums
{
    public enum PatientSurveyStatus
    {
        [Description("Created")]
        Created,

        [Description("Ongoing")]
        Ongoing,

        [Description("Completed")]
        Completed
    }
}
 