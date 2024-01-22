import { Component, Injector, ChangeDetectionStrategy, OnInit, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  HospitalDto,
  HospitalsServiceProxy,
  ReportingSettingsServiceProxy,
  UserServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { DataUpdateService } from '@shared/services/data-update.service';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';
import { ThrowStmt } from '@angular/compiler';
import { relativeTimeThreshold } from 'moment';

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent extends AppComponentBase implements OnInit {
  hospitals: HospitalDto[] = [];
  role: string;
  hospital: HospitalDto = new HospitalDto();
  currentHospitalId: string;
  isAdmin: boolean;
  reportUrl: string;
  reportingSettingEnabled = false;
  deviceManagementEnabled = false;
  reportView = false;
  isLoading = false;

  constructor(
    injector: Injector,
    private _cdf: ChangeDetectorRef,
    private _dataUpdateService: DataUpdateService<HospitalDto>,
    private _hospitalsService: HospitalsServiceProxy,
    private _localStorageService: LocalStorageService,
    private _userService: UserServiceProxy,
    private _reportSettingService: ReportingSettingsServiceProxy,
    private cdr: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);

    if (localHospital) {
      this.deviceManagementEnabled = localHospital.activeDevMgt;
    }

    this.isAdmin = this.appSession.user.isAdmin;
    this.getHospitals();
  }

  onHopsitalChange(): void {
    const hospital = _.find(this.hospitals, (e) => e.id === this.currentHospitalId);
    if (hospital) {
      this.deviceManagementEnabled = hospital.activeDevMgt;
    }
    if (hospital.setting) {
      this.reportingSettingEnabled = hospital.setting.isEnabled;
    }
    this.setHospitalData(hospital);
  }

  navigateToReportView(enabled: boolean): void {
    this.reportView = enabled;
  }
  private getHospitals(): void {
    this.isLoading = true;
    this._hospitalsService.getByUser(this.appSession.userId).subscribe((hospitals) => {
      this.isLoading = false;
      this._cdf.detectChanges();
      this.hospitals = hospitals;
      
      if (this.hospitals.length > 0) {
        this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
        if (this.hospital) {
          this.currentHospitalId = this.hospital.id;
          var hospitalSetting = this.hospitals.filter((e) => e.id == this.currentHospitalId)[0];
          if (!hospitalSetting) hospitalSetting = this.hospitals[0];

          this.reportingSettingEnabled = !hospitalSetting.setting ? false : hospitalSetting.setting.isEnabled;
          this.deviceManagementEnabled = this.hospital.activeDevMgt;
        }
        if (!this.hospital && this.hospitals.length > 0) {
          this.hospital = this.hospitals[0];
          this.currentHospitalId = this.hospital.id;
          this.deviceManagementEnabled = this.hospital.activeDevMgt;
        }

        this.setHospitalData(this.hospital);
        this._cdf.detectChanges();
      }
    });
  }

  private setHospitalData(hospital: HospitalDto): void {
    this._localStorageService.setObjectItem<HospitalDto>(this.localStorageKey.hospital, hospital);
    this._dataUpdateService.setData(hospital);
    this.reportUrl = hospital.reportUrl;
  }
}
