import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { Observable, Observer, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import {
  PatientsServiceProxy,
  PatientDto,
  HospitalDto,
  UserServiceProxy,
  UserDto,
  PreOperativeAssessmentsServiceProxy,
  PreOperativeAssessmentDto,
  BodyStructuresServiceProxy,
  MenuItemOutputDto,
  PoapProcedureDto,
  PoapRiskDto,
  CoMorbiditiesServiceProxy,
  CoMorbidityGroupDto,
  ProcedureMethodDto,
  BodyStructureGroupDto,
  BodyStructureGroupsServiceProxy,
  EthnicityDto,
  EthnicitiesServiceProxy,
  TheatersServiceProxy,
  SearchTheaterDto,
  TheaterDto,
  SurgeonSpecialtyDto,
  ProcedureMethodTypeDto,
  RiskMappingSettingDto,
  RiskMappingServiceProxy,
  HospitalsServiceProxy,
  DiagnosticReportDto,
  DiagnosticReportServiceProxy,
  PoapRiskFactorDto,
  DiagnosticRiskFactorsMappingDto,
  InstrumentPackDto,
  PoapInstrumentPacksServiceServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/ngx-bootstrap-typeahead';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { LocalStorageService } from '@shared/services/local-storage.service';
import { DateTimePickerModalComponent } from '@shared/components/modal/date-time-picker-modal/date-time-picker-modal.component';
import { SurgeonExperienceType } from '@shared/enums/surgeon-experience-type';
import { GenderType } from '@shared/enums/gender-type.enum';
import * as moment from 'moment';
import { Router, ActivatedRoute } from '@angular/router';
import { ProcedureSearchComponent } from './procedure-search/procedure-search.component';
import * as _ from 'lodash';
import { TreeNode } from 'primeng/api';
import { ShowDiagnosticReportDetailsComponent } from '../show-diagnostic-report-details/show-diagnostic-report-details.component';
import { ShowDemographicDetailsComponent } from '../show-demographic-details/show-demographic-details.component';
import { ShowPatientDetailsComponent } from '../show-patient-details/show-patient-details.component';
import { ProcedureSelectDeviceComponent } from './procedure-select-device/procedure-select-device.component';
import { InstrumentSelectComponent } from './instrument-select/instrument-select.component';

enum PoapRiskType {
  Select,
  Label,
}
@Component({
  selector: 'app-create-edit-poap',
  templateUrl: './create-edit-poap.component.html',
  styleUrls: ['./create-edit-poap.component.less'],
  animations: [appModuleAnimation()],
})
export class CreateEditPoapComponent extends AppComponentBase implements OnInit {
  id: string;
  hospital: HospitalDto;
  preOperativeAssessment: PreOperativeAssessmentDto;
  patientsDataSource: Observable<PatientDto[]>;
  surgeonsDataSource: Observable<UserDto[]>;
  anaesthetistsDataSource: Observable<UserDto[]>;
  theatersDataSource: Observable<SearchTheaterDto[]>;
  specialties: BodyStructureGroupDto[] = [];
  theathers: TheaterDto[];
  diagnosticReports: DiagnosticReportDto[] = [];
  bodyStructureGroups: BodyStructureGroupDto[];
  ethnicities: EthnicityDto[];
  procedureMethods: ProcedureMethodDto[];
  surgeonExperienceType = SurgeonExperienceType;
  poapRisk1Type = PoapRiskType;
  genderType = GenderType;
  clonedProcedures: { [s: string]: PoapProcedureDto } = {};
  coMorbidityNodes: TreeNode[] = [];
  selectedCoMorbidityNodes: TreeNode[] = [];
  isLoading = false;
  anaesthestists: UserDto[];
  selected: string;
  patients: PatientDto[];
  surgeons: UserDto[] = [];
  anaethetists: UserDto[];
  theaters: SearchTheaterDto[];
  patientPreparationExtras: PoapRiskDto[] = [];
  risksOneExtras: PoapRiskDto[] = [];
  isCheckedSelectAll = false;
  patientPreparations: PoapRiskDto[] = [];
  specialtyDisabled = false;
  procedureMethodTypes: ProcedureMethodTypeDto[] = [];
  instrumentPacks: InstrumentPackDto[] = [];
  savedProcedures: PoapProcedureDto[] = [];
  selectedProcedureMethodTypes: ProcedureMethodTypeDto[] = [];
  selectedinstrumentPacks: InstrumentPackDto[] = [];
  procedureMethodTypeName = '';
  instrumentPacksTraysName = '';
  surgeonHasChanged = false;
  riskCompletion: AwaitingRiskCompletionEnum;
  riskMappingSetting: boolean;
  @ViewChild('wizardTabs', { static: true }) wizardTabs: TabsetComponent;
  saveDiagnosticClicked = false;
  isTabRestrictionEnforced = true;
  selectedDiagnostic;
  confirmPatientIsClicked = false;
  saveDemographicIsClicked = false;
  saveDiagnosticReportIsClicked = false;
  savedPoapRiskFactors: DiagnosticRiskFactorsMappingDto[] = [];
  diagnosticReportIdPreviousValue;
  oldValue: string;
  selectedEthnicity: string;
  
  role: string;
  isSurgeon: boolean = false;
  isAdmin: boolean = false;
  isFilterLicensedStatus: boolean = false;
  
  selectedSmoker: string;

  constructor(
    injector: Injector,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _modalService: BsModalService,
    private _localStorageService: LocalStorageService,
    private _patientsService: PatientsServiceProxy,
    private _usersService: UserServiceProxy,
    private _preOperativeAssessmentsService: PreOperativeAssessmentsServiceProxy,
    private _bodyStructuresService: BodyStructuresServiceProxy,
    private _coMorbiditiesService: CoMorbiditiesServiceProxy,
    private _bodyStructureGroupsService: BodyStructureGroupsServiceProxy,
    private _ethnicitiesService: EthnicitiesServiceProxy,
    private _theatersService: TheatersServiceProxy,
    private _riskMappingService: RiskMappingServiceProxy,
    private _hospitalService: HospitalsServiceProxy,
    private _diagnosticReportService: DiagnosticReportServiceProxy,
    private _instrumentPackService: PoapInstrumentPacksServiceServiceProxy
  ) {
    super(injector);
    this.preOperativeAssessment = new PreOperativeAssessmentDto();
    this.preOperativeAssessment.procedures = [];
    this.preOperativeAssessment.risks = [];
  }

  ngOnInit(): void {
    this.preOperativeAssessment.timezone = moment.tz.guess();
    this.isLoading = true;
    this.hospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    this.preOperativeAssessment.hospitalId = this.hospital.id;
    this._activatedRoute.paramMap.subscribe((paramMap) => {
      this.id = paramMap.get('id');
      this.getRiskmappingSetting();
      if (this.id) {
        this.getPoap();
        this.isTabRestrictionEnforced = false;
      } else {
        this.getCoMorbidities();
        this.getAllPatients();
      }
      this.getProcedureMethods();
      this.getEthnicities();
      this.getTheatersByHospital();
      this.getAllAnaesthetists();
      this.getAllTheaters();
      this.getSpecialties();
      this.getUserDetails();
      this.getAllProcedureMethodTypes();
      this.getAllInstrumentPacks();
    });
  }

  formatDateTime(datetime: moment.Moment): string {
    if (datetime) {
      return datetime.format('DD/MM/YY hh:mm A');
    }

    return '';
  }

  handleDiagnosticReportChange(event): void {
    if (this.selectedCoMorbidityNodes.length > 0 && !this.saveDiagnosticClicked) {
      abp.message.confirm('There are selected risk factors', 'Unsaved changes', (result: boolean) => {
        if (result) {
          _.forEach(this.selectedCoMorbidityNodes, (comorbidity) => {
            if (comorbidity.parent) comorbidity.parent.partialSelected = false;
            else comorbidity.partialSelected = false;
          });
          this.selectedCoMorbidityNodes = [];
        }
      });
    } else if (this.saveDiagnosticClicked) {
      var existingDiagnostics = this.preOperativeAssessment.risks.filter((e) => e.diagnosticId == this.selectedDiagnostic);

      _.forEach(this.selectedCoMorbidityNodes, (comorbidity) => {
        if (comorbidity.parent) comorbidity.parent.partialSelected = false;
        else comorbidity.partialSelected = false;
      });
      this.selectedCoMorbidityNodes = [];
      if (existingDiagnostics.length > 0) {
        this.buildExistingDiagnosticsTreeNodes(existingDiagnostics);
      }
    }
    this.diagnosticReportIdPreviousValue = this.selectedDiagnostic;
  }

  buildExistingDiagnosticsTreeNodes(existingDiagnostics: PoapRiskDto[]): void {
    this._coMorbiditiesService.getAll().subscribe((results) => {
      _.forEach(results, (data) => {
        let hasSelectedChild = false;
        if (data.comorbidities.length > 0) {
          _.forEach(data.comorbidities, (comorbidity) => {
            var existing = existingDiagnostics.filter((e) => e.key == comorbidity.description);
            if (existing.length == 1) {
              const itemNode: TreeNode = {
                key: comorbidity.snomedId,
                label: comorbidity.description,
                data: {
                  isGroup: false,
                },
              };
              this.selectedCoMorbidityNodes.push(itemNode);
              hasSelectedChild = true;
            }
          });

          if (hasSelectedChild) {
            const groupNode: TreeNode = {
              key: data.description,
              label: data.description,
              data: {
                isGroup: true,
                partialSelected: false,
              },
              partialSelected: true,
            };
            this.selectedCoMorbidityNodes.push(groupNode);
          }
        }
      });
    });
  }

  clearSelection(): void {
    this.showSelectedComorbidities();
  }
  clearDiagnosticSelection(): void {
    this.selectedDiagnostic = null;
    var existingDiagnostics = this.preOperativeAssessment.risks.filter((e) => !e.diagnosticId);

    _.forEach(this.selectedCoMorbidityNodes, (comorbidity) => {
      if (comorbidity.parent) comorbidity.parent.partialSelected = false;
      else comorbidity.partialSelected = false;
    });
    this.selectedCoMorbidityNodes = [];
    if (existingDiagnostics.length > 0) {
      this.buildExistingDiagnosticsTreeNodes(existingDiagnostics);
    }
  }

  showDateTimePickerModal(datetime: moment.Moment, callback: (result: moment.Moment) => void): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      datetime: datetime,
    };
    const modalRef = this._modalService.show(DateTimePickerModalComponent, modalSettings);
    const modal: DateTimePickerModalComponent = modalRef.content;
    modal.modalSave.subscribe((result: moment.Moment) => {
      if (result) {
        callback(result);
      }
    });
  }

  disabledSaveButton(): boolean {
    const noMeanTimeProcedures = this.preOperativeAssessment.procedures.find((procedure) => procedure.meanTime === 0 
    && (procedure.isRisk === false || procedure.isRisk === undefined));
    
    

    return !noMeanTimeProcedures ? false : true;
  }

  setSelectedItem($event) {
    this.selectedProcedureMethodTypes.push($event.item);
    this.procedureMethodTypeName = '';
    const index = this.procedureMethodTypes.findIndex((e) => e.id == $event.item.id);
    if (index >= 0) {
      this.procedureMethodTypes.splice(index, 1);
    }
  }

  onRemoveClick(selected: ProcedureMethodTypeDto): void {
    const index = this.selectedProcedureMethodTypes.findIndex((e) => e.id === selected.id);
    if (index >= 0) {
      this.selectedProcedureMethodTypes.splice(index, 1);
      this.procedureMethodTypes.push(selected);
    }
  }

  setSelectedPackItem($event) {
    console.log($event.item);
    console.log(this.selectedinstrumentPacks);

    this.selectedinstrumentPacks.push($event.item);
    this.instrumentPacksTraysName = '';
    const index = this.instrumentPacks.findIndex((e) => e.id == $event.item.id);
    if (index >= 0) {
      this.instrumentPacks.splice(index, 1);
    }
  }

  onRemovePackClick(selected: InstrumentPackDto): void {
    const index = this.selectedinstrumentPacks.findIndex((e) => e.id === selected.id);
    if (index >= 0) {
      this.selectedinstrumentPacks.splice(index, 1);
      this.instrumentPacks.push(selected);
    }
  }

  setTabState(tabIndex: number, isEnabled: boolean) {
    this.wizardTabs.tabs[tabIndex].disabled = !isEnabled;
  }

  getMeanTimeSum(): number {
    return parseFloat(_.sumBy(this.preOperativeAssessment.procedures, 'meanTime').toFixed(2));
  }

  getStandardDeviationSum(): number {
    return parseFloat(_.sumBy(this.preOperativeAssessment.procedures, 'standardDeviation').toFixed(2));
  }

  onPatientSelectedRiskMapping(e): void {
    this.preOperativeAssessment.patientId = e.id;
    this.preOperativeAssessment.dateOfBirthYear = e.dobYear;
    this.preOperativeAssessment.gender = e.gender;
    this.preOperativeAssessment.ethnicityId =
      e.ethnicCategory != null ? parseInt(e.ethnicCategory) : this.preOperativeAssessment.ethnicityId;
    this.getDiagnosticReports();
  }

  onPatientSelected(e: TypeaheadMatch): void {
    this.preOperativeAssessment.patientId = e.item.id;
    this.preOperativeAssessment.dateOfBirthYear = e.item.dobYear;
    this.preOperativeAssessment.gender = e.item.gender;
    this.preOperativeAssessment.ethnicityId =
      e.item.ethnicCategory != null ? parseInt(e.item.ethnicCategory) : this.preOperativeAssessment.ethnicityId;
  }

  onSurgeonSelected(e: TypeaheadMatch): void {
    if (this.preOperativeAssessment.surgeonId !== e.item.id) this.surgeonHasChanged = true;

    this.preOperativeAssessment.surgeonId = e.item.id;
    this.preOperativeAssessment.surgeonExperience = e.item.experience;
  }

  onTheaterSelected(e: TypeaheadMatch): void {
    this.preOperativeAssessment.theaterId = e.item.id;
    this.preOperativeAssessment['theaterName'] = e.item.description;
  }

  onSurgeonSelect($event): void {
    _.forEach(this.specialties, (specialty) => {
      if ($event.target.value === specialty.id) {
        this.surgeons = specialty.surgeonSpecialties.map((e) => e.user);
      }
    });
  }

  onAnesthetistSelected(e: TypeaheadMatch): void {
    this.preOperativeAssessment.anesthetistName = e.item.fullName;
  }

  onChangeAssessmentDateClick(): void {
    this.showDateTimePickerModal(this.preOperativeAssessment.assessmentDate, (result: moment.Moment) => {
      this.preOperativeAssessment.assessmentDate = result;
      
      if (this.preOperativeAssessment.surgeryDate !== null && this.preOperativeAssessment.surgeryDate !== undefined) {
        var assessDate = moment(this.preOperativeAssessment.assessmentDate);
        var surgeDate = moment(this.preOperativeAssessment.surgeryDate);
  
        if (assessDate > surgeDate){
          abp.notify.info('Assessment date must less or equal to surgery date.', 'Warning');
  
          this.preOperativeAssessment.assessmentDate = null;
        }
      }

    });
  }

  onChangeSurgeryDateClick(): void {
    if (this.preOperativeAssessment.assessmentDate !== null && this.preOperativeAssessment.assessmentDate !== undefined) {
      this.showDateTimePickerModal(this.preOperativeAssessment.surgeryDate, (result: moment.Moment) => {
        this.preOperativeAssessment.surgeryDate = result;
        
        var assessDate = moment(this.preOperativeAssessment.assessmentDate);
        var surgeDate = moment(this.preOperativeAssessment.surgeryDate);

        if (assessDate > surgeDate){
          abp.notify.info('Assessment date must less or equal to surgery date.', 'Warning');

          this.preOperativeAssessment.surgeryDate = null;
        }

      });
    }else {
      abp.notify.info('Please define first an assessment date.', 'Warning');
    }    
  }

  getTotalPredictedTime(): number {
    return _.sumBy(this.patientPreparations, 'meanTime');
  }

  onTabChangeClick(tabIndex: number, fromTabIndex: number): void {
    // debugger;
    if (fromTabIndex === 0) {
      if (!this.preOperativeAssessment.id && !this.confirmPatientIsClicked && this.riskMappingSetting) {
        abp.notify.error('Please click confirm button', 'Error');
        return;
      }
      // temporary validation move this using angular form
      //
      if (this.preOperativeAssessment.dateOfBirthYear > new Date().getFullYear()) {
        abp.notify.error('Invalid Date of birth year.', 'Error');
        return;
      }
      if (this.preOperativeAssessment.risks.length === 0) {
        const bloodPressureRisk = new PoapRiskDto();
        bloodPressureRisk.group = 'anesthesia';
        bloodPressureRisk.key = 'Blood Pressure';
        bloodPressureRisk.value = 'Elevated';
        bloodPressureRisk.meanTime = 0;
        bloodPressureRisk.standardDeviation = 0;
        this.preOperativeAssessment.risks.push(bloodPressureRisk);

        const bmiRisk = new PoapRiskDto();
        bmiRisk.group = 'surgery';
        bmiRisk.key = 'BMI';
        bmiRisk.value = '<18.5';
        bmiRisk.meanTime = 0;
        bmiRisk.standardDeviation = 0;
        this.preOperativeAssessment.risks.push(bmiRisk);

        const genderRisk = new PoapRiskDto();
        genderRisk.group = 'surgery';
        genderRisk.key = 'Gender';
        genderRisk.value = GenderType[this.preOperativeAssessment.gender];
        genderRisk.meanTime = 0;
        genderRisk.standardDeviation = 0;
        this.preOperativeAssessment.risks.push(genderRisk);

        const ageRisk = new PoapRiskDto();
        ageRisk.group = 'surgery';
        ageRisk.key = 'Age';
        ageRisk.value = `${new Date().getFullYear() - this.preOperativeAssessment.dateOfBirthYear}`;
        ageRisk.meanTime = 0;
        ageRisk.standardDeviation = 0;
        this.preOperativeAssessment.risks.push(ageRisk);

        const ethnicityRisk = new PoapRiskDto();
        ethnicityRisk.group = 'surgery';
        ethnicityRisk.key = 'Ethnicity';
        ethnicityRisk.value = _.find(this.ethnicities, (e) => e.id === this.preOperativeAssessment.ethnicityId).description;
        ethnicityRisk.meanTime = 0;
        ethnicityRisk.standardDeviation = 0;
        this.preOperativeAssessment.risks.push(ethnicityRisk);

        const smoker = new PoapRiskDto();
        smoker.group = 'surgery';
        smoker.key = 'Smoker';
        smoker.value = 'Smoker';
        smoker.meanTime = 0;
        smoker.standardDeviation = 0;
        this.preOperativeAssessment.risks.push(smoker);
        this.risksOneExtras.push(smoker);

        const whoCheckListRisk = new PoapRiskDto();
        whoCheckListRisk.key = 'WHO Surgical Safety Check List';
        whoCheckListRisk.meanTime = 5;
        whoCheckListRisk.standardDeviation = 0;
        whoCheckListRisk.snomedId = '450729003';
        this.patientPreparations.push(whoCheckListRisk);

        const patientPositioningRisk = new PoapRiskDto();
        patientPositioningRisk.key = 'Patient Positioning';
        patientPositioningRisk.meanTime = 5;
        patientPositioningRisk.standardDeviation = 0;
        patientPositioningRisk.snomedId = '229824005';
        this.patientPreparations.push(patientPositioningRisk);

        const draping = new PoapRiskDto();
        draping.key = 'Application of surgical drapes';
        draping.meanTime = 5;
        draping.standardDeviation = 0;
        draping.snomedId = '397819007';
        this.patientPreparations.push(draping);

        const scrub = new PoapRiskDto();
        scrub.key = 'Cleaning and sterilisation of skin';
        scrub.meanTime = 5;
        scrub.standardDeviation = 0;
        scrub.snomedId = '450832005';
        this.patientPreparations.push(scrub);

        const identification = new PoapRiskDto();
        identification.key = 'Marking skin site prior to procedure';
        identification.meanTime = 5;
        identification.standardDeviation = 0;
        identification.snomedId = '225135006';
        this.patientPreparations.push(identification);
      } else {
        this.preOperativeAssessment.risks[2].value = GenderType[this.preOperativeAssessment.gender];
        this.preOperativeAssessment.risks[3].value = `${new Date().getFullYear() - this.preOperativeAssessment.dateOfBirthYear}`;
        this.preOperativeAssessment.risks[4].value = _.find(
          this.ethnicities,
          (e) => e.id === this.preOperativeAssessment.ethnicityId
        ).description;
      }
      if (!this.preOperativeAssessment.id) this.getDiagnosticReports();
    } else if (fromTabIndex === 1) {
      const existingSurgeon = this.surgeons.find((r) => r.fullName === this.preOperativeAssessment.surgeonName);
      if (!existingSurgeon) {
        abp.message.error('Surgeon name does not exists.');
        return;
      }
      if (this.preOperativeAssessment.bodyStructureGroupId) {
        this.specialtyDisabled = true;
      }
      if (this.preOperativeAssessment['theaterName']) {
        this._theatersService.getByHospitalId(this.hospital.id).subscribe((result) => {
          let theaterName = this.preOperativeAssessment['theaterName'].replace(/\s/g, '');
          theaterName = theaterName.split('/').pop();
          const existingTheater = result.find((r) => r.name.replace(/\s/g, '') === theaterName);

          if (existingTheater && !this.preOperativeAssessment.theaterId) {
            this.preOperativeAssessment.theaterId = existingTheater.id;
            this.preOperativeAssessment['theaterName'] = `${existingTheater.theaterId} / ${existingTheater.name}`;
          }
  
          if (!existingTheater) {
            abp.message.error('Theater name does not exists.');
            this.onTabChangeClick(1, 0);
          }
        });
      }
    } else if (fromTabIndex === 2) {
      if (!this.preOperativeAssessment.id && !this.saveDemographicIsClicked && this.riskMappingSetting) {
        abp.notify.error('Please click save button', 'Error');
        return;
      }
      if (this.preOperativeAssessment.id && this.preOperativeAssessment.isSmoker) {
        _.forEach(this.risksOneExtras, (risk) => {
          const existingRisk = this.preOperativeAssessment.procedures.find((r) => r.name === `RISK - ${risk.key}`);
          if (!existingRisk) {
            const procedure = new PoapProcedureDto();
            procedure.snomedId = '-';
            procedure.standardDeviation = risk.standardDeviation;
            procedure.meanTime = risk.meanTime;
            procedure.name = `RISK - ${risk.key}`;
            procedure.isRisk = true;
            this.preOperativeAssessment.procedures.push(procedure);
          } else {
            existingRisk.standardDeviation = risk.standardDeviation;
            existingRisk.meanTime = risk.meanTime;
          }
        });
      } else if (this.preOperativeAssessment.isSmoker) {
        _.forEach(this.risksOneExtras, (newRisk) => {
          this.preOperativeAssessment.risks.push(newRisk);
        });
      }
    } else if (fromTabIndex === 3) {
      if (!this.saveDiagnosticClicked && this.riskMappingSetting) {
        abp.notify.error('Please click save button', 'Error');
        return;
      }
      if (this.preOperativeAssessment.awaitingRiskCompletion == 1) {
        this.preOperativeAssessment.awaitingRiskCompletion = 2;
        this.updateAwaitingRiskCompletion();
        this._router.navigate(['/app/poaps']);
      }     

      if (this.preOperativeAssessment.bodyStructureGroupId) {
        this.getBodyStructureGroups(this.preOperativeAssessment.bodyStructureGroupId);
      }

      this.savedProcedures = this.preOperativeAssessment.procedures.filter((c) => !c.name.includes('RISK'));
      _.forEach(this.preOperativeAssessment.risks, (risk) => {
        const existingRisk = this.preOperativeAssessment.procedures.find((r) => r.name === `RISK - ${risk.key}`);
        this.preOperativeAssessment.procedures = this.preOperativeAssessment.procedures.filter((c) => c.name.includes('RISK'));
        if (risk.key === 'Smoker' && !this.preOperativeAssessment.isSmoker) {
          var removeIndex = this.preOperativeAssessment.procedures
            .map(function (item) {
              return item.id;
            })
            .indexOf(risk.id);
          this.preOperativeAssessment.procedures.splice(removeIndex, 1);
          // this.preOperativeAssessment.proce.preOperativeAssessment.procedures.filter(c => c.id !== risk.id);
        } else if (!existingRisk) {
          const procedureRisk = new PoapProcedureDto();
          procedureRisk.snomedId = risk.snomedId ? risk.snomedId : '-';
          procedureRisk.name = risk.group ? `RISK - ${risk.key}` : risk.key;
          procedureRisk.meanTime = risk.meanTime;
          procedureRisk.standardDeviation = risk.standardDeviation;
          procedureRisk.isRisk = true;
          this.preOperativeAssessment.procedures.push(procedureRisk);
        } else {
          risk.meanTime = existingRisk.meanTime;
          risk.standardDeviation = existingRisk.standardDeviation;
        }
      });

      _.forEach(this.selectedCoMorbidityNodes, (selectedNode) => {
        const key = selectedNode.label;
        const isExistRisk = this.preOperativeAssessment.risks.some((r) => r.key === key);
        const isExistProcedure = this.preOperativeAssessment.procedures.some((r) => r.name === `RISK - ${key}`);
        if (!selectedNode.data.isGroup && !isExistRisk && !isExistProcedure) {
          const risk = new PoapRiskDto();
          risk.group = selectedNode.data.group;
          risk.key = selectedNode.label;
          risk.value = 'true';
          risk.meanTime = 0;
          risk.standardDeviation = 0;
          risk.diagnosticId = this.selectedDiagnostic;
          this.preOperativeAssessment.risks.push(risk);

          const procedureRisk = new PoapProcedureDto();
          procedureRisk.snomedId = selectedNode.key;
          procedureRisk.name = `RISK - ${selectedNode.label}`;
          procedureRisk.meanTime = 0;
          procedureRisk.standardDeviation = 0;
          procedureRisk.isRisk = true;
          this.preOperativeAssessment.procedures.push(procedureRisk);
        }
      });

      _.forEach(this.savedProcedures, (savedProcedure) => {
        const isExistingProcedure = this.preOperativeAssessment.procedures.some((r) => r.name === savedProcedure.name);
        if (!isExistingProcedure) this.preOperativeAssessment.procedures.push(savedProcedure);
      });

      _.forEach(this.preOperativeAssessment.risks, (risk) => {
        const key = risk.key;
        const isExistRisk = this.preOperativeAssessment.risks.some((r) => r.key === key);
        const isExistProcedure = this.preOperativeAssessment.procedures.some((r) => r.name === `RISK - ${key}`);
        if (!isExistProcedure && risk.diagnosticId && risk.diagnosticId == this.selectedDiagnostic && this.riskMappingSetting) {
          const procedureRisk = new PoapProcedureDto();
          procedureRisk.snomedId = risk.key;
          procedureRisk.name = `RISK - ${risk.key}`;
          procedureRisk.meanTime = 0;
          procedureRisk.standardDeviation = 0;
          procedureRisk.isRisk = true;
          this.preOperativeAssessment.procedures.push(procedureRisk);
        }
      });

      // poap risk factors
      // _.forEach(this.savedPoapRiskFactors, poapRiskFactor => {
      //   if(this.selectedDiagnostic === poapRiskFactor.diagnosticId) {
      //     const isExistRisk = this.preOperativeAssessment.risks.some(r => r.key === poapRiskFactor.snomedId);
      //     const isExistProcedure = this.preOperativeAssessment.procedures.some(r => r.name === `RISK - ${poapRiskFactor.label}`);
      //     if(!poapRiskFactor.isGroup && !isExistRisk && !isExistProcedure) {
      //       const risk = new PoapRiskDto();
      //       risk.group = poapRiskFactor.group;
      //       risk.key = poapRiskFactor.label;
      //       risk.value = 'true';
      //       risk.meanTime = 0;
      //       risk.standardDeviation = 0;
      //       this.preOperativeAssessment.risks.push(risk);

      //       const procedureRisk = new PoapProcedureDto();
      //       procedureRisk.snomedId = poapRiskFactor.snomedId;
      //       procedureRisk.name = `RISK - ${poapRiskFactor.label}`;
      //       procedureRisk.meanTime = 0;
      //       procedureRisk.standardDeviation = 0;
      //       procedureRisk.isRisk = true;
      //       this.preOperativeAssessment.procedures.push(procedureRisk);
      //     }
      //   }
      // });
    } else if (fromTabIndex === 4) {
      _.forEach(this.patientPreparations, (patientPrep) => {
        const existingRisk = this.preOperativeAssessment.procedures.find((r) => r.name === patientPrep.key);
        if (!existingRisk) {
          const procedure = new PoapProcedureDto();
          procedure.snomedId = patientPrep.snomedId;
          procedure.standardDeviation = patientPrep.standardDeviation;
          procedure.meanTime = patientPrep.meanTime;
          procedure.name = patientPrep.key;
          procedure.isRisk = false;
          procedure.isPatientPreparation = true;
          this.preOperativeAssessment.procedures.push(procedure);
        } else {
          existingRisk.standardDeviation = patientPrep.standardDeviation;
          existingRisk.meanTime = patientPrep.meanTime;
        }
      });
    }

    this.wizardTabs.tabs[tabIndex].disabled = false;
    this.wizardTabs.tabs[tabIndex].active = true;
  }

  onAddProcedureClick(): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: this.preOperativeAssessment.bodyStructureId,
    };
    modalSettings.class = 'modal-lg';
    const modalRef = this._modalService.show(ProcedureSearchComponent, modalSettings);
    const modal: ProcedureSearchComponent = modalRef.content;
    modal.modalSave.subscribe((menuItems: MenuItemOutputDto[]) => {
      let procedures = _.map(menuItems, (menuItem) => {
        const procedure = new PoapProcedureDto();
        procedure.snomedId = menuItem.id === '' ? '-' : menuItem.id;
        procedure.name = menuItem.name;
        procedure.meanTime = 0;
        procedure.standardDeviation = 0;
        procedure.showButtonDevProc = menuItem.showButtonDevProc;
        return procedure;
      });

      // Check for duplicates
      //
      const duplicateProcedures = procedures.filter((procedure, index, array) => {
        return this.preOperativeAssessment.procedures.some(
          (s) => `${s.snomedId} - ${s.name}` === `${procedure.snomedId} - ${procedure.name}`
        );
      });

      this.showDuplicateSubProcedureError(duplicateProcedures);

      // Add only unique sub procedures
      //
      procedures = procedures.filter((procedure, index, array) => {
        return !this.preOperativeAssessment.procedures.some(
          (s) => `${s.snomedId} - ${s.name}` === `${procedure.snomedId} - ${procedure.name}`
        );
      });

      this.preOperativeAssessment.procedures.push(...procedures);
    });
  }

  onProcedureEditClick(procedure: PoapProcedureDto): void {
    this.clonedProcedures[procedure.name] = procedure;
  }

  onApplyProcedureChangesClick(procedure: PoapProcedureDto): void {
    delete this.clonedProcedures[procedure.name];
  }

  onRemoveProcedureClick(name: string): void {
    const i = this.preOperativeAssessment.procedures.findIndex((e) => e.name === name);
    if (i >= 0) {
      this.preOperativeAssessment.procedures.splice(i, 1);
    }
  }

  onExpandAllClick(): void {
    _.forEach(this.coMorbidityNodes, (coMorbidityNode) => {
      coMorbidityNode.expanded = true;
    });
  }

  onSelectAllClick($event): void {
    if (this.isCheckedSelectAll) {
      _.forEach(this.coMorbidityNodes, (coMorbidityNode) => {
        const groupNode: TreeNode = {
          key: coMorbidityNode.key,
          label: coMorbidityNode.label,
          data: {
            isGroup: true,
          },
        };
        this.selectedCoMorbidityNodes.push(groupNode);
        if (coMorbidityNode.children.length > 0) {
          _.forEach(coMorbidityNode.children, (coMorbidity) => {
            const itemNode: TreeNode = {
              key: coMorbidity.key,
              label: coMorbidity.label,
              data: {
                isGroup: false,
                group: coMorbidityNode.key,
              },
            };
            this.selectedCoMorbidityNodes.push(itemNode);
          });
        }
      });
    } else {
      this.selectedCoMorbidityNodes = [];
    }
  }

  onCheckSelectAllClick(): void {
    const selectedTreeNodes: TreeNode[] = [];
    _.forEach(this.selectedCoMorbidityNodes, (selectedMorbidity) => {
      if (selectedMorbidity.data.isGroup) {
        selectedTreeNodes.push(selectedMorbidity);
      }
    });
    if (selectedTreeNodes.length !== this.coMorbidityNodes.length) {
      this.isCheckedSelectAll = false;
    } else {
      this.isCheckedSelectAll = true;
    }
  }
  onCollapseAllClick(): void {
    _.forEach(this.coMorbidityNodes, (coMorbidityNode) => {
      coMorbidityNode.expanded = false;
    });
  }

  onSaveClick(): void {
    this.preOperativeAssessment.diagnosticReportId = this.selectedDiagnostic;
    this.preOperativeAssessment.timezone = moment.tz.guess();
    this.preOperativeAssessment.procedureMethodTypes = this.selectedProcedureMethodTypes;
    this.preOperativeAssessment.instrumentPacks = this.selectedinstrumentPacks;

    // this.preOperativeAssessment.methodTypeIds = this.selectedProcedureMethodTypes;
    const predictedTimeChecker = this.disabledSaveButton();
    this.isLoading = true;
    if (this.preOperativeAssessment.procedureMethodTypes.length == 0) {
      this.isLoading = false;
      abp.message.error('Procedure method type is required.');
    } else if (predictedTimeChecker) {
      this.isLoading = false;
      abp.message.error('Predicted time entries are mandatory.');
    }else {
      const request = !this.id
        ? this._preOperativeAssessmentsService.create(this.preOperativeAssessment)
        : this._preOperativeAssessmentsService.update(this.preOperativeAssessment);
      request.subscribe((response) => {
        this.notify.info(this.l('SavedSuccessfully'));

        this._router.navigate(['/app/poaps']);
      });
    }
  }

  confirmPatientDetails(): void {
    var ethnicity = this.ethnicities.filter((e) => e.id == this.preOperativeAssessment.ethnicityId)[0];

    if (ethnicity) this.selectedEthnicity = ethnicity.description;

    this.confirmPatientIsClicked = true;
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: this.preOperativeAssessment.patientId,
      surgeryDate: this.preOperativeAssessment.surgeryDate,
      ethnicGroup: this.selectedEthnicity,
    };
    modalSettings.class = 'modal-lg';
    const modalRef = this._modalService.show(ShowPatientDetailsComponent, modalSettings);
    const modal: ShowPatientDetailsComponent = modalRef.content;
  }

  saveDemographic(): void {
    this.saveDemographicIsClicked = true;
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      patientId: this.preOperativeAssessment.patientId,
      bloodPressure: this.preOperativeAssessment.risks[0].value,
      bmi: this.preOperativeAssessment.risks[1].value,
      gender: this.preOperativeAssessment.risks[2].value,
      age: this.preOperativeAssessment.risks[3].value,
      ethnicity: this.preOperativeAssessment.risks[4].value,
      isSmoker: this.preOperativeAssessment.isSmoker,
      surgeryDate: this.preOperativeAssessment.surgeryDate,
    };
    modalSettings.class = 'modal-lg';
    const modalRef = this._modalService.show(ShowDemographicDetailsComponent, modalSettings);
    const modal: ShowDemographicDetailsComponent = modalRef.content;
  }

  saveDiagnosticReport(): void {
    this.saveDiagnosticClicked = true;
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      patientId: this.preOperativeAssessment.patientId,
      bloodPressure: this.preOperativeAssessment.risks[0].value,
      bmi: this.preOperativeAssessment.risks[1].value,
      gender: this.preOperativeAssessment.risks[2].value,
      age: this.preOperativeAssessment.risks[3].value,
      ethnicity: this.preOperativeAssessment.risks[4].value,
      isSmoker: this.preOperativeAssessment.isSmoker,
      surgeryDate: this.preOperativeAssessment.surgeryDate,
      riskFactors: this.selectedCoMorbidityNodes,
    };
    modalSettings.class = 'modal-xl';
    const modalRef = this._modalService.show(ShowDiagnosticReportDetailsComponent, modalSettings);
    const modal: ShowDiagnosticReportDetailsComponent = modalRef.content;
    modal.modalSave.subscribe((response: boolean) => {
      this.saveDiagnosticClicked = true;
      _.forEach(this.selectedCoMorbidityNodes, (selectedNode) => {
        const key = selectedNode.label;
        const isExistRisk = this.preOperativeAssessment.risks.some((r) => r.key === key);
        const isExistProcedure = this.preOperativeAssessment.procedures.some((r) => r.name === `RISK - ${key}`);
        if (!selectedNode.data.isGroup && !isExistRisk && !isExistProcedure) {
          const risk = new PoapRiskDto();
          risk.group = selectedNode.data.group;
          risk.key = key;
          risk.value = 'true';
          risk.meanTime = 0;
          risk.standardDeviation = 0;
          risk.diagnosticId = this.riskMappingSetting ? this.diagnosticReportIdPreviousValue : null;
          risk.snomedId = selectedNode.key;
          this.preOperativeAssessment.risks.push(risk);

          // const procedureRisk = new PoapProcedureDto();
          // procedureRisk.snomedId = selectedNode.key;
          // procedureRisk.name = `RISK - ${selectedNode.label}`;
          // procedureRisk.meanTime = 0;
          // procedureRisk.standardDeviation = 0;
          // procedureRisk.isRisk = true;
          // this.preOperativeAssessment.procedures.push(procedureRisk);
        }
      });
    });
  }

  private getPoap(): void {
    this.isLoading = true;
    this._preOperativeAssessmentsService.getPoap(this.id, this.preOperativeAssessment.timezone).subscribe((poap) => {
      this.preOperativeAssessment = poap;
      if (this.preOperativeAssessment)
        this.selectedSmoker = this.preOperativeAssessment.isSmoker? 'Yes': 'No';

      console.log(this.selectedSmoker);

      this.selectedProcedureMethodTypes = this.preOperativeAssessment.procedureMethodTypes;
      
      if (this.preOperativeAssessment.instrumentPacks)
        this.selectedinstrumentPacks = this.preOperativeAssessment.instrumentPacks;

      if (this.preOperativeAssessment.bodyStructureGroupId) {
        this.specialtyDisabled = true;
      }
      if (this.preOperativeAssessment.diagnosticReportId) {
        this.selectedDiagnostic = this.preOperativeAssessment.diagnosticReportId;
        this.saveDiagnosticClicked = true;
        this.saveDemographicIsClicked = true;
        this.confirmPatientIsClicked = true;
      }
      this.preOperativeAssessment.procedures = this.preOperativeAssessment.procedures.filter((r) => r.snomedId !== '-');
      this.risksOneExtras = this.checkRiskOneTabRisks();
      this.patientPreparations = this.checkPatientPreparation();
      this.preOperativeAssessment['theaterName'] = `${poap.theater.theaterId} / ${poap.theater.name}`;

      this.getCoMorbidities();
      this.getDiagnosticReports();
      this.getAllPatients();
      if (poap.surgeonId) {
        this.getBodyStructureGroups(poap.bodyStructureGroupId);
      }
      this.isLoading = false;

      if (this.preOperativeAssessment.awaitingRiskCompletion == 2) {
        abp.notify.error('Poap has been already completed', 'Error');
        this._router.navigate(['/app/poaps']);
      }
    });
  }

  private getPatients(): void {
    this.patientsDataSource = new Observable((observer: Observer<string>) => {
      observer.next(this.preOperativeAssessment.patientId);
    }).pipe(
      switchMap((query: string) => {
        return this._patientsService.getByHospital(this.hospital.id, query);
      })
    );
  }

  private checkRiskOneTabRisks() {
    const newRisks: PoapRiskDto[] = [];
    const response: PoapRiskDto[] = [];
    const smoker = new PoapRiskDto();
    smoker.group = 'surgery';
    smoker.key = 'Smoker';
    smoker.value = 'Smoker';
    smoker.meanTime = 0;
    smoker.standardDeviation = 0;
    newRisks.push(smoker);

    _.forEach(newRisks, (newRisk) => {
      const existingRisk = this.preOperativeAssessment.procedures.find((r) => r.name === `RISK - ${newRisk.key}`);
      if (!existingRisk) {
        response.push(newRisk);
      } else {
        const risk = new PoapRiskDto();
        risk.snomedId = existingRisk.snomedId;
        risk.standardDeviation = existingRisk.standardDeviation;
        risk.key = newRisk.key;
        risk.meanTime = existingRisk.meanTime;
        response.push(risk);
      }
    });

    return response;
  }

  private checkNewPatientPreparations() {
    const newRisks: PoapRiskDto[] = [];
    const response: PoapRiskDto[] = [];
    const draping = new PoapRiskDto();
    draping.key = 'Application of surgical drapes';
    draping.meanTime = 5;
    draping.standardDeviation = 0;
    draping.snomedId = '397819007';
    newRisks.push(draping);

    const scrub = new PoapRiskDto();
    scrub.key = 'Cleaning and sterilisation of skin';
    scrub.meanTime = 5;
    scrub.standardDeviation = 0;
    scrub.snomedId = '450832005';
    newRisks.push(scrub);

    const identification = new PoapRiskDto();
    identification.key = 'Marking skin site prior to procedure';
    identification.meanTime = 5;
    identification.standardDeviation = 0;
    identification.snomedId = '225135006';
    newRisks.push(identification);

    _.forEach(newRisks, (newRisk) => {
      const existingRisk = this.preOperativeAssessment.procedures.find((r) => r.snomedId === newRisk.snomedId);
      if (!existingRisk) {
        response.push(newRisk);
      } else {
        const risk = new PoapRiskDto();
        risk.snomedId = newRisk.snomedId;
        risk.standardDeviation = newRisk.standardDeviation;
        risk.key = newRisk.key;
        risk.meanTime = newRisk.meanTime;
        response.push(risk);
      }
    });

    return response;
  }

  private checkPatientPreparation() {
    const patientPreparations = this.preOperativeAssessment.procedures.filter((r) => r.isPatientPreparation);
    const existingPatientPreparations: PoapRiskDto[] = [];
    _.forEach(patientPreparations, (procedure) => {
      const risk = new PoapRiskDto();
      risk.key = procedure.name;
      risk.meanTime = procedure.meanTime;
      risk.standardDeviation = procedure.standardDeviation;
      risk.snomedId = procedure.snomedId;
      existingPatientPreparations.push(risk);
    });

    const newRisks: PoapRiskDto[] = [];

    const whoCheckListRisk = new PoapRiskDto();
    whoCheckListRisk.key = 'WHO Surgical Safety Check List';
    whoCheckListRisk.meanTime = 5;
    whoCheckListRisk.standardDeviation = 0;
    whoCheckListRisk.snomedId = '450729003';
    newRisks.push(whoCheckListRisk);

    const patientPositioningRisk = new PoapRiskDto();
    patientPositioningRisk.key = 'Patient Positioning';
    patientPositioningRisk.meanTime = 5;
    patientPositioningRisk.standardDeviation = 0;
    patientPositioningRisk.snomedId = '229824005';
    newRisks.push(patientPositioningRisk);

    const draping = new PoapRiskDto();
    draping.key = 'Application of surgical drapes';
    draping.meanTime = 5;
    draping.standardDeviation = 0;
    draping.snomedId = '397819007';
    newRisks.push(draping);

    const scrub = new PoapRiskDto();
    scrub.key = 'Cleaning and sterilisation of skin';
    scrub.meanTime = 5;
    scrub.standardDeviation = 0;
    scrub.snomedId = '450832005';
    newRisks.push(scrub);

    const identification = new PoapRiskDto();
    identification.key = 'Marking skin site prior to procedure';
    identification.meanTime = 5;
    identification.standardDeviation = 0;
    identification.snomedId = '225135006';
    newRisks.push(identification);

    _.forEach(newRisks, (newRisk) => {
      const existingRisk = this.preOperativeAssessment.procedures.find((r) => r.snomedId === newRisk.snomedId);
      if (!existingRisk) {
        existingPatientPreparations.push(newRisk);
      }
      // else {
      //   const existingPreparations = existingPatientPreparations.some(e => e.snomedId == newRisk.snomedId);
      //   if(!existingPreparations) {
      //     const risk = new PoapRiskDto();
      //     risk.snomedId = newRisk.snomedId;
      //     risk.standardDeviation = newRisk.standardDeviation;
      //     risk.key = newRisk.key;
      //     risk.meanTime = newRisk.meanTime;
      //     existingPatientPreparations.push(risk);
      //   }
      // }
    });

    return existingPatientPreparations;
  }

  private getTheaters(): void {
    this.theatersDataSource = new Observable((observer: Observer<string>) => {
      observer.next(this.preOperativeAssessment['theaterName']);
    }).pipe(
      switchMap((query: string) => {
        return this._theatersService.search(this.hospital.id, query);
      })
    );
  }

  private getSurgeons(): void {
    this.surgeonsDataSource = new Observable((observer: Observer<string>) => {
      observer.next(this.preOperativeAssessment.surgeonName);
    }).pipe(
      switchMap((query: string) => {
        return this._usersService.getSurgeons(query, this.hospital.id);
      })
    );
  }

  // private getBodyStructureGroups(surgeonId?: number): void {
  //   this._bodyStructureGroupsService.getBySurgeon(surgeonId).subscribe((bodyStructureGroups) => {
  //     this.bodyStructureGroups = bodyStructureGroups;
  //     if (this.bodyStructureGroups && this.bodyStructureGroups.length > 0) {
  //       _.forEach(this.bodyStructureGroups, (bodyStructureGroup) => {
  //         if (!this.preOperativeAssessment.bodyStructureId) {
  //           if (bodyStructureGroup.bodyStructures && bodyStructureGroup.bodyStructures.length > 0) {
  //             this.preOperativeAssessment.bodyStructureId = bodyStructureGroup.bodyStructures[0].id;
  //             return false;
  //           }
  //         }
  //       });
  //     }
  //   });
  // }

  private getBodyStructureGroups(bodyStructureGroupId: string): void {
    this._bodyStructureGroupsService.getByBodyStructureGroupId(bodyStructureGroupId).subscribe((bodyStructureGroups) => {
      this.bodyStructureGroups = bodyStructureGroups;
      if (this.bodyStructureGroups && this.bodyStructureGroups.length > 0) {
        _.forEach(this.bodyStructureGroups, (bodyStructureGroup) => {
          if (!this.preOperativeAssessment.bodyStructureId) {
            if (bodyStructureGroup.bodyStructures && bodyStructureGroup.bodyStructures.length > 0) {
              this.preOperativeAssessment.bodyStructureId = bodyStructureGroup.bodyStructures[0].id;
              return false;
            }
          }
        });
      }
    });
  }

  private getEthnicities(): void {
    this._ethnicitiesService.getAll().subscribe((ethnicities) => {
      this.ethnicities = ethnicities;
    });
  }

  private getAllPatients(): void {
    this.isLoading = true;
    this._patientsService
      .getAll(this.hospital.id, this.preOperativeAssessment.patientId ? this.preOperativeAssessment.patientId : '')
      .subscribe((patients) => {
        this.patients = patients;
        this.isLoading = false;
      });
  }

  private getAllSurgeons(): void {
    this._usersService.getAllSurgeons(this.hospital.id).subscribe((surgeons) => {
      this.surgeons = surgeons;
    });
  }

  private getAllAnaesthetists(): void {
    this._usersService.getAllAnaesthetists(this.hospital.id).subscribe((anaesthetists) => {
      this.anaethetists = anaesthetists;
    });
  }

  private getAllTheaters(): void {
    this._theatersService.getAllTheaters(this.hospital.id).subscribe((theaters) => {
      this.theaters = theaters;
      this.isLoading = false;
    });
  }

  private getTheatersByHospital(): void {
    this._theatersService.getByHospitalId(this.hospital.id).subscribe((theaters) => {
      this.theathers = theaters;
    });
  }
  private getSpecialties(): void {
    this._bodyStructureGroupsService.getAllBodyStructureGroupByUsers(this.hospital.id).subscribe((specialties) => {
      this.specialties = specialties;
      _.forEach(this.specialties, (specialty) => {
        if (specialty.id == this.preOperativeAssessment.bodyStructureGroupId) {
          this.surgeons = specialty.surgeonSpecialties.map((e) => e.user);
        }
      });
    });
  }

  private getAnaesthetists(): void {
    this.anaesthetistsDataSource = new Observable((observer: Observer<string>) => {
      observer.next(this.preOperativeAssessment.anesthetistName);
    }).pipe(
      switchMap((query: string) => {
        return this._usersService.getAnaesthetists(query, this.hospital.id);
      })
    );
  }

  private getProcedureMethods(): void {
    this._bodyStructuresService.getMethods().subscribe((methods) => {
      this.procedureMethods = methods;
      if (!this.id && this.procedureMethods && this.procedureMethods.length > 0) {
        this.preOperativeAssessment.methodId = this.procedureMethods[0].id;
      }
    });
  }

  private getCoMorbidities(): void {
    this._coMorbiditiesService.getAll().subscribe((results) => {
      this.coMorbidityNodes = this.buildTreeNodes(results);
    });
  }

  private buildTreeNodes(treeItems: CoMorbidityGroupDto[]): TreeNode[] {
    const treeNodes: TreeNode[] = [];
    _.forEach(treeItems, (treeItem) => {
      let hasSelectedChild = false;
      const groupNode: TreeNode = {
        key: treeItem.description,
        label: treeItem.description,
        data: {
          isGroup: true,
        },
      };

      var comorbidities = treeItem.comorbidities;
      if (_.isArray(treeItem.comorbidities)) {
        const children: TreeNode[] = [];
        _.forEach(treeItem.comorbidities, (coMorbidity) => {
          const itemNode: TreeNode = {
            key: coMorbidity.snomedId != null || coMorbidity.snomedId != undefined ? coMorbidity.snomedId.toString() : '',
            label: coMorbidity.description,
            data: {
              isGroup: false,
              group: groupNode.key,
            },
          };
          children.push(itemNode);
          if (this.riskMappingSetting && this.preOperativeAssessment.diagnosticReportId) {
            if (
              this.preOperativeAssessment.risks.findIndex(
                (e) => e.key === coMorbidity.description && e.diagnosticId == this.preOperativeAssessment.diagnosticReportId
              ) >= 0
            ) {
              this.selectedCoMorbidityNodes.push(itemNode);
              hasSelectedChild = true;
            }
          } else {
            if (this.preOperativeAssessment.risks.findIndex((e) => e.key === coMorbidity.description) >= 0) {
              this.selectedCoMorbidityNodes.push(itemNode);
              hasSelectedChild = true;
            }
          }
        });
        groupNode.children = children;
      }

      comorbidities = comorbidities.filter(
        (e) => !this.selectedCoMorbidityNodes.some((c) => `${c.key} - ${c.label}` === `${e.snomedId} - ${e.description}`)
      );
      if (hasSelectedChild) {
        groupNode.partialSelected = comorbidities.length === 0 ? false : true;
        this.selectedCoMorbidityNodes.push(groupNode);
      }
      treeNodes.push(groupNode);
    });
    return treeNodes;
  }

  private showDuplicateSubProcedureError(duplicateProcedures: PoapProcedureDto[]) {
    if (!duplicateProcedures || (duplicateProcedures && duplicateProcedures.length <= 0)) {
      return;
    }
    let errorListHtml = '<u style="text-decoration: none;">';
    duplicateProcedures.forEach((dupProcedure) => {
      errorListHtml += '<li style="text-align: left; margin-left: 30px;">' + dupProcedure.name + '</li>';
    });
    errorListHtml += '</u>';

    abp.message.error(errorListHtml, this.l('DuplicateSubProcedureFoundErrorTittle'), {
      isHtml: true,
    });
  }

  private getAllProcedureMethodTypes(): void {
    this._preOperativeAssessmentsService.getAllProcedureMethodTypes().subscribe((procedureMethodTypes) => {
      this.procedureMethodTypes = procedureMethodTypes.filter((e) => !this.selectedProcedureMethodTypes.some((c) => c.id == e.id));
    });
  }

  private getAllInstrumentPacks(): void {
    this._instrumentPackService.getAllInstrumentPacks().subscribe((ip) => {
      if (this.selectedinstrumentPacks){
        this.instrumentPacks = ip.filter((e) => !this.selectedinstrumentPacks.some((c) => c.id == e.id));        
      }
      else {
        this.instrumentPacks = ip;
      }
    });
  }

  private updateAwaitingRiskCompletion(): void {
    this._preOperativeAssessmentsService
      .updateAwaitingRiskCompletion(this.preOperativeAssessment.id, this.preOperativeAssessment.awaitingRiskCompletion)
      .subscribe((res) => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }
  private getRiskmappingSetting(): void {
    this._riskMappingService.getRiskMappingSettingByHospital(this.hospital.id).subscribe((response) => {
      this.riskMappingSetting = response;
    });
  }

  private getDiagnosticReports(): void {
    this._diagnosticReportService.getAll(this.preOperativeAssessment.patientId).subscribe((response) => {
      this.diagnosticReports = response;
    });
  }

  private showSelectedComorbidities() {
    if (!this.selectedCoMorbidityNodes || (this.selectedCoMorbidityNodes && this.selectedCoMorbidityNodes.length <= 0)) {
      return;
    }
    let errorListHtml = '';
    this.selectedCoMorbidityNodes.forEach((dupProcedure, i) => {
      errorListHtml += i + 1 + ')' + dupProcedure.label + '\n';
    });

    abp.message.confirm(errorListHtml, 'Selected risk factors', (result: boolean) => {
      if (result) {
        this.selectedCoMorbidityNodes.forEach((comorbidity) => {
          if (!comorbidity.parent) comorbidity.partialSelected = false;
        });
        this.selectedCoMorbidityNodes = [];
      }
    });
  }


  //#region Apply Device to procedure

  public onApplyDevice(procedure: PoapProcedureDto): void {

    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-xl';
    modalSettings.initialState = {
      procedure: procedure,
      bodyStructureId: this.preOperativeAssessment.bodyStructureId,
      bodyStructureGroupId: this.preOperativeAssessment.bodyStructureGroupId,
      isFilterLicensedStatus: this.isFilterLicensedStatus
    };

    const modalRef = this._modalService.show(ProcedureSelectDeviceComponent, modalSettings);
    const modal: ProcedureSelectDeviceComponent = modalRef.content;
    modal.returnedProcedure.subscribe((poapProcedure: PoapProcedureDto) => {
      if (poapProcedure) {
        modal.onCloseClick();
        _.forEach(this.preOperativeAssessment.procedures, (proc) => {
           if (proc.snomedId == poapProcedure.snomedId && 
            proc.preOperativeAssessmentId == poapProcedure.preOperativeAssessmentId && 
            proc.id == poapProcedure.id) {
                proc.poapProcedureDevices = poapProcedure.poapProcedureDevices;
            }
        });
      }
    });
  }

  private getUserDetails(): void {
    this._usersService.get(this.appSession.userId).subscribe((userInfo) => {
      this.role = userInfo.roleNames.find((u) => u === 'SUPER ADMIN' || u === 'ADMIN' || u === 'SURGEON');
      
      if (this.role === 'SUPER ADMIN' || this.role  === 'ADMIN') {
        this.isAdmin = true;
      }
      
      if (this.role === 'SURGEON') {
        this.isSurgeon = true;
      }

      if(this.role === 'SURGEON' || this.role === 'ADMIN'){
        this.isFilterLicensedStatus = true;
      }
    });
  }

  showInstruments(selected: InstrumentPackDto): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-lg';
    
    modalSettings.initialState = {
      instrumentPack: selected
    };

    const modalRef = this._modalService.show(InstrumentSelectComponent, modalSettings);
    const modal: InstrumentSelectComponent = modalRef.content;
  }

  public onSelectSmoker(smoker): void {
    this.selectedSmoker = smoker;
    if (smoker === 'Yes') {
      this.preOperativeAssessment.isSmoker = true;
    }
    else
      this.preOperativeAssessment.isSmoker = false;

    console.log(this.preOperativeAssessment.isSmoker);
  }
  //#endregion
}

export enum AwaitingRiskCompletionEnum {
  open,
  true,
  false,
}
