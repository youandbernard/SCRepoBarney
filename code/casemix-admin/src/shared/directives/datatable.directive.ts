import { Directive, OnDestroy, OnInit, Input, ElementRef } from '@angular/core';
import { Subject } from 'rxjs';

@Directive({
  selector: '[datatable]'
})
export class DatatableDirective implements OnDestroy, OnInit {
  @Input() dtOptions: DataTables.Settings = {};
  @Input() dtTrigger: Subject<any>;

  dtInstance: Promise<DataTables.Api>;
  private dt: DataTables.Api;

  constructor(private el: ElementRef) {
    const dtApi: any = $.fn.dataTable.Api;
    dtApi.register('column().searchable()', function () {
      const ctx = this.context[0];
      return ctx.aoColumns[this[0]].bSearchable;
    });
  }

  ngOnInit(): void {
    if (this.dtTrigger) {
      this.dtTrigger.subscribe(() => {
        this.displayTable();
      });
    } else {
      this.displayTable();
    }
  }

  ngOnDestroy(): void {
    if (this.dtTrigger) {
      this.dtTrigger.unsubscribe();
    }
    if (this.dt) {
      this.dt.destroy(true);
    }
  }

  private displayTable(): void {
    this.dtInstance = new Promise((resolve, reject) => {
      Promise.resolve(this.dtOptions).then(dtOptions => {
        setTimeout(() => {
          this.dt = $(this.el.nativeElement).DataTable(dtOptions);
          resolve(this.dt);
        });
      });
    });
  }
}
