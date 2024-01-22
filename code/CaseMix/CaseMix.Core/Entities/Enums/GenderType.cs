using System.ComponentModel;

namespace CaseMix.Entities.Enums
{
    public enum GenderType
    {
        [Description("Male")]
        Male,

        [Description("Female")]
        Female,

        [Description("Male")]
        MaleCode = 248153007,

        [Description("Female")]
        FemaleCode = 248152002,
    }
}
