import { result } from "lodash";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
import { AppConsts } from "../AppConsts";
import { BehaviorSubject, Observable } from "rxjs";
import {HttpResponse} from '@angular/common/http';
import { environment } from "../../environments/environment.prod";
import { ClientAuditVisitModel } from "@shared/Dto/Client-Audit-Visit-model";



export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


@Injectable({
    providedIn: 'root',

  })

  export class AuditReportService
  {

    private baseUrl: string;

    private REST_API_SERVER = AppConsts.remoteServiceBaseUrl;
    $isDataLoaded = new EventEmitter();

    constructor(
      private http: HttpClient
    ) { }

   /////User Academic
CreateWithFile(values): Observable<any>

{    let headers = new HttpHeaders({

  'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),

});
let options = { headers: headers };
   return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/CreateAuditReport`,values,options)

}
GetAuditReportBYId(id)
{


 return this.http.get<any>(`${environment.apiUrl}/api/ClientAuditVisit/GetAuditReportById?id=${id}`);
     }

     GetPagedAuditReport(value): Observable<any> {

let headers = new HttpHeaders({
   'Content-Type': 'application/json',
   'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
});
let options = { headers: headers };

return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/GetPagedAuditReport`,value,options);
// return this.http.post<any>(`${environment.apiUrl}​/api​/Standard​/GetPagedStandard`, value, options);

}



GetPagedAuditReportById(id:number,value): Observable<any> {

let headers = new HttpHeaders({
   'Content-Type': 'application/json',
   'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
});
let options = { headers: headers };

return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/GetPagedAuditReportById?id=${id}`,value,options);
// return this.http.post<any>(`${environment.apiUrl}​/api​/Standard​/GetPagedStandard`, value, options);

}
//   private data = new BehaviorSubject<any>(Response);
//   getSecRoleForm() {
//
//   this.data.next(JSON.parse(localStorage.getItem('secRoleForm')))
//   return this.data.asObservable()
// }



AuditReportDeleteById(id) {
let headers = new HttpHeaders({
'Content-Type': 'application/json',
'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
});
let options = {headers : headers};

return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/AuditReportDeleteById?id=${id}`,options)

}
downloadFile(id:number)
{
  return this.http.get(`${environment.apiUrl}/api/ClientAuditVisit/DownloadAuditReport?id=${id}`,{responseType:'blob'})

}

GetAllDocumentsType() : Observable<any> {

  let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
    let options = {headers : headers};

  return this.http.get<any>(`${environment.apiUrl}/api/ClientAuditVisit/GetALLAuditDoucmentsType`,options)
 }

 SubmitForReview(value: any) : Observable<any>

 {

   var userId =parseInt(localStorage.getItem('userId'));
 debugger
   let headers = new HttpHeaders({

     'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
 });
 let options = { headers: headers };

  // var Id =localStorage.getItem('organizationId');
 return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/AuditSubmitForReview`,value,options);
 }

 onStageOne(value: any) : Observable<any>

 {

   var userId =parseInt(localStorage.getItem('userId'));
 debugger
   let headers = new HttpHeaders({

     'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
 });
 let options = { headers: headers };

  // var Id =localStorage.getItem('organizationId');
 return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/onStageOne`,value,options);
 }

  AuditComplete(body: ClientAuditVisitModel | undefined): Observable<any> {

    const content_ = JSON.stringify(body);

    let headers = new HttpHeaders({
        "Content-Type": "application/json",
        'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        Accept: "text/plain",
    });
    let options = { headers: headers };
    // return this.http.post(this.REST_API_SERVER + '/api/services/app/PurchaseOrderService/CreatePurchaseOrder',content_)
    return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/AuditComplete`, content_, options)
}

GetAllAuditReviewerDocuments(id:number): Observable<any> {

  let headers = new HttpHeaders({
     'Content-Type': 'application/json',
     'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
  });
  let options = { headers: headers };
  
  return this.http.get<any>(`${environment.apiUrl}/api/ClientAuditVisit/GetAllAuditReviewerDocuments?id=${id}`,options);
  // return this.http.post<any>(`${environment.apiUrl}​/api​/Standard​/GetPagedStandard`, value, options);
  
  }

  CreateAuditReviewerDocument(values): Observable<any>

{    let headers = new HttpHeaders({

  'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),

});
let options = { headers: headers };
   return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/CreateAuditReviewerDocument`,values,options)

}


DownloadReviewerfile(id:number)
{
  return this.http.get(`${environment.apiUrl}/api/ClientAuditVisit/DownloadAuditReviewerDocuments?id=${id}`,{responseType:'blob'})

}

//Manager
CreateAuditManagerDocument(values): Observable<any>

{    let headers = new HttpHeaders({

  'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),

});
let options = { headers: headers };
   return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/AuditManagerDocumentCreate`,values,options)

}

DownloadManagerfile(id:number)
{
  return this.http.get(`${environment.apiUrl}/api/ClientAuditVisit/DownloadAuditManagerDocuments?id=${id}`,{responseType:'blob'})

}


GetAllAuditManagerDocuments(id:number): Observable<any> {

  let headers = new HttpHeaders({
     'Content-Type': 'application/json',
     'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
  });
  let options = { headers: headers };
  
  return this.http.get<any>(`${environment.apiUrl}/api/ClientAuditVisit/GetAllAuditManagerDocuments?id=${id}`,options);
  // return this.http.post<any>(`${environment.apiUrl}​/api​/Standard​/GetPagedStandard`, value, options);
  
  }



AuditManagerDocumentDeactiveById(id) {
  let headers = new HttpHeaders({
  'Content-Type': 'application/json',
  'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
  });
  let options = {headers : headers};
  
  return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/AuditManagerDocumentDeactiveById?id=${id}`,options)
  
  }
  AuditReviewerDocumentDeleteById(id) {
    let headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = {headers : headers};
    
    return this.http.post<any>(`${environment.apiUrl}/api/ClientAuditVisit/AuditReviewerDocumentDeleteById?id=${id}`,options)
    
    }
  
 
    }
