import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, ElementRef, Injector, OnInit, Output, ViewChild, EventEmitter, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DeviceManagementDevicesComponent } from '@app/devices/device-management-devices/device-management-devices.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { BodyStructureGroupDto, BodyStructureGroupsServiceProxy, DeviceClassDto, DeviceFamilyDto, DeviceServiceProxy, DocumentServiceProxy, ValidationResponseDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize, map } from 'rxjs/operators';

@Component({
  selector: 'app-import-device',
  templateUrl: './import-device.component.html',
  styleUrls: ['./import-device.component.less'],
  animations: [appModuleAnimation()],
})
export class ImportDeviceComponent extends AppComponentBase implements OnInit {
  @Output() uploaded: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() specialties: BodyStructureGroupDto[];
  @Input() deviceClass: DeviceClassDto[];
  @Input() deviceFamily: DeviceFamilyDto[];

  @ViewChild('fileForm', {static: true}) fileForm: NgForm;
  @ViewChild('fileInput') fileInputVar: ElementRef;

  fileUploader: any;
  files: any[] = [];
  
  allowedDocuments = ['csv'];

  basicErrors: ValidationResponseDto = new ValidationResponseDto;
  fileErrors: ValidationResponseDto = new ValidationResponseDto;
  basicValidatePass: boolean = false;
  dataValidatePass: boolean = false;
  
  showBasicValidated: boolean = false;
  showFileValidated: boolean = false;

  isUploading: boolean = false;
  isValidating: boolean = false;
  progress: number = 0;
  numRows: number = 0;
  
  selectedSpeciality: string = "";
  selectedFamily: number = 0;
  selectedClass: string = '';

  constructor(injector: Injector,
    private _modalRef1: BsModalRef,
    private _deviceService: DeviceServiceProxy,
    private _documentService: DocumentServiceProxy,
    private _deviceSrvc: DeviceServiceProxy,
  ) { 
    super(injector)
  }

  ngOnInit(): void {   
    if (this.specialties) {
      if (this.specialties.length > 0) {
        this.getDeviceFamily(this.specialties[0].id);
      }
    }
  }

  public getDeviceFamily(myspecialty): void {
    this.selectedSpeciality = myspecialty;
    this._deviceSrvc.getAllDeviceFamily(myspecialty )
      .subscribe((res) => {
        if (res) {
          this.deviceFamily = res;
          if (res.length > 0)
            this.selectedFamily = res[0].id;
        }
      });
  }
 
  public onSelectSpecialty(myspecialty): void {
    this.selectedSpeciality = myspecialty;
    this._deviceSrvc.getAllDeviceFamily(myspecialty)
      .subscribe((res) => {
        if (res) {
          this.deviceFamily = res;
          if (res.length > 0)
            this.selectedFamily = res[0].id;
        }
      });
  }

  public changeListener(file: FileList){
    if(file && file.length > 0) {
      this.files = [];

      let file_ : File = file.item(0); 
      const blob = new Blob([file_], { type: file_.type });

      this.files.push({
        blob: blob,
        fileName: file_.name
      });

    }
  }

  validateFile(): void {
    if (!this.selectedSpeciality)
    {
      this.notify.warn("Please select a Specialty for import.");
      
      return;
    }
    if (!this.selectedFamily) {
      this.notify.warn("Please select a Device Family.");

      return;
    }
    if (!this.selectedClass)
    {
      this.notify.warn("Please select a Medical Class for import.");
      
      return;
    }

    this.basicValidatePass = false;
    this.dataValidatePass = false;
    this.showBasicValidated = false;
    this.showFileValidated = false;

    this.fileForm.form.markAllAsTouched();
    
    if (this.fileForm.invalid) return;
  
    if (this.files.length == 0){
      this.notify.warn("No files has been selected.");
      return;
    }

    let param = 
    {
      fileName: this.files[0].fileName,
      data: this.files[0].blob
    };   
    this.isValidating = true;
    
    this.progress = 50;

    this._documentService.basicValidation(null, "", "", null, null, param)   
    .pipe(finalize(() => this.isValidating = false ))
    .subscribe((res: ValidationResponseDto) => {
      if (res.hasErrors) {
        this.basicValidatePass = false;
        this.basicErrors = res;
      }
      else {
        this.basicValidatePass = true;

        this.isValidating = true;
        this._documentService.validateDataFiles(null, "", this.selectedSpeciality, this.selectedClass, this.selectedFamily, param)
        .pipe(finalize(() => this.isValidating = false))
        .subscribe((res1: ValidationResponseDto) => {
          if (res1.hasErrors) {
            this.fileErrors = res1;
          }
          else {
            this.dataValidatePass = true;
            this.numRows = res1.numberRows;
          }
  
          this.showFileValidated = true;
        });
      }

      this.showBasicValidated = true;
    });
  }

  uploadFile(): void {
    if (this.basicValidatePass && this.dataValidatePass) {

      this.fileForm.form.markAllAsTouched();
    
      if (this.fileForm.invalid) return;
    
      if (this.files.length == 0){
        this.notify.warn("No files has been selected.");
        return;
      }
  
      this.isUploading = true;
      let param = 
      {
        fileName: this.files[0].fileName,
        data: this.files[0].blob
      };    
  
      this._deviceService.uploadFile(null, null, this.selectedSpeciality, this.selectedClass, this.selectedFamily, param)
      .pipe(finalize(() => this.isUploading = false))
      .subscribe((res) => {
          this.fileInputVar.nativeElement.value = '';
          this.uploaded.emit(true);
          this.close();
          this.notify.info("Manufacturer file has been uploaded.");
      });
    }  
  }

  // private getSpecialties(): void {
  //   this._bodyStructureGroupSrvc.getAll()
  //     .subscribe((res) => {
  //         if (res){
  //             this.specialties = res;   
  //         }
  //     });
  // }

  // private getDeviceClass(): void {
  //   this._deviceService.getAllDeviceClass()
  //   .subscribe((res) => {
  //       if (res){
  //          this.deviceClass = res;   
  //       }
  //   });
  // }

  // private getDeviceFamily(): void {
  //   this._deviceService.getAllDeviceFamily()
  //     .subscribe((res) => {
  //       if (res) {
  //         this.deviceFamily = res;
  //       }
  //     });
  // }

  oncloseCLick(): void{
    this.close();
  }

  private close(): void {
    this._modalRef1.hide();
  }

}
