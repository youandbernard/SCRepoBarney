using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMix.Entities
{
    [Table("PatientSurgeryProgresses")]
    public class PatientSurgeryProgress : Entity<int>
    {
        public virtual string PatientId { get; set; }
        public virtual string UserId { get; set; }
        public virtual string SurgeonId { get; set; }
        public virtual string AnaesthesiaId { get; set; }
        public virtual string ProcedureCode { get; set; }
        public virtual DateTime SurgeryDate { get; set; }
        public virtual DateTime AdmissionCheckinTime { get; set; }
        public virtual DateTime AdmissionCheckoutTime { get; set; }
        public virtual DateTime WaitingCheckinTime { get; set; }
        public virtual DateTime WaitingCheckoutTime { get; set; }
        public virtual DateTime PreparationCheckinTime { get; set; }
        public virtual DateTime PreparationCheckoutTime { get; set; }
        public virtual DateTime AnaesthesiaCheckinTime { get; set; }
        public virtual DateTime AnaesthesiaCheckoutTime { get; set; }
        public virtual DateTime SurgeryCheckinTime { get; set; }
        public virtual DateTime SurgeryCheckoutTime { get; set; }
        public virtual DateTime RecoveryCheckinTime { get; set; }
        public virtual DateTime RecoveryCheckoutTime { get; set; }
        public virtual string AdmissionNotes { get; set; }
        public virtual string WaitingAreaNotes { get; set; }
        public virtual string PreparationNotes { get; set; }
        public virtual string AnaesthesiaNotes { get; set; }
        public virtual string SurgeryNotes { get; set; }
        public virtual string RecoveryNotes { get; set; }
        public virtual string ProcedureTypeDefinition { get; set; }
        public virtual int SnomedId { get; set; }
        public virtual string SnomedDesc { get; set; }
    }
}
