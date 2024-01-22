import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { DeviceServiceProxy, DevicesTermCodeDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-device-gmdntermcodes',
  templateUrl: './device-gmdntermcodes.component.html',
  styleUrls: ['./device-gmdntermcodes.component.less'],
  animations: [appModuleAnimation()],
})
export class DeviceGmdntermcodesComponent extends AppComponentBase implements OnInit {
  
  deviceList: DevicesTermCodeDto[] = [];
  gmdntermcode = '';

  constructor(injector: Injector,
    private _modalRef: BsModalRef,
    private _deviceService: DeviceServiceProxy) { 
      super(injector)
    }

  ngOnInit(): void {
    this.getDeviceList();
  }

  getDeviceList(): void {
    this._deviceService.getByDeviceGMDNCode(this.gmdntermcode)
      .subscribe((res: DevicesTermCodeDto[]) => {
        this.deviceList = res;
      });
  }

  onCloseClick(): void {
      this.close();
  }

  private close(): void {
    this._modalRef.hide();
  }


}
