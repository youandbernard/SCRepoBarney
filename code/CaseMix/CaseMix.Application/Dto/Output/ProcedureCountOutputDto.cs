using System;
using System.Collections.Generic;
using System.Text;

namespace CaseMix.Dto
{
    public class ProcedureCountOutputDto
    {
        public int snomedid { get; set; }
        public string snomed_desc { get; set; }
        public int procedure_count { get; set; }
    }
}
