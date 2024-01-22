import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { UserServiceProxy, UserDto, UserDtoPagedResultDto, HospitalDto } from '@shared/service-proxies/service-proxies';
import { CreateUserDialogComponent } from './create-user/create-user-dialog.component';
import { EditUserDialogComponent } from './edit-user/edit-user-dialog.component';
import { ResetPasswordDialogComponent } from './reset-password/reset-password.component';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';

class PagedUsersRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.less'],
  animations: [appModuleAnimation()],
})
export class UsersComponent extends PagedListingComponentBase<UserDto> {
  users: UserDto[] = [];
  hospital: HospitalDto = new HospitalDto();
  keyword = '';
  isActive: boolean | null;
  advancedFiltersVisible = false;
  realmsText: string;

  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
    private _modalService: BsModalService,
    private _localStorageService: LocalStorageService
  ) {
    super(injector);
  }

  createUser(): void {
    this.showCreateOrEditUserDialog();
  }

  editUser(user: UserDto): void {
    this.showCreateOrEditUserDialog(user.id);
  }

  public resetPassword(user: UserDto): void {
    this.showResetPasswordUserDialog(user.id);
  }

  clearFilters(): void {
    this.keyword = '';
    this.isActive = undefined;
    this.getDataPage(1);
  }

  protected list(request: PagedUsersRequestDto, pageNumber: number, finishedCallback: Function): void {
    request.keyword = this.keyword;
    request.isActive = this.isActive;
    this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    this._userService
      .getAll(request.keyword, request.isActive, this.hospital.id, request.skipCount, request.maxResultCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: UserDtoPagedResultDto) => {
        _.forEach(result.items, (item) => {
          if (item.userRealmMappings.length > 0) {
            var mappingsRegion = item.userRealmMappings
              .map((e) => {
                if (e.regionId != null) return e?.region?.name;
              })
              .filter((item) => item);
            var mappingsHospital = item.userRealmMappings
              .map((e) => {
                if (e.hospitalId != null) return e?.hospital?.name;
              })
              .filter((item) => item);

            item.userRealmMappingRegions = mappingsRegion.join(',');
            item.userRealmMappingHospitals = mappingsHospital.join(',');

            this.realmsText = 'Regions: ' + item.userRealmMappingRegions + ' \n' + 'Hospitals: ' + item.userRealmMappingHospitals;
          }
        });

        this.users = result.items;  
        this.showPaging(result, pageNumber);
      });
  }
  
  protected delete(user: UserDto): void {
    abp.message.confirm(this.l('UserDeleteWarningMessage', user.fullName), undefined, (result: boolean) => {
      if (result) {
        this._userService.delete(user.id).subscribe(() => {
          abp.notify.success(this.l('SuccessfullyDeleted'));
          this.refresh();
        });
      }
    });
  }

  private showResetPasswordUserDialog(id?: number): void {
    this._modalService.show(ResetPasswordDialogComponent, {
      class: 'modal-lg',
      initialState: {
        id: id,
      },
    });
  }

  private showCreateOrEditUserDialog(id?: number): void {
    let createOrEditUserDialog: BsModalRef;
    if (!id) {
      createOrEditUserDialog = this._modalService.show(CreateUserDialogComponent, {
        class: 'modal-lg',
      });
    } else {
      createOrEditUserDialog = this._modalService.show(EditUserDialogComponent, {
        class: 'modal-lg',
        initialState: {
          id: id,
        },
      });
    }

    createOrEditUserDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
