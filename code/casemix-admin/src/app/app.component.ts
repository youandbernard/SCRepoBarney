import { Component, Injector, OnInit, Renderer2 } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SignalRAspNetCoreHelper } from '@shared/helpers/SignalRAspNetCoreHelper';
import { LayoutStoreService } from '@shared/layout/layout-store.service';
import * as moment from 'moment';
import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from 'abp-ng2-module';
import { Router } from '@angular/router';

@Component({
  templateUrl: './app.component.html'
})
export class AppComponent extends AppComponentBase implements OnInit {
  sidebarExpanded: boolean;

  constructor(
    injector: Injector,
    private renderer: Renderer2,
    private _layoutStore: LayoutStoreService,
    private _router: Router
  ) {
    super(injector);
    moment.fn.toISOString = function() {
      return this.format('YYYY-MM-DD HH:mm:ss');
    };
  }

  ngOnInit(): void {
    this.renderer.addClass(document.body, 'sidebar-mini');
    const encryptedAuthToken = new UtilsService().getCookieValue(
      AppConsts.authorization.encryptedAuthTokenName
    );
    const encryptedAbpAuthToken = new UtilsService().getCookieValue(
      AppConsts.authorization.encryptedAbpAuthTokenName
    );
    if (encryptedAuthToken && encryptedAbpAuthToken) {
      // SignalRAspNetCoreHelper.initSignalR();
    } else {
      this._router.navigate(['account/login']);
    }

    abp.event.on('abp.notifications.received', userNotification => {
      abp.notifications.showUiNotifyForUserNotification(userNotification);

      // Desktop notification
      Push.create('AbpZeroTemplate', {
        body: userNotification.notification.data.message,
        icon: abp.appPath + 'assets/app-logo-small.png',
        timeout: 6000,
        onClick: function() {
          window.focus();
          this.close();
        }
      });
    });

    this._layoutStore.sidebarExpanded.subscribe(value => {
      this.sidebarExpanded = value;
    });
  }

  toggleSidebar(): void {
    this._layoutStore.setSidebarExpanded(!this.sidebarExpanded);
  }
}
