import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
import { AppConsts } from "../AppConsts";
import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import {HttpResponse} from '@angular/common/http';
import { environment } from "environments/environment";
import { result } from "lodash";
import { AgencyModel } from "../Dto/Agency-model";

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


@Injectable({
    providedIn: 'root',
    
  })

  export class GeneralFormService 
  {

    private baseUrl: string;

    
    $isDataLoaded = new EventEmitter();
  
    constructor(
      private http: HttpClient
    ) { }
    private data = new BehaviorSubject<any>(Response);
  
     
    ProjectGeneralFormCreateWithFile(values): Observable<any> 
   
    {   
      
      let headers = new HttpHeaders({
        //'Content-Type': 'application/json',
      'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
       
    });
    let options = { headers: headers };
       return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/ProjectGeneralFormCreateWithFile`,values,options)
     
    }
    
   
    GetAllVerificationType() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetAllVerificationType`,options) 

       }
       GetAllProjectType() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetAllProjectType`,options) 

       }
     
       GetALLServicesType() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLServicesType`,options) 

       }
       GetALLMethodology() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLMethodology`,options) 
       }
       GetALLRequestOfSite() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLRequestOfSite`,options) 
       }
       
       GetALLAssessmentCompleted() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLAssessmentCompleted`,options) 
       }
       
       GetALLAccreditation() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLAccreditation`,options) 
       }
       GetALLCompletedModule() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLCompletedModule`,options) 
       }
       GetALLCompletedSetup() : Observable<any> {
 
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLCompletedSetup`,options) 
       }
       GetAllClientSites(id:number) : Observable<any> {
 debugger
        let headers = new HttpHeaders({
            
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
         
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetAllClientSites?id=${id}`,options) 
       }

       GetAllSmetaAuditPillars() : Observable<any> {
 
   
        let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
          return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetAllSmetaAuditPillars`,options) 
       }
    
       GetProjectGeneralFormBYId(id)
       {
         
         let headers = new HttpHeaders({
           'Content-Type': 'application/json',
           'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
       });
       let options = { headers: headers };
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetProjectGeneralFormBYId?id=${id}`,options);
            }
    

   
     
     
    
  
    downloadApplicationForm(id:number)
    {
      
      return this.http.get(`${environment.apiUrl}/api/ProjectGeneralForm/DownloadApplicationForm?id=${id}`,{responseType:'blob'})
    
    }
    

    GetProjectRemarks(value): Observable<any> {
       
    let headers = new HttpHeaders({
       'Content-Type': 'application/json',
       'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };
    
    return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetPagedPrjectRemarks`,value,options);
    // return this.http.post<any>(`${environment.apiUrl}​/api​/Standard​/GetPagedStandard`, value, options);
       
    }
    
    GetALLProjectStatus(id)
    {
      
      let headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };
     return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLProjectStatus?id=${id}`,options);
         }

         ApprovedProject(values): Observable<any> 
   
         { 
          let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
    
            return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/CreateApproval`,values)
          
         }


         SubmitForReview(id: number) : Observable<any>
   
         {   

          var loginUserId =localStorage.getItem('userId');
         
           let headers = new HttpHeaders({
             
             'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
         });
         let options = { headers: headers };
         
          // var Id =localStorage.getItem('organizationId');
         return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/SubmitForReview?id=${id}`,parseInt(loginUserId),options);
         }

         DeleteProject(id) { 
          let headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
              });
          let options = {headers : headers};
          
          return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/ProjectGeneralFormDeleteById?id=${id}`,options)
          
        }

        GetALLModuleVersion(id) : Observable<any> {
 
            let headers = new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                  });
              let options = {headers : headers};
             
            return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLModuleVersion?id=${id}`,options) 
           }
           GetALLModuleShare() : Observable<any> {
 
            let headers = new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                  });
              let options = {headers : headers};
             
            return this.http.get<any>(`${environment.apiUrl}/api/ProjectGeneralForm/GetALLModuleShare`,options) 
           }


           GetAllLeadAuditor(values) : Observable<any> {
            
                   let headers = new HttpHeaders({
                       
                       'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                         });
                     let options = {headers : headers};
                    
                   return this.http.post<any>(`${environment.apiUrl}/api/Client/GetAuditorByStandardId`,values,options) 
                  }
                  GetLeadAllLeadAuditor(values) : Observable<any> {
            
                    let headers = new HttpHeaders({
                        
                        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                          });
                      let options = {headers : headers};
                     
                    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetLeadAuditorByStandardId`,values,options) 
                   }
                  GetReviewerByStandardId(values) : Observable<any> {
            
                    let headers = new HttpHeaders({
                        
                        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                          });
                      let options = {headers : headers};
                     
                    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetReviewerByStandardId`,values,options) 
                   }
                   GetReviewerByStandard(values) : Observable<any> {
 
                    let headers = new HttpHeaders({
                        'Content-Type': 'application/json',
                        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                          });
                      let options = {headers : headers};
                     
                      return this.http.post<any>(`${environment.apiUrl}/api/Client/GetReviewerByStandard`,values,options)  
                   }
                  ContarctsSubmit(values): Observable<any> 
   
                  {    let headers = new HttpHeaders({
                  
                    'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                     
                  });
                  let options = { headers: headers };
                     return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/ContractSubmit`,values,options)
                   
                  }
                  
                  ContractApproval(values): Observable<any> 
   
                  {    let headers = new HttpHeaders({
                  
                    'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
                     
                  });
                  
                  let options = { headers: headers };
                     return this.http.post<any>(`${environment.apiUrl}/api/ProjectGeneralForm/ContractApproval`,values,options)
                   
                  }

                  downloadContract(id:number)
    {
      
      return this.http.get(`${environment.apiUrl}/api/ProjectGeneralForm/DownloadContract?id=${id}`,{responseType:'blob'})
    
    }

    getAllServices() : Observable<any> {
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
        let options = {headers : headers};
        return this.http.get<any>(`${environment.apiUrl}/api/ProjectAmount/getAllAgency`,options) 
       }
    
  }