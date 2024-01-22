using Abp.Application.Services;
using CaseMix.Services.Patients.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseMix.Services.Patients
{
    public interface IPatientsAppService : IApplicationService
    {
        Task<IEnumerable<PatientDto>> GetByHospital(string hospitalId, string keyword);
        Task<IEnumerable<PatientDto>> GetAll(string hospitalId, string patientId = "");
        Task<PatientDto> Get(string id);
    }
}
