import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
import { AppConsts } from "../AppConsts";
import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import {HttpResponse} from '@angular/common/http';
import { environment } from "environments/environment";
import { result } from "lodash";




export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL')




@Injectable({
    providedIn: 'root',
    
  })

  export class FileUploadingService 
  {

    private baseUrl: string;

    private REST_API_SERVER = AppConsts.remoteServiceBaseUrl;
    $isDataLoaded = new EventEmitter();
  
    constructor(
      private http: HttpClient
    ) { }
    private data = new BehaviorSubject<any>(Response);


    create(values): Observable<any> 
   
    {    
      
      debugger
      let headers = new HttpHeaders({
    
      'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
       
    });
    let options = { headers: headers };
       return this.http.post<any>(`${environment.apiUrl}/api/FileUploading/Create`,values,options)
     
    }

    GetFileUploadList(id:number,value): Observable<any> {
      debugger
      let headers = new HttpHeaders({
        "Content-Type": "application/json",
        Authorization: "Bearer " + JSON.parse(localStorage.getItem("token")),
      });
      let options = { headers: headers };
  
      // var id = parseInt(localStorage.getItem("organizationId"));
      return this.http.post<any>(
        `${environment.apiUrl}/api/FileUploading/GetPagedFiles?id=${id}`,
        value,
        options
      );
    }


    downloadFile(id:number)
      {
        let headers = new HttpHeaders({
         
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
        return this.http.get(`${environment.apiUrl}/api/FileUploading/download?id=${id}`,{responseType:'blob'})
     
      }
      Delete(id) { 
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
        let options = {headers : headers};
        
        return this.http.post<any>(`${environment.apiUrl}/api/FileUploading/FileDeleteById?id=${id}`,options)
        
      }

  }
