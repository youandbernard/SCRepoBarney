import { Component, Injector, ChangeDetectionStrategy, OnInit, ChangeDetectorRef } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { HospitalDto } from '@shared/service-proxies/service-proxies';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';

@Component({
  templateUrl: './settings.component.html',
  animations: [appModuleAnimation()],
})
export class SettingsComponent extends AppComponentBase implements OnInit {
  currentHospitalId: string;
  hospital: HospitalDto = new HospitalDto();
  deviceManagementEnabled = false;
  isAdmin: boolean;

  constructor(injector: Injector, 
    private _localStorageService: LocalStorageService) {
      super(injector);
  }

  ngOnInit(): void {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (localHospital) {
      this.hospital = localHospital;
      this.deviceManagementEnabled = this.hospital.activeDevMgt;
    }

    this.isAdmin = this.appSession.user.isAdmin;
  }

}
