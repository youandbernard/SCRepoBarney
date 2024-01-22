// import { Component, OnInit, Injector, Input } from '@angular/core';
// import { AppComponentBase } from '@shared/app-component-base';
// import { appModuleAnimation } from '@shared/animations/routerTransition';
// import { Observable, Observer } from 'rxjs';
// import { PatientDto, PatientSurveyDto, PatientsServiceProxy, HospitalDto, PatientSurveysServiceProxy, BodyStructuresServiceProxy,
// BodyStructureDto, ProcedureMethodDto, PatientSurveyProcedureDto, MenuItemOutputDto } from '@shared/service-proxies/service-proxies';
// import { switchMap } from 'rxjs/operators';
// import { LocalStorageService } from '@shared/services/local-storage.service';
// import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/ngx-bootstrap-typeahead';
// import { Router, ActivatedRoute } from '@angular/router';
// import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
// import * as moment from 'moment';
// import { TimePickerModalComponent } from '@shared/components/modal/time-picker-modal/time-picker-modal.component';
// import { BsModalService } from 'ngx-bootstrap/modal';
// import { DateTimePickerModalComponent } from '@shared/components/modal/date-time-picker-modal/date-time-picker-modal.component';
// import * as _ from 'lodash';
// import { ProcedureSearchComponent } from '@app/poaps/create-edit-poap/procedure-search/procedure-search.component';

// @Component({
//   selector: 'app-create-edit-survey',
//   templateUrl: './create-edit-survey.component.html',
//   styleUrls: ['./create-edit-survey.component.less'],
//   animations: [appModuleAnimation()]
// })
// export class CreateEditSurveyComponent extends AppComponentBase implements OnInit {
//   @Input() id: string;
//   hospital: HospitalDto;
//   survey: PatientSurveyDto;
//   bodyStructures: BodyStructureDto[];
//   procedureMethods: ProcedureMethodDto[];
//   patientsDataSource: Observable<PatientDto[]>;
//   clonedProcedures: { [s: string]: PatientSurveyProcedureDto; } = {};
//   saving = false;
//   datePickerConfig: BsDatepickerConfig;

//   constructor(
//     injector: Injector,
//     private _router: Router,
//     private _activatedRoute: ActivatedRoute,
//     private _modalService: BsModalService,
//     private _localStorageService: LocalStorageService,
//     private _patientsService: PatientsServiceProxy,
//     private _patientSurveysService: PatientSurveysServiceProxy,
//     private _bodyStructuresService: BodyStructuresServiceProxy,
//     ) {
//     super(injector);
//     this.survey = new PatientSurveyDto();
//     this.datePickerConfig = new BsDatepickerConfig();
//     this.datePickerConfig.showWeekNumbers = false;
//     this.datePickerConfig.dateInputFormat = 'DD/MM/YYYY';
//   }

//   ngOnInit(): void {
//     this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
//     this.getPatients();
//     this._activatedRoute.paramMap.subscribe(paramMap => {
//       this.id = paramMap.get('id');
//       if (this.id) {
//         this.getSurvey();
//       } else {
//         this.survey.hospitalId = this.hospital.id;
//         this.survey.procedures = [];
//       }
//       this.getBodyStructures();
//       this.getProcedureMethods();
//     });
//   }

//   showDateTimePickerModal(datetime: moment.Moment, callback: (result: moment.Moment) => void): void {
//     const modalRef = this._modalService.show(DateTimePickerModalComponent, {
//       initialState: {
//         datetime: datetime,
//       }
//     });
//     const modal: DateTimePickerModalComponent = modalRef.content;
//     modal.modalSave.subscribe((result: moment.Moment) => {
//       if (result) {
//         callback(result);
//       }
//     });
//   }

//   showTimePickerModal(time: string, callback: (result: string) => void): void {
//     const modalRef = this._modalService.show(TimePickerModalComponent, {
//       initialState: {
//         sTime: time,
//       }
//     });
//     const modal: TimePickerModalComponent = modalRef.content;
//     modal.modalSave.subscribe((result: string) => {
//       if (result) {
//         callback(result);
//       }
//     });
//   }

//   formatDateTime(datetime: moment.Moment): string {
//     if (datetime) {
//       return datetime.format('DD/MM/YY hh:mm A');
//     }

//     return '';
//   }

//   getMeanTimeSum(): number {
//     return _.sumBy(this.survey.procedures, 'meanTime');
//   }

//   getStandardDeviationSum(): number {
//     return _.sumBy(this.survey.procedures, 'standardDeviation');
//   }

//   onPatientSelected(e: TypeaheadMatch): void {
//     this.survey.patientDobYear = e.item.dobYear;
//   }

//   onChangeSurgeryDateClick(): void {
//     this.showDateTimePickerModal(this.survey.surgeryDate, (result: moment.Moment) => {
//       this.survey.surgeryDate = result;
//     });
//   }

//   onChangeEndTimeClick(): void {
//     this.showTimePickerModal(this.survey.endTime, (result: string) => {
//       this.survey.endTime = result;
//     });
//   }

//   onAddProcedureClick(): void {
//     const modalRef = this._modalService.show(ProcedureSearchComponent, {
//       initialState: {
//         id: this.survey.bodyStructureId,
//       },
//       class: 'modal-lg',
//     });
//     const modal: ProcedureSearchComponent = modalRef.content;
//     modal.modalSave.subscribe((menuItems: MenuItemOutputDto[]) => {
//       const procedures = _.map(menuItems, menuItem => {
//         const procedure = new PatientSurveyProcedureDto();
//         procedure.snomedId = menuItem.id;
//         procedure.name = menuItem.name;
//         procedure.meanTime = 0;
//         procedure.standardDeviation = 0;
//         return procedure;
//       });
//       this.survey.procedures.push(...procedures);
//     });
//   }

//   onApplyProcedureChangesClick(procedure: PatientSurveyProcedureDto): void {
//     delete this.clonedProcedures[procedure.name];
//   }

//   onProcedureEditClick(procedure: PatientSurveyProcedureDto): void {
//     this.clonedProcedures[procedure.name] = procedure;
//   }

//   onRemoveProcedureClick(name: string): void {
//     const i = this.survey.procedures.findIndex(e => e.name === name);
//     if (i >= 0) {
//       this.survey.procedures.splice(i, 1);
//     }
//   }

//   onForSubmit(): void {
//     this.saving = true;
//     if (this.id) {
//       this._patientSurveysService.update(this.survey)
//         .subscribe(() => {
//           this.notify.info(this.l('SavedSuccessfully'));
//           this._router.navigate(['/app/surveys']);
//         });
//     } else {
//       this._patientSurveysService.create(this.survey)
//         .subscribe(() => {
//           this.notify.info(this.l('SavedSuccessfully'));
//           this._router.navigate(['/app/surveys']);
//         });
//     }
//   }

//   private getPatients(): void {
//     this.patientsDataSource = new Observable((observer: Observer<string>) => {
//       observer.next(this.survey.patientId);
//     }).pipe(
//       switchMap((query: string) => {
//         return this._patientsService.getByHospital(this.hospital.id, query);
//       })
//     );
//   }

//   private getSurvey(): void {
//     this._patientSurveysService.get(this.id)
//       .subscribe(survey => {
//         this.survey = survey;
//       });
//   }

//   private getBodyStructures(): void {
//     this._bodyStructuresService.getAll()
//       .subscribe(bodyStructures => {
//         this.bodyStructures = bodyStructures;
//         if (!this.id && this.bodyStructures && this.bodyStructures.length > 0) {
//           this.survey.bodyStructureId = this.bodyStructures[0].id;
//         }
//       });
//   }

//   private getProcedureMethods(): void {
//     this._bodyStructuresService.getMethods()
//       .subscribe(methods => {
//         this.procedureMethods = methods;
//         if (!this.id && this.procedureMethods && this.procedureMethods.length > 0) {
//           this.survey.methodId = this.procedureMethods[0].id;
//         }
//       });
//   }
// }
