import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import LoginDto from '../types/LoginDto';
import TokenDto from '../types/TokenDto';
import RegisterDto from '../types/RegisterDto';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  baseUrl = 'https://localhost:7293/api/Users/';
  public isAuthenticated$ = new BehaviorSubject<boolean>(false);

  constructor(private readonly clinet: HttpClient) {}

  public login(credentials: LoginDto): Observable<TokenDto> {
    return this.clinet.post<TokenDto>(this.baseUrl + 'login', credentials).pipe(
      tap((token) => {
        sessionStorage.setItem('token', token.token);
        sessionStorage.setItem('expiry', token.expiry);
        this.isAuthenticated$.next(true);
      })
    );
  }

  public register(registerdto: RegisterDto): Observable<any> {
    return this.clinet.post<any>(this.baseUrl + 'register', registerdto);
  }
}
