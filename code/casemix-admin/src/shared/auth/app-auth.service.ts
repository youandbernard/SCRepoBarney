import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { TokenService, LogService, UtilsService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AuthenticateModel, AuthenticateResultModel, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

@Injectable()
export class AppAuthService {
  authenticateModel: AuthenticateModel;
  authenticateResult: AuthenticateResultModel;
  rememberMe: boolean;
  failedLoginAttemps = 0;
  constructor(
    private _tokenAuthService: TokenAuthServiceProxy,
    private _router: Router,
    private _utilsService: UtilsService,
    private _tokenService: TokenService,
    private _logService: LogService
  ) {
    this.clear();
  }

  logout(reload?: boolean): void {
    abp.auth.clearToken();
    abp.utils.setCookieValue(AppConsts.authorization.encryptedAuthTokenName, undefined, undefined, abp.appPath);
    if (reload !== false) {
      location.href = AppConsts.appBaseUrl;
    }
  }

  authenticate(finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => {});

    this._tokenAuthService
      .authenticate(this.authenticateModel)
      .pipe(
        finalize(() => {
          this.failedLoginAttemps += 1;
          if (this.failedLoginAttemps === 4) {
            let errorListHtml = '<a style="cursor: pointer;" href="/account/reset-password">';
            errorListHtml += '<i class="fa fa-unlock-alt"></i> Reset Password';
            errorListHtml += '</a>';
            abp.message.error(errorListHtml, 'Your account will be locked out if you enter the incorrect password again, reset your password here', {
                isHtml: true
            });
          }
          finallyCallback();
        })
      )
      .subscribe((result: AuthenticateResultModel) => {
        this.processAuthenticateResult(result);
      });
  }


resetPassword(): void {
    debugger
    this._router.navigate(['/account/reset-password']);
}

  private processAuthenticateResult(authenticateResult: AuthenticateResultModel) {
    this.authenticateResult = authenticateResult;

    if (authenticateResult.accessToken) {
      // Successfully logged in
      this.login(
        authenticateResult.accessToken,
        authenticateResult.encryptedAccessToken,
        authenticateResult.expireInSeconds,
        this.rememberMe
      );
    } else {
      // Unexpected result!

      this._logService.warn('Unexpected authenticateResult!');
      this._router.navigate(['account/login']);
    }
  }

  private login(accessToken: string, encryptedAccessToken: string, expireInSeconds: number, rememberMe?: boolean): void {
    const tokenExpireDate = rememberMe ? new Date(new Date().getTime() + 1000 * expireInSeconds) : undefined;

    this._tokenService.setToken(accessToken, tokenExpireDate);

    this._utilsService.setCookieValue(AppConsts.authorization.encryptedAuthTokenName, encryptedAccessToken, tokenExpireDate, abp.appPath);

    let initialUrl = UrlHelper.initialUrl;
    if (
      initialUrl.indexOf('/login') > 0 ||
      initialUrl.indexOf('/reset-password') > 0 ||
      initialUrl.indexOf('/complete-reset-password') > 0
    ) {
      initialUrl = AppConsts.appBaseUrl;
    }

    location.href = initialUrl;
  }

  private clear(): void {
    this.authenticateModel = new AuthenticateModel();
    this.authenticateModel.rememberClient = false;
    this.authenticateResult = null;
    this.rememberMe = false;
  }
}
