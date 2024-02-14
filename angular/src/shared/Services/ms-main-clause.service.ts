import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class MsMainClauseService {

  constructor(
    private http: HttpClient
  ) { }

  submitData(obj:any){
debugger
    let headers = new  HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),

    });
    let options= {headers:headers}

    return this.http.post<any>(`${environment.apiUrl}/api/MainClause/Create`,obj,options)
  }
  Get(value){
        
    let headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };
    return this.http.post<any>(`${environment.apiUrl}/api/MainClause/GetPagedMainClause`, value, options);
        
  }

  Delete(id:any) { 
    debugger  
    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
    let options = {headers : headers};
    
    return this.http.post<any>(`${environment.apiUrl}/api/MainClause/MainClauseDeleteById?id=${id}`,options)
  }

}
