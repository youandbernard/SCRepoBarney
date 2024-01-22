import { Component, ElementRef, Injector, NgModule, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import {
  BodyStructureGroupDto, BodyStructureGroupsServiceProxy, DocumentFileDto, DocumentFileDtoPagedResultDto, DocumentServiceProxy,
  DeviceServiceProxy, DeviceClassDto, DeviceFamilyDto
} from '@shared/service-proxies/service-proxies';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ImportDeviceComponent } from './import-device/import-device.component';
import { base64StringToBlob } from 'blob-util';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-device-management-import',
  templateUrl: './device-management-import.component.html',
  styleUrls: ['./device-management-import.component.less'],
  animations: [appModuleAnimation()],
})
export class DeviceManagementImportComponent extends AppComponentBase implements OnInit {

  //Datatable
  dtTriggerAdmin: Subject<any> = new Subject();
  dtOptionsAdmin: DataTables.Settings = {};
  documents: DocumentFileDto[];

  specialties: BodyStructureGroupDto[] = [];
  deviceClass: DeviceClassDto[] = [];
  deviceFamily: DeviceFamilyDto[] = [];
  selectedSpecialty: string = '';
  selectedClass: string = '';
  selectedFamily: number = 0;

  sortColumns: string[] = ['id', 'filename', 'filepath', 'filetype', 'enable', 'modifiedDate', 'manufacturer', 'specialtyName', 'deviceFamilyName', 'deviceClassName'];
  isLoading = false;
  isLoadingCard = false;

  importFileURL: string = "";

  constructor(injector: Injector,
    private _bodyStructureGroupSrvc: BodyStructureGroupsServiceProxy,
    private _deviceSrvc: DeviceServiceProxy,
    private _documentService: DocumentServiceProxy,
    private _modalService: BsModalService) {
    super(injector);
  }

  ngOnInit(): void {
    this.initializeDataTable();

    this.getSpecialties();
    // this.getDeviceFamily();
    this.getDeviceClass();
  }

  ngAfterViewInit(): void {
    this.dtTriggerAdmin.next();
  }

  ngOnDestroy(): void {
    this.dtTriggerAdmin.unsubscribe();
  }

  private initializeDataTable(): void {
    this.dtOptionsAdmin = this.defaultDtOptions;
    this.dtOptionsAdmin.language.zeroRecords = this.l('NoDocumentAvailable');
    this.dtOptionsAdmin.columnDefs = [{ targets: [-1], orderable: false }];
    this.dtOptionsAdmin.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
      const orderColumn = dtParams.order[0];
      const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
      this.isLoading = true;
      this._documentService
        .getAll(dtParams.search.value, sortColumn, dtParams.start, dtParams.length)
        .subscribe((result: DocumentFileDtoPagedResultDto) => {
          this.documents = result.items;
          callback({
            recordsFiltered: result.totalCount,
            data: [],
          });
          this.isLoading = false;
        });
    };
  }

  private getSpecialties(): void {
    this._bodyStructureGroupSrvc.getAll()
      .subscribe((res) => {
        if (res) {
          this.specialties = res;
        }
      });
  }

  private getDeviceClass(): void {
    this._deviceSrvc.getAllDeviceClass()
      .subscribe((res) => {
        if (res) {
          this.deviceClass = res;
        }
      });
  }

  private getDeviceFamily(): void {
    this._deviceSrvc.getAllDeviceFamily(null)
      .subscribe((res) => {
        if (res) {
          this.deviceFamily = res;
        }
      });
  }

  public onSelectSpecialty(myspecialty): void {
    this.selectedSpecialty = myspecialty;
    this._deviceSrvc.getAllDeviceFamily(this.selectedSpecialty)
      .subscribe((res) => {
        if (res) {
          this.deviceFamily = res;
          if (res.length > 0)
            this.selectedFamily = res[0].id;
        }
      });
  }

  newUploadClick(): void {
    const modalSettings1 = this.defaultModalSettings;
    modalSettings1.class = 'modal-lg';
    modalSettings1.initialState = {
      specialties: this.specialties,
      deviceClass: this.deviceClass,
      deviceFamily: this.deviceFamily
    };

    const modalRef1 = this._modalService.show(ImportDeviceComponent, modalSettings1);
    const modal1: ImportDeviceComponent = modalRef1.content;
    modal1.uploaded.subscribe((res) => {
      if (res === true) {
        modal1.oncloseCLick();
        this.dtTriggerAdmin.next();
      }
    });
  }

  public downloadClick(): void {
    if (!this.selectedSpecialty) {
      this.notify.warn("Please select a Specialty.");
      return;
    }
    if (!this.selectedClass) {
      this.notify.warn("Please select a Medical Class.");
      return;
    }
    if (!this.selectedFamily) {
      this.notify.warn("Please select a Device Family.");
      return;
    }
    let specialityName = this.specialties.find(f => f.id == this.selectedSpecialty).name;
    let deviceClassName = this.deviceClass.find(f => f.id == this.selectedClass).class;
    let deviceFamilyName = this.deviceFamily.find(f => f.id == this.selectedFamily).name;
    let fileName = `${specialityName}-Class${deviceClassName}-${deviceFamilyName}`.replace(/\s/g, '');;
    this.isLoadingCard = true;
    this._deviceSrvc.generateDeviceCSVTemplate(this.selectedSpecialty, this.selectedFamily, this.selectedClass)
      .subscribe(ret => {
        const file = base64StringToBlob(ret, "text/csv");
        saveAs(file, fileName, { autoBOM: true });
        this.isLoadingCard = false;
      })
  }
}
