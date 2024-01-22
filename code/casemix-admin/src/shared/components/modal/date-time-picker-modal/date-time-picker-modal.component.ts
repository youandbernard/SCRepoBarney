import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import * as moment from 'moment';

@Component({
  selector: 'app-date-time-picker-modal',
  templateUrl: './date-time-picker-modal.component.html',
  styleUrls: ['./date-time-picker-modal.component.less']
})
export class DateTimePickerModalComponent extends AppComponentBase implements OnInit {
  @Input() datetime: moment.Moment;
  @Output() modalSave: EventEmitter<moment.Moment> = new EventEmitter<moment.Moment>();

  date: Date;
  time: Date;
  datePickerConfig: BsDatepickerConfig;

  constructor(
    injector: Injector,
    private _modalRef: BsModalRef,
  ) {
    super(injector);
    this.datePickerConfig = new BsDatepickerConfig();
    this.datePickerConfig.showWeekNumbers = false;
    this.datePickerConfig.dateInputFormat = 'DD/MM/YYYY';
    this.location.onPopState(() => this.close());
  }

  ngOnInit(): void {
    if (this.datetime) {
      this.date = this.datetime.toDate();
      this.time = this.datetime.toDate();
    } else {
      this.date = new Date();
      this.time = new Date();
    }
  }

  onCloseClick(): void {
    this.close();
  }

  onFormSubmit(): void {
    const date = moment(this.date).format('YYYY-MM-DD');
    const time = moment(this.time).format('HH:mm');
    const datetime = moment(`${date} ${time}`);
    this.modalSave.emit(datetime);
    this.close();
  }

  private close(): void {
    this._modalRef.hide();
  }
}
