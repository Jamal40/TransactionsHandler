import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import ProcessTransactionResponse from '../types/ProcessTransactionResponse';
import ProcessTransactionRequest from '../types/ProcessTransactionRequest';
import { CryptoService } from './crypto.service';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class TransactionsService {
  baseUrl = 'https://localhost:7293/api/Transactions';
  constructor(
    private readonly clinet: HttpClient,
    private readonly cryptoService: CryptoService
  ) {}

  public submitTransaction(
    request: ProcessTransactionRequest
  ): Observable<ProcessTransactionResponse> {
    request.iv = this.cryptoService.GetIV();

    Object.keys(request)
      .filter((k) => k !== 'iv')
      .forEach((key) => {
        var current = Reflect.get(request, key);
        Reflect.set(
          request,
          key,
          this.cryptoService.Encrypt(
            current,
            environment.encryptionKey,
            request.iv
          )
        );
      });

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${sessionStorage.getItem('token')}`,
    });

    return this.clinet
      .post<ProcessTransactionResponse>(this.baseUrl, request, { headers })
      .pipe(
        map((response) => {
          Object.keys(response)
            .filter((k) => k !== 'iv')
            .forEach((key) => {
              var current = Reflect.get(response, key);
              Reflect.set(
                response,
                key,
                this.cryptoService.Decrypt(
                  current,
                  environment.encryptionKey,
                  response.iv
                )
              );
            });

          return response;
        })
      );
  }
}
