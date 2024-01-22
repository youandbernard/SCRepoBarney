import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { InstrumentPackDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-instrument-select',
  templateUrl: './instrument-select.component.html',
  styleUrls: ['./instrument-select.component.less']
})
export class InstrumentSelectComponent extends AppComponentBase implements OnInit {
  @Input() instrumentPack: InstrumentPackDto;

  basicHipTray: boolean = false;
  basicSet20: boolean = false;
  hipExras: boolean = false;
  kimal: boolean = false;

  constructor(
    injector: Injector,
    private _modalRef: BsModalRef,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.instrumentPack){
      if (this.instrumentPack.packName === 'Surgical Holdings: BASIC HIP TRAY-15'){
          this.basicHipTray = true;
      }
      else if (this.instrumentPack.packName === 'Surgical Holdings: BASIC SET-20'){
          this.basicSet20 = true;
      }
      else if (this.instrumentPack.packName === 'Surgical Holdings: HIP EXTRAS-10'){
          this.hipExras = true;
      }
      else if (this.instrumentPack.packName === 'Kimal: K201-14001'){
        this.kimal = true;
      }
    }
  }

  onCloseClick(): void{
    this.close();
  }

  private close(): void {
    this._modalRef.hide();
  }
}
