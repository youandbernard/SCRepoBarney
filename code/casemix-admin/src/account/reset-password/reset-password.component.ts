import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { PasswordResetsServiceProxy } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.less'],
  animations: [accountModuleAnimation()],
})
export class ResetPasswordComponent extends AppComponentBase implements OnInit {
  emailAddress: string;
  saving = false;

  constructor(
    injector: Injector,
    private _router: Router,
    private _passwordResetsService: PasswordResetsServiceProxy,
    ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  onResetPasswordSubmit(): void {
    this._passwordResetsService.create(this.emailAddress)
      .subscribe(() => {
        this.notify.info(this.l('PasswordResetEmailSent'));
        this._router.navigate(['/account/login']);
      });
  }
}
