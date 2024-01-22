import { Component, OnInit, Input, Output, EventEmitter, Injector } from '@angular/core';
import * as moment from 'moment';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-time-picker-modal',
  templateUrl: './time-picker-modal.component.html',
  styleUrls: ['./time-picker-modal.component.less']
})
export class TimePickerModalComponent extends AppComponentBase implements OnInit {
  @Input() sTime: string;
  @Output() modalSave: EventEmitter<string> = new EventEmitter<string>();

  time: Date;

  constructor(
    injector: Injector,
    private _modalRef: BsModalRef,
    ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.sTime) {
      this.time = moment(`${this.sTime}`, 'hh:mm A').toDate();
    } else {
      this.time = new Date();
    }
  }

  onCloseClick(): void {
    this.close();
  }

  onFormSubmit(): void {
    const time = moment(this.time).format('hh:mm A');
    this.modalSave.emit(time);
    this.close();
  }

  private close(): void {
    this._modalRef.hide();
  }

}
