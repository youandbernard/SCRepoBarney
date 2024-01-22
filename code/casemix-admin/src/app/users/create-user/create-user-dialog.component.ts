import { Component, Injector, OnInit, EventEmitter, Output, ViewChild, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import {
  UserServiceProxy,
  CreateUserDto,
  RoleDto,
  ManufacturesServiceProxy,
  ManufactureDto,
  RegionsServiceProxy,
  RegionManagementNodeDto,
  RegionManagementDataDto,
  CountriesServiceProxy,
  HospitalsServiceProxy,
  UserHospitalsServiceProxy,
  RegionDto,
  HospitalDto,
  RegionHospitalMappingDto,
  UserHospitalDto,
} from '@shared/service-proxies/service-proxies';
import { AbpValidationError } from '@shared/components/validation/abp-validation.api';
import { AssignHospitalsComponent } from '../assign-hospitals/assign-hospitals.component';
import { AssignSpecialtiesComponent } from '../assign-specialties/assign-specialties.component';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { TreeNode } from 'primeng/api';
import { Observable } from 'rxjs';
@Component({
  templateUrl: './create-user-dialog.component.html',
})
export class CreateUserDialogComponent extends AppComponentBase implements OnInit {
  @ViewChildren("checkboxesMain") checkboxesMain: QueryList<ElementRef>;
  @ViewChildren("checkboxesManufacturer") checkboxesManufacturer: QueryList<ElementRef>;
  @ViewChildren("checkboxesSuperAdmin") checkboxesSuperAdmin: QueryList<ElementRef>;
  
  @ViewChildren("mainRolesRadio") mainRolesRadio: QueryList<ElementRef>;
  @ViewChildren("otherRolesRadio") otherRolesRadio: QueryList<ElementRef>;
  @ViewChildren("superAdminRadio") superAdminRadio: QueryList<ElementRef>;

  @Output() onSave = new EventEmitter<any>();
  
  isManufacturer: number = 2;
  isMainRole: number = 1;
  isSuperAdmin: number = 3;

  disableMainRole: boolean = true;
  disableManufacturer: boolean = true;
  disableSuperadmin: boolean = true;

  selectedManfacturer: string;
  saving = false;
  user = new CreateUserDto();
  regions: TreeNode[];
  selectedNodes: RegionManagementNodeDto[] = [];
  roles: RoleDto[] = [];
  manufactures: ManufactureDto[] = [];
  checkedRolesMap: { [key: string]: boolean } = {};
  defaultRoleCheckedStatus = false;
  regionsMultiSelect: RegionDto[] = [];
  hospitalsMultiSelect: HospitalDto[] = [];
  selectedRegions: RegionDto[] = [];
  selectedHospitals: HospitalDto[] = [];
  regionHospitalMapping: RegionHospitalMappingDto = new RegionHospitalMappingDto();
  userHospitals: UserHospitalDto[] = [];
  hospitals: HospitalDto[] = [];
  passwordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'pattern',
      localizationKey: 'PasswordsMustBeAtLeast8CharactersContainLowercaseUppercaseSpecialCharactersNumber',
    },
  ];
  confirmPasswordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'validateEqual',
      localizationKey: 'PasswordsDoNotMatch',
    },
  ];

  @ViewChild('assignHospitalsComponent')
  private assignHospitalsComponent: AssignHospitalsComponent;
  @ViewChild('assignSpecialtiesComponent')
  private assignSpecialtiesComponent: AssignSpecialtiesComponent;

  constructor(
    injector: Injector,
    public _userService: UserServiceProxy,
    public _manufacturesService: ManufacturesServiceProxy,
    public _regionsService: RegionsServiceProxy,
    private _countriesService: CountriesServiceProxy,
    private _hospitalsService: HospitalsServiceProxy,
    public bsModalRef: BsModalRef,
    private _userHospitalsService: UserHospitalsServiceProxy
  ) {
    super(injector);
    this.location.onPopState(() => this.bsModalRef.hide());
  }
  
  ngOnInit(): void {
    this.user.isActive = true;
    this.user.experience = 0;
    this.getManufactures();
    this.getHospitals();
    this.getAllRegions();
    this._userService.getRoles().subscribe((result) => {
      this.roles = result.items;
      this.setInitialRolesStatus();
      
      this.mainRolesRadio.forEach((element) => {
        element.nativeElement.checked = true;
      });
      
      this.disableMainRole = false;
    });    
  }

  getAllRegions(): void {
    this._regionsService.getAllRegionData(true, null).subscribe((result: RegionManagementNodeDto[]) => {
      this.regions = result;
    });
  }

  setInitialRolesStatus(): void {
    _.map(this.roles, (item) => {
      this.checkedRolesMap[item.normalizedName] = this.isRoleChecked(item.normalizedName);
    });
  }

  isRoleChecked(normalizedName: string): boolean {
    return this.defaultRoleCheckedStatus;
  }

  onRoleChange(role: RoleDto, $event) {
    this.checkedRolesMap[role.normalizedName] = $event.target.checked;
  }

  onManufactureChange(): void {
    const manufacturer = _.find(this.manufactures, (e) => e.id === this.selectedManfacturer);
    this.user.manufactureName = manufacturer.name;
    this.user.manufactureId = manufacturer.id;

  }

  getCheckedRoles(): string[] {
    const roles: string[] = [];
    _.forEach(this.checkedRolesMap, function (value, key) {
      if (value) {
        roles.push(key);
      }
    });
    return roles;
  }

  onChangeRegion(e: any): void {
    this.selectedRegions = e.value;
  }

  onChangeHospital(e: any): void {
    this.selectedHospitals = e.value;
  }

  save(): void {

    this.saving = true;

    this.user.roleNames = this.getCheckedRoles();
    // var isCheckUserHospitals = this.assignHospitalsComponent.userHospitals.some((e) => e.isSelected);
    this.regionHospitalMapping.hospitalIds = this.selectedNodes
      .map((e) => {
        if (e.data.dataType == 'Hospital') return e.key;
      })
      .filter((item) => item);
    this.regionHospitalMapping.regionIds = this.selectedNodes
      .map((e) => {
        if (e.data.dataType == 'Region') return e.key;
      })
      .filter((item) => item);

    if (!this.regionHospitalMapping.hospitalIds) {
      abp.notify.error('Please assign user to a hospital', 'Error');
      this.saving = false;
      return;
    }

    if (this.user.roleNames.length === 0) {
      abp.notify.error('Please assign user to a role', 'Error');
      this.saving = false;
      return;
    }

    this._userService
      .create(this.user)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((user) => {
        // debugger;
        _.forEach(this.hospitals, (hospital) => {
          var userHospital = new UserHospitalDto();
          userHospital.userId = user.id;
          userHospital.isSelected = false;
          userHospital.hospitalId = hospital.id;
          var check = this.regionHospitalMapping.hospitalIds.find((item) => item == userHospital.hospitalId);
          if (check) {
            userHospital.isSelected = true;
          }
          this.userHospitals.push(userHospital);
        });
        // debugger;
        this.regionHospitalMapping.userId = user.id;

        this._regionsService.saveUserRealm(this.regionHospitalMapping).subscribe(() => {});

        this.saveUserHospitals().subscribe(() => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
        });
        this.assignSpecialtiesComponent.saveSurgeonSpecialties(user.id).subscribe(() => {});
      });
  }

  getManufactures(): void {
    this._manufacturesService.getAll().subscribe((manufactures) => {
      this.manufactures = manufactures;
    });
  }
  private getHospitals(): void {
    this._hospitalsService.getAll(null, null, 0, 999).subscribe((hospitals) => {
      this.hospitals = hospitals.items;
      // this.getUserHospitals();
    });
  }

  private prepareUserHospitals(userHospitals: UserHospitalDto[]): void {
    for (const hospitalId of Object.keys(this.hospitals)) {
      let userHospital = _.find(userHospitals, (e) => e.hospitalId === hospitalId);
      if (userHospital) {
        userHospital.isSelected = true;
        this.userHospitals.push(userHospital);
      } else {
        userHospital = new UserHospitalDto();
        // userHospital.userId = this.user.id;
        userHospital.hospitalId = hospitalId;
        userHospital.isSelected = false;
        this.userHospitals.push(userHospital);
      }
    }
  }

  public filterMainRoles(role: RoleDto) {
    return role.normalizedName !== 'MANUFACTURER' && role.normalizedName != 'SUPER ADMIN';
  }

  public filterManufacturer(role: RoleDto) {
    return role.normalizedName === 'MANUFACTURER';
  }

  public filterSuperAdmin(role: RoleDto) {
    return role.normalizedName === 'SUPER ADMIN';
  }
 
  public saveUserHospitals(): Observable<void> {
    return this._userHospitalsService.saveAll(this.userHospitals);
  }  
  
  onRadioClick(val: number): void {
    this.EnableDisableTreeNodes(true);

    if (val === 1){
      _.map(this.roles, (item) => {
          if (item.normalizedName === 'MANUFACTURER' || item.normalizedName === 'SUPER ADMIN') {
            this.checkedRolesMap[item.normalizedName] = false;
          }
      });

      this.checkboxesManufacturer.forEach((element) => {
        element.nativeElement.checked = false;
      });

      this.checkboxesSuperAdmin.forEach((element) => {
        element.nativeElement.checked = false;
      });

      this.disableMainRole = false;
      this.disableManufacturer = true;
      this.disableSuperadmin = true;

      this.selectedManfacturer = '';

      this.ClearAllNodes();
      
    } else if (val === 2) {
      _.map(this.roles, (item) => {
          if (item.normalizedName !== 'MANUFACTURER') {
            this.checkedRolesMap[item.normalizedName] = false;
          }else {
            this.checkedRolesMap[item.normalizedName] = true;
          }       
      });
      
      this.checkboxesMain.forEach((element) => {
        element.nativeElement.checked = false;
      });

      this.checkboxesSuperAdmin.forEach((element) => {
        element.nativeElement.checked = false;
      });

      this.checkboxesManufacturer.forEach((element) => {
        element.nativeElement.checked = true;
      });
      
      this.disableMainRole = true;
      this.disableManufacturer = false;
      this.disableSuperadmin = true;

      this.ClearAllNodes();
    }
    else {
      _.map(this.roles, (item) => {
        if (item.normalizedName !== 'SUPER ADMIN') {
          this.checkedRolesMap[item.normalizedName] = false;
        } else {
          this.checkedRolesMap[item.normalizedName] = true;
        }       
      });
      
      this.checkboxesMain.forEach((element) => {
        element.nativeElement.checked = false;
      });

      this.checkboxesSuperAdmin.forEach((element) => {
        element.nativeElement.checked = true;
      });

      this.checkboxesManufacturer.forEach((element) => {
        element.nativeElement.checked = false;
      });
      
      this.disableSuperadmin = false;
      this.disableManufacturer = true;
      this.disableMainRole = true;
     
      this.selectAllNodes();
      this.EnableDisableTreeNodes(false);
    }
  }

  EnableDisableTreeNodes(val: boolean) {
    _.forEach(this.regions, (node) => {
      node.selectable = val;
      this.SetSelectableValue(node.children, val);
    });

  }

  SetSelectableValue(nodes: TreeNode[], val: boolean) {
    _.forEach(nodes, (child) => {
      child.selectable = val;
      this.SetSelectableValue(child.children, val);
    });
  }

  ClearAllNodes(): void {
    this.selectedNodes = [];
  }

  selectAllNodes() {
    this.selectedNodes = [];

    if (this.regions){
      var node = this.regions.find(n => n.label === 'Service Delivery Organisation');      
      if (node) {        
        var thisNode = this.AssignNode(node);    
      }
    }
  }

  SelectAllNodeChildren(node: TreeNode): RegionManagementNodeDto[] {
    var childrens: RegionManagementNodeDto[] = []
    
    _.forEach(node.children, (child) => {
      childrens.push(this.AssignNode(child));
    });

    return childrens;
  }


  AssignNode(node: TreeNode): RegionManagementNodeDto {

    const regionNode: RegionManagementNodeDto = {
      key: node.key,
      label: node.label,
      type: node.type,
      styleClass: node.styleClass,
      expanded: node.expanded,
      icon: node.icon,
      partialSelected: true,
      selected: true,
      data: node.data,
      children: this.SelectAllNodeChildren(node),
      init: function (_data?: any): void {
        if (_data) {
          this.key = _data["key"];
          this.label = _data["label"];
          this.type = _data["type"];
          this.styleClass = _data["styleClass"];
          this.expanded = _data["expanded"];
          this.icon = _data["icon"];
          this.partialSelected = _data["partialSelected"];
          this.selected = _data["selected"];
          this.data = _data["data"] ? RegionManagementDataDto.fromJS(_data["data"]) : <any>undefined;
          if (Array.isArray(_data["children"])) {
              this.children = [] as any;
              for (let item of _data["children"])
                  this.children.push(RegionManagementNodeDto.fromJS(item));
          }
      }
      },
      toJSON: function (data?: any) {
        data = typeof data === 'object' ? data : {};
        data["key"] = this.key;
        data["label"] = this.label;
        data["type"] = this.type;
        data["styleClass"] = this.styleClass;
        data["expanded"] = this.expanded;
        data["icon"] = this.icon;
        data["partialSelected"] = this.partialSelected;
        data["selected"] = this.selected;
        data["data"] = this.data ? this.data.toJSON() : <any>undefined;
        if (Array.isArray(this.children)) {
            data["children"] = [];
            for (let item of this.children)
                data["children"].push(item.toJSON());
        }
        return data;
      },
      clone: function (): RegionManagementNodeDto {
        const json = this.toJSON();
        let result = new RegionManagementNodeDto();
        result.init(json);
        return result;
      }
    };

    this.selectedNodes.push(regionNode);
    return regionNode;
  }

}
