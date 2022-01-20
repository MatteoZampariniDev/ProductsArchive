import { Injectable } from "@angular/core";
import { IdentityClient } from "./web-api-client";

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(private identityClient: IdentityClient) {

  }

  getRole(id: string, onSuccess: (result: string) => void) {
    this.identityClient.getRole(id)
      .subscribe(onSuccess, error => console.error(error));
  }

  getUserId(onSuccess: (result: string) => void) {
    this.identityClient.getUserId()
      .subscribe(onSuccess, error => console.error(error));
  }
}
