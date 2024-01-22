import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { UserServiceProxy, UserDto, HospitalDto, HospitalsServiceProxy, ManufacturesServiceProxy } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.less'],
  animations: [appModuleAnimation()],
})
export class DevicesComponent extends AppComponentBase implements OnInit {
  data: TreeNode[];
  isLoading = false;

  isAdmin: boolean = false;
  isManufacturer: boolean = false;
  isSurgeon: boolean = false;
  isSuperAdmin: boolean = false;

  currentHospitalName: string = "";
  currentManufacturer: string = "";

  // role: string;
  userDetail: UserDto;

  constructor(injector: Injector,
      private _router: Router,
      private _localStorageService: LocalStorageService,
      private _hospitalsService: HospitalsServiceProxy,
      private _manufacturerService: ManufacturesServiceProxy,
    ) {
    super(injector);
  }

  ngOnInit(): void {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (localHospital) {
      this.currentHospitalName = localHospital.name;

      if (!localHospital.activeDevMgt) {
        this._router.navigate(['/app/home']);
      }
    }
    else {
      this.getHospitalByUser();
    }

    this.data = [
      {
        label: 'World',
        data: 'World Folder',
        // expandedIcon: 'pi pi-folder-open',
        // collapsedIcon: 'pi pi-folder',
        children: [
          {
            label: 'UK',
            data: 'Documents Folder',
            // expandedIcon: 'pi pi-folder-open',
            // collapsedIcon: 'pi pi-folder',
            children: [
              {
                label: 'South West',
                data: 'Work Folder',
                // expandedIcon: 'pi pi-folder-open',
                // collapsedIcon: 'pi pi-folder',
                children: [
                  { label: 'Hospital1', icon: 'pi pi-th-large', data: 'Expenses Document' },
                  { label: 'Hospital2', icon: 'pi pi-th-large', data: 'Resume Document' },
                ],
              },
              {
                label: 'South East',
                data: 'Home Folder',
                // expandedIcon: 'pi pi-folder-open',
                // collapsedIcon: 'pi pi-folder',
                children: [{ label: 'Hospital3', icon: 'pi pi-th-large', data: 'Invoices for this month' }],
              },
            ],
          },
          {
            label: 'South Korea',
            data: 'Pictures Folder',
            // expandedIcon: 'pi pi-folder-open',
            // collapsedIcon: 'pi pi-folder',
            children: [
              { label: 'barcelona.jpg', icon: 'pi pi-image', data: 'Barcelona Photo' },
              { label: 'logo.jpg', icon: 'pi pi-image', data: 'PrimeFaces Logo' },
              { label: 'primeui.png', icon: 'pi pi-image', data: 'PrimeUI Logo' },
            ],
          },
          {
            label: 'Hungary',
            data: 'Movies Folder',
            // expandedIcon: 'pi pi-folder-open',
            // collapsedIcon: 'pi pi-folder',
            children: [
              {
                label: 'Al Pacino',
                data: 'Pacino Movies',
                children: [
                  { label: 'Scarface', icon: 'pi pi-video', data: 'Scarface Movie' },
                  { label: 'Serpico', icon: 'pi pi-video', data: 'Serpico Movie' },
                ],
              },
              {
                label: 'Robert De Niro',
                data: 'De Niro Movies',
                children: [
                  { label: 'Goodfellas', icon: 'pi pi-video', data: 'Goodfellas Movie' },
                  { label: 'Untouchables', icon: 'pi pi-video', data: 'Untouchables Movie' },
                ],
              },
            ],
          },
        ],
      },
    ];

    this.getUserDetails();
    this.getManufacturer();
  }

  getManufacturer(): void {
    if (this.appSession.user.manufactureId) {
      this._manufacturerService.getById(this.appSession.user.manufactureId).subscribe((manufacturer) => {      
        if (manufacturer) {
          this.currentManufacturer = manufacturer.name;
        }
      });
    }    
  }

  getHospitalByUser() : void {
    this._hospitalsService.getByUser(this.appSession.userId).subscribe((hospitals) => {
      
      if (hospitals) {
        var currentHospital = hospitals[0];
        if (currentHospital) {
          if (!currentHospital.activeDevMgt)
            this._router.navigate(['/app/home']);
        }
        else
          this._router.navigate(['/app/home']);
      }
    });
  }

  expandAll() {
    this.data.forEach((node) => {
      this.expandRecursive(node, true);
    });
  }

  collapseAll() {
    this.data.forEach((node) => {
      this.expandRecursive(node, false);
    });
  }

  private expandRecursive(node: TreeNode, isExpand: boolean) {
    node.expanded = isExpand;
    if (node.children) {
      node.children.forEach((childNode) => {
        this.expandRecursive(childNode, isExpand);
      });
    }
  }

  private getUserDetails(): void {
    var roleNames = this.appSession.getUserRoles();
    // this.role = userInfo.roleNames.find((u) => u === 'SUPER ADMIN' || u === 'ADMIN' || u === 'MANUFACTURER');

    if (roleNames.find((u) => u === 'ADMIN') !== undefined) {
      if (roleNames.find((u) => u === 'ADMIN').length > 0) {
        this.isAdmin = true;
      }
    }

    if (roleNames.find((u) => u === 'SUPER ADMIN') !== undefined) {
      if (roleNames.find((u) => u === 'SUPER ADMIN').length > 0) {
        this.isSuperAdmin = true;
      }
    }

    if (roleNames.find((u) => u === 'MANUFACTURER') !== undefined) {
      if (roleNames.find((u) => u === 'MANUFACTURER').length > 0) {
        this.isManufacturer = true;
      }
    }

    if (roleNames.find((u) => u === 'SURGEON') !== undefined) {
      if (roleNames.find((u) => u === 'SURGEON').length > 0) {
        this.isSurgeon = true;
      }
    }
  }
}
