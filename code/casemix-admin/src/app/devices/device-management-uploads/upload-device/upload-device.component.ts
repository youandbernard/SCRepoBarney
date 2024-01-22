import { ViewChild, ElementRef, Component, OnInit, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { SelectManufacturerComponent } from '../select-manufacturer/select-manufacturer.component';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';
import { DeviceServiceProxy, ManufactureDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
  selector: 'app-upload-device',
  templateUrl: './upload-device.component.html',
  styleUrls: ['./upload-device.component.less'],
  animations: [appModuleAnimation()],
})

export class UploadDeviceComponent extends AppComponentBase implements OnInit {
  @Output() uploaded: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('fileForm1', {static: true}) fileForm1: NgForm;
  @ViewChild('fileInput1') fileInputVar: ElementRef;

  // fileUploader: any;
  submitting1 = false;
  files: any[] = [];
  filename: string;

  manufacturerId: any;
  manufacturerName: string;
  documentId: number;

  allowedDocuments = ['csv'];

  constructor(
    injector: Injector,
    private _modalRef1: BsModalRef,
    private _deviceService: DeviceServiceProxy,
    private _modalService: BsModalService
  ) 
  { 
    super(injector);
  }

  ngOnInit(): void {
  }

  //#region Document Upload

  public changeListenerAdmin(file: FileList){
    if(file && file.length > 0) {
      // console.log(file)
      this.files = [];

      let file_ : File = file.item(0); 
      // console.log(file_);
      const blob = new Blob([file_], { type: file_.type });

      this.files.push({
        blob: blob,
        fileName: file_.name
      });

    }
  }

  uploadFileAdmin(): void {
    this.fileForm1.form.markAllAsTouched();
    
    if (this.fileForm1.invalid) return;
  
    if (this.files.length == 0){
      this.notify.warn("No files has been selected.");
      return;
    }

    //the file extension should be based on upload file extension
    const newExt = this.files[0].fileName.split('.').pop();
    if (newExt !== "csv"){
      this.notify.warn("Invalid file extension. Please upload csv file.");
      return;
    }

    this.submitting1 = true;
    this.filename = this.filename + "." + newExt;

    let param = 
    {
      fileName: this.filename,
      data: this.files[0].blob
    };    

    this._deviceService.uploadFile(null, this.manufacturerId, null, null, null, param)
    .pipe(finalize(() => this.submitting1 = false))
    .subscribe(response => {
        this.fileInputVar.nativeElement.value = '';
        this.uploaded.emit(true);
        this.close();
        this.notify.info("Admin file has been uploaded.");
    });
  }

  selectManufacturer(): void{
    const modalSettings = this.defaultModalSettings;
    const modalRef = this._modalService.show(SelectManufacturerComponent, modalSettings);
    const modal: SelectManufacturerComponent = modalRef.content;
    modal.manufacturer.subscribe((manufacturer: ManufactureDto) => {
      this.manufacturerId = manufacturer.id;
      this.manufacturerName = manufacturer.name;
    });
  }

  oncloseCLick(): void{
    this.close();
  }

  private close(): void {
    this._modalRef1.hide();
  }

  //#endregion
}
