import { PipeTransform, Pipe, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Pipe({
    name: 'callback',
    pure: false
})
export class CallbackPipe extends AppComponentBase implements PipeTransform {

    constructor(
        injector: Injector,
      ) {
        super(injector);
      }

      
    transform(items: any[], callback: (item: any) => boolean): any {
        if (!items || !callback) {
            return items;
        }
        return items.filter(item => callback(item));
    }
}