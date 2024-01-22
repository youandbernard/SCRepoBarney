import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { BsDatepickerModule, DatePickerComponent } from 'ngx-bootstrap/datepicker';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { TreeModule } from 'primeng/tree';
import { CheckboxModule } from 'primeng/checkbox';
import { TableModule } from 'primeng/table';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { MultiSelectModule } from 'primeng/multiselect';
import { NgSelectModule } from '@ng-select/ng-select';

import { HomeComponent } from '@app/home/home.component';
import { AboutComponent } from '@app/about/about.component';
// tenants
import { TenantsComponent } from '@app/tenants/tenants.component';
import { CreateTenantDialogComponent } from './tenants/create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './tenants/edit-tenant/edit-tenant-dialog.component';
// roles
import { RolesComponent } from '@app/roles/roles.component';
import { CreateRoleDialogComponent } from './roles/create-role/create-role-dialog.component';
import { EditRoleDialogComponent } from './roles/edit-role/edit-role-dialog.component';
// users
import { UsersComponent } from '@app/users/users.component';
import { CreateUserDialogComponent } from '@app/users/create-user/create-user-dialog.component';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ResetPasswordDialogComponent } from './users/reset-password/reset-password.component';
// layout
import { HeaderComponent } from './layout/header.component';
import { HeaderLeftNavbarComponent } from './layout/header-left-navbar.component';
import { HeaderLanguageMenuComponent } from './layout/header-language-menu.component';
import { HeaderUserMenuComponent } from './layout/header-user-menu.component';
import { FooterComponent } from './layout/footer.component';
import { SidebarComponent } from './layout/sidebar.component';
import { SidebarLogoComponent } from './layout/sidebar-logo.component';
import { SidebarUserPanelComponent } from './layout/sidebar-user-panel.component';
import { SidebarMenuComponent } from './layout/sidebar-menu.component';
import { AssignHospitalsComponent } from './users/assign-hospitals/assign-hospitals.component';
import { SurveysComponent } from './surveys/surveys.component';
import { PoapsComponent } from './poaps/poaps.component';
import { CreateEditPoapComponent } from './poaps/create-edit-poap/create-edit-poap.component';
import { DateTimePickerModalComponent } from '@shared/components/modal/date-time-picker-modal/date-time-picker-modal.component';
import { TimePickerModalComponent } from '@shared/components/modal/time-picker-modal/time-picker-modal.component';
import { ProcedureSearchComponent } from './poaps/create-edit-poap/procedure-search/procedure-search.component';
import { ViewSurveyComponent } from './surveys/view-survey/view-survey.component';
import { EditActualTimeComponent } from './surveys/view-survey/edit-actual-time/edit-actual-time.component';
import { AssignSpecialtiesComponent } from './users/assign-specialties/assign-specialties.component';
import { TheatersComponent } from './theaters/theaters.component';
import { CreateEditTheaterComponent } from './theaters/create-edit-theater/create-edit-theater.component';
import { SurveySettingsComponent } from './settings/survey-settings/survey-settings.component';
import { SurveyTimestampComponent } from './settings/survey-settings/survey-timestamp/survey-timestamp.component';
import { DeviceDemoSettingsComponent } from './settings/device-demo-settings/device-demo-settings.component';
import { SettingsComponent } from './settings/settings.component';
import { ReportingSettingsComponent } from './settings/reporting-settings/reporting-settings.component';
import { RiskmappingSettingsComponent } from './settings/riskmapping-settings/riskmapping-settings.component';
import { OpenImageComponent } from './settings/riskmapping-settings/open-image/open-image.component';
import { CheckboxSetingsComponent } from './settings/riskmapping-settings/checkbox-setings/checkbox-setings.component';
import { RiskmappingSamplesComponent } from './settings/riskmapping-settings/riskmapping-samples/riskmapping-samples.component';
import { ShowDiagnosticReportDetailsComponent } from './poaps/show-diagnostic-report-details/show-diagnostic-report-details.component';
import { ShowDemographicDetailsComponent } from './poaps/show-demographic-details/show-demographic-details.component';
import { ShowPatientDetailsComponent } from './poaps/show-patient-details/show-patient-details.component';
import { RegionManagementComponent } from './settings/region-management/region-management.component';
import { DevicesComponent } from './devices/devices.component';
import { CreateEditRegionComponent } from './settings/region-management/create-edit-region/create-edit-region.component';
import { CreateEditHospitalComponent } from './settings/region-management/create-edit-hospital/create-edit-hospital.component';
import { HospitalsComponent } from './hospitals/hospitals.component';
import { CreateEditHospitalsComponent } from './hospitals/create-edit-hospitals/create-edit-hospitals.component';
import { DeviceManagementDevicesComponent } from './devices/device-management-devices/device-management-devices.component';
import { DeviceManufacturerDevices } from './devices/device-manufacturer-devices/device-manufacturer-devices.component';
import { DeviceManagementUploadsComponent } from './devices/device-management-uploads/device-management-uploads.component';
import { DeviceManagementImportComponent } from './devices/device-management-import/device-management-import.component';
import { SelectManufacturerComponent } from './devices/device-management-uploads/select-manufacturer/select-manufacturer.component';
import { UploadDeviceComponent } from './devices/device-management-uploads/upload-device/upload-device.component';
import { DeviceProceduresComponent } from './devices/device-management-devices/device-procedures/device-procedures.component';
import { ImportDeviceComponent } from './devices/device-management-import/import-device/import-device.component';
import { ProcedureSelectDeviceComponent } from './poaps/create-edit-poap/procedure-select-device/procedure-select-device.component';
import { SurveyProcedureSelectDeviceComponent }  from './surveys/view-survey/procedure-select-device-survey/procedure-select-device-survey.component';
import { SurveyProcedureSearchComponent } from './surveys/view-survey/procedure-search-survey/procedure-search-survey.component';
import { RegionManagementV2Component } from './settings/region-management-v2/region-management-v2.component';
import { DeviceGmdntermcodesComponent } from './devices/device-management-devices/device-gmdntermcodes/device-gmdntermcodes.component';
import { InstrumentSelectComponent } from './poaps/create-edit-poap/instrument-select/instrument-select.component';
import { ViewSurveyNotesComponent } from './surveys/view-survey/view-survey-notes/view-survey-notes.component';
import { SetSpecialtiesComponent } from './hospitals/set-specialties/set-specialties.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    // tenants
    TenantsComponent,
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    RolesComponent,
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    UsersComponent,
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ChangePasswordComponent,
    ResetPasswordDialogComponent,
    // layout
    HeaderComponent,
    HeaderLeftNavbarComponent,
    HeaderLanguageMenuComponent,
    HeaderUserMenuComponent,
    FooterComponent,
    SidebarComponent,
    SidebarLogoComponent,
    SidebarUserPanelComponent,
    SidebarMenuComponent,
    AssignHospitalsComponent,
    SurveysComponent,
    PoapsComponent,
    CreateEditPoapComponent,
    DateTimePickerModalComponent,
    TimePickerModalComponent,
    ProcedureSearchComponent,
    ViewSurveyComponent,
    EditActualTimeComponent,
    AssignSpecialtiesComponent,
    TheatersComponent,
    CreateEditTheaterComponent,
    SurveySettingsComponent,
    SurveyTimestampComponent,
    DeviceDemoSettingsComponent,
    SettingsComponent,
    ReportingSettingsComponent,
    RiskmappingSettingsComponent,
    OpenImageComponent,
    CheckboxSetingsComponent,
    RiskmappingSamplesComponent,
    ShowDiagnosticReportDetailsComponent,
    ShowDemographicDetailsComponent,
    ShowPatientDetailsComponent,
    RegionManagementComponent,
    DevicesComponent,
    CreateEditRegionComponent,
    CreateEditHospitalComponent,
    HospitalsComponent,
    CreateEditHospitalsComponent,
    DeviceManagementDevicesComponent,
    DeviceManagementUploadsComponent,
    DeviceManagementImportComponent,
    SelectManufacturerComponent,
    UploadDeviceComponent,
    DeviceProceduresComponent,
    ImportDeviceComponent,
    ProcedureSelectDeviceComponent,
    RegionManagementV2Component,
    DeviceGmdntermcodesComponent,
    InstrumentSelectComponent,
    ViewSurveyNotesComponent,
    SurveyProcedureSelectDeviceComponent,
    SurveyProcedureSearchComponent,
    DeviceManufacturerDevices,
    SetSpecialtiesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule.forChild(),
    BsDropdownModule,
    CollapseModule,
    TabsModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
    TypeaheadModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
    TreeModule,
    TableModule,
    CheckboxModule,
    NgxMaterialTimepickerModule.setLocale('en-GB'),
    PdfViewerModule,
    NgxExtendedPdfViewerModule,
    CarouselModule.forRoot(),
    OrganizationChartModule,
    MultiSelectModule,
    NgSelectModule
  ],
  providers: [DatePipe],
  entryComponents: [
    // tenants
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ResetPasswordDialogComponent,
    // casemix
    DateTimePickerModalComponent,
    TimePickerModalComponent,
    ProcedureSearchComponent,
    EditActualTimeComponent,
    CreateEditTheaterComponent,
  ],
})
export class AppModule {}
