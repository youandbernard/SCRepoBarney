import { Component, OnInit, Injector, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PatientSurveyDto,
  PatientSurveysServiceProxy,
  HospitalDto,
  BodyStructureDto,
  BodyStructureGroupDto,
  PoapProcedureDto,
  PreOperativeAssessmentDto,
  ProcedureMethodDto,
  TimeSpan,
  SurveyTimestampServiceProxy,
  SurveyTimestampSettingDto,
  PatientSurveyNotesDto,
  UserServiceProxy
} from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { BsModalService } from 'ngx-bootstrap/modal';
import { EditActualTimeComponent } from './edit-actual-time/edit-actual-time.component';
import { SurveyProcedureSelectDeviceComponent } from './procedure-select-device-survey/procedure-select-device-survey.component';
import * as moment from 'moment';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { tz } from 'moment-timezone';
import { start } from 'repl';
import { ViewSurveyNotesComponent } from './view-survey-notes/view-survey-notes.component';

@Component({
  selector: 'app-view-survey',
  templateUrl: './view-survey.component.html',
  styleUrls: ['./view-survey.component.less'],
  animations: [appModuleAnimation()]
})
export class ViewSurveyComponent extends AppComponentBase implements OnInit {
  id: string;
  survey: PatientSurveyDto = new PatientSurveyDto();
  forNotes: PoapProcedureDto[] = [];

  isLoading = false;
  procedures = [new PoapProcedureDto()];
  surveyHospitalSetting: SurveyTimestampSettingDto = new SurveyTimestampSettingDto();
  hospital: HospitalDto;
  isSettingDisabled: boolean;
  defaultStartTime: string;
  moment: any = moment;
  date: moment.Moment;
  dateStartTime: moment.Moment;
  startTime = '';
  startDate = '';
  startTimeInput: moment.Moment;
  timeZone: string;

  role: string;
  isSurgeon: boolean = false;
  isAdmin: boolean = false;
  isFilterLicensedStatus: boolean = false;

  constructor(
    injector: Injector,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _modalService: BsModalService,
    private _patientSurveysService: PatientSurveysServiceProxy,
    private _surveyTimestampSettingService: SurveyTimestampServiceProxy,
    private _localStorageService: LocalStorageService,
    private _usersService: UserServiceProxy,
  ) {
    super(injector);
    this.survey.hospital = new HospitalDto();
    this.survey.bodyStructure = new BodyStructureDto();
    this.survey.bodyStructure.bodyStructureGroup = new BodyStructureGroupDto();
    this.survey.preOperativeAssessment = new PreOperativeAssessmentDto();
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.timeZone = moment.tz.guess();
    this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    this.getSetting();
    this.getUserDetails();
    this._activatedRoute.paramMap.subscribe(paramMap => {
      this.id = paramMap.get('id');
      if (this.id) {
        this.getSurvey();
      }
      setInterval(() => {
        this.date = moment(new Date());
      }, 1000);
    });
    this.isLoading = false;
  }

  setStartTimeClockClick(date: Date) {
    this.survey.startTime = moment(date).format('h:mm A');
    this.survey.dateStart = moment(date);
    this.dateStartTime = moment(date);
    this.startTime = moment.utc(date).format('h:mm A');
    if (this.survey.dateStart) {
      this.isLoading = true;
      this._patientSurveysService.saveSurveyStartTime(this.survey.id, this.dateStartTime, this.timeZone).subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.isLoading = false;
      });
    }
  }

  private getUserDetails(): void {
    this._usersService.get(this.appSession.userId).subscribe((userInfo) => {
      this.role = userInfo.roleNames.find((u) => u === 'SUPER ADMIN' || u === 'ADMIN' || u === 'SURGEON');

      if (this.role === 'SUPER ADMIN' || this.role === 'ADMIN') {
        this.isAdmin = true;
      }

      if (this.role === 'SURGEON') {
        this.isSurgeon = true;
      }

      if(this.role === 'SURGEON' || this.role === 'ADMIN'){
        this.isFilterLicensedStatus = true;
      }
    });
  }


  getActualTimeSum(): number {
    const actualTime = _.sumBy(this.survey.preOperativeAssessment.procedures, 'actualTime');
    return parseFloat(actualTime.toFixed(2));
  }

  getMeanTimeSum(): number {
    return _.sumBy(this.survey.preOperativeAssessment.procedures, 'meanTime');
  }

  getStandardDeviationSum(): number {
    return _.sumBy(this.survey.preOperativeAssessment.procedures, 'standardDeviation');
  }

  getTotalProcedureTime(): number {
    let minutes = 0;
    const dates = this.procedures.filter(procedure => {
      if (procedure.clockStartTimestamp && procedure.clockEndTimestamp) {
        const duration = procedure.clockEndTimestamp.diff(procedure.clockStartTimestamp);
        minutes = minutes + duration;
      }
    });
    const response = minutes / 60000;
    return response;
  }

  getAllProceduresEndTime(): moment.Moment {
    let response: moment.Moment;
    const dates = this.procedures.filter(procedure => {
      if (procedure.clockStartTimestamp && procedure.clockEndTimestamp) {
        return procedure;
      }
    });
    if (dates.length > 0) {
      const latest = dates.sort((a, b) => b.clockStartTimestamp.valueOf() - a.clockStartTimestamp.valueOf())[0];
      response = moment(latest.clockStartTimestamp).add(latest.actualTime, 'm');
    }
    return dates.length > 0 ? response : null;
  }

  getElapsedTime(): any {
    const startTimeRegex = this.getRegexValues(this.survey.startTime);
    const allProcedureEndTimeRegex = this.getRegexValues(moment(this.getAllProceduresEndTime()).format('LT'));
    const startTime = startTimeRegex.split(':').map(function(item) {
      return Number(item);
    });
    const endTime = allProcedureEndTimeRegex.split(':').map(function(item) {
      return Number(item);
    });

    startTime[0] = this.survey.startTime.includes('pm') ? startTime[0] + 12 : startTime[0];
    endTime[0] = moment(this.getAllProceduresEndTime())
      .format('LT')
      .includes('PM')
      ? endTime[0] + 12
      : endTime[0];

    const minutes = endTime[1] > startTime[1] ? endTime[1] - startTime[1] : startTime[1] - endTime[1];
    const hours = endTime[0] > startTime[0] ? endTime[0] - startTime[0] : startTime[0] - endTime[0];
    const minutesValue = minutes > 10 ? minutes : '0' + minutes;
    const hourValue = hours > 10 ? hours : '0' + hours;
    if (moment(this.getAllProceduresEndTime()).format('LT') === 'Invalid date') {
      return null;
    }

    return hourValue + ':' + minutesValue;
  }

  getElapsedTimeTotal(): any {
    if (this.getAllProceduresEndTime()) {
      const endTime = this.getAllProceduresEndTime()
        .toDate()
        .getTime();
      const startTime = this.survey.dateStart.toDate().getTime();

      let diff = (endTime - startTime) / 1000;
      diff /= 60;
      const totalMinutes = Math.abs(Math.round(diff));
      const hours = Math.floor(totalMinutes / 60) >= 10 ? Math.floor(totalMinutes / 60) : '0' + Math.floor(totalMinutes / 60);
      const minutes = totalMinutes % 60 >= 10 ? totalMinutes % 60 : '0' + (totalMinutes % 60);

      return hours + ':' + minutes;
    }
  }

  public onApplyDevice(procedure: PoapProcedureDto): void {

    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-xl';
    modalSettings.initialState = {
      procedure: procedure,
      bodyStructureId: this.survey.preOperativeAssessment.bodyStructureId,
      bodyStructureGroupId: this.survey.preOperativeAssessment.bodyStructure.bodyStructureGroup.id,
      isFilterLicensedStatus: this.isFilterLicensedStatus
    };

    const modalRef = this._modalService.show(SurveyProcedureSelectDeviceComponent, modalSettings);
    const modal: SurveyProcedureSelectDeviceComponent = modalRef.content;
    modal.returnedProcedure.subscribe((poapProcedure: PoapProcedureDto) => {
      if (poapProcedure) {
        modal.onCloseClick();
        _.forEach(this.survey.preOperativeAssessment.procedures, (proc) => {
          if (proc.snomedId == poapProcedure.snomedId &&
            proc.preOperativeAssessmentId == poapProcedure.preOperativeAssessmentId &&
            proc.id == poapProcedure.id) {
            proc.poapProcedureDevices = poapProcedure.poapProcedureDevices;
          }
        });
      }
    });
  }

  onEditProdureClick(procedure: PoapProcedureDto): void {
    const modalRef = this._modalService.show(EditActualTimeComponent, {
      initialState: {
        procedure: procedure
      },
      ignoreBackdropClick: true
    });

    const modal: EditActualTimeComponent = modalRef.content;
    modal.modalSave.subscribe((ret: { actualTime: number, isAddNotes: boolean}) => {
      let startTime: moment.Moment;
      let endTime: moment.Moment;
      
      if (this.isSettingDisabled) {
        startTime = null;
        endTime = null;
      } else {
        startTime = moment(procedure.clockStartTimestamp);
        endTime = moment(procedure.clockEndTimestamp);
      }

      if (ret.actualTime) {
        procedure.actualTime = ret.actualTime;
        this._patientSurveysService.updateProcedureActualTime(procedure.id, ret.actualTime, startTime, endTime, this.timeZone).subscribe(() => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.getAllProceduresEndTime();

          this.addNotes(ret.isAddNotes, procedure);
          procedure.hasSurveyNotes = ret.isAddNotes;
        });
      }
    });
  }

  onCloseClick(): void {
    this._router.navigate(['/app/surveys']);
  }

  onRowChange(): void {
    this.procedures = this.procedures.map((procedure, index) => {
      procedure.displayOrder = index;
      return procedure;
    });

    this._patientSurveysService.upateProceduresDisplayOrder(this.survey.preOperativeAssessment.id, this.procedures).subscribe(() => {});
  }

  onReplicateProcedureClick(procedure: PoapProcedureDto): void {
    this.survey.isReplicate = true;
    this._patientSurveysService.updatePatientSurvey(this.survey).subscribe(() => {
      this._patientSurveysService.replicateProcedure(procedure.id, procedure.preOperativeAssessmentId).subscribe(() => {
        this._patientSurveysService.getSurvey(this.id, this.timeZone).subscribe(survey => {
          this.survey = survey;
          this.procedures = survey.preOperativeAssessment.procedures.sort(
            (n1: PoapProcedureDto, n2: PoapProcedureDto) => n1.displayOrder - n2.displayOrder
          );
          this.notify.info(this.l('SavedSuccessfully'));

          this.validateIfSelectedForNotes(this.procedures);
        });
      });
    });
  }

  validateIfSelectedForNotes(procedures: PoapProcedureDto[]): void {
    if (this.forNotes) {
      if (this.forNotes.length > 0) {
        if (procedures) {
            if (procedures.length > 0) {                
              _.forEach(procedures, (proc) => {
                  _.forEach(this.forNotes, (fn) => {
                      if (proc.id === fn.id) {
                        proc.hasSurveyNotes = fn.hasSurveyNotes;
                      }
                  });
              });
            }
        }
      }
    }
  }

  onForSubmit(): void {
    this.isLoading = true;
    this.survey.preOperativeAssessment.isArchived = true;
    this._patientSurveysService.updatePatientSurvey(this.survey).subscribe(() => {
      this.notify.info(this.l('SavedSuccessfully'));
      this.isLoading = false;

      if (this.forNotes) {
        if (this.forNotes.length > 0) {
          this.callAddNotesForm();
        } else {
          this._router.navigate(['/app/surveys']);
        }
      } else {
        this._router.navigate(['/app/surveys']);
      }
    });
  }

  isLatestDuplicate(procedure: PoapProcedureDto): boolean {
    const procedures = this.procedures.filter(s => s.name.toLocaleLowerCase() === procedure.name.toLocaleLowerCase());
    if (procedures.length === 1) {
      return true;
    }

    const maxDisplayOrder: number = Math.max.apply(
      Math,
      procedures.map(function(o) {
        return o.displayOrder;
      })
    );

    if (procedure.displayOrder === maxDisplayOrder) {
      return true;
    }

    return false;
  }

  disabledCompleteButton(): boolean {
    if (!this.procedures || this.procedures.length <= 0) {
      return true;
    }

    if (this.survey.preOperativeAssessment.isArchived) {
      return true;
    }
    const noActualTimeProcedures = this.procedures.find(procedure => procedure.actualTime === 0);
    return !noActualTimeProcedures ? false : true;
  }

  onSaveNotes(): void {
    this._patientSurveysService.updateNotes(this.survey.id, this.survey.observerNotes).subscribe(() => {
      this.notify.info(this.l('SavedSuccessfully'));
    });
  }

  onAddNotes($event, thisProcedure: PoapProcedureDto): void {
    thisProcedure.hasSurveyNotes = $event.target.checked;
    this.addNotes($event.target.checked, thisProcedure);
  }

  addNotes(val: boolean, proc: PoapProcedureDto): void {
    if (val === true){
      const index = this.forNotes.findIndex((e) => e.id == proc.id);
      if (index < 0) {
        this.forNotes.push(proc);
      }
    }
    else {
      const index = this.forNotes.findIndex((e) => e.id == proc.id);
      if (index >= 0) {
        this.forNotes.splice(index, 1);
      }
    }
  }

  callAddNotesForm() {
    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-lg';
    modalSettings.initialState = {
      patientSurveyId: this.survey.id,
      forNotes: this.forNotes,
      existingSurveyNotes: this.survey.patientSurveyNotes
    };
    modalSettings.ignoreBackdropClick = true;

    const modalRef = this._modalService.show(ViewSurveyNotesComponent, modalSettings);
    const modal: ViewSurveyNotesComponent = modalRef.content;
    modal.modalNoteSave.subscribe((result: boolean) => {
      if (result) {
        this.notify.info(this.l('Succesfully added notes.'));
        this._router.navigate(['/app/surveys']);
      } else {
        this.notify.info(this.l('Fail to add notes.'));
      }
    });
  }

  callEditNotesForm() {
    const isDeleteAllSurveyNotes = _.filter(this.survey.preOperativeAssessment.procedures, (f) => f.hasSurveyNotes).length == 0;
    if(this.survey.patientSurveyNotes.length == 0 && isDeleteAllSurveyNotes){
      this.notify.info(this.l('Please select a Sub-Procedure to add notes.'));
      return;
    }
    const modalSettings = this.defaultModalSettings;
    modalSettings.class = isDeleteAllSurveyNotes ? 'modal-md' : 'modal-lg';
    modalSettings.initialState = {
      patientSurveyId: this.survey.id,
      forNotes: this.survey.preOperativeAssessment.procedures,
      existingSurveyNotes: this.survey.patientSurveyNotes
    };
    modalSettings.ignoreBackdropClick = true;

    const modalRef = this._modalService.show(ViewSurveyNotesComponent, modalSettings);
    const modal: ViewSurveyNotesComponent = modalRef.content;
    modal.modalNoteSave.subscribe((result: boolean) => {
      if (result || isDeleteAllSurveyNotes) {
        this.notify.info(this.l('Succesfully updated notes.'));
        this._router.navigate(['/app/surveys']);
      } else {
        this.notify.info(this.l('Fail to update notes.'));
      }
    });
  }

  private getSurvey(): void {
    this.isLoading = true;
    this._patientSurveysService.getSurvey(this.id, this.timeZone).subscribe(survey => {
      this.survey = survey;
      this.procedures = survey.preOperativeAssessment.procedures.sort(
        (n1: PoapProcedureDto, n2: PoapProcedureDto) => n1.displayOrder - n2.displayOrder
      );
      this.isLoading = false;

      this.validateIfSelectedForNotes(this.procedures);
    });
  }

  private formatAMPM(date) {
    date = new Date(date);
    let hours = date.getHours();
    let minutes = date.getMinutes();
    const ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    const strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
  }

  private getSetting(): void {
    this.isLoading = true;
    this._surveyTimestampSettingService.getAll().subscribe(settings => {
      this.surveyHospitalSetting = _.find(settings, e => e.hospitalId === this.hospital.id);
      if (this.surveyHospitalSetting && this.surveyHospitalSetting.isEnabled) {
        this.isSettingDisabled = false;
      } else {
        this.isSettingDisabled = true;
      }
    });
  }

  private convertUTCDateToLocalDate(date: Date): Date {
    return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
  }

  private formatDateTime(date) {
    return moment.utc(moment(date).format('YYYY-MM-DD hh:mm:ss'));
  }

  private getRegexValues(value: string): any {
    const mapObj = { am: '', pm: '', AM: '', PM: '' };

    const re = new RegExp(Object.keys(mapObj).join('|'), 'gi');
    value = value.replace(re, function(matched) {
      return mapObj[matched];
    });

    return value;
  }
}
