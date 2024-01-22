import { Component, ChangeDetectionStrategy, OnInit, ChangeDetectorRef, Injector } from '@angular/core';
import { DataUpdateService } from '@shared/services/data-update.service';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { HospitalDto, HospitalsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent extends AppComponentBase implements OnInit {
  hospital: HospitalDto;
  hospitals: HospitalDto[] = [];

  constructor(
    injector: Injector,
    private _cdf: ChangeDetectorRef,
    private _dataUpdateService: DataUpdateService<HospitalDto>,
    private _localStorageService: LocalStorageService,
    private _hospitalsService: HospitalsServiceProxy
    ) {
    super(injector);
    this.hospital = new HospitalDto();
  }

  ngOnInit(): void {
    const lsHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (lsHospital) {
      this.hospital = lsHospital;
    }
    this._dataUpdateService.getData().subscribe(hospital => {
      if (hospital) {
        this.hospital = hospital;
      } else {
      }
      this._cdf.detectChanges();
    });
    this._hospitalsService.getByUser(this.appSession.userId).subscribe(hospitals => {
        this.hospitals = hospitals;
        this._cdf.detectChanges();
    })
  }
}
