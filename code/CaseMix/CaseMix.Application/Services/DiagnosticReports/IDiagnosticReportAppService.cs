using CaseMix.Services.DiagnosticReports.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaseMix.Services.DiagnosticReports
{
    public interface IDiagnosticReportAppService
    {
        Task<IEnumerable<DiagnosticReportDto>> GetAllAsync(string patientId);
    }
}
