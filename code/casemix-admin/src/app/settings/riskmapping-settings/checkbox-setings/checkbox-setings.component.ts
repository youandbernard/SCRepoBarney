import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { HospitalDto, HospitalsServiceProxy, RiskMappingServiceProxy, RiskMappingSettingDto } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'checkbox-setings',
  templateUrl: './checkbox-setings.component.html',
  styleUrls: ['./checkbox-setings.component.less']
})
export class CheckboxSetingsComponent extends AppComponentBase implements OnInit {
  riskMappingSettings: RiskMappingSettingDto[] = [];
  isSaving = false;
  hospitals: HospitalDto[] = [];
  constructor(
    injector: Injector,
    private _riskMappingService: RiskMappingServiceProxy,
    private _hospitalServivce: HospitalsServiceProxy
  ) {
    super(injector);
   }

  ngOnInit(): void {
    this.getHospitals();
  }

  saveRiskMappingSettings(): void {
    this.isSaving = true;
    this._riskMappingService.saveAll(this.riskMappingSettings)
    .subscribe(() => {
      this.notify.success(this.l('SavedSuccessfully'));
      this.isSaving = false;
    });
  }
  

  private getHospitals(): void {
    this._hospitalServivce.getByUser(this.appSession.userId)
      .subscribe(hospitals => {
        this.hospitals = hospitals;
        this.getUserRiskMappingSettings();
      });
  }

  private getUserRiskMappingSettings(): void {
    this._riskMappingService.getAll()
        .subscribe(settings => {
          _.forEach(this.hospitals, (hospital) => {
            let setting = _.find(settings, e => e.hospitalId === hospital.id);
            if(setting) {
              setting.hospitalName = setting.hospital.name;
              this.riskMappingSettings.push(setting);
            }
            else {
              setting = new RiskMappingSettingDto();
              setting.isEnabled = false;
              setting.hospitalId = hospital.id;
              setting.hospitalName = hospital.name;
              this.riskMappingSettings.push(setting);
            }
          })
        })
  }
}
