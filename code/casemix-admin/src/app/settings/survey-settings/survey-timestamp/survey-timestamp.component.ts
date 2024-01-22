import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { HospitalsServiceProxy, SurveyTimestampServiceProxy, SurveyTimestampSettingDto, HospitalDto } from '@shared/service-proxies/service-proxies';
import { Observable } from 'rxjs';
import * as _ from 'lodash';

@Component({
  selector: 'survey-timestamp',
  templateUrl: './survey-timestamp.component.html',
  styleUrls: ['./survey-timestamp.component.less']
})
export class SurveyTimestampComponent extends AppComponentBase implements OnInit {
  surveyTimestampSettings: SurveyTimestampSettingDto[] = [];
  hospitals: HospitalDto[] = [];
  isSaving = false;

  constructor(
    injector: Injector,
    private _hospitalService: HospitalsServiceProxy,
    private _surveyTimestampService: SurveyTimestampServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getHospitals();
  }


  saveSurveyTimestampSettings(): void {
    this.isSaving = true;
    this._surveyTimestampService.saveAll(this.surveyTimestampSettings)
    .subscribe(() => {
      this.notify.success(this.l('SavedSuccessfully'));
      this.isSaving = false;
    });
  }

  private getHospitals(): void {
    this._hospitalService.getByUser(this.appSession.userId)
      .subscribe(hospitals => {
        this.hospitals = hospitals;
        this.getUserSurveyTimestampSettings();
      });
  }

  private getUserSurveyTimestampSettings(): void {
    this._surveyTimestampService
    .getAll()
    .subscribe(settings => {
      _.forEach(this.hospitals, (hospital) => {
        let setting = _.find(settings, e => e.hospitalId === hospital.id);
        if (setting) {
          setting.hospitalName = setting.hospital.name;
          this.surveyTimestampSettings.push(setting);
        } else {
          setting = new SurveyTimestampSettingDto();
          setting.isEnabled = false;
          setting.hospitalId = hospital.id;
          setting.hospitalName = hospital.name;
          this.surveyTimestampSettings.push(setting);
        }
      });
    });
  }
}
