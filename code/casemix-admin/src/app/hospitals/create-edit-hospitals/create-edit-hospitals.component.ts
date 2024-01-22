import { ChangeDetectorRef, Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CountriesServiceProxy,
  CountryDto,
  HospitalDto,
  HospitalsServiceProxy,
  RegionDto,
  RegionsServiceProxy,
  TrustDto,
  TrustsDto,
  TrustsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { DataUpdateService } from '@shared/services/data-update.service';
import { LocalStorageService } from '@shared/services/local-storage.service';
import * as _ from 'lodash';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { SetSpecialtiesComponent } from '../set-specialties/set-specialties.component';

@Component({
  selector: 'app-create-edit-hospitals',
  templateUrl: './create-edit-hospitals.component.html',
  styleUrls: ['./create-edit-hospitals.component.less'],
})
export class CreateEditHospitalsComponent extends AppComponentBase implements OnInit {
  @Output() modalSave = new EventEmitter<any>();

  id: string;
  isSaving = false;
  hospital: HospitalDto = new HospitalDto();
  isLoading = false;
  countries: CountryDto[] = [];
  regions: RegionDto[] = [];
  trusts: TrustsDto[] = [];

  selectedCountry = '';

  isNew = false;
  
  @ViewChild('setSpecialtiesComponent')
  private setSpecialtiesComponent: SetSpecialtiesComponent;

  constructor(
    injector: Injector,
    private _modal: BsModalRef,
    private _countriesService: CountriesServiceProxy,
    private _regionsService: RegionsServiceProxy,
    private _trustsService: TrustsServiceProxy,
    private _hospitalsService: HospitalsServiceProxy,
    private _dataUpdateService: DataUpdateService<HospitalDto>,
    private _localStorageService: LocalStorageService,
    private _cdf: ChangeDetectorRef,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getCountries();
    if (!this.id) {
      this.hospital.id = this.id;
    } else {
      this.getHospital();
    }

    if (this.id === '' || this.id === undefined) {
      this.isNew = true;
    }
  }

  onFormSubmit(): void {
    var errors: string[] = [];

    if (this.hospital.countryName === null || this.hospital.countryName === undefined)
      errors.push('Country is required.');
    else {
        if (this.hospital.countryName.trim() === '')
            errors.push('Country is required.');
    }

    if (this.hospital.regionName === null || this.hospital.regionName === undefined )    
      errors.push('Region is required.');
    else {
       if (this.hospital.regionName.trim() === '')
           errors.push('Region is required.');
    }

    if (this.hospital.trustName === null || this.hospital.trustName === undefined)
      errors.push('Trust is required.');
    else {
        if (this.hospital.trustName.trim() === '')
          errors.push('Trust is required.');
    }

    if (this.hospital.postcode === null || this.hospital.postcode.trim() === '') {
      errors.push('Please enter a postcode / zip code.');
    }

    if (this.hospital.name === null || this.hospital.name.trim() === '') {
      errors.push('Please enter hospital name.');
    }

    if (this.hospital.id === null || this.hospital.id.trim() === '') {
      errors.push('Please enter a hospital id.');
    }

    if (this.hospital.id.length > 20) {
      errors.push('Hospital ID exceeds the maximum length of 20 characters.');
    }
    
    if (errors) {
      if (errors.length > 0) {
        _.forEach(errors, (e) => {
            this.notify.error(this.l(e));
        });

        return;
      }
    }

    this.isSaving = true;

    const hospitalSubscription = this.id ? this._hospitalsService.update(this.hospital) : this._hospitalsService.create(this.hospital);
    hospitalSubscription.subscribe(
      (result) => {
        
        if (this.hospital.activeDevMgt) {
          if (this.setSpecialtiesComponent !== undefined) {
            this.setSpecialtiesComponent.saveHospitalSpecialties(this.hospital.id).subscribe(() => {});
          }
        }        

        this.CheckforExistingHospital(this.hospital);

        this.notify.success(this.l('SavedSuccessfully'));
        this.isSaving = false;
        this.modalSave.emit();
        this.close();
      },
      (error) => {
        this.isSaving = false;
      }
    );
  }

  onCloseClick(): void {
    this.close();
  }

  private close(): void {
    this._modal.hide();
  }

  private getHospital(): void {
    this._hospitalsService.getByHospitalId(this.id).subscribe((hospital) => {
      this.hospital = hospital;
    });
  }

  onCountryChange(e: TypeaheadMatch): void {
    this.getRegions(e.item.name);
    this.hospital.countryName = e.item.name;
    this.selectedCountry = e.item.name;
  }

  onRegionChange(e: TypeaheadMatch): void {
    this.getTrusts(e.item.id, e.item.name);
    this.hospital.regionName = e.item.name;
  }

  onTrustChange(e: TypeaheadMatch): void {
    this.hospital.trustName = e.item.groupTrust;
  }

  getCountries(): void {
    this._countriesService.getAll(false).subscribe((result) => {
      this.countries = result;
    });
  }

  getRegions(countryName: any): void {
    this._regionsService.getAll(countryName).subscribe((result) => {
      this.regions = result;
    });
  }

  getTrusts(regionId: any, regionName: any): void {
    this._trustsService.getAll(regionId, regionName, false, this.selectedCountry).subscribe((result) => {
      this.trusts = result;
    });
  }

  private CheckforExistingHospital(h: HospitalDto) {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);    
    if (localHospital) {
      if (localHospital.id === h.id) {
        localHospital.activeDevMgt = h.activeDevMgt;
        localHospital.name = h.name;
        localHospital.postcode = h.postcode;
        localHospital.showButtonDevProc = h.showButtonDevProc;
        this.setHospitalData(localHospital);
        this._cdf.detectChanges();
      }
    }
  }

  private setHospitalData(hospital: HospitalDto): void {
    console.log('setHospitalData');
    this._localStorageService.setObjectItem<HospitalDto>(this.localStorageKey.hospital, hospital);
    this._dataUpdateService.setData(hospital);
  }
}
