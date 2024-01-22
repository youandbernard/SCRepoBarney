import { DatePipe } from '@angular/common';
import { Component, Injector, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { PatientDto, PatientsServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-show-demographic-details',
  templateUrl: './show-demographic-details.component.html',
  styleUrls: ['./show-demographic-details.component.less']
})
export class ShowDemographicDetailsComponent extends AppComponentBase implements OnInit {
  @Input() patientId: string;
  @Input() bloodPressure: string;
  @Input() bmi: string;
  @Input() gender: string;
  @Input() age: string;
  @Input() ethnicity: string;
  @Input() isSmoker: boolean;
  @Input() surgeryDate: Date;

  idValue: string;
  bpValue: string;
  bmiValue: string;
  genderValue: string;
  ageValue: string;
  ethnicityValue: string;
  smokingStatus: string;
  surgeryDateValue: Date;
  patient: PatientDto;
  dobYear = 0;
  date: Date;
  datelabel;
  constructor(
    injector: Injector, 
    private _modal: BsModalRef, 
    private _patientService: PatientsServiceProxy,
    public datepipe: DatePipe
    ) {
    super(injector);
    this.location.onPopState(() => this.close());
   }

  ngOnInit(): void {
    this.idValue = this.patientId;
    this.bpValue = this.bloodPressure;
    this.bmiValue = this.bmi;
    this.genderValue = this.gender;
    this.ageValue = this.age;
    this.ethnicityValue = this.ethnicity;
    this.smokingStatus = this.isSmoker ? 'Yes' : 'No';
    this.surgeryDateValue = this.surgeryDate;
    this.date = new Date();
    this.datelabel = 'BMI [' + this.datepipe.transform(this.date, 'yyyy-MM-dd') + ']';
    // console.log(this.datelabel);
    this.getPatientRecord();
  }


  onCloseClick(): void {
    this.close();
  }

  private close(): void {
    this._modal.hide();
  }

  private getPatientRecord(): void {
    this._patientService.get(this.patientId).subscribe((response) => {
      this.patient = response;
      this.dobYear = this.patient.dobYear;
    });
  }

}