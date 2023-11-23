import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { result } from "lodash";
import { AppConsts } from "../AppConsts";
import { BehaviorSubject, Observable } from "rxjs";
import {HttpResponse} from '@angular/common/http';
import { environment } from "../../environments/environment.prod";
import { PagedRequestModel } from "@shared/Dto/paged-Request-model";
import { AuditDocumentsModel } from "../Dto/Audit-Documents-model";


export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


@Injectable({
    providedIn: 'root',

  })

  export class AuditorDocumentsService
  {
    Get(value): Observable<any> {
        
      let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
      return this.http.post<any>(`${environment.apiUrl}/api/AuditDocumentType/GetPagedAuditDocumentType`, value, options);
          
    }

   
    private baseUrl: string;

    private REST_API_SERVER = AppConsts.remoteServiceBaseUrl;
    $isDataLoaded = new EventEmitter();
  
    constructor(
      private http: HttpClient
    ) { }

    create(body: AuditDocumentsModel | undefined) : Observable<any> {
        
      const content_ = JSON.stringify(body);
   
      let headers = new HttpHeaders({
       "Content-Type": "application/json",
       
       'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
         Accept: "text/plain",     
     });
    let options = { headers: headers };
    // return this.http.post(this.REST_API_SERVER + '/api/services/app/PurchaseOrderService/CreatePurchaseOrder',content_) 
        return this.http.post<any>(`${environment.apiUrl}/api/AuditDocumentType/Create`,content_,options) 
      }
      Delete(id) {


        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
        let options = {headers : headers};

        return this.http.post<any>(`${environment.apiUrl}/api/AuditDocumentType/AuditDocumentTypeDeleteById?id=${id}`,options)

      }
      getAuditDocumentTypeById(id){
        let headers=new HttpHeaders({'Content-type':'application/json','Authorization':'Bearer' + JSON.parse(localStorage.getItem('token')),})
        let options={headers:headers};

        return this.http.get<any>(`${environment.apiUrl}/api/AuditDocumentType/getAuditDocumentTypeById?id=${id}`,options);
      }

 
    }