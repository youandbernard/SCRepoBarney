using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CaseMix.Entities;
using CaseMix.Services.Patients.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseMix.Services.Patients
{
    public class PatientsAppService : CaseMixAppServiceBase, IPatientsAppService
    {
        private readonly IRepository<HospitalPatient, Guid> _hospitalPatientsRepository;
        private readonly IRepository<Patient, string> _patientsRepository;
        private readonly IRepository<Ethnicity, int> _ethnicityRepository;

        public PatientsAppService(IRepository<HospitalPatient, Guid> hospitalPatientsRepository, IRepository<Patient, string> patientsRepository, IRepository<Ethnicity, int> ethnicityRepository)
        {
            _hospitalPatientsRepository = hospitalPatientsRepository;
            _patientsRepository = patientsRepository;
            _ethnicityRepository = ethnicityRepository;
        }

        public async Task<IEnumerable<PatientDto>> GetByHospital(string hospitalId, string keyword)
        {
            var patients = await _hospitalPatientsRepository.GetAll()
                .Where(e => e.HospitalId == hospitalId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), e => e.PatientId.Contains(keyword))
                .OrderBy(e => e.PatientId)
                .Take(10)
                .Select(e => ObjectMapper.Map<PatientDto>(e.Patient))
                .ToListAsync();

            return patients;
        }
        public async Task<IEnumerable<PatientDto>> GetAll(string hospitalId, string patientId = "")
        {
            var patients = new List<PatientDto>();
            var selectedPatient = new PatientDto();
            if (!string.IsNullOrEmpty(patientId))
            {
                selectedPatient = await _hospitalPatientsRepository.GetAll()
                    .Where(e => e.PatientId == patientId && e.Patient.Name != null)
                    .Select(e => ObjectMapper.Map<PatientDto>(e.Patient))
                    .FirstOrDefaultAsync();

                patients.Add(selectedPatient);
            }

            var data = new List<PatientDto>();

            if(selectedPatient != null)
            {
                data = await _hospitalPatientsRepository.GetAll()
                .Where(e => e.HospitalId == hospitalId && selectedPatient.Id != e.Patient.Id && e.Patient.Name != null)
                .OrderBy(e => e.PatientId)
                .Select(e => ObjectMapper.Map<PatientDto>(e.Patient))
                .ToListAsync();
            } 
            else
            {
                data = await _hospitalPatientsRepository.GetAll()
                .Where(e => e.HospitalId == hospitalId)
                .OrderBy(e => e.PatientId)
                .Select(e => ObjectMapper.Map<PatientDto>(e.Patient))
                .ToListAsync();
            }
                patients.AddRange(data);


            return patients;
        }

        public async Task<PatientDto> Get(string id)
        {
            var patient = await _patientsRepository.GetAll()
                .Where(e => e.Id == id)
                .Select(e => ObjectMapper.Map<PatientDto>(e))
                .FirstOrDefaultAsync();

            if(patient.EthnicCategory != null)
                patient.EthnicCategory = _ethnicityRepository.FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(patient.EthnicCategory)).Result.Description;
            
            return patient;
        }
    }
}
