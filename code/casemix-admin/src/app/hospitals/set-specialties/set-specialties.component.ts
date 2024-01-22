import { Component, OnInit, Input, Output } from '@angular/core';
import { BodyStructureGroupDto, BodyStructureGroupsServiceProxy, SpecialtyInfoDto } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-set-specialties',
  templateUrl: './set-specialties.component.html',
  styleUrls: ['./set-specialties.component.less']
})
export class SetSpecialtiesComponent implements OnInit {
  @Input() hospitalId: string;
  @Output() hospitalSpecialties: SpecialtyInfoDto[] = [];

  existingSpecialties:  SpecialtyInfoDto[] = [];
  bodyStructureGroups: BodyStructureGroupDto[];
  checkedSpecialtiesMap: { [key: string]: boolean } = {};
  defaultSpecialtyCheckedStatus = false;

  constructor(
    private _bodyStructureGroupsService: BodyStructureGroupsServiceProxy,
  ) { }

  ngOnInit(): void {
    this.getBodyStructureGroups();
  }

  public saveHospitalSpecialties(id: string): Observable<void> {
    _.forEach(this.hospitalSpecialties, (specialty) => {
        specialty.hospitalId = id;
        specialty.isSelected = this.checkedSpecialtiesMap[specialty.specialtyName];
    });

    return this._bodyStructureGroupsService.saveHospitalSpecialties(this.hospitalSpecialties);
  }

  private getBodyStructureGroups(): void {
    this._bodyStructureGroupsService.getAll().subscribe((bodyStructureGroups) => {
      this.bodyStructureGroups = bodyStructureGroups;
      this.getHospitalSpecialties();
    });
  }

  private getHospitalSpecialties(): void {
    this._bodyStructureGroupsService.getHospitalSpecialty(this.hospitalId)
    .subscribe((specialties) => {
      this.existingSpecialties = specialties;
      this.prepareHospitalSpecialties(this.existingSpecialties);
      this.setInitialSpecialtiesStatus();
    });
  }

  private prepareHospitalSpecialties(existingSpecialties: SpecialtyInfoDto[]): void {  

    _.forEach(this.bodyStructureGroups, (bodyStructure) => {
      let hospSpecialty = _.find(existingSpecialties, (e) => e.specialtyId === bodyStructure.id);
      if (hospSpecialty) {
        hospSpecialty.isSelected = true;
        hospSpecialty.specialtyName = bodyStructure.name;
        this.hospitalSpecialties.push(hospSpecialty);
      } else {
        hospSpecialty = new SpecialtyInfoDto();
        hospSpecialty.hospitalId = this.hospitalId;
        hospSpecialty.specialtyId = bodyStructure.id;
        hospSpecialty.specialtyName = bodyStructure.name;
        hospSpecialty.isSelected = false;
        this.hospitalSpecialties.push(hospSpecialty);
      }
    });  
  }

  public isSpecialtyChecked(bodyStructureName: string): boolean {
    if (this.existingSpecialties.length > 0) {
      var exists = this.existingSpecialties.find((f) => f.specialtyName == bodyStructureName);
      if (exists){
        return exists.isSelected;
      }else {
        return false;
      }      
    } else {
      return this.defaultSpecialtyCheckedStatus;
    }
  }

  onSpecialtyChange(specialty: SpecialtyInfoDto, $event) {
    this.checkedSpecialtiesMap[specialty.specialtyName] = $event.target.checked;
  }

  setInitialSpecialtiesStatus() {
    _.map(this.hospitalSpecialties, (item) => {
      this.checkedSpecialtiesMap[item.specialtyName] = this.isSpecialtyChecked(item.specialtyName);
    });
  }

}
