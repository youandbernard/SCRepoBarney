import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { TreeNode } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { RegionsServiceProxy, RegionManagementNodeDto, RegionDto, RegionManagementDataDto } from '@shared/service-proxies/service-proxies';
import { CreateEditRegionComponent } from './create-edit-region/create-edit-region.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { CreateEditHospitalComponent } from './create-edit-hospital/create-edit-hospital.component';

@Component({
  selector: 'app-region-management',
  templateUrl: './region-management.component.html',
  styleUrls: ['./region-management.component.less'],
  animations: [appModuleAnimation()],
})
export class RegionManagementComponent extends AppComponentBase implements OnInit {
  data1: TreeNode[];
  selectedNode: TreeNode;
  selectedRegion: any;
  constructor(injector: Injector, private _regionService: RegionsServiceProxy, private _modalService: BsModalService) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllRegions();
  }

  onEditClick(event: any): void {
    this.selectedRegion = event.node;
    if (event.node.data.dataType == RegionType.Hospital) {
      this.showCreateEditHospitalModal();
    } else {
      this.showCreateEditModal();
    }
  }

  getAllRegions(): void {
    this._regionService.getAllRegionData(false, null).subscribe((result: RegionManagementNodeDto[]) => {
      this.data1 = result;
    });
  }

  private showCreateEditModal(): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: this.selectedRegion.key,
      type: this.selectedRegion.data.dataType,
      parentId: this.selectedRegion.data.parentId,
      isEnabled: true,
      icsId: this.selectedRegion.data.icsId,
      groupTrust: this.selectedRegion.data.groupTrust,
      name: this.selectedRegion.label,
      ics: this.selectedRegion.data.selectedIcs,
      countryName: this.selectedRegion.data.countryName,
      regionName: this.selectedRegion.data.regionName,
    };
    const modalRef = this._modalService.show(CreateEditRegionComponent, modalSettings);
    modalRef.setClass('modal-md');
    const modal: CreateEditRegionComponent = modalRef.content;
    modal.modalSave.subscribe(() => {
      this.getAllRegions();
    });
  }

  private showCreateEditHospitalModal(): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: this.selectedRegion.key,
      regionId: this.selectedRegion.data.parentId,
      trustId: this.selectedRegion.data.trustId,
      icsId: this.selectedRegion.data.icsId,
      countryName: this.selectedRegion.data.countryName,
      regionName: this.selectedRegion.data.regionName,
      isEnabled: true,
      trustName: this.selectedRegion.data.trustName,
      name: this.selectedRegion.label,
      postcode: this.selectedRegion.data.postcode,
      ics: this.selectedRegion.data.selectedIcs,
      activeDevMgt: this.selectedRegion.data.activeDevMgt
    };
    const modalRef = this._modalService.show(CreateEditHospitalComponent, modalSettings);
    const modal: CreateEditHospitalComponent = modalRef.content;
    modal.modalSave.subscribe(() => {
      this.getAllRegions();
    });
  }
}

export class RegionType {
  static readonly Region = 'Region';
  static readonly Country = 'Country';
  static readonly Hospital = 'Hospital';
}
