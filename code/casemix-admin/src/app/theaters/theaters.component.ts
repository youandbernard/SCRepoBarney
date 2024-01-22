import { Component, OnInit, Injector, AfterViewInit, OnDestroy } from '@angular/core';
import {
  TheaterDto,
  TheatersServiceProxy,
  HospitalDto,
  TheaterDtoPagedResultDto,
  PreOperativeAssessmentsServiceProxy,
  PreOperativeAssessmentDto
} from '@shared/service-proxies/service-proxies';
import { Subject } from 'rxjs';
import { AppComponentBase } from '@shared/app-component-base';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BsModalService } from 'ngx-bootstrap/modal';
import { CreateEditTheaterComponent } from './create-edit-theater/create-edit-theater.component';

@Component({
  selector: 'app-theaters',
  templateUrl: './theaters.component.html',
  styleUrls: ['./theaters.component.less'],
  animations: [appModuleAnimation()]
})
export class TheatersComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {
  hospital: HospitalDto;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  theaters: TheaterDto[] = [];
  sortColumns: string[] = ['theaterId', 'name'];
  preOperativeAssessments: PreOperativeAssessmentDto[];
  isLoading = false;

  constructor(
    injector: Injector,
    private _theatersService: TheatersServiceProxy,
    private _localStorageService: LocalStorageService,
    private _modalService: BsModalService,
    private _preOperativeAssessmentsService: PreOperativeAssessmentsServiceProxy
  ) {
    super(injector);
    this.initializeDataTable();
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

  onCreateClick(): void {
    this.showCreateEditModal();
  }

  onEditClick(id: string): void {
    this.showCreateEditModal(id);

  }

  onDeleteClick(id: string): void {
    abp.message.confirm('', undefined,
      (result: boolean) => {
        if (result) {
          this._preOperativeAssessmentsService.getPoapDataByTheater(id)
          .subscribe((poaps) => {
            if (poaps.length === 0) {
              this.deleteTheater(id);
            } else {
              abp.message.error('Theater is currently assigned to a preoperative assessment.')
            }
          });
        }
      }
    );
  }

  private initializeDataTable(): void {
    this.dtOptions = this.defaultDtOptions;
    this.dtOptions.language.zeroRecords = this.l('NoTheatersAvailable');
    this.dtOptions.columnDefs = [{ targets: [-1], orderable: false }];
    this.dtOptions.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
      const orderColumn = dtParams.order[0];
      const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
      this.isLoading = true;
      this._theatersService
        .getAll(this.hospital.id, dtParams.search.value, sortColumn, dtParams.start, dtParams.length)
        .subscribe((result: TheaterDtoPagedResultDto) => {
          this.theaters = result.items;
          callback({
            recordsFiltered: result.totalCount,
            data: [],
          });
          this.isLoading = false;
        });
    };
  }

  private deleteTheater(id: string): void {
    this._theatersService.delete(id).subscribe(() => {
      abp.notify.success(this.l('SuccessfullyDeleted'));
      this.dtTrigger.next();
    });
  }

  private showCreateEditModal(id = ''): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: id,
      hospitalId: this.hospital.id,
    };
    const modalRef = this._modalService.show(CreateEditTheaterComponent, modalSettings);
    const modal: CreateEditTheaterComponent = modalRef.content;
    modal.modalSave.subscribe(() => {
      this.dtTrigger.next();
    });
  }
}
