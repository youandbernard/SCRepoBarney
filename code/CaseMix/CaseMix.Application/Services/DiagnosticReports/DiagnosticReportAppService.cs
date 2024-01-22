using Abp.Domain.Repositories;
using CaseMix.Entities;
using CaseMix.Services.DiagnosticReports.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.DiagnosticReports
{
    public class DiagnosticReportAppService : CaseMixAppServiceBase, IDiagnosticReportAppService
    {
        private readonly IRepository<DiagnosticReport, int> _diagnosticReportRepository;

        public DiagnosticReportAppService(IRepository<DiagnosticReport, int> diagnosticReportRepository)
        {
            _diagnosticReportRepository = diagnosticReportRepository;
        }

        public async Task<IEnumerable<DiagnosticReportDto>> GetAllAsync(string patientId)
        {
            var diagnosticReports = await _diagnosticReportRepository.GetAll()
                .Where(e => e.Subject == patientId.ToString())
                .Select(e => ObjectMapper.Map<DiagnosticReportDto>(e))
                .ToListAsync();

            return diagnosticReports;
        }
    }
}
