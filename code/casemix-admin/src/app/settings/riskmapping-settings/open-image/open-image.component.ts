import { Component, OnInit, Output, EventEmitter, Injector, Input, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import  dataList  from '../../riskmapping.json';

@Component({
  selector: 'app-open-image',
  templateUrl: './open-image.component.html',
  styleUrls: ['./open-image.component.less']
})
export class OpenImageComponent extends AppComponentBase implements OnInit {
  // @Output modalSave = new EventEmitter<any>();
  @Input() id: number;
  imageId: number;
  currentSlideIndex = 0;
  list: {id: number, imageUrl: string }[] = [];
  constructor(injector: Injector, private _modal: BsModalRef, private _cdf: ChangeDetectorRef) {
    super(injector);
   }

  ngOnInit(): void {
    this.list = dataList;
    this.currentSlideIndex = this.list.map(function(e) { return e.id; }).indexOf(this.id);
  }
  onCloseClick(): void {
    this.close();
  }

  private close(): void {
    this._modal.hide();
  }
}
