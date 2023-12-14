import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
import { AppConsts } from "../AppConsts";
import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import {HttpResponse} from '@angular/common/http';
import { environment } from "../../environments/environment.prod";

import { result } from "lodash";
import { AgencyModel } from "../Dto/Agency-model";


export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


@Injectable({
    providedIn: 'root',

  })

  export class AuditorService
  {

    private baseUrl: string;

    private REST_API_SERVER = AppConsts.remoteServiceBaseUrl;
    $isDataLoaded = new EventEmitter();

    constructor(
      private http: HttpClient
    ) { }
    private data = new BehaviorSubject<any>(Response);


    Get(value): Observable<any> {

        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
        return this.http.post<any>(`${environment.apiUrl}/api/AuditorReports/GetPagedAuditorReportsResponse`, value, options);

      }

      GetAuditorReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetAuditorReports',values,options);
      }

      GetAuditorReportDetails(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetAuditorReportsDetails',values,options);
      }
      GetClientProjectReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetClientProjectReport',values,options);
      }


      GetAuditorListReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetAuditorListReport',values,options);
      }

      GetAuditorScheduleReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetAuditorScheduleReport',values,options);
      }
      GetScheduleOfAuditReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetScheduleOfAuditReport',values,options);
      }
      GetImpartialityReviewReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetImpartialityReviewReport',values,options);
      }
      GetAuditorReportHistory(id): Observable<any> {

        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
        return this.http.get<any>(`${environment.apiUrl}/api/AuditorReports/AuditorReportsHistory?id=${id}` , options);

      }

      AuditNCS(id): Observable<any> {
debugger
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
        return this.http.get<any>(`${environment.apiUrl}/api/AuditorReports/AuditNCS?id=${id}` , options);

      }
      
      GetCertifiedClientReport(values: any):Observable<any>
      {
        ;debugger
        let headers = new HttpHeaders({
          'Content-Type':'application/json'
        });
        let options={headers:headers};
        return this.http.post(this.REST_API_SERVER+'/api/AuditorReports/GetCertifiedClientReport',values,options);
      }
  }
