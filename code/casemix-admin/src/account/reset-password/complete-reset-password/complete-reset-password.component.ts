import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { PasswordResetsServiceProxy, PasswordResetInputDto } from '@shared/service-proxies/service-proxies';
import { accountModuleAnimation } from '@shared/animations/routerTransition';

@Component({
  selector: 'app-complete-reset-password',
  templateUrl: './complete-reset-password.component.html',
  styleUrls: ['./complete-reset-password.component.less'],
  animations: [accountModuleAnimation()],
})
export class CompleteResetPasswordComponent extends AppComponentBase implements OnInit {
  passwordReset: PasswordResetInputDto;
  saving = false;

  constructor(
    injector: Injector,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _passwordResetsService: PasswordResetsServiceProxy,
  ) {
    super(injector);
    this.passwordReset = new PasswordResetInputDto();
  }

  ngOnInit(): void {
    this._activatedRoute.paramMap.subscribe(paramMap => {
      this.passwordReset.id = paramMap.get('id');
      this.validatePasswordReset();
    });
  }

  onResetPasswordSubmit(): void {
    this.saving = true;
    this._passwordResetsService.resetPassword(this.passwordReset)
      .subscribe(() => {
        this.saving = false;
        this.notify.info(this.l('SavedSuccessfully'));
        this._router.navigate(['/account/login']);
      }, error => {
        this.saving = false;
      });
  }

  private validatePasswordReset(): void {
    this.saving = true;
    this._passwordResetsService.validate(this.passwordReset.id)
      .subscribe(() => {
        this.saving = false;
      }, error => {
        this.saving = false;
        this._router.navigate(['/account/login']);
      });
  }
}
