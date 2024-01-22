import { Component, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
  selector: 'app-survey-settings',
  templateUrl: './survey-settings.component.html',
  styleUrls: ['./survey-settings.component.less'],
  animations: [appModuleAnimation()]
})
export class SurveySettingsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
