import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { RegionManagementNodeDto, RegionsServiceProxy } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TreeNode } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { CreateEditHospitalComponent } from '../region-management/create-edit-hospital/create-edit-hospital.component';
import { CreateEditRegionComponent } from '../region-management/create-edit-region/create-edit-region.component';

@Component({
  selector: 'app-region-management-v2',
  templateUrl: './region-management-v2.component.html',
  styleUrls: ['./region-management-v2.component.less'],
  animations: [appModuleAnimation()],
})
export class RegionManagementV2Component extends AppComponentBase implements OnInit {

  regionsNodes: TreeNode[] = [];
  selectedRegionNode: TreeNode;

  isLoading: boolean = false;

  constructor(
    injector: Injector,
    private _regionService: RegionsServiceProxy,
    private _modalService: BsModalService
  ) { super(injector); }

  ngOnInit(): void {
    this.getAllRegions();
  }
  
  getAllRegions(): void {
    this.isLoading = true;
    this._regionService.getAllRegionData(false, null)
    .pipe(finalize(() => this.isLoading = false))
    .subscribe((result: RegionManagementNodeDto[]) => {
      this.regionsNodes = result;
    });
  }

  public onCheckSelectAllClick(): void {
    
  }

  public nodeSelect(event: any): void{
    this.selectedRegionNode = event.node;    
    if (event.node.data.dataType == RegionType.Hospital) {
      this.showCreateEditHospitalModal();
    } else {
      this.showCreateEditModal();
    }
  }

  private showCreateEditHospitalModal(): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: this.selectedRegionNode.key,
      regionId: this.selectedRegionNode.data.parentId,
      trustId: this.selectedRegionNode.data.trustId,
      icsId: this.selectedRegionNode.data.icsId,
      countryName: this.selectedRegionNode.data.countryName,
      regionName: this.selectedRegionNode.data.regionName,
      isEnabled: true,
      trustName: this.selectedRegionNode.data.trustName,
      name: this.selectedRegionNode.label,
      postcode: this.selectedRegionNode.data.postcode,
      ics: this.selectedRegionNode.data.selectedIcs,
      activeDevMgt: this.selectedRegionNode.data.activeDevMgt
    };
    const modalRef = this._modalService.show(CreateEditHospitalComponent, modalSettings);
    const modal: CreateEditHospitalComponent = modalRef.content;
    modal.modalSave.subscribe(() => {
      this.getAllRegions();
    });
  }

  private showCreateEditModal(): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: this.selectedRegionNode.key,
      type: this.selectedRegionNode.data.dataType,
      parentId: this.selectedRegionNode.data.parentId,
      isEnabled: true,
      icsId: this.selectedRegionNode.data.icsId,
      groupTrust: this.selectedRegionNode.data.groupTrust,
      name: this.selectedRegionNode.label,
      ics: this.selectedRegionNode.data.selectedIcs,
      countryName: this.selectedRegionNode.data.countryName,
      regionName: this.selectedRegionNode.data.regionName,
    };
    const modalRef = this._modalService.show(CreateEditRegionComponent, modalSettings);
    modalRef.setClass('modal-md');
    const modal: CreateEditRegionComponent = modalRef.content;
    modal.modalSave.subscribe(() => {
      this.getAllRegions();
    });
  }

  onExpandAllClick(): void {
    _.forEach(this.regionsNodes, (r) => {
      r.expanded = true;
    });
  }

  onCollapseAllClick(): void {
    _.forEach(this.regionsNodes, (r) => {
      r.expanded = false;
    });
  }
}

export class RegionType {
  static readonly Region = 'Region';
  static readonly Country = 'Country';
  static readonly Hospital = 'Hospital';
}