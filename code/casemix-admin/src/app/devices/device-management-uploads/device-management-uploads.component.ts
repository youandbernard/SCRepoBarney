import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DocumentFileDto, DocumentFileDtoPagedResultDto, DocumentServiceProxy, DownloadFileInput } from '@shared/service-proxies/service-proxies';
import { base64StringToBlob } from 'blob-util';
import { saveAs } from 'file-saver';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { UploadDeviceComponent } from './upload-device/upload-device.component';

@Component({
  selector: 'app-device-management-uploads',
  templateUrl: './device-management-uploads.component.html',
  styleUrls: ['./device-management-uploads.component.less'],
  animations: [appModuleAnimation()],
})
export class DeviceManagementUploadsComponent extends AppComponentBase implements OnInit {

  //Datatable
  dtTriggerAdmin: Subject<any> = new Subject();
  dtOptionsAdmin: DataTables.Settings = {};
  documents: DocumentFileDto[];
  sortColumns: string[] = ['id', 'filename', 'filepath', 'filetype', 'enable', 'modifiedDate', 'manufacturer'];
  isLoading1 = false;
  
  constructor(
    injector: Injector,     
    private _documentService: DocumentServiceProxy,
    private _modalService: BsModalService
  ) {
      super(injector);
  }

  ngOnInit(): void {
    this.initializeDataTable();
  }

  //#region Document Table
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
      this.isLoading1 = true;
      this._documentService
        .getAll(dtParams.search.value, sortColumn, dtParams.start, dtParams.length)
        .subscribe((result: DocumentFileDtoPagedResultDto) => {
          this.documents = result.items;
          callback({
            recordsFiltered: result.totalCount,
            data: [],
          });
          this.isLoading1 = false;
        });
    };    
  }

  enableOrDisabe(id: number, flag: boolean): void {
    abp.message.confirm('', 'Enable/ Disable file', (result: boolean) => {
      if (result) {
        var newFlag: boolean;
        newFlag = !flag;
        this._documentService.enableOrDisable(id, newFlag)
        .subscribe(() => {
            this.dtTriggerAdmin.next();
            
            if (newFlag === true){
              this.notify.success("File document was enabled.");        
            } else{
              this.notify.success("File document was disabled.");
            }
        });
      }
    });  
  }

  //#endregion

  //#region Document Download
  public downloadFile(id: number, userId: number): void {
    abp.message.confirm('Do you want to continue download file?', 'Download file', (result: boolean) => {
      if (result) {
        var downloadInput = new DownloadFileInput();
        downloadInput.documentId = id;
        downloadInput.userId = userId;
    
        this._documentService.downloadFile(downloadInput)
        .subscribe(ret => {
          const file = base64StringToBlob(ret.fileContent, ret.fileType);
          saveAs(file, ret.fileName, { autoBOM: true });
        })
      }
    });    
  }

  newUploadClick(): void {
    const modalSettings1 = this.defaultModalSettings;
    modalSettings1.class = 'modal-lg';
    const modalRef1 = this._modalService.show(UploadDeviceComponent, modalSettings1);
    const modal1: UploadDeviceComponent = modalRef1.content;
    modal1.uploaded.subscribe((res) => {
      if (res === true){
        modal1.oncloseCLick();  
        this.dtTriggerAdmin.next();
      }
    });
  }  
}

