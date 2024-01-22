import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router, RouterEvent, NavigationEnd, PRIMARY_OUTLET } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { MenuItem } from '@shared/layout/menu-item';
import { HospitalDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { data } from 'jquery';
import { map } from 'lodash';
import { LocalStorageService } from '@shared/services/local-storage.service';

@Component({
  selector: 'sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
})
export class SidebarMenuComponent extends AppComponentBase implements OnInit {
  menuItems: MenuItem[];
  menuItemsMap: { [key: number]: MenuItem } = {};
  activatedMenuItems: MenuItem[] = [];
  routerEvents: BehaviorSubject<RouterEvent> = new BehaviorSubject(undefined);
  homeRoute = '/app/home';
  isAdmin: boolean;
  role: string;

  hospital: HospitalDto;

  constructor(
    injector: Injector,
    private _cdf: ChangeDetectorRef,
    private router: Router,
    private _userService: UserServiceProxy,
    private _localStorageService: LocalStorageService,
    private cdr: ChangeDetectorRef
  ) {
    super(injector);
    this.router.events.subscribe(this.routerEvents);
  }

  ngOnInit(): void {
    const lsHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (lsHospital) {
      this.hospital = lsHospital;
    }

    this.getUserDetails();
    this.menuItems = this.getMenuItems();
    this.patchMenuItems(this.menuItems);
    this.routerEvents.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event) => {
      const currentUrl = event.url !== '/' ? event.url : this.homeRoute;
      const primaryUrlSegmentGroup = this.router.parseUrl(currentUrl).root.children[PRIMARY_OUTLET];
      if (primaryUrlSegmentGroup) {
        this.activateMenuItems('/' + primaryUrlSegmentGroup.toString());
      }
    });
  }

  getMenuItems(): MenuItem[] {
    return [
      new MenuItem(this.l('Dashboard'), '/app/home', 'fas fa-tachometer-alt', 'Pages.Dashboard'),
      new MenuItem(this.l('Surveys'), '/app/surveys', 'fas fa-poll', 'Pages.Surveys'),
      new MenuItem(this.l('POAPs'), '/app/poaps', 'fas fa-user-md', 'Pages.Poaps'),
      new MenuItem(this.l('Tenants'), '/app/tenants', 'fas fa-building', 'Pages.Tenants'),
      new MenuItem(this.l('Devices'), '/app/devices', 'fas fa-laptop', 'Pages.Devices'),
      // new MenuItem(this.l('Users'), '/app/users', 'fas fa-users', 'Pages.Users'),
      // new MenuItem(this.l('Roles'), '/app/roles', 'fas fa-theater-masks', 'Pages.Roles'),
      new MenuItem(this.l('Settings'), '/app/settings', 'fas fa-cog', 'Pages.Settings', [
        new MenuItem(this.l('User Management'), '', '', '', [
          new MenuItem(this.l('Users'), '/app/users', 'fas fa-users', 'Pages.Users'),
          new MenuItem(this.l('Roles'), '/app/roles', 'fas fa-theater-masks', 'Pages.Roles'),
        ]),
        new MenuItem(this.l('Modules'), '', '', '', [
          new MenuItem(this.l('ReportingSettings'), '/app/reporting-settings', 'fas fa-video', ''),
          new MenuItem(this.l('SurveySettings'), '/app/survey-settings', 'fas fa-tachometer-alt', ''),
        ]),

        new MenuItem(this.l('DeviceManagement'), '/app/devices', 'fas fa-laptop', 'Pages.DeviceManagement'),
        new MenuItem(this.l('RiskMappingSettings'), '/app/risk-mapping-settings', 'fas fa-image', ''),
        new MenuItem(this.l('RegionManagement'), '/app/region-management', 'fas fa-image', 'Pages.RegionManagement'),
        
        new MenuItem(this.l('Hospital Management'), '/app/hospital-management', 'fas fa-building', 'Pages.HospitalManagement'),
        new MenuItem(this.l('Anaesthesia Room'), '/app/hospital-management', 'fas fa-building', 'Pages.HospitalManagement'),
        new MenuItem(this.l('Theaters'), '/app/theaters', 'fas fa-person-booth', 'Pages.Theaters'),

        // new MenuItem(this.l('Hospital Management'), '/app/hospital-management', 'fas fa-building', 'Pages.HospitalManagement', [
        //   // new MenuItem(this.l('HospitalManagement'), '', '', ''),
        // new MenuItem(this.l('Anaesthesia Room'), '/app/anaesthesia-room', 'fas fa-building', 'Pages.HospitalManagement'),
        // new MenuItem(this.l('Theaters'), '/app/theaters', 'fas fa-person-booth', 'Pages.Theaters'),
        // ]),
      ]),
    ];
  }

  patchMenuItems(items: MenuItem[], parentId?: number): void {
    items.forEach((item: MenuItem, index: number) => {
      item.id = parentId ? Number(parentId + '' + (index + 1)) : index + 1;
      if (parentId) {
        item.parentId = parentId;
      }
      if (parentId || item.children) {
        this.menuItemsMap[item.id] = item;
      }
      if (item.children) {
        this.patchMenuItems(item.children, item.id);
      }
    });
  }

  activateMenuItems(url: string): void {
    this.deactivateMenuItems(this.menuItems);
    this.activatedMenuItems = [];
    const foundedItems = this.findMenuItemsByUrl(url, this.menuItems);
    foundedItems.forEach((item) => {
      this.activateMenuItem(item);
    });
  }

  deactivateMenuItems(items: MenuItem[]): void {
    items.forEach((item: MenuItem) => {
      item.isActive = false;
      item.isCollapsed = true;
      if (item.children) {
        this.deactivateMenuItems(item.children);
      }
    });
  }

  findMenuItemsByUrl(url: string, items: MenuItem[], foundedItems: MenuItem[] = []): MenuItem[] {
    items.forEach((item: MenuItem) => {
      if (item.route === url) {
        foundedItems.push(item);
      } else if (item.children) {
        this.findMenuItemsByUrl(url, item.children, foundedItems);
      }
    });
    return foundedItems;
  }

  activateMenuItem(item: MenuItem): void {
    item.isActive = true;
    if (item.children) {
      item.isCollapsed = false;
    }
    this.activatedMenuItems.push(item);
    if (item.parentId) {
      this.activateMenuItem(this.menuItemsMap[item.parentId]);
    }
  }

  isMenuItemVisible(item: MenuItem, isAdmin: boolean): boolean {
    if (!item.permissionName) {
      return true;
    }
    if (!this.isAdmin) {
      if (item.label === 'Surveys' || item.label === 'POAPs') {
        return false;
      }
    }
    if (this.isAdmin) {
      if (item.label === 'Devices') {
        return false;
      }
    }
    
    if (!this.isAdmin) {      
      if (this.hospital !== undefined) {
        if (this.hospital.activeDevMgt === false) {      
          if (item.label === 'Device Management') {
            return false;
          }
        }
      }
      
    }
    
    return this.permission.isGranted(item.permissionName);
  }

  private getUserDetails(): void {
    this._userService.get(this.appSession.userId).subscribe((userInfo) => {
      this.role = userInfo.roleNames.find((u) => u === 'SUPER ADMIN');

      if (this.role === 'SUPER ADMIN') {
        this.isAdmin = true;
      }
      this._cdf.detectChanges();
    });
  }
}
