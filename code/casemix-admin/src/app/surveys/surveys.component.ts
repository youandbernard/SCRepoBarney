import { Component, Injector, AfterViewInit, OnDestroy, OnInit, ChangeDetectorRef } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
import {
  PatientSurveyDto,
  PatientSurveyDtoPagedResultDto,
  PatientSurveysServiceProxy,
  HospitalDto,
  BodyStructureGroupDto
} from '@shared/service-proxies/service-proxies';
import { Subject, Observable } from 'rxjs';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-surveys',
  templateUrl: './surveys.component.html',
  styleUrls: ['./surveys.component.less'],
  animations: [appModuleAnimation()]
})
export class SurveysComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {
  hospital: HospitalDto;
  timezone: string;
  surveys: PatientSurveyDto[];
  bodyStructureGroups: BodyStructureGroupDto[];
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  sortColumns: string[] = [
    'preOperativeAssessment.surgeryDate',
    'preOperativeAssessment.patientId',
    'preOperativeAssessment.surgeon.name',
    'preOperativeAssessment.theater.name',
    'preOperativeAssessment.bodyStructure.description'
  ];
  isSurveysLoading = false;

  isFilterReady = false;
  surgeonFilter = '';
  theaterIdFilter = '';
  bodyStructureGroupFilter = '';
  isDisplayCompletedSurveys = this.appSession.user.displayCompletedSurvey;
  constructor(
    injector: Injector,
    private _router: Router,
    private _localStorageService: LocalStorageService,
    private _patientSurveysService: PatientSurveysServiceProxy,
    private _cdr: ChangeDetectorRef
  ) {
    super(injector);
    this.initializeSurveysTable();
  }

  ngOnInit(): void {
    this.getDisplayCompletedSurveysetting();
    this.timezone = moment.tz.guess();
    this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
  }

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  onCreateSurveyClick(): void {
    this._router.navigate(['/app/surveys/manage']);
  }

  onViewSurveyClick(id: string): void {
    this._router.navigate(['/app/surveys/view', id]);
  }

  onDeleteSurveyClick(id: string): void {
    abp.message.confirm('', undefined, (result: boolean) => {
      if (result) {
        this._patientSurveysService.delete(id).subscribe(() => {
          abp.notify.success(this.l('SuccessfullyDeleted'));
          this.dtTrigger.next();
        });
      }
    });
  }

  refreshDataTable(): void {
    this.dtTrigger.next();
    this.appSession.user.displayCompletedSurvey = this.isDisplayCompletedSurveys;
    this._patientSurveysService.saveDisplayCompletedSurveySetting(this.isDisplayCompletedSurveys).subscribe(() => {});
  }

  private initializeSurveysTable(): void {
    const self = this;
    const groupColumn = 6;
    this.dtOptions = this.defaultDtOptions;
    this.dtOptions.language.zeroRecords = this.l('NoSurveyAvailable');
    this.dtOptions.orderCellsTop = true;
    this.dtOptions.columnDefs = [
      { targets: [-1, -2, -3], orderable: false },
      { searchable: false, targets: [0, 1, -1, -2, -3] },
      { visible: false, targets: groupColumn }
    ];
    this.dtOptions.columns = [
      { data: 'preOperativeAssessment.surgeryDate' },
      { data: 'preOperativeAssessment.patientId' },
      { data: 'preOperativeAssessment.surgeon.fullName' },
      { data: 'preOperativeAssessment.theater.name' },
      { data: 'preOperativeAssessment.bodyStructure.bodyStructureGroup.name' },
      { data: 'totalMeanTime' },
      { data: 'weekOfYear' },
      {
        data: 'id',
        render: function(data, type, full, meta) {
          if (full.isAdmin) {
            return `<button type="button" class="btn btn-sm bg-secondary view-survey" data-id="${data}">
              <i class="fas fa-eye"></i>
            </button>
            <button type="button" class="btn btn-sm bg-danger mx-2 delete-survey" data-id="${data}">
              <i class="fas fa-trash"></i>
            </button></td>`;
          } else {
            return `<button type="button" class="btn btn-sm bg-secondary view-survey" data-id="${data}">
              <i class="fas fa-eye"></i>
            </button></td>`;
          }
        }
      }
    ];
    this.dtOptions.rowGroup = {
      dataSrc: 'weekOfYear'
    };
    this.dtOptions.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
      const orderColumn = dtParams.order[0];
      const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
      this.isSurveysLoading = true;
      const surgeonFilter = this.setUndefinedIfEmpty(this.surgeonFilter);
      const theaterIdFilter = this.setUndefinedIfEmpty(this.theaterIdFilter);
      const bodyStructureGroupFilter = this.setUndefinedIfEmpty(this.bodyStructureGroupFilter);

      this._patientSurveysService
        .getAll(
          this.hospital.id,
          dtParams.search.value,
          surgeonFilter,
          theaterIdFilter,
          this.isDisplayCompletedSurveys,
          bodyStructureGroupFilter,
          this.timezone,
          sortColumn,
          dtParams.start,
          dtParams.length
        )
        .subscribe((result: PatientSurveyDtoPagedResultDto) => {
          this.surveys = result.items;
          callback({
            recordsFiltered: result.totalCount,
            data: result.items
          });
          this.isSurveysLoading = false;
        });
    };

    this.dtOptions.initComplete = function() {
      if (!self.isFilterReady) {
        this.api()
          .columns()
          .every(function() {
            if (this.searchable()) {
              const that = this;
              const index = that.index();

              const select = $('<select><option value="">All</option></select>')
                .appendTo($('thead tr:eq(1) td').eq(this.index()))
                .on('change', function() {
                  const val = $(this)
                    .val()
                    .toString();
                  if (index === 2) {
                    self.surgeonFilter = val;
                  }
                  if (index === 3) {
                    self.theaterIdFilter = val;
                  }
                  if (index === 4) {
                    self.bodyStructureGroupFilter = val;
                  }
                  self.dtTrigger.next();
                });

              if (index === 2) {
                self._patientSurveysService.getSurgeons(self.hospital.id).subscribe(surgeons => {
                  _.forEach(surgeons, surgeon => {
                    select.append($(`<option value="${surgeon.id}">${surgeon.fullName}</option>`));
                  });
                });
              }

              if (index === 3) {
                self._patientSurveysService.getTheaterIds(self.hospital.id).subscribe(theaterIds => {
                  _.forEach(theaterIds, theaterId => {
                    select.append($(`<option value="${theaterId}">${theaterId}</option>`));
                  });
                });
              }

              if (index === 4) {
                self._patientSurveysService.getBodyStructureGroups(self.hospital.id).subscribe(groups => {
                  _.forEach(groups, group => {
                    select.append($(`<option value="${group.id}">${group.name}</option>`));
                  });
                });
              }
            }
          });

        $(document).on('click', '.view-survey', function(e) {
          self.onViewSurveyClick($(this).data('id'));
        });
        $(document).on('click', '.delete-survey', function(e) {
          self.onDeleteSurveyClick($(this).data('id'));
        });
      }
      self.isFilterReady = true;
    };
  }

  private getDisplayCompletedSurveysetting(): void {
    this._patientSurveysService.getDisplayCompletedSurveySetting().subscribe(setting => {
      this.isDisplayCompletedSurveys = setting.displayCompletedSurvey;
      this._cdr.detectChanges();
    });
  }
}
