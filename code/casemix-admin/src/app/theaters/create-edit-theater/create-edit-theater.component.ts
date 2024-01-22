import { Component, OnInit, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TheaterDto, TheatersServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-create-edit-theater',
  templateUrl: './create-edit-theater.component.html',
  styleUrls: ['./create-edit-theater.component.less']
})
export class CreateEditTheaterComponent extends AppComponentBase implements OnInit {
  @Output() modalSave = new EventEmitter<any>();

  id: string;
  hospitalId: string;
  isSaving = false;
  theater: TheaterDto = new TheaterDto();
  isLoading = false;

  constructor(
    injector: Injector,
    private _modal: BsModalRef,
    private _theatersService: TheatersServiceProxy,
  ) {
    super(injector);
    this.location.onPopState(() => this.close());
  }

  ngOnInit(): void {
    if (!this.id) {
      this.theater.hospitalId = this.hospitalId;
    } else {
      this.getTheater();
    }
  }

  onFormSubmit(): void {
    this.isSaving = true;
    const theaterSubscription = this.id ? this._theatersService.update(this.theater) : this._theatersService.createTheater(this.theater);
    if (this.id) {
      this._theatersService.update(this.theater)
      .subscribe((result) => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.isSaving = false;
        this.modalSave.emit();
        this.close();
      },
      error => {
        this.isSaving = false;
      });
    }
    else {
      this._theatersService.createTheater(this.theater)
      .subscribe((result) => {
        if (result.isSuccess) {
          this.notify.success(this.l('SavedSuccessfully'));
          this.isSaving = false;
          this.modalSave.emit();
          this.close();
        }else {
          this.notify.error(this.l(result.errorMessage));
          this.isSaving = false;
          // this.modalSave.emit();
          // this.close();
        }
        
      },
      error => {
        this.isSaving = false;
      });
    }

    // theaterSubscription.subscribe((result: any) => {
    //   this.notify.success(this.l('SavedSuccessfully'));
    //   this.isSaving = false;
    //   this.modalSave.emit();
    //   this.close();
    // },
    // error => {
    //   this.isSaving = false;
    // });
  }

  onCloseClick(): void {
    this.close();
  }

  private close(): void {
    // debugger
    this._modal.hide();
  }

  private getTheater(): void {
    this._theatersService.get(this.id)
      .subscribe(theater => {
        this.theater = theater;
      });
  }
}
