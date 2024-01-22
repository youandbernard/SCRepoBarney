using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Services.PoapRiskFactors.Dto
{
    public class DiagnosticRiskFactorsMappingDto
    {
        public int DiagnosticId { get; set; }
        public string SnomedId { get; set; }
        public string Label { get; set; }
        public bool IsGroup { get; set; }
        public string Group { get; set; }
    }
}
