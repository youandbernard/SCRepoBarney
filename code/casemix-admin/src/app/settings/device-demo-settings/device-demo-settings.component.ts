import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
  selector: 'app-device-demo-settings',
  templateUrl: './device-demo-settings.component.html',
  styleUrls: ['./device-demo-settings.component.less'],
  animations: [appModuleAnimation()]
})
export class DeviceDemoSettingsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
