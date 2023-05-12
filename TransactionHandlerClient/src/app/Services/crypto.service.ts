import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class CryptoService {
  public Encrypt(text: string, key: string, iv: string): string {
    const plaintextBytes = CryptoJS.enc.Utf8.parse(text);
    const keyArray = CryptoJS.enc.Utf8.parse(key);
    const ivArray = CryptoJS.enc.Base64.parse(iv);

    const encryptedBytes = CryptoJS.AES.encrypt(plaintextBytes, keyArray, {
      iv: ivArray,
      mode: CryptoJS.mode.CBC,
    });

    return encryptedBytes.toString();
  }

  public Decrypt(encryptedText: string, key: string, iv: string): string {
    const keyArray = CryptoJS.enc.Utf8.parse(key);
    const ivArray = CryptoJS.enc.Base64.parse(iv);

    const decryptedBytes = CryptoJS.AES.decrypt(encryptedText, keyArray, {
      iv: ivArray,
      mode: CryptoJS.mode.CBC,
    });

    return decryptedBytes.toString(CryptoJS.enc.Utf8);
  }

  public GetIV(): string {
    const iv = CryptoJS.lib.WordArray.random(16);
    const base64String = CryptoJS.enc.Base64.stringify(iv);
    return base64String;
  }
}
