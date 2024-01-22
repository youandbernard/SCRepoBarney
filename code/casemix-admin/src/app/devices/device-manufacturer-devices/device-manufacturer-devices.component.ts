import { AfterViewInit, Component, Injector, Input, OnDestroy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DeviceDto, DeviceDtoPagedResultDto, DeviceServiceProxy } from '@shared/service-proxies/service-proxies';
import { Subject } from 'rxjs';
import { BsModalService } from 'ngx-bootstrap/modal';
import { DeviceGmdntermcodesComponent } from '../device-management-devices/device-gmdntermcodes/device-gmdntermcodes.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-device-manufacturer-devices',
  templateUrl: './device-manufacturer-devices.component.html',
  styleUrls: ['./device-manufacturer-devices.component.less'],
  animations: [appModuleAnimation()],
})

export class DeviceManufacturerDevices extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  sortColumns: string[] = ['id', 'gmdnTermCode', 'deviceName', 'bodyStructureGroup.name', 'deviceClass.class', 'brandName', 'model', 'createdDate', 'status'];
  devices: DeviceDto[];
  isLoading = false;

  manufacturerUser: boolean = true;
  // role: string;

  constructor(injector: Injector,
    private _devicesService: DeviceServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }


  ngOnInit(): void {
    this.initializeDataTable();
  }

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }


  private initializeDataTable(): void {
    this.dtOptions = this.defaultDtOptions;
    this.dtOptions.language.zeroRecords = this.l('NoDeviceAvailable');
    this.dtOptions.columnDefs = [{ targets: [], orderable: false }];
    this.dtOptions.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
      const orderColumn = dtParams.order[0];
      const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
      this.isLoading = true;
      this._devicesService
        .getAll("", "", "", "", false, false, dtParams.search.value, sortColumn, dtParams.start, dtParams.length)
        .subscribe((result: DeviceDtoPagedResultDto) => {
          this.devices = result.items;

          callback({
            recordsFiltered: result.totalCount,
            data: [],
          });
          this.isLoading = false;
        });
    };
  }

  getgmdnTermCode(code: string): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-lg';
    modalSettings.initialState = {
      gmdntermcode: code
    };

    const modalRef = this._modalService.show(DeviceGmdntermcodesComponent, modalSettings);
    const modal: DeviceGmdntermcodesComponent = modalRef.content;
  }
}
