import { Injectable } from '@angular/core';
import jwt_decode from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class UserInfoService {
  getToken(): string {
    return localStorage.getItem('derat-token');
  }

  setToken(token: string): void {
    localStorage.setItem('derat-token', token);
  }

  removeToken(): void {
    localStorage.removeItem('derat-token');
  }

  getUserName(): string {
    const decoded = jwt_decode(this.getToken());
    return decoded['derat-user-name'];
  }

  getUserRole(): string {
    const token = this.getToken();

    if (token) {

      const decoded = jwt_decode(this.getToken());
      return decoded['derat-user-role'];
    }

    return null;
  }

  isEmployee(): boolean {
    return this.getUserRole() === 'EMPLOYEE';
  }

  isProvider(): boolean {
    return this.getUserRole() === 'PROVIDER';
  }
}
