using System.ComponentModel;

namespace CaseMix.Entities.Enums
{
    public enum SurgeonExperienceType
    {
        [Description("Unknown")]
        Unknown,

        [Description("0 - 1 Year")]
        ZeroToOneYear,
        
        [Description("1 - 5 Years")]
        OneToFiveYears,
        
        [Description("5 - 10 Years")]
        FiveToTenYears,
        
        [Description("Over 10 Years")]
        OverTenYears,
    }
}
