import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ManufactureDto, ManufacturesServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-select-manufacturer',
  templateUrl: './select-manufacturer.component.html',
  styleUrls: ['./select-manufacturer.component.less']
})

export class SelectManufacturerComponent implements OnInit {

  @Output() manufacturer: EventEmitter<ManufactureDto> = new EventEmitter<ManufactureDto>();
  manufacturers: ManufactureDto[];
  selectedManufacturer: ManufactureDto;    
  selectedManufacturerId: string;
  
  constructor(
    private _modalRef: BsModalRef,
    private _manufacturesService: ManufacturesServiceProxy
    ) { }

  ngOnInit(): void {
    this.loadManufacturers();
  }

  loadManufacturers(): void{
    this._manufacturesService.getAll().subscribe((res) => {
      this.manufacturers = res;
    });
  }
  
  onModalSubmit(): void {    
    this.selectedManufacturer = this.manufacturers.filter((m) => m.id == this.selectedManufacturerId)[0];
    
    this.manufacturer.emit(this.selectedManufacturer);
    this.close();
  }

  onCloseClick(): void {
    this.close();
  }

  private close(): void {
    this._modalRef.hide();
  }
}
