import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, Injector } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { PoapProcedureDto } from '@shared/service-proxies/service-proxies';
import { interval, Observable, Subscription } from 'rxjs';
import { mainModule } from 'process';
import * as moment from 'moment';
import { AppComponentBase } from '@shared/app-component-base';

export interface TimeSpan {
  hours: number;
  minutes: number;
  seconds: number;
}

@Component({
  selector: 'app-edit-actual-time',
  templateUrl: './edit-actual-time.component.html',
  styleUrls: ['./edit-actual-time.component.less']
})
export class EditActualTimeComponent extends AppComponentBase implements OnInit {
  @Input() procedure: PoapProcedureDto;
  @Output() modalSave: EventEmitter<{ actualTime: number, isAddNotes: boolean}> = new EventEmitter<{ actualTime: number, isAddNotes: boolean}>();

  isAddNotes: boolean = false;
  disableBox: boolean = false;

  timer: Observable<number>;
  timerSubscription: Subscription;
  currentTime: Date;
  running = false;
  elapsed: TimeSpan;
  savedTime: TimeSpan;

  constructor(injector: Injector, private _modalRef: BsModalRef, private _changeDetector: ChangeDetectorRef) {
    super(injector);
    this.timer = interval(1000);
    this.location.onPopState(() => this.close());
  }

  ngOnInit(): void {
    if (this.procedure.actualTime > 0) {
      this.getDefaultTime();
    } else {
      this.getElapsedTime();
    }

    this.isAddNotes = this.procedure.hasSurveyNotes;
  }

  onFormSubmit(): void {
    this.close();
  }

  onCloseClick(): void {
    this.close();
  }

  onTimerStart(): void {
    this.disableBox = true;
    this.running = true;
    this.currentTime = new Date();
    this.procedure.clockStartTimestamp = moment(this.currentTime);
    this.timerSubscription = this.timer.subscribe(() => {
      this.getElapsedTime();
      this._changeDetector.detectChanges();
    });
  }

  onTimerStop(): void {
    this.disableBox = false;
    this.running = false;
    this.currentTime = new Date();
    this.procedure.clockEndTimestamp = moment(this.currentTime);
    this.timerSubscription.unsubscribe();
    let actualTime = this.elapsed.hours * 60;
    actualTime += this.elapsed.minutes;
    actualTime += this.elapsed.seconds / 60;
    
    let calcActualTime = +actualTime.toFixed(2);
    this.modalSave.emit({ actualTime: calcActualTime, isAddNotes: this.isAddNotes});
    this.close();
  }

  getElapsedTime(): void {
    if (!this.currentTime) {
      this.elapsed = {
        hours: 0,
        minutes: 0,
        seconds: 0
      };
      return;
    }

    if (this.procedure.actualTime === 0) {
      this.savedTime = {
        hours: 0,
        minutes: 0,
        seconds: 0
      };
    }

    let totalSeconds = this.savedTime.seconds + Math.floor((new Date().getTime() - this.currentTime.getTime()) / 1000);

    let hours = 0;
    let minutes = 0;
    let seconds = 0;

    if (totalSeconds >= 3600) {
      hours = Math.floor(totalSeconds / 3600);
      totalSeconds -= 3600 * hours;
    }

    if (totalSeconds >= 60) {
      minutes = Math.floor(totalSeconds / 60);
      totalSeconds -= 60 * minutes;
    }

    seconds = totalSeconds;
    hours = hours + this.savedTime.hours;
    minutes = minutes + this.savedTime.minutes;
    this.elapsed = {
      hours: hours,
      minutes: minutes,
      seconds: seconds
    };
  }

  getDefaultTime() {
    const mins = this.procedure.actualTime;
    let hh = 0;
    let mm = 0;
    let ss = 0;
    if (this.procedure.actualTime > 0) {
      hh = Math.floor(mins / 60);
      mm = Math.floor(mins - (hh * 3600) / 60);
      ss = Math.floor(mins * 60 - hh * 3600 - mm * 60);
      this.elapsed = {
        hours: hh,
        minutes: mm,
        seconds: ss
      };
      this.savedTime = this.elapsed;
      return;
    }
  }

  formatDigit(digit: number): string {
    if (digit < 10) {
      return `0${digit}`;
    }
    return `${digit}`;
  }

  onCheckNote($event): void {
    this.isAddNotes = $event.target.checked;
  }

  private close(): void {
    this._modalRef.hide();
  }
}
