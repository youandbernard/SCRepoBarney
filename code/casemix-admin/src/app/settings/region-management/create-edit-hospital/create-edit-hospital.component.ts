import { Component, OnInit, Output, EventEmitter, Injector, ChangeDetectorRef, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CountriesServiceProxy,
  CountryDto,
  HospitalDto,
  HospitalsServiceProxy,
  RegionDto,
  RegionsServiceProxy,
  TrustDto,
  TrustsServiceProxy,
  IntegratedCareSystemDto,
  IntegratedCareSystemsServiceProxy,
  TrustsDto,
} from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import * as _ from 'lodash';
import { DataUpdateService } from '@shared/services/data-update.service';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { SetSpecialtiesComponent } from '@app/hospitals/set-specialties/set-specialties.component';

@Component({
  selector: 'app-create-edit-hospital',
  templateUrl: './create-edit-hospital.component.html',
  styleUrls: ['./create-edit-hospital.component.less'],
})
export class CreateEditHospitalComponent extends AppComponentBase implements OnInit {
  @Output() modalSave = new EventEmitter<any>();
  id: string;
  trustId: string;
  trustName: string;
  icsId: number;
  ics: any;
  regionId: string;
  icsData: IntegratedCareSystemDto[] = [];
  countryId: string;
  isEnabled: boolean;
  isSaving: boolean;
  name: string;
  countryName: string;
  regionName: string;
  countries: CountryDto[] = [];
  regions: RegionDto[] = [];
  trusts: TrustsDto[] = [];
  selectedIcs: any;
  postcode: string;
  hospital: HospitalDto = new HospitalDto();
  activeDevMgt: boolean;
  ctyModelName = '';
  rgModelName = '';
  trustModelName = '';

  selectedCountry = '';

  isNew = false;

  @ViewChild('setSpecialtiesComponent')
  private setSpecialtiesComponent: SetSpecialtiesComponent;

  constructor(
    injector: Injector,
    private _modal: BsModalRef,
    private _regionsService: RegionsServiceProxy,
    private _countriesService: CountriesServiceProxy,
    private _hospitalsService: HospitalsServiceProxy,
    private _trustsService: TrustsServiceProxy,
    private _icsService: IntegratedCareSystemsServiceProxy,
    private _dataUpdateService: DataUpdateService<HospitalDto>,
    private _localStorageService: LocalStorageService,
    private _cdf: ChangeDetectorRef,
  ) {
    super(injector);
  }

  ngOnInit(): void {   
    this.hospital.id = this.id;
    this.hospital.regionId = this.regionId;
    this.hospital.trustId = this.trustId;
    this.hospital.icsId = this.icsId;
    this.hospital.countryId = this.countryId;
    this.hospital.name = this.name;
    this.hospital.activeDevMgt = this.activeDevMgt;
    this.ctyModelName = this.countryName;
    this.rgModelName = this.regionName;
    this.hospital.regionName = this.rgModelName;
    this.trustModelName = this.trustName;
    this.hospital.postcode = this.postcode;
    this.selectedCountry = this.countryName;
    if (this.countryName === 'United Kingdom' && this.ics)
      this.selectedIcs = _.map(this.ics, (item) => {
        return item.name;
      }).join(',');
    this.getCountries();
    this.getRegions(this.ctyModelName);
    this.getTrusts();
    this.getIcs();      

    if (this.id === '' || this.id === undefined) {
      this.isNew = true;
    }

  }

  onFormSubmit(): void {
    this.isSaving = true;
    
    var errors: string[] = [];

    if (this.countryName === null || this.countryName.trim() === '')
    {
      errors.push('Country is required.');
    }

    if (this.hospital.regionName === null || this.hospital.regionName.trim() === '')
    {
      errors.push('Region is required.');
    }

    if (this.trustName === null || this.trustName.trim() === '')
    {
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

    this.hospital.countryName = this.countryName;
    this.hospital.trustName = this.trustName;

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

  onCountryChange(e: TypeaheadMatch): void {
    this.getRegions(e.item.id);
    this.selectedCountry = e.item.name;
  }

  onRegionChange(e: TypeaheadMatch): void {
    this.hospital.regionId = e.item.id;
    this.hospital.regionName = this.rgModelName;
  }

  onTrustChange(e: TypeaheadMatch): void {
    this.trustModelName = e.item.groupTrust;
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

  getTrusts(): void {
    this._trustsService.getAll(null, this.regionName, true, this.selectedCountry).subscribe((result) => {
      this.trusts = result;
    });
  }

  getIcs(): void {
    this._icsService.getAll().subscribe((result) => {
      this.icsData = result;
    });
  }

  private CheckforExistingHospital(h: HospitalDto) {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (localHospital) {
      if (localHospital.id === h.id) {
        localHospital.activeDevMgt = h.activeDevMgt;
        localHospital.name = h.name;
        localHospital.postcode = h.postcode;
        this.setHospitalData(localHospital);
        this._cdf.detectChanges();
      }
    }
  }

  private setHospitalData(hospital: HospitalDto): void {
    this._localStorageService.setObjectItem<HospitalDto>(this.localStorageKey.hospital, hospital);
    this._dataUpdateService.setData(hospital);
  }
}
