import { Component, Injector } from '@angular/core';
import { AbpSessionService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { HospitalDto } from '@shared/service-proxies/service-proxies';
import { DataUpdateService } from '@shared/services/data-update.service';
import { LocalStorageService } from '@shared/services/local-storage.service';

@Component({
  templateUrl: './login.component.html',
  animations: [accountModuleAnimation()]
})
export class LoginComponent extends AppComponentBase {
  submitting = false;

  constructor(
    injector: Injector,
    public authService: AppAuthService,
    private _sessionService: AbpSessionService,
    private _dataUpdateService: DataUpdateService<HospitalDto>,
    private _localStorageService: LocalStorageService,
  ) {
    super(injector);
  }

  get multiTenancySideIsTeanant(): boolean {
    return this._sessionService.tenantId > 0;
  }

  get isSelfRegistrationAllowed(): boolean {
    if (!this._sessionService.tenantId) {
      return false;
    }

    return true;
  }

  login(): void {
    this.submitting = true;
    this.authService.authenticate(() => (this.submitting = false));

    this.setClearHospitalData();

  }

  private setClearHospitalData(): void {
    this._localStorageService.clearItem();
    this._dataUpdateService.setData(null);
  }
}
