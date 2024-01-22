import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { BodyStructureGroupDto, BodyStructureGroupsServiceProxy, DeviceProcedureServiceProxy, TreeNodeInput, TreeNodeGroup, HospitalDto } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-device-procedures',
  templateUrl: './device-procedures.component.html',
  styleUrls: ['./device-procedures.component.less'],
  animations: [appModuleAnimation()],
})
export class DeviceProceduresComponent extends AppComponentBase implements OnInit {

  @Input() id: number;
  @Input() code: string;
  @Input() name: string;
  @Output() submitted: EventEmitter<boolean> = new EventEmitter<boolean>();

  isTreeLoading: boolean = false;

  deviceCode: string
  deviceName: string;

  currentHospitalId: string;
  currentHospitalName: string;

  bodyStructureGroupDto: BodyStructureGroupDto[] = [];
  selectedbodyStructureGroupDto: BodyStructureGroupDto[] = [];

  bodystructuresGroupNodes: TreeNode[] = [];
  selectedBodystructuresNodes: TreeNode[] = [];

  constructor(injector: Injector,
    private _modalRef: BsModalRef,
    private _localStorageService: LocalStorageService,
    private  _deviceProcedureService: DeviceProcedureServiceProxy,
    private _bodyStructureGroupsService: BodyStructureGroupsServiceProxy) { 
    super(injector);
  }

  ngOnInit(): void {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (localHospital) {
      this.currentHospitalId = localHospital.id;
      this.currentHospitalName = localHospital.name;
    }

    this.isTreeLoading = true;
    if (this.id)
    {
      this.getDeviceProcedures(this.id);
      this.deviceCode = this.code;
      this.deviceName= this.name;
    }

    this.loadProcedures();
  }

  loadProcedures(): void {

    this._bodyStructureGroupsService.getAllWithSubProcedures()
      .subscribe((res) => {
        this.bodyStructureGroupDto = res;

        this.bodystructuresGroupNodes = this.BuildBodyStructureTreeNodes(this.bodyStructureGroupDto);
        
        this.expandSelected();
        
        if (this.isTreeLoading)
          this.isTreeLoading = false;
      });
  }

  expandSelected(): void {
    if (this.bodystructuresGroupNodes){
        _.forEach(this.bodystructuresGroupNodes, (nodes) => {                
            var isExist = this.selectedBodystructuresNodes.find(u => u.key === nodes.key);
            if (isExist) {
                nodes.expanded = true;
            }

            _.forEach(nodes.children, (level2) => {
                var isExist2 = this.selectedBodystructuresNodes.find(u => u.key === level2.key);
                if (isExist2) {
                    level2.expanded = true;
                }
            });
        });
    }
  }

  private getDeviceProcedures(id: number): void {

    this._deviceProcedureService.getByDeviceId(id, this.currentHospitalId)
      .subscribe((res) => {
        this.selectedbodyStructureGroupDto = res;

        this.selectedBodystructuresNodes = this.BuildSelectedBodyStructureTreeNodes(this.selectedbodyStructureGroupDto);

          this.expandSelected();

      });

  }
  
  private BuildSelectedBodyStructureTreeNodes(items: BodyStructureGroupDto[]): TreeNode[] {
    const treeNodes: TreeNode[] = [];

    _.forEach(items, (item) => {
        const groupNode: TreeNode = {
          key: item.id,  
          label: item.name,
          data: {
            isGroup: true,
          },
          expanded: true,
        };
        
        if (_.isArray(item.bodyStructures)) {
          const childBS: TreeNode[] = [];
          
          _.forEach(item.bodyStructures, (bs) => {
              if (bs !== undefined && bs) {
                  const childNode: TreeNode = {
                    key: bs? bs.id.toString(): "",
                    label: bs.description,
                    data: {
                      isGroup: false,
                      group: groupNode.key,
                    },
                    expanded: true,
                  };
                  
    
                  treeNodes.push(childNode);
                  childBS.push(childNode);
                  
                  if (_.isArray(bs.bodyStructureSubProcedures)){
                      const childSPBS: TreeNode[] = [];
    
                      _.forEach(bs.bodyStructureSubProcedures, (spbs) => {
                          if (spbs !== undefined && spbs) {
                              if (spbs.id !== undefined) {
                                const childSPNode: TreeNode = {
                                  key: spbs.snomedId? bs.id.toString() + "|" + spbs.snomedId: "",
                                  label: (spbs.snomedId? spbs.snomedId: "") + " - " + spbs.description,
                                  data: {
                                    isGroup: false,
                                    group: childNode.key,
                                  },
                                  expanded: true,
                                };
                                
                                treeNodes.push(childSPNode);
                                childSPBS.push(childSPNode);
                              }                                
                          }
                              
    
                      }); 
    
                      childNode.children = childSPBS;
                  }
              }
          });
          
          groupNode.children = childBS;
        }          
        
        treeNodes.push(groupNode);
    });      

    return treeNodes;
  }

  private BuildBodyStructureTreeNodes(items: BodyStructureGroupDto[]): TreeNode[] {
    const treeNodes: TreeNode[] = [];

    _.forEach(items, (item) => {
        const groupNode: TreeNode = {
          key: item.id,
          label: item.name,
          data: {
            isGroup: true,
          },
        };
        
        if (_.isArray(item.bodyStructures)) {
          const childBS: TreeNode[] = [];
          
          _.forEach(item.bodyStructures, (bs) => {
            if (bs !== undefined && bs) {
                  const childNode: TreeNode = {
                    key: bs.id.toString(),
                    label: bs.description,
                    data: {
                      isGroup: false,
                      group: groupNode.key,
                    },
                  };
      
                  childBS.push(childNode);
                  
                  const childSPBS: TreeNode[] = [];
                      
                  if (_.isArray(bs.bodyStructureSubProcedures)){
    
                      _.forEach(bs.bodyStructureSubProcedures, (spbs) => {
                          if (spbs !== undefined && spbs) {
                            if (spbs.id !== undefined) {
                              const childSPNode: TreeNode = {
                                key: spbs.snomedId? bs.id.toString() + "|" + spbs.snomedId: "",
                                label: (spbs.snomedId? spbs.snomedId: "") + " - " + spbs.description,
                                data: {
                                  isGroup: false,
                                  group: childNode.key,
                                },
                              };
                              
                              childSPBS.push(childSPNode);
                            }                    
                          }                          
                          
                      });
                  }

                  childNode.children = childSPBS;
              }
          });
          
          groupNode.children = childBS;
        }          
        
        treeNodes.push(groupNode);
    });

    return treeNodes;
  }
  
  public onModalSubmit(): void{
    if (this.selectedBodystructuresNodes) {    

      let treeNodeInputs: TreeNodeInput[] = [];
        treeNodeInputs = this.TreeNodes_To_TreeNodeInputs(this.selectedBodystructuresNodes);

      this._deviceProcedureService.saveSelectedDeviceProcedures(this.id, treeNodeInputs)
        .subscribe((res) => {
            this.submitted.emit(true);
            this.notify.success("Procedures is succesfully assigned to device " + this.deviceCode + " - " + this.deviceName );
        });
    }
  }

  private TreeNodes_To_TreeNodeInputs(items: TreeNode[]): TreeNodeInput[] {
      const treeNodeInputs: TreeNodeInput[] = [];
      
      _.forEach(items, (item) => {
          let treeNodeChild: TreeNodeInput[] = [];
          treeNodeChild = this.TreeNodes_To_TreeNodeInputs(item.children);

          let treeNodeInput = new TreeNodeInput();
          let treeNodeGroup = new TreeNodeGroup;
          treeNodeGroup.group = item.data.group;
          treeNodeGroup.isGroup = item.data.isGroup;

          treeNodeInput.children = treeNodeChild;
          treeNodeInput.data = treeNodeGroup;
          treeNodeInput.key = item.key;
          treeNodeInput.label = item.label;
          treeNodeInput.partialSelected = item.partialSelected
          
          treeNodeInputs.push(treeNodeInput);
      });
  
      return treeNodeInputs;
  }
  
  public onCheckSelectAllClick(): void {    
  }

  public clearSelection(): void {
    if (!this.selectedBodystructuresNodes || (this.selectedBodystructuresNodes && this.selectedBodystructuresNodes.length <= 0)) {
      return;
    }    
    abp.message.confirm('Clear selected procedures?', 'Device Procedures', (result: boolean) => {
      if (result) {
        this.selectedBodystructuresNodes.forEach((bs) => {
          if (!bs.parent) bs.partialSelected = false;
        });
        this.selectedBodystructuresNodes = [];
      }
    });
  }

  public onCollapseAllClick(): void {
    _.forEach(this.bodystructuresGroupNodes, (bs) => {
      bs.expanded = false;
    });
  }

  public onExpandAllClick(): void {
    _.forEach(this.bodystructuresGroupNodes, (bs) => {
      bs.expanded = true;
    });
  }

  public onCloseClick(): void {
    this._modalRef.hide();
  }

}
