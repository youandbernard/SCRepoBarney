import { Component, Injector, OnInit } from '@angular/core';
import  dataList  from '../../riskmapping.json';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalService } from 'ngx-bootstrap/modal';
import { OpenImageComponent } from '../open-image/open-image.component';

@Component({
  selector: 'app-riskmapping-samples',
  templateUrl: './riskmapping-samples.component.html',
  styleUrls: ['./riskmapping-samples.component.less'],
  animations: [appModuleAnimation()]
})
export class RiskmappingSamplesComponent extends AppComponentBase implements OnInit {

  list: {id: number, imageUrl: string }[] = dataList; 
  constructor(injector: Injector, private _modalService: BsModalService) {
    super(injector);
   }


  ngOnInit(): void {
  }

  onOpenImageClick(id: number): void {
    this.showOpenImageModal(id);
  }

  private showOpenImageModal(id: number): void {
    const modalSettings = this.defaultModalSettings;
    modalSettings.initialState = {
      id: id
    };
    
    const modalRef = this._modalService.show(OpenImageComponent, {initialState: modalSettings.initialState, class: 'modal-xl'});
    const modal: OpenImageComponent = modalRef.content;
    // modal.modalSave.subribe(() => {

    // })
  }

}
