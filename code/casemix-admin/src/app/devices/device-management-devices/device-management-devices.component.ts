import { AfterViewInit, Component, Injector, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DeviceDto, DeviceDtoPagedResultDto, DeviceServiceProxy, HospitalDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { Subject } from 'rxjs';
import { DeviceProceduresComponent } from './device-procedures/device-procedures.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { DeviceGmdntermcodesComponent } from './device-gmdntermcodes/device-gmdntermcodes.component';
import * as _ from 'lodash';
import { DatePipe } from '@angular/common';
import { LocalStorageService } from '@shared/services/local-storage.service';

@Component({
  selector: 'app-device-management-devices',
  templateUrl: './device-management-devices.component.html',
  styleUrls: ['./device-management-devices.component.less'],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})

export class DeviceManagementDevicesComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

  @Input() superAdminUser: boolean;

  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  dtColumns: DataTables.ColumnSettings[] = [];

  sortColumns: string[] = ['id', 'id', 'gmdnTermCode', 'deviceName', 'manufacturer.name', 'bodyStructureGroup.name', 'deviceClass.class', 'gtinCode', 'brandName', 'model', 'createdDate', 'createdDate', 'status', 'isAvailable'];
  devices: DeviceDto[];
  checkedDevices: number[] = [];

  isLoading = false;
  isFilterReady = false;
  filterManufacturer = '';
  filterSpecialty = '';
  filterClass = '';
  disabledOnly = false;
  unavailableOnly = false;
  
  adminUser: boolean = false;
  surgeonUser: boolean = false;
  manufacturerUser: boolean = false;

  selectAll: boolean = false;
  check_uncheck_all: boolean = false;
  
  currentHospitalId: string;

  constructor(injector: Injector,
    private _localStorageService: LocalStorageService,
    private _userService: UserServiceProxy,
    private _devicesService: DeviceServiceProxy,
    private _modalService: BsModalService,
    private _datepipe: DatePipe,
    ) { 
      super(injector);
    }

    
  ngOnInit(): void {
    const localHospital = this._localStorageService.getObjectItem<HospitalDto>(this.localStorageKey.hospital);
    if (localHospital) {
      this.currentHospitalId = localHospital.id;
    }

    this.getUserDetails();

    this.initializeDataTable();
  }

    ngAfterViewInit(): void {
      this.dtTrigger.next();
      this.validateCheck();
    }
  
    ngOnDestroy(): void {
      this.dtTrigger.unsubscribe();
    }


  private getUserDetails(): void {
    this._userService.get(this.appSession.userId).subscribe((userInfo) => {
      
      if (userInfo.roleNames.find((u) => u === 'ADMIN') !== undefined)  {
        if (userInfo.roleNames.find((u) => u === 'ADMIN').length > 0) {
          this.adminUser = true;
        } 
      }

      if (userInfo.roleNames.find((u) => u === 'SUPER ADMIN') !== undefined) {
        if (userInfo.roleNames.find((u) => u === 'SUPER ADMIN').length > 0) {
          this.superAdminUser = true;
        }
      }
      
      if (userInfo.roleNames.find((u) => u === 'MANUFACTURER') !== undefined) {
        if (userInfo.roleNames.find((u) => u === 'MANUFACTURER').length > 0) {
          this.manufacturerUser = true;
        }
      }
      
      if (userInfo.roleNames.find((u) => u === 'SURGEON') !== undefined) {
        if (userInfo.roleNames.find((u) => u === 'SURGEON').length > 0) {
          this.surgeonUser = true;
        }
      }     

    });

  }

  private initializeDataTable(): void {
      const self = this;

      this.dtOptions = this.defaultDtOptions;
      this.dtOptions.language.zeroRecords = this.l('NoDeviceAvailable');
      this.dtOptions.orderCellsTop = true;
      this.dtOptions.columnDefs = [
        { targets: [-1, -2, 0], orderable: false },
        { searchable: false, targets: [1, 2, 3, 7, 8, 9, 10, 11, 12] }
      ];

      if (self.superAdminUser) {
        this.dtColumns = [
          { 
            data: 'id',
            render: function(data, type, full, meta) {
              var spanElement = `<div class='custom-control custom-checkbox'>
                                    <input type='checkbox' style='opacity: 100 !important;'
                                      class='custom-control-input chkSelect'
                                      name='chkSelect'
                                      id='chkSelect_${data}'
                                      data-id='${data}'
                                    />
                                  </div>`;
              
              return spanElement;
            }
          },
          { data: 'id' },
          { 
            data: 'gmdnTermCode',
            render: function(data, type, full, meta) {
              var spanElement = `<span class="term-code-blue view-gmdn" data-id="${data}">${data}</span>`;
              
              return spanElement;
            }
          },
          { 
            data: 'deviceName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data.length > 20 ? data.slice(0, 20).concat("..."): data}</span>`;
            }
          },
          { 
            data: 'manufacturerName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data.length > 15 ? data.slice(0, 15).concat("..."): data}</span>`;
            }
          },
          { 
            data: 'specialtyName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data.length > 15 ? data.slice(0, 15).concat("..."): data}</span>`;
            }
          },
          {   
            data: 'deviceClassName',
          },
          { data: 'gtinCode'},
          { 
            data: 'brandName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data? data.length > 20 ? data.slice(0, 20).concat("..."): data: ""}</span>`;
            }
          },
          { data: 'model' },
          { 
            data: 'createdDate',
            render: function(data, type, full, meta) {
              return `<span>${self._datepipe.transform(data, 'dd/MM/yyyy  hh:mm:ss')}</span>`;
            }
          },
          { 
            data: 'createdDate',
            render: function(data, type, full, meta) {
              return `<span>${self._datepipe.transform(data, 'dd/MM/yyyy  hh:mm:ss')}</span>`;
            }
          },
          { 
            data: 'statusName',
            render: function(data, type, full, meta) {
                return `<span>${data}</span>`;            
            }
          },          
          {
            data: 'id',
            render: function(data, type, full, meta) {
              if ((self.adminUser || self.surgeonUser || self.superAdminUser) && !self.manufacturerUser) {
                var buttonElement = '';
  
                //---------------------------------------------------------------------------------------
                //Do not delete this comments, this can be used if requirement want them to put it back.
                //---------------------------------------------------------------------------------------

                // var isDisabled = ``;
                // if (full.status == 0) {
                //   isDisabled == `disabled="true"`;
                // }
  
                // var button1 = `<button type="button" 
                //         data-id="${data}"
                //         data-gmdn="${full.gmdnTermCode}"
                //         data-devicename="${full.deviceName}"
                //         class="${full.status == 1? 'btn btn-sm bg-success assign-procedure': 'btn btn-sm bg-secondary assign-procedure'}" 
                //         title="${full.status == 0? 'Device is disabled': 'Assign procedures'}"
                //         ${isDisabled}
                //       >
                //         <i class="fas fa-procedures mr-1"></i>
                //       </button>`;
                  
                // var button2 = `<button type="button" 
                //         data-id="${data}"
                //         data-status"${full.status}"
                //         class="${full.status == 1? 'btn btn-sm bg-success enable-disable': 'btn btn-sm bg-danger enable-disable'}"
                //         style="margin-left: 5px;"
                //         title="${full.status == 0? 'Enable device': 'Disable device'}"
                //       >
                //         <i class="${full.status == 1? 'fas fa-check-circle mr-1': 'fas fa-ban mr-1'}"></i>
                //       </button>`;
                  
                // if (self.superAdminUser) {
                //   buttonElement = button1 + ' ' + button2;
                // }
                // else {
                //   buttonElement = button1;
                // }

                //---------------------------------------------------------------------------------------
                //Do not delete this comments, this can be used if requirement want them to put it back.
                //---------------------------------------------------------------------------------------

                return buttonElement;
              }
            }
          }
        ];
  
      }
      else {
        this.dtColumns = [
          { 
            data: 'id',
            render: function(data, type, full, meta) {
              var spanElement = `<div class='custom-control custom-checkbox'>
                                    <input type='checkbox' style='opacity: 100 !important;'
                                      class='custom-control-input chkSelect'
                                      name='chkSelect'
                                      id='chkSelect_${data}'
                                      data-id='${data}'
                                    />
                                  </div>`;
              
              return spanElement;
            }
          },
          { data: 'id' },
          { 
            data: 'gmdnTermCode',
            render: function(data, type, full, meta) {
              var spanElement = `<span class="term-code-blue view-gmdn" data-id="${data}">${data}</span>`;
              
              return spanElement;
            }
          },
          { 
            data: 'deviceName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data.length > 20 ? data.slice(0, 20).concat("..."): data}</span>`;
            }
          },
          { 
            data: 'manufacturerName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data !== null? data.length > 15 ? data.slice(0, 15).concat("..."): data: ""}</span>`;
            }
          },
          { 
            data: 'specialtyName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data !== null? data.length > 15 ? data.slice(0, 15).concat("..."): data: ""}</span>`;
            }
          },
          {   
            data: 'deviceClassName',
          },
          { data: 'gtinCode'},
          { 
            data: 'brandName',
            render: function(data, type, full, meta) {
              return `<span title="${data}">${data? data.length > 20 ? data.slice(0, 20).concat("..."): data: ""}</span>`;
            }
          },
          { data: 'model' },
          { 
            data: 'createdDate',
            render: function(data, type, full, meta) {
              return `<span>${self._datepipe.transform(data, 'dd/MM/yyyy  hh:mm:ss')}</span>`;
            }
          },
          { 
            data: 'createdDate',
            render: function(data, type, full, meta) {
              return `<span>${self._datepipe.transform(data, 'dd/MM/yyyy  hh:mm:ss')}</span>`;
            }
          },
          { 
            data: 'isAvailable',
            render: function(data, type, full, meta) {
                var av = '';
                if (data)
                  av = 'Yes';
                else
                  av = 'No';

                return `<span>${av}</span>`;            
            }
          },
          {
            data: 'id',
            render: function(data, type, full, meta) {
              if ((self.adminUser || self.surgeonUser || self.superAdminUser) && !self.manufacturerUser) {
                var buttonElement = '';
                
                //---------------------------------------------------------------------------------------
                //Do not delete this comments, this can be used if requirement want them to put it back.
                //---------------------------------------------------------------------------------------

                // var isDisabled = ``;
                // if (full.status == 0) {
                //   isDisabled == `disabled="true"`;
                // }
  
                // var button1 = `<button type="button" 
                //         data-id="${data}"
                //         data-gmdn="${full.gmdnTermCode}"
                //         data-devicename="${full.deviceName}"
                //         class="${full.status == 1? 'btn btn-sm bg-success assign-procedure': 'btn btn-sm bg-secondary assign-procedure'}" 
                //         title="${full.status == 0? 'Device is disabled': 'Assign procedures'}"
                //         ${isDisabled}
                //       >
                //         <i class="fas fa-procedures mr-1"></i>
                //       </button>`;
                  
                // var button2 = `<button type="button" 
                //         data-id="${data}"
                //         data-status"${full.status}"
                //         class="${full.status == 1? 'btn btn-sm bg-success enable-disable': 'btn btn-sm bg-danger enable-disable'}"
                //         style="margin-left: 5px;"
                //         title="${full.status == 0? 'Enable device': 'Disable device'}"
                //       >
                //         <i class="${full.status == 1? 'fas fa-check-circle mr-1': 'fas fa-ban mr-1'}"></i>
                //       </button>`;
                  
                // if (self.superAdminUser) {
                //   buttonElement = button1 + ' ' + button2;
                // }
                // else {
                //   buttonElement = button1;
                // }

                //---------------------------------------------------------------------------------------
                //Do not delete this comments, this can be used if requirement want them to put it back.
                //---------------------------------------------------------------------------------------

                return buttonElement;
              }
            }
          }
        ];
      }

      this.dtOptions.columns = this.dtColumns;
      this.dtOptions.createdRow = function( row, data, index) {
          var _device: DeviceDto;
          _device = data as DeviceDto;
          if (!self.superAdminUser) {
            if (_device.isAvailable) {
              $(row).addClass('text-highlight bg-success');

              $(row).find('.view-gmdn').addClass('term-code-white');
              $(row).find('.view-gmdn').removeClass('term-code-blue');
            }else {
              $(row).removeClass('text-highlight bg-success');

              $(row).find('.view-gmdn').addClass('term-code-blue');
              $(row).find('.view-gmdn').removeClass('term-code-white');
            }
          }else {
            $(row).removeClass('text-highlight bg-success');

            $(row).find('.view-gmdn').addClass('term-code-blue');
            $(row).find('.view-gmdn').removeClass('term-code-white');
          }
      };
      this.dtOptions.ajax = (dtParams: DataTables.AjaxDataRequest, callback) => {
        const orderColumn = dtParams.order[0];
        const sortColumn = `${this.sortColumns[orderColumn.column]} ${orderColumn.dir}`;
        this.isLoading = true;

        const filterManufacturer = this.setUndefinedIfEmpty(this.filterManufacturer);
        const filterSpecialty = this.setUndefinedIfEmpty(this.filterSpecialty);
        const filterClass = this.setUndefinedIfEmpty(this.filterClass);

        this._devicesService
            .getAll(filterManufacturer, filterSpecialty, filterClass, this.currentHospitalId, this.disabledOnly, this.unavailableOnly,
            dtParams.search.value, sortColumn, dtParams.start, dtParams.length)
          .subscribe((result: DeviceDtoPagedResultDto) => {
              this.devices = result.items;              

            _.forEach(this.devices, (device) => {
              if (device.bodyStructureGroup) {
                device.specialtyName = device.bodyStructureGroup.name; 
              } else {
                device.specialtyName = "";
              }      
              
              if (device.manufacturer) {
                device.manufacturerName = device.manufacturer.name;               
              }

              if (device.deviceClass) {
                device.deviceClassName = "Class " + device.deviceClass.class;
              }

              if (device.status == 1)
                device.statusName = "Enabled";
              else
                device.statusName = "Disabled";
                
              this.superAdminUser = device.superAdminUser;
            });

            if (this.check_uncheck_all)
              this.selectDeselectAllDevice(this.selectAll);
                        
            callback({
              recordsFiltered: result.totalCount,
              data: result.items,
            });

            this.validateCheck();
            this.isLoading = false;
          });
      };
      
      this.dtOptions.initComplete = function() {
        if (!self.isFilterReady) {
          this.api()
            .columns()
            .every(function() {
              if (this.searchable()) {
                const that = this;
                const index = that.index();
                
                if (index === 0) {                  
                  const checkx = 
                  $(
                    `<div class='custom-control custom-checkbox'>                        
                        <input type='checkbox' 
                          class='custom-control-input checkbox-input-select-all'
                          name='chkSelectAll'
                          id='chkSelectAll'
                        />
                        <label for='chkSelectAll' class='checkbox-label-select-all'>Select All</label>
                      </div>`
                  )                  
                  .appendTo($('thead tr:eq(1) td').eq(0))
                  .on('click', function() {
                    if (index === 0) {
                      self.selectAll = $('.checkbox-input-select-all').is(':checked');
                      // self.checkedDevices = [];
                      
                      self.check_uncheck_all = true;
                      self.selectDeselectAllDevice(self.selectAll);
                    }                    
                  });

                }

                if (index !== 0) {
                  const selectx = $('<select><option value="">All</option></select>')
                    .appendTo($('thead tr:eq(1) td').eq(this.index()))
                    .on('change', function() {
                      const val = $(this).val().toString();                    
  
                      if (index === 4) {
                        self.filterManufacturer = val;
                      }
                      if (index === 5) {
                        self.filterSpecialty = val;
                      }
                      if (index === 6) {
                        self.filterClass = val;
                      }    
                      self.dtTrigger.next();
                      self.validateCheck();
                    });
            
                  if (index === 4) {
                    self._devicesService.getManufacturers().subscribe(manufaturers => {
                      _.forEach(manufaturers, manufaturer => {
                        if (manufaturer) {
                          selectx.append($(`<option value="${manufaturer.id}">${manufaturer.name}</option>`));
                        }
                      });
                    });
                  }
    
                  if (index === 5) {
                    self._devicesService.getBodyStructureGroups().subscribe(groups => {
                      _.forEach(groups, group => {
                        if (group){
                          if (group.id !== undefined) {
                            selectx.append($(`<option value="${group.id}">${group.name}</option>`));
                          }
                        }
                      });
                    });
                  }
  
                  if (index === 6) {
                    self._devicesService.getDeviceClass().subscribe(dclass => {
                      _.forEach(dclass, c => {
                        if (c){
                          if (c !== undefined) {
                            selectx.append($(`<option value="${c.id}">Class ${c.class}</option>`));
                          }
                        }
                      });
                    });
                  }                
                }
              }
            });
  
          $(document).on('click', '.view-gmdn', function(e) {
            self.getgmdnTermCode($(this).data('id'));
          });

          $(document).on('click', '.assign-procedure', function(e) {
            self.assignProcedure($(this).data('id'), $(this).data('gmdn'), $(this).data('devicename'));
          });

          $(document).on('click', '.enable-disable', function(e) {
            self.enableOrDisable($(this).data('id'), $(this).data('status'));
          });

          $(document).on('click', '.chkSelect', function(e) {
            self.addSelected(this, $(this).data('id'));
          });
        }
        self.isFilterReady = true;
      };

  } 

  assignProcedure(id: number, code: string, name: string): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-xl';
    modalSettings.initialState = {
      id: id,
      code: code,
      name: name
    };

    const modalRef = this._modalService.show(DeviceProceduresComponent, modalSettings);
    const modal: DeviceProceduresComponent = modalRef.content;
    modal.submitted.subscribe((res) => {
      if (res === true){
        modal.onCloseClick();  
      }
    });
  }

  public getgmdnTermCode(code: string): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.class = 'modal-lg';
    modalSettings.initialState = {
      gmdntermcode: code
    };

    const modalRef = this._modalService.show(DeviceGmdntermcodesComponent, modalSettings);
    const modal: DeviceGmdntermcodesComponent = modalRef.content;    
  }

  enableOrDisable(id: number, stat: number): void {
    if (stat === 0) {
      abp.message.confirm('Do you want to continue to enable device?', 'Enable device.', (result: boolean) => {
        if (result) {
          this._devicesService.enableDisableDevice(id, stat)
          .subscribe((ret) => {
              if (ret){
                this.notify.success('Device is successfully enabled.');
                this.dtTrigger.next();
                this.validateCheck();
              }
              else {
                this.notify.error('Device is unable to activate.');
              }            
          });
        }
      });  
    } else if (stat === 1) {
      abp.message.confirm('Do you want to continue to disable device?', 'Disable device.', (result: boolean) => {
        if (result) {
          this._devicesService.enableDisableDevice(id, stat)
          .subscribe((ret) => {
              if (ret){
                this.notify.success('Device is successfully disabled.');
                this.dtTrigger.next();
                this.validateCheck();
              }
              else {
                this.notify.error('Device is unable to deactivate.');
              }
          });
        }
      }); 
    }
  }

  public EnableOrDisableDevices(stat: number): void {
    if (this.checkedDevices.length > 0) {
      var confirmMsg = '';        
      var strnumDevices = this.checkedDevices.length == 1? 'device': 'devices';

      if (stat === 1) {
        let confirmMsg = 'Do you want to enable ' + this.checkedDevices.length + ' ' + strnumDevices + ', making them available to Hospitals?';

        abp.message.confirm(confirmMsg, 'Enable selected devices.', (result: boolean) => {
          if (result) {
            this._devicesService.enableDisableSelected(stat, this.checkedDevices)
            .subscribe((ret) => {
                if (ret){
                  this.notify.success('Devices are successfully enabled.');
                  this.dtTrigger.next();
                  this.checkedDevices = [];
                }
                else {
                  this.notify.error('Devices are unable to activate.');
                }            
            });
          }
        });  
      } else if (stat === 0) {
        let confirmMsg = 'Do you want to disable ' + this.checkedDevices.length + ' ' + strnumDevices + ', removing there availability to Hospitals?';

        abp.message.confirm(confirmMsg, 'Disable selected devices.', (result: boolean) => {
          if (result) {
            this._devicesService.enableDisableSelected(stat, this.checkedDevices)
            .subscribe((ret) => {
                if (ret){
                  this.notify.success('Devices are successfully disabled.');
                  this.dtTrigger.next();
                  this.checkedDevices = [];
                }
                else {
                  this.notify.error('Devices are unable to deactivate.');
                }
            });
          }
        }); 
      }
    }    
  }

  public MakeAvailableDevices(stat: number): void {
    if (this.checkedDevices.length > 0) {
        var confirmMsg = '';        
        var strnumDevices = this.checkedDevices.length == 1? 'device': 'devices';

        if (stat === 1) {
          let confirmMsg = 'You are about to make (' + this.checkedDevices.length + ') ' + strnumDevices + ' available?';

          abp.message.confirm(confirmMsg, 'Make available ' + strnumDevices + '.', (result: boolean) => {
            if (result) {
              this._devicesService.devicesAvailability(stat, this.currentHospitalId, this.checkedDevices)
              .subscribe((ret) => {
                  if (ret){
                    this.notify.success('Device/s are now available to the hospital.');
                    this.dtTrigger.next();
                    this.checkedDevices = [];
                  }
                  else {
                    this.notify.error('Device/s are unable to make available.');
                  }            
              });
            }
          });  
        } else if (stat === 0) {
          let confirmMsg = 'You are about to make (' + this.checkedDevices.length + ') ' + strnumDevices + ' unavailable?';

          abp.message.confirm(confirmMsg, 'Make unavailable ' + strnumDevices + '.', (result: boolean) => {
            if (result) {
              this._devicesService.devicesAvailability(stat, this.currentHospitalId, this.checkedDevices)
              .subscribe((ret) => {
                  if (ret){
                    this.notify.success('Device/s are unavailable to the hospital.');
                    this.dtTrigger.next();
                    this.checkedDevices = [];
                  }
                  else {
                    this.notify.error('Device/s are unable to make unavailable.');
                  }
              });
            }
          }); 
        }
      }

  }

  showDisabled($event): void {
    this.disabledOnly = $event.target.checked;

    this.dtTrigger.next();
    this.validateCheck();
  }

  showUnAvailable($event): void {
    this.unavailableOnly = $event.target.checked;

    this.dtTrigger.next();
    this.validateCheck();
  }

  public addSelected(event, id): void {
    this.check_uncheck_all = false;
    if (event.checked) {
      const index = this.checkedDevices.findIndex((e) => e == id);
      if (index < 0) {
        this.checkedDevices.push(id);        
      }
    } else {
      const index = this.checkedDevices.findIndex((e) => e == id);
      if (index >= 0) {
        this.checkedDevices.splice(index, 1);
      }
    }
  }

  public addPerDevice(id): void {
    const index = this.checkedDevices.findIndex((e) => e == id);
      if (index < 0) {
        this.checkedDevices.push(id);        
      }
  }

  public removePerDevice(id) {
    const index = this.checkedDevices.findIndex((e) => e == id);
      if (index >= 0) {
        this.checkedDevices.splice(index, 1);
      }
  }

  public validateCheck(): boolean {
    var ret: boolean = false;

    if (this.checkedDevices !== undefined) {
      if (this.checkedDevices.length > 0) {
        _.forEach(this.checkedDevices, (i) => {
          $('#chkSelect_' + i).prop( 'checked', true );
        });
      }
      else {
        $('#devices').find('input:checkbox').prop('checked', false);
      }
    }

    return ret;
  }
  
  selectDeselectAllDevice(chk: boolean): void {
    if (chk) {
      _.forEach(this.devices, (i) => {
        this.addPerDevice(i.id);
      });
    } else {
      this.checkedDevices = [];
    }    

    // this.dtTrigger.next();
    this.validateCheck();

  }
}
