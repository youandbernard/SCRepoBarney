import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
  providers: [
    ApiServiceProxies.SurgeonSpecialtiesServiceProxy,
    ApiServiceProxies.EthnicitiesServiceProxy,
    ApiServiceProxies.BodyStructureGroupsServiceProxy,
    ApiServiceProxies.CoMorbiditiesServiceProxy,
    ApiServiceProxies.BodyStructuresServiceProxy,
    ApiServiceProxies.PatientSurveysServiceProxy,
    ApiServiceProxies.PreOperativeAssessmentsServiceProxy,
    ApiServiceProxies.PatientsServiceProxy,
    ApiServiceProxies.PasswordResetsServiceProxy,
    ApiServiceProxies.UserHospitalsServiceProxy,
    ApiServiceProxies.HospitalsServiceProxy,
    ApiServiceProxies.RoleServiceProxy,
    ApiServiceProxies.SessionServiceProxy,
    ApiServiceProxies.TenantServiceProxy,
    ApiServiceProxies.UserServiceProxy,
    ApiServiceProxies.TokenAuthServiceProxy,
    ApiServiceProxies.AccountServiceProxy,
    ApiServiceProxies.ConfigurationServiceProxy,
    ApiServiceProxies.TheatersServiceProxy,
    ApiServiceProxies.SurveyTimestampServiceProxy,
    ApiServiceProxies.ReportingSettingsServiceProxy,
    ApiServiceProxies.RiskMappingServiceProxy,
    ApiServiceProxies.DiagnosticReportServiceProxy,
    ApiServiceProxies.PoapRiskFactorServiceProxy,
    ApiServiceProxies.RegionsServiceProxy,
    ApiServiceProxies.CountriesServiceProxy,
    ApiServiceProxies.ManufacturesServiceProxy,
    ApiServiceProxies.TrustsServiceProxy,
    ApiServiceProxies.UkRegionsServiceProxy,
    ApiServiceProxies.IntegratedCareSystemsServiceProxy,
    ApiServiceProxies.DeviceServiceProxy,
    ApiServiceProxies.DocumentServiceProxy,
    ApiServiceProxies.BodyStructureProceduresServiceProxy,
    ApiServiceProxies.DeviceProcedureServiceProxy,
    ApiServiceProxies.PoapProcedureDevicesServiceProxy,
    ApiServiceProxies.PoapInstrumentPacksServiceServiceProxy,
    { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
  ],
})
export class ServiceProxyModule {}
