import { Component, OnInit, Input, Injector, Output } from '@angular/core';
import { HospitalsServiceProxy, HospitalDto, UserHospitalsServiceProxy, UserHospitalDto } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-assign-hospitals',
  templateUrl: './assign-hospitals.component.html',
  styleUrls: ['./assign-hospitals.component.less'],
})
export class AssignHospitalsComponent extends AppComponentBase implements OnInit {
  @Input() userId: number;
  @Output() userHospitals: UserHospitalDto[] = [];

  hospitals: HospitalDto[] = [];

  constructor(
    injector: Injector,
    private _hospitalsService: HospitalsServiceProxy,
    private _userHospitalsService: UserHospitalsServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getHospitals();
  }

  public saveUserHospitals(): Observable<void> {
    return this._userHospitalsService.saveAll(this.userHospitals);
  }

  private getHospitals(): void {
    this._hospitalsService.getAll(null, null, 0, 999).subscribe((hospitals) => {
      _.forEach(hospitals.items, (hospital) => {
        this.hospitals[hospital.id] = hospital;
      });
      this.getUserHospitals();
    });
  }

  private getUserHospitals(): void {
    this._userHospitalsService.getAll(this.userId).subscribe((userHospitals) => {
      this.prepareUserHospitals(userHospitals);
    });
  }

  private prepareUserHospitals(userHospitals: UserHospitalDto[]): void {
    for (const hospitalId of Object.keys(this.hospitals)) {
      let userHospital = _.find(userHospitals, (e) => e.hospitalId === hospitalId);
      if (userHospital) {
        userHospital.isSelected = true;
        this.userHospitals.push(userHospital);
      } else {
        userHospital = new UserHospitalDto();
        userHospital.userId = this.userId;
        userHospital.hospitalId = hospitalId;
        userHospital.isSelected = false;
        this.userHospitals.push(userHospital);
      }
    }
  }
}
