import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { SurveysComponent } from './surveys/surveys.component';
import { PoapsComponent } from './poaps/poaps.component';
import { CreateEditPoapComponent } from './poaps/create-edit-poap/create-edit-poap.component';
import { ViewSurveyComponent } from './surveys/view-survey/view-survey.component';
import { TheatersComponent } from './theaters/theaters.component';
import { SurveySettingsComponent } from './settings/survey-settings/survey-settings.component';
import { DeviceDemoSettingsComponent } from './settings/device-demo-settings/device-demo-settings.component';
import { SettingsComponent } from './settings/settings.component';
import { ReportingSettingsComponent } from './settings/reporting-settings/reporting-settings.component';
import { RiskmappingSettingsComponent } from './settings/riskmapping-settings/riskmapping-settings.component';
import { RegionManagementV2Component } from './settings/region-management-v2/region-management-v2.component';
import { DevicesComponent } from './devices/devices.component';
import { HospitalsComponent } from './hospitals/hospitals.component';
import { DeviceManagementDevicesComponent } from './devices/device-management-devices/device-management-devices.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        component: AppComponent,
        children: [
          { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
          { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
          { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
          { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
          { path: 'about', component: AboutComponent },
          { path: 'update-password', component: ChangePasswordComponent },
          { path: 'surveys', component: SurveysComponent },
          { path: 'surveys/view/:id', component: ViewSurveyComponent },
          { path: 'poaps', component: PoapsComponent },
          { path: 'poaps/manage', component: CreateEditPoapComponent },
          { path: 'poaps/manage/:id', component: CreateEditPoapComponent },
          { path: 'theaters', component: TheatersComponent },
          { path: 'survey-settings', component: SurveySettingsComponent },
          { path: 'device-demo-settings', component: DeviceDemoSettingsComponent },
          { path: 'settings', component: SettingsComponent },
          { path: 'reporting-settings', component: ReportingSettingsComponent },
          { path: 'risk-mapping-settings', component: RiskmappingSettingsComponent },
          { path: 'region-management', component: RegionManagementV2Component },
          // { path: 'region-management', component: RegionManagementComponent },
          { path: 'devices', component: DevicesComponent },
          { path: 'hospital-management', component: HospitalsComponent },
          { path: 'device-management-devices', component: DeviceManagementDevicesComponent },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
