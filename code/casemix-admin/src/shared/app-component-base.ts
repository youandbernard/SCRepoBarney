import { Injector, ElementRef, inject } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import {
  LocalizationService,
  PermissionCheckerService,
  FeatureCheckerService,
  NotifyService,
  SettingService,
  MessageService,
  AbpMultiTenancyService,
} from 'abp-ng2-module';

import { AppSessionService } from '@shared/session/app-session.service';
import { BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { PlatformLocation } from '@angular/common';

export abstract class AppComponentBase {
  localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

  localization: LocalizationService;
  permission: PermissionCheckerService;
  feature: FeatureCheckerService;
  notify: NotifyService;
  setting: SettingService;
  message: MessageService;
  multiTenancy: AbpMultiTenancyService;
  appSession: AppSessionService;
  elementRef: ElementRef;
  location: PlatformLocation;

  localStorageKey: {
    hospital?: string;
  } = {};
  defaultDtOptions: DataTables.Settings = {};
  defaultModalSettings: ModalOptions;

  constructor(injector: Injector) {
    this.localization = injector.get(LocalizationService);
    this.permission = injector.get(PermissionCheckerService);
    this.feature = injector.get(FeatureCheckerService);
    this.notify = injector.get(NotifyService);
    this.setting = injector.get(SettingService);
    this.message = injector.get(MessageService);
    this.multiTenancy = injector.get(AbpMultiTenancyService);
    this.appSession = injector.get(AppSessionService);
    this.elementRef = injector.get(ElementRef);
    this.location = injector.get(PlatformLocation);
    this.localStorageKey.hospital = `${this.appSession.user}-hospital`;

    this.defaultDtOptions = {
      destroy: true,
      // dom: 'rt<"dt-bottom"lp<"clearfix">>',
      pageLength: 10,
      serverSide: true,
      autoWidth: false,
      stateSave: false,
      language: {
        infoFiltered: '',
        zeroRecords: 'No record available',
      },
      order: [0, 'asc'],
    };
    this.defaultModalSettings = {
      backdrop: 'static',
      ignoreBackdropClick: true,
      keyboard: false,
    };
  }

  l(key: string, ...args: any[]): string {
    let localizedText = this.localization.localize(key, this.localizationSourceName);

    if (!localizedText) {
      localizedText = key;
    }

    if (!args || !args.length) {
      return localizedText;
    }

    args.unshift(localizedText);
    return abp.utils.formatString.apply(this, args);
  }

  isGranted(permissionName: string): boolean {
    return this.permission.isGranted(permissionName);
  }

  setUndefinedIfEmpty(val: any): any {
    if (val) {
      return val;
    }

    return undefined;
  }
}
