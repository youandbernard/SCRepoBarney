import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TreeNode } from 'primeng/api';
import { BodyStructuresServiceProxy, MenuItemOutputDto } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'app-procedure-search',
  templateUrl: './procedure-search.component.html',
  styleUrls: ['./procedure-search.component.less']
})
export class ProcedureSearchComponent extends AppComponentBase implements OnInit {
  @Input() id: number;
  @Output() modalSave: EventEmitter<MenuItemOutputDto[]> = new EventEmitter<MenuItemOutputDto[]>();
  treeNodes: TreeNode[] = [];
  selectedTreeItems: TreeNode[] = [];
  isMenuLoading = false;
  isCheckedSelectAll = false;

  constructor(injector: Injector, private _modalRef: BsModalRef, private _bodyStructuresService: BodyStructuresServiceProxy) {
    super(injector);
    this.location.onPopState(() => this.close());
  }

  ngOnInit(): void {
    this.getMenuItems();
  }

  onCloseClick(): void {
    this.close();
  }

  onSelectAllClick($event): void {
    if (this.isCheckedSelectAll) {
      _.forEach(this.treeNodes, treeItemNode => {
        this.selectedTreeItems.push(treeItemNode);
      });
    } else {
      this.selectedTreeItems = [];
    }
  }

  onCheckSelectAllClick(): void {
    const selectedTreeNodes: TreeNode[] = [];
    _.forEach(this.selectedTreeItems, selectedTreeItem => {
      selectedTreeNodes.push(selectedTreeItem);
    });
    if (selectedTreeNodes.length !== this.treeNodes.length) {
      this.isCheckedSelectAll = false;
    } else {
      this.isCheckedSelectAll = true;
    }
  }

  onFormSubmit(): void {
    const menuItems = _.map(this.selectedTreeItems, selectedTreeItem => selectedTreeItem.data as MenuItemOutputDto);
    this.modalSave.emit(menuItems);
    this.close();
  }

  private close(): void {
    this._modalRef.hide();
  }

  private getMenuItems(): void {
    this.isMenuLoading = true;
    this._bodyStructuresService.getMenu(this.id).subscribe(results => {
      this.treeNodes = this.buildTreeNodes(results);
      this.isMenuLoading = false;
    });
  }

  private buildTreeNodes(treeItems: MenuItemOutputDto[]): TreeNode[] {
    const treeNodes: TreeNode[] = [];
    _.forEach(treeItems, treeItem => {
      const hasDuplicate = treeItems.find(e => e.id == treeItem.id && e.name != treeItem.name);
      const treeNode: TreeNode = {
        key: hasDuplicate ? treeItem.id + ' ' +  treeItem.name : treeItem.id,
        label: treeItem.name,
        data: treeItem
      };
      if (_.isArray(treeItem.children)) {
        treeNode.children = this.buildTreeNodes(treeItem.children);
      }
      treeNodes.push(treeNode);
    });
    return treeNodes;
  }
}
