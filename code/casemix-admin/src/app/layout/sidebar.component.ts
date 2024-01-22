import { Component, ChangeDetectionStrategy, Renderer2, OnInit, ChangeDetectorRef, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { LayoutStoreService } from '@shared/layout/layout-store.service';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'sidebar',
  templateUrl: './sidebar.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent extends AppComponentBase implements OnInit {
  sidebarExpanded: boolean;
  isAdmin: boolean;
  role: string;

  constructor(
    injector: Injector,
    private renderer: Renderer2,
    private _layoutStore: LayoutStoreService,
    private _userService: UserServiceProxy,
    private cdr: ChangeDetectorRef,
    private _cdf: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._layoutStore.sidebarExpanded.subscribe((value) => {
      this.sidebarExpanded = value;
      this.toggleSidebar();
    });
    this.getUserDetails();
  }

  toggleSidebar(): void {
    if (this.sidebarExpanded) {
      this.hideSidebar();
    } else {
      this.showSidebar();
    }
  }

  showSidebar(): void {
    this.renderer.removeClass(document.body, 'sidebar-collapse');
    this.renderer.addClass(document.body, 'sidebar-open');
  }

  hideSidebar(): void {
    this.renderer.removeClass(document.body, 'sidebar-open');
    this.renderer.addClass(document.body, 'sidebar-collapse');
  }

  private getUserDetails(): void {
    this._userService.get(this.appSession.userId).subscribe((userInfo) => {
      this.role = userInfo.roleNames.find((u) => u === 'SUPER ADMIN');

      if (this.role === 'SUPER ADMIN') {
        this.isAdmin = true;
      }

      if (!this.isAdmin) {
        this.hideSidebar();
      }
      this._cdf.detectChanges();
    });
  }
}
