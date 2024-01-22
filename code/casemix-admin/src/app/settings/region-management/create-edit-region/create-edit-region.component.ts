import { Component, OnInit, Output, EventEmitter, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import {
  RegionDto,
  RegionsServiceProxy,
  CountriesServiceProxy,
  CountryDto,
  CreateRegionDto,
  UkRegionDto,
  UkRegionsServiceProxy,
  TrustsServiceProxy,
  TrustDto,
  IntegratedCareSystemDto,
  IntegratedCareSystemsServiceProxy,
  TrustsDto,
} from '@shared/service-proxies/service-proxies';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-create-edit-region',
  templateUrl: './create-edit-region.component.html',
  styleUrls: ['./create-edit-region.component.less'],
})
export class CreateEditRegionComponent extends AppComponentBase implements OnInit {
  @Output() modalSave = new EventEmitter<any>();
  countries: CountryDto[] = [];
  trusts: TrustsDto[] = [];
  ics: any;
  ukRegions: UkRegionDto[] = [];
  icsData: IntegratedCareSystemDto[] = [];
  isLoading = true;
  id: string;
  type: string;
  groupTrust: string;
  parentId: string;
  icsId: number;
  countryName: string;
  regionName: string;
  isEnabled: boolean;
  isSaving = false;
  name: string;
  selectedIcs: any[] = [];
  region: CreateRegionDto = new CreateRegionDto();
  groupTrustModel = '';
  rgName = '';
  ctryName = '';

  hideDeleteButton: boolean;

  selectedCountry = '';

  constructor(
    injector: Injector,
    private _modal: BsModalRef,
    private _regionsService: RegionsServiceProxy,
    private _countriesService: CountriesServiceProxy,
    private _trustService: TrustsServiceProxy,
    private _ukRegionsService: UkRegionsServiceProxy,
    private _icsService: IntegratedCareSystemsServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.id === null){
      this.hideDeleteButton = false;
    } else {
      if (this.id.trim() === ""){
        this.hideDeleteButton = false;
      }else 
        this.hideDeleteButton = true;
    }

    this.region.id = this.id;
    this.region.type = this.type;
    this.region.parentId = this.parentId;
    this.region.isEnabled = this.isEnabled;
    this.region.name = this.name;
    this.groupTrustModel = this.groupTrust;
    this.ctryName = this.countryName;
    this.rgName = this.name;

    if (this.type == 'Trust') {
      this.rgName = this.regionName;
    }else if (this.type == 'Region') {
      this.rgName = this.countryName;
    }
    
    if (this.type == 'Country') {
      this.selectedCountry = this.name;
    } else {
      this.selectedCountry = this.countryName;
    }

    if (this.countryName === 'United Kingdom') this.selectedIcs = this.ics;
    this.getCountries();
    this.getTrusts();
    this.getUKRegions();
    this.getIcs();
  }

  onFormSubmit(): void {
    this.isSaving = true;
    // console.log(this.selectedCountry);
    // console.log(this.countryName);
    // console.log(this.name);
    // console.log(this.rgName);
    // console.log(this.region);

    if (this.type === 'Country') {
        if (this.selectedCountry === null || this.selectedCountry === undefined) {
            this.notify.error('Country is required.');
            return;
        }
        else {
          if (this.selectedCountry.trim() === '') {
            this.notify.error('Country is required.');
            return;
          }            
        }
    }

    if (this.type === 'Region') {
        if (this.region.name.trim() === '') {
          this.notify.error('Region is required.');
          return;
        }
    }

    if (this.type === 'Trust') {
      if (this.region.name.trim() === '') {
        this.notify.error('Sub Region is required.');
        return;
      }
    }

    if (this.countryName === 'United Kingdom' && (this.type === 'Trust')) {
        if (!this.selectedIcs) {
          this.notify.error('ICS is required for UK country.');

            return;
        } else {
          if (this.selectedIcs.length <= 0){
            this.notify.error('ICS is required for UK country.');

            return;
          }     
        }
    }

    if (this.region.type == 'Trust')
      this.region.icsIds = this.selectedIcs.map((e) => {
        return e.id;
      });

    const regionsSubscription = this.id ? this._regionsService.update(this.region) : this._regionsService.create(this.region);
    regionsSubscription.subscribe(
      (result) => {
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

  onDeleteClick(): void {
    this.isSaving = true;
    if (this.region.type=='Trust') {
      abp.message.confirm('This will remove Sub-region and its hopsital/s. Would you like to continue?', 'Remove Sub-Region', (result: boolean) => {
        if (result) {
          this.removeFromTree();
        }
      });    
    }

    if (this.region.type=='Region') {
      abp.message.confirm('Would you like to continue to remove Region?', 'Remove Region', (result: boolean) => {
        if (result) {
          this.removeFromTree();
        }
      });    
    }

    if (this.region.type=='Country') {
      abp.message.confirm('Would you like to continue to remove Country?', 'Remove Country', (result: boolean) => {
        if (result) {
          this.removeFromTree();
        }
      });    
    }   
  }

  removeFromTree(): void {
    this._regionsService.delete(this.region.id, this.region.type).subscribe(
      (result) => {
        if (result.deleted) {
          this.notify.success(this.l(result.successMessage));
          this.isSaving = false;
          this.modalSave.emit();
          this.close();
        }else {
          this.notify.error(this.l(result.errorMessage));
          this.isSaving = false;
        }        
      },
      (error) => {
        if (this.region.type=='Trust') {
          this.notify.error("Unable to remove sub region. Please remove first the hospitals.");
        }
        
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
    this.region.name = e.item.name;
    this.selectedCountry = e.item.name;
  }

  onChangeIcs(e: any): void {
    this.selectedIcs = e.value;
  }

  onTrustChange(e: TypeaheadMatch): void {
    this.region.name = e.item.groupTrust;
  }

  getCountries(): void {
    this._countriesService.getAll(true).subscribe((result) => {
      this.countries = result;
    });
  }

  getTrusts(): void {
    // console.log(this.selectedCountry)
    this._trustService.getAll(null, this.rgName, true, this.selectedCountry).subscribe((result) => {
      this.trusts = result;
    });
  }

  getIcs(): void {
    this._icsService.getAll().subscribe((result) => {
      this.icsData = result;
    });
  }

  getUKRegions(): void {
    this._ukRegionsService.getAll().subscribe((result) => {
      this.ukRegions = result;
    });
  }
}
