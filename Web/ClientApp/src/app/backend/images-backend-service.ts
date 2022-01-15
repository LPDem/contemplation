import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class ImagesBackendService {

  readonly baseUrl: string = '/api/images';

  constructor(
    private httpClient: HttpClient
  ) {
  }

  getAll(): Observable<string[]> {
    return this.httpClient.get<string[]>(`${this.baseUrl}`);
  }

  getFullImageUrl(name: string, width: number, height: number): string {
    return `${this.baseUrl}/${name}?w=${width}&h=${height}`;
  }

}
