import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PatientSurveyNotesDto, PatientSurveysServiceProxy, PoapProcedureDto } from '@shared/service-proxies/service-proxies';
import { PatientSurveyNotesModel } from '@shared/models/survey-notes'
import * as _ from 'lodash';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-view-survey-notes',
  templateUrl: './view-survey-notes.component.html',
  styleUrls: ['./view-survey-notes.component.less']
})
export class ViewSurveyNotesComponent implements OnInit {

  @Input() patientSurveyId: string;
  @Input() forNotes: PoapProcedureDto[] = [];
  @Input() existingSurveyNotes: PatientSurveyNotesDto[];
  @Output() modalNoteSave: EventEmitter<boolean> = new EventEmitter<boolean>();

  surveyNotes: PatientSurveyNotesDto[] = [];
  surveyNote: PatientSurveyNotesDto;

  surveyNotesModel: PatientSurveyNotesModel[] = [];
  surveyNoteModel: PatientSurveyNotesModel;

  isMaxPage: boolean = false;
  isMinPage: boolean = true;
  notePaging: string = "Note 1 of 1";
  paging: number = 1;

  constructor(
    private _modalRef: BsModalRef,
    private _patientSurveyService: PatientSurveysServiceProxy
  ) { }

  ngOnInit(): void {
    if (this.patientSurveyId){
      this.defaultValues();
    }
  }

  defaultValues(): void {
    if (this.existingSurveyNotes) {
      this.prepareDataModelToEditView();
    } else {
      if (this.forNotes) {
        this.prepareDataModelToView();
      }
    }
  }

  onModalSubmit(): void {    
    var _surveyNotes = this.prepareDataViewToModel();
    if (_surveyNotes !== undefined) {
      if (_surveyNotes !== null) {
        this._patientSurveyService.saveSurveyNotes(
          this.patientSurveyId,          
          _surveyNotes
        ).subscribe(survey => {
            this.modalNoteSave.emit(survey);
            this.onCloseClick();
        });
      }
    }
  }

  prepareDataModelToEditView(): void {
    _.forEach(this.forNotes, (fn) => {
      if(fn.hasSurveyNotes){
        this.surveyNoteModel = new PatientSurveyNotesModel;

        this.surveyNoteModel.patientSurveyId = this.patientSurveyId;
        this.surveyNoteModel.poapProcedureId = fn.id;
        this.surveyNoteModel.preOperativeAssessmentId = fn.preOperativeAssessmentId;
        this.surveyNoteModel.poapProcedureName = fn.name;

        let map = new Map<string, string>();
        _.filter(this.existingSurveyNotes, (f) => f.poapProcedureId == fn.id).map(m => {
          map.set(`tab${m.noteTabs}noteDescription`, m.noteDescription);
        })
        this.surveyNoteModel.tab1noteDescription = map.get('tab1noteDescription');
        this.surveyNoteModel.tab2noteDescription = map.get('tab2noteDescription');
        this.surveyNoteModel.tab3noteDescription = map.get('tab3noteDescription');
        this.surveyNoteModel.tab4noteDescription = map.get('tab4noteDescription');
        this.surveyNoteModel.tab5noteDescription = map.get('tab5noteDescription');
        map.clear();
        this.surveyNotesModel.push(this.surveyNoteModel);
      }
    });

    this.notePaging = "Note " + this.paging + " of " + this.surveyNotesModel.length;
  }

  prepareDataModelToView(): void {
    _.forEach(this.forNotes, (fn) => {
      this.surveyNoteModel = new PatientSurveyNotesModel;

      this.surveyNoteModel.patientSurveyId = this.patientSurveyId;
      this.surveyNoteModel.poapProcedureId = fn.id;
      this.surveyNoteModel.preOperativeAssessmentId = fn.preOperativeAssessmentId;
      this.surveyNoteModel.poapProcedureName = fn.name;
      this.surveyNoteModel.tab1noteDescription = "";
      this.surveyNoteModel.tab1noteDescription = "";
      this.surveyNoteModel.tab2noteDescription = "";
      this.surveyNoteModel.tab3noteDescription = "";
      this.surveyNoteModel.tab4noteDescription = "";
      this.surveyNoteModel.tab5noteDescription = "";

      this.surveyNotesModel.push(this.surveyNoteModel);
    });    

    this.notePaging = "Note " + this.paging + " of " + this.surveyNotesModel.length;
  }

  prepareDataViewToModel(): PatientSurveyNotesDto[] {
    if (this.surveyNotesModel.length > 0 ) {

      _.forEach(this.surveyNotesModel, (model) => {
          this.surveyNote = new PatientSurveyNotesDto;
        
          this.surveyNote.patientSurveyId = model.patientSurveyId;
          this.surveyNote.poapProcedureId = model.poapProcedureId;
          this.surveyNote.preOperativeAssessmentId = model.preOperativeAssessmentId;
          this.surveyNote.noteDescription = model.tab1noteDescription;
          this.surveyNote.noteSeries = 1
          this.surveyNote.noteTabs = 1;
          
          this.surveyNotes.push(this.surveyNote);

          this.surveyNote = new PatientSurveyNotesDto;
        
          this.surveyNote.patientSurveyId = model.patientSurveyId;
          this.surveyNote.poapProcedureId = model.poapProcedureId;
          this.surveyNote.preOperativeAssessmentId = model.preOperativeAssessmentId;
          this.surveyNote.noteDescription = model.tab2noteDescription;
          this.surveyNote.noteSeries = 1
          this.surveyNote.noteTabs = 2;
          
          this.surveyNotes.push(this.surveyNote);

          this.surveyNote = new PatientSurveyNotesDto;
        
          this.surveyNote.patientSurveyId = model.patientSurveyId;
          this.surveyNote.poapProcedureId = model.poapProcedureId;
          this.surveyNote.preOperativeAssessmentId = model.preOperativeAssessmentId;
          this.surveyNote.noteDescription = model.tab3noteDescription;
          this.surveyNote.noteSeries = 1
          this.surveyNote.noteTabs = 3;
          
          this.surveyNotes.push(this.surveyNote);

          this.surveyNote = new PatientSurveyNotesDto;
        
          this.surveyNote.patientSurveyId = model.patientSurveyId;
          this.surveyNote.poapProcedureId = model.poapProcedureId;
          this.surveyNote.preOperativeAssessmentId = model.preOperativeAssessmentId;
          this.surveyNote.noteDescription = model.tab4noteDescription;
          this.surveyNote.noteSeries = 1
          this.surveyNote.noteTabs = 4;
          
          this.surveyNotes.push(this.surveyNote);

          this.surveyNote = new PatientSurveyNotesDto;
        
          this.surveyNote.patientSurveyId = model.patientSurveyId;
          this.surveyNote.poapProcedureId = model.poapProcedureId;
          this.surveyNote.preOperativeAssessmentId = model.preOperativeAssessmentId;
          this.surveyNote.noteDescription = model.tab5noteDescription;
          this.surveyNote.noteSeries = 1
          this.surveyNote.noteTabs = 5;
          
          this.surveyNotes.push(this.surveyNote);

      });
    }

    return this.surveyNotes;
  }

  setPrevious(): void {
    if (this.paging == 1) {
      this.paging = 1;
      this.isMinPage = true;
      this.isMaxPage = false;
    }
    else {
      this.paging -= 1; 
      
      if (this.paging == 1){
        this.isMinPage = true;
        this.isMaxPage = false;
      } else {
        this.isMinPage = false;
        this.isMaxPage = false;
      }    
    }      

    this.notePaging = "Note " + this.paging + " of " + this.surveyNotesModel.length;
  }

  setNext(): void {
    if (this.paging == this.surveyNotesModel.length) {
      this.paging = this.surveyNotesModel.length;
      this.isMinPage = false;
      this.isMaxPage = true;
    }
    else {
      this.paging += 1;
      if (this.paging == this.surveyNotesModel.length) {
        this.isMinPage = false;
        this.isMaxPage = true;
      }else {
        this.isMinPage = false;
        this.isMaxPage = false;
      }
    }

    this.notePaging = "Note " + this.paging + " of " + this.surveyNotesModel.length;
  }
  
  onCloseClick(): void {
    this._modalRef.hide();
  }
}  
