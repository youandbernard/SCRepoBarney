import { Component, OnInit, Input, Injector, Output } from '@angular/core';
import {
  BodyStructureGroupsServiceProxy,
  BodyStructureGroupDto,
  SurgeonSpecialtyDto,
  SurgeonSpecialtiesServiceProxy,
} from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-assign-specialties',
  templateUrl: './assign-specialties.component.html',
  styleUrls: ['./assign-specialties.component.less'],
})
export class AssignSpecialtiesComponent extends AppComponentBase implements OnInit {
  @Input() userId: number;
  @Output() surgeonSpecialties: SurgeonSpecialtyDto[] = [];
  
  userSpecialties: SurgeonSpecialtyDto[] = [];
  bodyStructureGroups: BodyStructureGroupDto[];
  checkedSpecialtiesMap: { [key: string]: boolean } = {};
  defaultSpecialtyCheckedStatus = false;

  constructor(
    injector: Injector,
    private _bodyStructureGroupsService: BodyStructureGroupsServiceProxy,
    private _surgeonSpecialtyService: SurgeonSpecialtiesServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getBodyStructureGroups();
  }

  public specialtyRadioOnClick(selectedSpecialty: SurgeonSpecialtyDto): void {
    _.forEach(this.surgeonSpecialties, (specialty) => {
      if (specialty.bodyStructureGroupId === selectedSpecialty.bodyStructureGroupId) {
        specialty.isSelected = true;
      } else {
        specialty.isSelected = false;
      }
    });
  }

  public saveSurgeonSpecialties(id?: number): Observable<void> {
    _.forEach(this.surgeonSpecialties, (specialty) => {
        specialty.surgeonId = id;
        specialty.isSelected = this.checkedSpecialtiesMap[specialty.bodyStructureName];
    });

    return this._surgeonSpecialtyService.saveAll(this.surgeonSpecialties);
  }

  private getBodyStructureGroups(): void {
    this._bodyStructureGroupsService.getAll().subscribe((bodyStructureGroups) => {
      this.bodyStructureGroups = bodyStructureGroups;
      this.getUserSpecialties();
    });
  }

  private getUserSpecialties(): void {
    this._surgeonSpecialtyService.getAll(this.userId).subscribe((userSpecialties) => {
      this.userSpecialties = userSpecialties;
      this.prepareUserSpecialties(userSpecialties);
      this.setInitialSpecialtiesStatus();
    });
  }

  private prepareUserSpecialties(userSpecialties: SurgeonSpecialtyDto[]): void {  

    _.forEach(this.bodyStructureGroups, (bodyStructure) => {
      let userSpecialty = _.find(userSpecialties, (e) => e.bodyStructureGroupId === bodyStructure.id && e.surgeonId === this.userId);
      if (userSpecialty) {
        userSpecialty.isSelected = true;
        userSpecialty.bodyStructureName = bodyStructure.name;
        this.surgeonSpecialties.push(userSpecialty);
      } else {
        userSpecialty = new SurgeonSpecialtyDto();
        userSpecialty.surgeonId = this.userId;
        userSpecialty.bodyStructureGroupId = bodyStructure.id;
        userSpecialty.bodyStructureName = bodyStructure.name;
        userSpecialty.isSelected = false;
        this.surgeonSpecialties.push(userSpecialty);
      }
    });  
  }

  isspecialtyChecked(bodyStructureName: string): boolean {
    // 
    if (this.userSpecialties.length > 0){
      var exists = this.userSpecialties.find((f) => f.bodyStructureName ==bodyStructureName);
      if (exists){
        return exists.isSelected;
      }else {
        return false;
      }      
    } else {
      return this.defaultSpecialtyCheckedStatus;
    }
   
  }

  onspecialtyChange(specialty: SurgeonSpecialtyDto, $event) {
    this.checkedSpecialtiesMap[specialty.bodyStructureName] = $event.target.checked;
  }

  setInitialSpecialtiesStatus() {
    _.map(this.surgeonSpecialties, (item) => {
      this.checkedSpecialtiesMap[item.bodyStructureName] = this.isspecialtyChecked(item.bodyStructureName);
    });
  }
}
