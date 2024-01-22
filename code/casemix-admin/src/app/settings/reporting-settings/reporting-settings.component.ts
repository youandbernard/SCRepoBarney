import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  HospitalDto,
  HospitalsServiceProxy,
  ReportingSettingDto,
  ReportingSettingsServiceProxy,
  SurveyTimestampServiceProxy
} from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'app-reporting-settings',
  templateUrl: './reporting-settings.component.html',
  styleUrls: ['./reporting-settings.component.less'],
  animations: [appModuleAnimation()]
})
export class ReportingSettingsComponent extends AppComponentBase implements OnInit {
  reportingSettings: ReportingSettingDto[] = [];
  hospitals: HospitalDto[] = [];
  isSaving = false;
  isLoading = false;
  constructor(
    injector: Injector,
    private _hospitalService: HospitalsServiceProxy,
    private _reportingSettingService: ReportingSettingsServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getHospitals();
  }

  onFormSubmit() {
    this.saveUserReportingsettings();
  }

  private saveUserReportingsettings(): void {
    this.isSaving = true;
    this._reportingSettingService.saveAll(this.reportingSettings).subscribe(() => {
      this.notify.success(this.l('SavedSuccessfully'));
      this.isSaving = false;
    });
  }

  private getHospitals(): void {
    this._hospitalService.getByUser(this.appSession.userId).subscribe(hospitals => {
      this.hospitals = hospitals;
      this.getUserReportingSettings();
    });
  }

  private getUserReportingSettings(): void {
    this.isLoading = true;
    this._reportingSettingService.getAll().subscribe(settings => {
      this.isLoading = false;
      _.forEach(this.hospitals, hospital => {
        let setting = _.find(settings, e => e.hospitalId === hospital.id);
        if (setting) {
          setting.hospitalName = setting.hospital.name;
          this.reportingSettings.push(setting);
        } else {
          setting = new ReportingSettingDto();
          setting.isEnabled = false;
          setting.hospitalId = hospital.id;
          setting.hospitalName = hospital.name;
          this.reportingSettings.push(setting);
        }
      });
    });
  }
}
