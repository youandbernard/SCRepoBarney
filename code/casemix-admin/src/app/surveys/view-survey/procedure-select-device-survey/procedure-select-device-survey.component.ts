import { Component, Injector, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DeviceBrandFamilyDto, DeviceDto, DeviceProcedureServiceProxy, HospitalDto, ManufactureDto, PoapProcedureDevicesDto, PoapProcedureDevicesServiceProxy, PoapProcedureDto } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';
import { BsModalRef } from 'ngx-bootstrap/modal';

class ManufacturerDevicesModel {
  manufacturerId: string;
  manufacturerName: string;
  deviceBrandFamily: DeviceBrandFamilyDto[] = [];
}

@Component({
  selector: 'app-procedure-select-device-survey',
  templateUrl: './procedure-select-device-survey.component.html',
  styleUrls: ['./procedure-select-device-survey.component.less'],
  animations: [appModuleAnimation()],
  providers: [ManufacturerDevicesModel]
})

export class SurveyProcedureSelectDeviceComponent extends AppComponentBase implements OnInit {

  // @Output() saved: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() returnedProcedure: EventEmitter<PoapProcedureDto> = new EventEmitter<PoapProcedureDto>();
  @Input() procedure: PoapProcedureDto;
  @Input() bodyStructureId: number;
  @Input() bodyStructureGroupId: string;
  @Input() isFilterLicensedStatus: boolean;

  snomedId: string = "";
  procedureName: string;
  currentHospitalId: string;
  deviceLookupDesc: string = '';
  devices: DeviceDto[] = [];
  selectedDevices: DeviceDto[] = [];
  isLoading = false;

  groupedBrandDevFamily: DeviceBrandFamilyDto[] = [];

  selectedBrandFamily: string = '';
  selectedBrandName: string = '';
  selectedDeviceFamily: number = 0;
  hasSelectedBrandFamily: boolean = false;

  inputModelName: string = '';
  isSelecting: boolean = false;

  constructor(
    injector: Injector,
    private _modalRef: BsModalRef,
    private _localStorageService: LocalStorageService,
    private _deviceProcedureService: DeviceProcedureServiceProxy,
    private _poapDeviceProcedureService: PoapProcedureDevicesServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (localHospital) {
      this.currentHospitalId = localHospital.id;
    }

    this.snomedId = this.procedure.snomedId;
    this.procedureName = this.procedure.name
    this.getGroupedDevices(this.snomedId);
  }

  getGroupedDevices(snomedId: string): void {
    this.isLoading = true;
    this._deviceProcedureService.getBySnomedIdGrouped(snomedId, this.bodyStructureId, this.currentHospitalId, this.bodyStructureGroupId, this.isFilterLicensedStatus)
      .subscribe((res) => {
        if (res) {
          if(res.isLicensed){
            this.groupedBrandDevFamily = res.deviceBrandFamilies;
            this.getExistingDevices();
          }
        }
      });
    this.isLoading = false;
  }  

  getDevices(snomedId: string): void {
    this._deviceProcedureService.getBySnomedId(
      snomedId,
      this.bodyStructureId,
      this.currentHospitalId,
      this.selectedBrandName,
      this.selectedDeviceFamily,
      this.inputModelName)
      .subscribe((res) => {
        if (res) {
          this.devices = res;

          _.forEach(this.devices, (d) => {
            d.deviceLookupDesc = d.deviceName + ' - ' + d.model + ' - ' + (d.deviceDescription.length > 30 ? d.deviceDescription.substring(0, 30) + '...' : d.deviceDescription);
          });

          this.devices = this.devices.filter((e) => !this.selectedDevices.some((c) => c.id == e.id));
        }
      });
  }

  onBrandFamilyChange() {
    this.selectedBrandName = '';
    this.selectedDeviceFamily = 0;
    this.devices = [];
    this.inputModelName = '';
    this.deviceLookupDesc = '';
    this.isSelecting = false;

    if (this.selectedBrandFamily != '') {
      this.hasSelectedBrandFamily = true;

      var splitStr = this.selectedBrandFamily.split('|__|');
      if (splitStr.length > 0) {
        this.selectedBrandName = splitStr[0];
        this.selectedDeviceFamily = parseInt(splitStr[1]);

        this.isSelecting = true;
      }
    }
  }

  onModelChange() {
    // if (this.inputModelName != '') {
    //   this.isSelecting = true;
    //   this.getDevices(this.snomedId);
    // } else {
    //   this.devices = [];
    // }

    this.isSelecting = true;
    this.getDevices(this.snomedId);
  }

  getExistingDevices(): void {
    if (this.procedure.poapProcedureDevices) {
      _.forEach(this.procedure.poapProcedureDevices, (d) => {
        var device = d.device;
        this.selectedDevices.push(device);
      });
    }

    _.forEach(this.selectedDevices, (d) => {
      d.deviceLookupDesc = d.deviceName + ' - ' + d.model + ' - ' + (d.deviceDescription.length > 30 ? d.deviceDescription.substring(0, 30) + '...' : d.deviceDescription);
    });
  }

  setSelectedDevice($event, device): void {
    if ($event.target.checked) {
      this.selectedDevices.push(device);
      this.deviceLookupDesc = '';
      const index = this.devices.findIndex((e) => e.id == device.id);
      if (index >= 0) {
        this.devices.splice(index, 1);
      }
    }
    else {
      this.onRemoveClick(device);
    }

  }

  onRemoveClick(selected: DeviceDto): void {
    const index = this.selectedDevices.findIndex((e) => e.id === selected.id);
    if (index >= 0) {
      this.selectedDevices.splice(index, 1);

      if (this.isSelecting) {
        this.devices.push(selected);
        this.onModelChange();
      }
    }
  }

  onFormSubmit(): void {
    if (this.procedure.poapProcedureDevices) {
      this.procedure.poapProcedureDevices = [];
    }
    if (this.selectedDevices.length > 0) {
      this._poapDeviceProcedureService.savePoapProcedureDevices(
        this.procedure.preOperativeAssessmentId, this.procedure.id, this.procedure.snomedId, this.selectedDevices)
        .subscribe((res) => {
          this.notify.success('Sub procedure is successfully assigned to devices.');
          this.onCloseModalProcess();
        });
    } else {
      this.onCloseModalProcess();
    }
  }

  onCloseModalProcess(): void {
    _.forEach(this.selectedDevices, (d) => {
      var poapProcedureDevice = new PoapProcedureDevicesDto();

      poapProcedureDevice.device = d;
      poapProcedureDevice.deviceId = d.id;
      poapProcedureDevice.poapProcedureId = this.procedure.id;
      poapProcedureDevice.preOperativeAssessmentId = this.procedure.preOperativeAssessmentId;
      poapProcedureDevice.snomedId = this.procedure.snomedId;

      if (this.procedure.poapProcedureDevices) {

        const isExist = this.procedure.poapProcedureDevices.some((r) => r.deviceId === poapProcedureDevice.deviceId &&
          r.poapProcedureId === poapProcedureDevice.poapProcedureId && r.preOperativeAssessmentId === poapProcedureDevice.preOperativeAssessmentId);
        if (!isExist) {
          this.procedure.poapProcedureDevices.push(poapProcedureDevice);
        }
      }
      else {
        var newPoapProcedureDevices: PoapProcedureDevicesDto[] = [];

        newPoapProcedureDevices.push(poapProcedureDevice);
        this.procedure.poapProcedureDevices = newPoapProcedureDevices;
      }
    });
    this.returnedProcedure.emit(this.procedure);
    this.onCloseClick();
  }

  onCloseClick(): void {
    this._modalRef.hide();
  }

}
