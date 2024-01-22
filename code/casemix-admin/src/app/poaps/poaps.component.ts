import { Component, OnInit, Injector, AfterViewInit, OnDestroy } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
import {
  PreOperativeAssessmentsServiceProxy,
  PreOperativeAssessmentDtoPagedResultDto,
  PreOperativeAssessmentDto,
  HospitalDto
} from '@shared/service-proxies/service-proxies';
import { Subject } from 'rxjs';
import { LocalStorageService } from '@shared/services/local-storage.service';

@Component({
  selector: 'app-poaps',
  templateUrl: './poaps.component.html',
  styleUrls: ['./poaps.component.less'],
  animations: [appModuleAnimation()]
})
export class PoapsComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {
  hospital: HospitalDto;
  preOperativeAssessments: PreOperativeAssessmentDto[];
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  sortColumns: string[] = ['patientId', 'surgeonName', 'anesthetistName', 'theater.theaterId', 'bodyStructure.description'];
  isPoapsLoading = false;
  isDisplayRiskAwaitingCompletion = false;
  isFirstStart = true;

  constructor(
    injector: Injector,
    private _router: Router,
    private _localStorageService: LocalStorageService,
    private _preOperativeAssessmentsService: PreOperativeAssessmentsServiceProxy
  ) {
    super(injector);
    this.initializeAssessmentsTable();
  }

  ngOnInit(): void {
    this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
  }

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  onCreatePoapClick(): void {
    this._router.navigate(['/app/poaps/manage']);
  }

  onEditPoapClick(id: string): void {
    this._router.navigate(['/app/poaps/manage', id]);
  }

  onDeletePoapClick(id: string): void {
    abp.message.confirm('', undefined, (result: boolean) => {
      if (result) {
        this._preOperativeAssessmentsService.delete(id).subscribe(() => {
          abp.notify.success(this.l('SuccessfullyDeleted'));
          this.dtTrigger.next();
        });
      }
    });
  }

  refreshDataTable(): void {
    this.dtTrigger.next();
  }

  private initializeAssessmentsTable(): void {
    this.dtOptions = this.defaultDtOptions;
    this.dtOptions.language.zeroRecords = this.l('NoPoapAvailable');
    this.dtOptions.columnDefs = [{ targets: [5, 6], orderable: false }];
    this.dtOptions.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
      const orderColumn = dtParams.order[0];
      const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
      this.isPoapsLoading = true;
      this._preOperativeAssessmentsService
        .getAll(this.hospital.id, dtParams.search.value, this.isDisplayRiskAwaitingCompletion, sortColumn, dtParams.start, dtParams.length)
        .subscribe((result: PreOperativeAssessmentDtoPagedResultDto) => {
          this.preOperativeAssessments = result.items;
          callback({
            recordsFiltered: result.totalCount,
            data: []
          });
          this.isPoapsLoading = false;
        });
    };
  }
}
