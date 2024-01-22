import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  public setItem(key: string, value: string): void {
    localStorage.setItem(key, value);
  }

  public setObjectItem<TObject>(key: string, value: TObject): void {
    const objString = JSON.stringify(value);
    this.setItem(key, objString);
  }

  public getItem(key: string): string {
    return localStorage.getItem(key);
  }

  public getObjectItem<TObject>(key: string): TObject {
    const objString = localStorage.getItem(key);
    const obj: TObject = JSON.parse(objString);
    return obj;
  }

  public clearItem(): void {
    localStorage.clear();
  }
}
