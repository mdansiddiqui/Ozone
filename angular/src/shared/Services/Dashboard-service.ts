import { result } from "lodash";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
import { AppConsts } from "../AppConsts";
import { BehaviorSubject, Observable } from "rxjs";
import { HttpResponse } from '@angular/common/http';
import { environment } from "../../environments/environment.prod";
//import { AccreditationModel } from "../Dto/Accreditation-model";


export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

   
@Injectable({
  providedIn: 'root',
       
})

export class DashboardService {

  private baseUrl: string;

  private REST_API_SERVER = AppConsts.remoteServiceBaseUrl;
  $isDataLoaded = new EventEmitter();

  constructor(
    private http: HttpClient
  ) { }

  GetDashboardData(id, value): Observable<any> {

    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };
    return this.http.post<any>(`${environment.apiUrl}/api/Dashboard/GetDashboardData?id=${id}`, value, options);
  }

  GetAllProjects(id: number, value): Observable<any> {

debugger
    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };

    //  var id =localStorage.getItem('organizationId');
    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetPagedAllProject?id=${id}`, value, options);

  }
  GetAllAudits(id, value): Observable<any> {


    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };

    //  var id =localStorage.getItem('organizationId');
    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetPagedAllAudits?id=${id}`, value, options);

  }

  GetAllClientChange(id, value): Observable<any> {


    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };

    //  var id =localStorage.getItem('organizationId');
    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetPagedAllClientChange?id=${id}`, value, options);

  }


  GetAllProjectChangeRequest(id, value): Observable<any> {


    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };

    //  var id =localStorage.getItem('organizationId');
    return this.http.post<any>(`${environment.apiUrl}/api/ProjectSA/GetAllProjectChangeRequest?id=${id}`, value, options);

  }


  GetAllClientSitesChangeRequest(id, value): Observable<any> {


    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };

    //  var id =localStorage.getItem('organizationId');
    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetAllClientSitesChangeRequest?id=${id}`, value, options);

  }



  GetClientChangeDataById(id: number, value): Observable<any> {

debugger
    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
    });
    let options = { headers: headers };

    //  var id =localStorage.getItem('organizationId');
    return this.http.post<any>(`${environment.apiUrl}/api/Client/GetClientChangeDataById?id=${id}`, value, options);

  }


  GetClientSitesChangeDataById(id: number, value): Observable<any> {

    debugger
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
    
        //  var id =localStorage.getItem('organizationId');
        return this.http.post<any>(`${environment.apiUrl}/api/Client/GetClientSitesChangeDataById?id=${id}`, value, options);
    
      }


  GetProjectChangeDataById(id: number, value): Observable<any> {

    debugger
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
    
        //  var id =localStorage.getItem('organizationId');
        return this.http.post<any>(`${environment.apiUrl}/api/ProjectSA/GetProjectChangeDataById?id=${id}`, value, options);
    
      }

  GetClientChangeRemarksDataById(id: number): Observable<any> {

    debugger
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
    
        //  var id =localStorage.getItem('organizationId');
        return this.http.post<any>(`${environment.apiUrl}/api/Client/GetClientChangeRemarksDataById?id=${id}`, options);
    
      }

      GetClientSitesChangeRemarksDataById(id: number): Observable<any> {

        debugger
            let headers = new HttpHeaders({
              'Content-Type': 'application/json',
              'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
            let options = { headers: headers };
        
            //  var id =localStorage.getItem('organizationId');
            return this.http.post<any>(`${environment.apiUrl}/api/Client/GetClientSitesChangeRemarksDataById?id=${id}`, options);
        
          }

      GetProjectSA8000RemarksDataById(id: number): Observable<any> {

        debugger
            let headers = new HttpHeaders({
              'Content-Type': 'application/json',
              'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
            let options = { headers: headers };
        
            //  var id =localStorage.getItem('organizationId');
            return this.http.post<any>(`${environment.apiUrl}/api/ProjectSA/GetProjectSA8000RemarksDataById?id=${id}`, options);
        
          }
  
  ApprovedClientChange(value): Observable<any> {

    debugger
        let headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
        });
        let options = { headers: headers };
    
        //  var id =localStorage.getItem('organizationId');
        return this.http.post<any>(`${environment.apiUrl}/api/Client/ApprovedClientChange`, value);
    
      }


      ApprovedClientSitesChange(value): Observable<any> {

        debugger
            let headers = new HttpHeaders({
              'Content-Type': 'application/json',
              'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
            let options = { headers: headers };
        
            //  var id =localStorage.getItem('organizationId');
            return this.http.post<any>(`${environment.apiUrl}/api/Client/ApprovedClientSitesChange`, value);
        
          }

      ApprovedProjectChange(value): Observable<any> {

        debugger
            let headers = new HttpHeaders({
              'Content-Type': 'application/json',
              'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('token')),
            });
            let options = { headers: headers };
        
            //  var id =localStorage.getItem('organizationId');
            return this.http.post<any>(`${environment.apiUrl}/api/ProjectSA/ApprovedProjectChange`, value);
        
          }
      

      downloadFile(id:number)
      {
        let headers = new HttpHeaders({
         
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
        return this.http.get(`${environment.apiUrl}/api/Client/download?id=${id}`,{responseType:'blob'})
     
      }

      
      ProjectChangeRequestDownloadfile(id:number)
      {
        let headers = new HttpHeaders({
         
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
   
        return this.http.get(`${environment.apiUrl}/api/Dashboard/ProjectChangeRequestDownloadfile?id=${id}`,{responseType:'blob'})
     
      }

      oldValuesDownloadfile(id:number)
      {
        let headers = new HttpHeaders({
         
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
   
        return this.http.get(`${environment.apiUrl}/api/Dashboard/oldValuesDownloadfile?id=${id}`,{responseType:'blob'})
     
      }
      NewValuesDownloadfile(id:number)
      {
        let headers = new HttpHeaders({
         
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
   
        return this.http.get(`${environment.apiUrl}/api/Dashboard/NewValuesDownloadfile?id=${id}`,{responseType:'blob'})
     
      }

      ClientSitesdownloadFile(id:number)
      {
        let headers = new HttpHeaders({
         
          'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('token')),
      });
      let options = { headers: headers };
        return this.http.get(`${environment.apiUrl}/api/Client/ClientSitesdownloadFile?id=${id}`,{responseType:'blob'})
     
      }


}
