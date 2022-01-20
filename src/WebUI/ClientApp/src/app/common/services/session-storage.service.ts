import { Injectable } from '@angular/core';

@Injectable({ providedIn: "root" })
export class SessionStorageService {

  private sessionStorage: Storage = sessionStorage;

  setData(key: string, data: any) {
    this.sessionStorage.setItem(key, JSON.stringify(data));
  }

  getData(key: string) {
    return this.sessionStorage.getItem(key);
  }

  removeData(key: string) {
    this.sessionStorage.removeItem(key);
  }
}
