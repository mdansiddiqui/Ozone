import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MsSubClauseService {

  constructor(
    private http:HttpClient
  ) { }

  submitData(obj:any){
    debugger
        let headers = new  HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    
        });
        let options= {headers:headers}
    
        return this.http.post<any>(`${environment.apiUrl}/api/SubClause/Create`,obj,options)
      }

      Get(value){
        debugger
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
        return this.http.post<any>(`${environment.apiUrl}/api/SubClause/GetPagedSubClause`, value, options);
            
      }

      Delete(id:any) { 
        debugger  
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
        let options = {headers : headers};
        
        return this.http.post<any>(`${environment.apiUrl}/api/SubClause/SubClauseDeleteById?id=${id}`,options)
      }

      GetMainClause(id)
      {
        debugger
        let headers = new HttpHeaders({
          'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
        return this.http.get<any>(`${environment.apiUrl}/api/SubClause/GetMainClause?id=${id}`, options)
      }
}
