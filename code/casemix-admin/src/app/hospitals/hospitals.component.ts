import { Component, OnInit, Injector, AfterViewInit, OnDestroy } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { HospitalDto, HospitalDtoPagedResultDto, HospitalsServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { CreateEditHospitalsComponent } from './create-edit-hospitals/create-edit-hospitals.component';
@Component({
  selector: 'app-hospitals',
  templateUrl: './hospitals.component.html',
  styleUrls: ['./hospitals.component.less'],
  animations: [appModuleAnimation()],
})
export class HospitalsComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  sortColumns: string[] = ['id', 'name'];
  hospitals: HospitalDto[];
  isLoading = false;

  constructor(
    injector: Injector,
    private _hospitalsService: HospitalsServiceProxy,
    private _localStorageService: LocalStorageService,
    private _modalService: BsModalService
  ) {
    super(injector);
    this.initializeDataTable();
  }

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  onDeleteClick(id: string): void {
    abp.message.confirm('', undefined, (result: boolean) => {
      if (result) {
        this.deleteHospital(id);
      }
    });
  }

  onCreateClick(): void {
    this.showCreateEditModal();
  }

  onEditClick(id: string): void {
    this.showCreateEditModal(id);
  }

  private initializeDataTable(): void {
    this.dtOptions = this.defaultDtOptions;
    this.dtOptions.language.zeroRecords = this.l('NoTheatersAvailable');
    this.dtOptions.columnDefs = [{ targets: [-1], orderable: false }];
    this.dtOptions.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
      const orderColumn = dtParams.order[0];
      const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
      this.isLoading = true;
      this._hospitalsService
        .getAll(dtParams.search.value, sortColumn, dtParams.start, dtParams.length)
        .subscribe((result: HospitalDtoPagedResultDto) => {
          this.hospitals = result.items;
          callback({
            recordsFiltered: result.totalCount,
            data: [],
          });
          this.isLoading = false;
        });
    };
  }

  private deleteHospital(id: string): void {
    this._hospitalsService.delete(id).subscribe(() => {
      abp.notify.success(this.l('SuccessfullyDeleted'));
      this.dtTrigger.next();
    });
  }

  private showCreateEditModal(id = ''): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: id,
    };
    const modalRef = this._modalService.show(CreateEditHospitalsComponent, modalSettings);
    const modal: CreateEditHospitalsComponent = modalRef.content;
    modal.modalSave.subscribe(() => {
      this.dtTrigger.next();
    });
  }
}
