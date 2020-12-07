import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Cliente } from '../models/cliente';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {


  constructor(private httpClient: HttpClient) { }

  get(id?: string): Observable<Cliente[]> {
    return this.httpClient.get<Cliente[]>(`${environment.apiUrl}cliente${id ? `/${id}` : ''}`);
  }

  post(cliente: Cliente): Observable<Cliente> {
    return this.httpClient.post<Cliente>(`${environment.apiUrl}cliente`, cliente);
  }

  delete(cliente: Cliente): Observable<any> {
    return this.httpClient.delete<any>(`${environment.apiUrl}cliente/${cliente.id}`);
  }

  put(cliente: Cliente): Observable<Cliente> {
    return this.httpClient.put<Cliente>(`${environment.apiUrl}cliente/${cliente.id}`, cliente);
  }

  save(cliente: Cliente): Observable<Cliente> {
    return cliente.id ? this.put(cliente) : this.post(cliente);
  }
}
