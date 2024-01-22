import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataUpdateService<TData> {
  private data: BehaviorSubject<TData> = new BehaviorSubject(null);

  constructor() { }

  getData(): Observable<TData> {
    return this.data.asObservable();
  }

  setData(data: TData) {
      this.data.next(data);
  }
}
