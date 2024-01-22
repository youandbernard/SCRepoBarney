import { ChangeDetectorRef, Component, Injector, Input, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { PatientDto, PatientsServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-show-patient-details',
  templateUrl: './show-patient-details.component.html',
  styleUrls: ['./show-patient-details.component.less']
})
export class ShowPatientDetailsComponent extends AppComponentBase implements OnInit {
  @Input() id: string;
  @Input() surgeryDate: Date;
  @Input() ethnicGroup: string;
  patient: PatientDto;
  date: Date;
  gender: string;
  patientId = '0';
  dobYear = 0;
  ethnicCategory = '';
   constructor(injector: Injector, private _modal: BsModalRef, private _patientService: PatientsServiceProxy, private _cdr: ChangeDetectorRef) {
    super(injector);
    this.location.onPopState(() => this.close());
   }

  ngOnInit(): void {
    debugger
    this.date = this.surgeryDate;
    this.getPatientRecord();
    this.ethnicCategory = this.ethnicGroup;
  }

  onCloseClick(): void {
    this.close();
  }

  private close(): void {
    this._modal.hide();
  }

  private getPatientRecord(): void {
    this._patientService.get(this.id).subscribe((response) => {
      this.patient = response;
      this.gender = this.patient.gender == 0? 'Male' : 'Female';
      this.patientId = this.patient.id;
      this.dobYear = this.patient.dobYear;
      this._cdr.detectChanges();
    });
  }

}
