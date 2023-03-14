// import { Component, OnInit } from '@angular/core';
import { ProjectAmountReportsService } from 'shared/Services/Project-Amount-Reports-service';
import { SecUserService } from '@shared/Services/sec-user.service';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { create } from 'lodash-es';
import { FormControl, FormGroup } from '@angular/forms';

import { RoleFormService } from '@shared/Services/sec-role-service'
import { RoleFormModel } from '@shared/Dto/role-form-model';
import { Router } from '@angular/router';
import { SecRoleFormModel } from '@shared/Dto/sec-role-form-model';
import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

import { FileUploadingService } from '@shared/Services/FileUploading-service';
import { LibraryResourceModel } from '@shared/Dto/Library-Resource_model';
import { ActivatedRoute} from '@angular/router';
import { DatePipe } from '@angular/common';



//for test file
import { HttpClient, HttpEventType, HttpHeaders, HttpResponse } from "@angular/common/http";
//import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
//import { AppConsts } from "../AppConsts";
//import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import { ToastrService } from 'ngx-toastr';
import { TagContentType } from '@angular/compiler';
import { Content } from '@angular/compiler/src/render3/r3_ast';
import { Employee } from '@app/pages/sales/employees.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';


@Component({
  selector: 'app-file-uploading',
  templateUrl: './file-uploading.component.html',
  styleUrls: ['./file-uploading.component.css']
})
export class FileUploadingComponent implements OnInit {
  public OrganizationList = [];
  public UsersList = [];
  @ViewChild('content',{static: false}) content : ElementRef;
  employees: Employee[] = [];
  public id: number
  public url:string
  message: string;
  progress: number;
  pipe = new DatePipe('en-US');
  datePipe = new DatePipe("en-US");
  formName : string
  public isAddShown : boolean
   secRoleForm
   public isViewShown : boolean  
   fileToUpload: File;
   public pagedDto: PagedRequestModel = new PagedRequestModel()
   public totalCount: number
   public UserRemarksList = [];
   public FileupoadList = [];

 // public item: LibraryResourceModel = new LibraryResourceModel();
  FileUploading = new FormGroup({
    Id: new FormControl(''),
    ParentAgencyId: new FormControl(''),
    Description: new FormControl(''),
    FormUser: new FormControl(''),
    ParentUserId: new FormControl(''),
     File: new FormControl(''),
   
  })

  constructor(
   
    public SecUserService: SecUserService,
    private http: HttpClient,
    private _makerAuthorizerFormService: MakerAuthorizerFormService,

    private _FileUploadingService: FileUploadingService, 
   // private route: Router,
    private _toster: ToastrService,
    private router: Router,
    private route: ActivatedRoute,

  ) { 

    this.Downloadfile = this.Downloadfile.bind(this);
    this.delete=this.delete.bind(this);
  }



  ngOnInit(): void {
    this.FileUploading.controls.FormUser.setValue(localStorage.getItem('userName').replace(/['"]+/g, ''));
  
     this.loadAllAgency();
  this.fileuploadList();

    this.loadSecRoleForm()
     


}

onTableDataChange(event) {
  this.pagedDto.page = event;
  this.fileuploadList();
}

reloadGrid()

{

  this.pagedDto.page =1;
  this.fileuploadList();
}

loadAllAgency(): void {
  debugger
      this.SecUserService.getAllAgency().subscribe((Response) => {
        this.OrganizationList = Response
      })     
    }   

fileuploadList(){
  debugger 

  this.FileupoadList=[];
  this.totalCount = 0;
var userid = localStorage.getItem("userId");

console.log(userid);

 var PageNo=this.pagedDto.page;

 debugger
 this._FileUploadingService.GetFileUploadList(parseInt(userid), this.pagedDto).subscribe((Response) => {

  debugger
  this.FileupoadList = Response.fileUploadingModel
  this.totalCount = Response.totalCount
  
})

}



onSearch(){
  this.pagedDto.page = 1;
  this.fileuploadList();
  
   }


   loadAllUsers(organizationId): void {
    debugger
        this.SecUserService.getAllUsers(organizationId).subscribe((Response) => {
          this.UsersList = Response
          
        })    
      }


      loadSecRoleForm() {
        debugger
    this.formName = "LibraryResources"
    this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {
        
      let formName = (this.formName == undefined ? localStorage.getItem('formName') : this.formName)
        
      this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)
  
    //this.isEditShown= this.secRoleForm.authAllowed
    // this.isViewShown = this.secRoleForm.authAllowed

    // var formName = "User"
    // this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {
    //   this.secRoleForm = data.find(x => x.formName.toLocaleLowerCase() == formName.toLocaleLowerCase())
      if(this.secRoleForm.viewRecordAllowed == true )
      {
        this.isViewShown = true
       
        if(this.secRoleForm.insertAllowed==true)
        {
            this.isAddShown = true
        }
            else
            {
              
              this.isAddShown = false
              //abp.message.info("Not Allowed")
        //this.reloadGrid();
        //this.router.navigateByUrl('/app/home');
            }
      }
      else{
        this.isViewShown = false
        // abp.message.info("Not Allowed")
        // //this.reloadGrid();
        // this.router.navigateByUrl('/app/home');
       
       
      }
      //this.isViewShown = this.secRoleForm.authAllowed
    })
      
  } 


  Downloadfile(e): void {
    debugger
        this.id=e.row.data.id;
        var fullpath=e.row.data.filePath;
       var filename =  fullpath.replace(/^.*[\\\/]/, '')
     
        this._FileUploadingService.downloadFile(this.id).subscribe((result:Blob)=>{
          const Blb =new Blob([result], { type: result.type });
    
          const a = document.createElement('a');
            a.setAttribute('style', 'display1:none;');
            document.body.appendChild(a);
           a.download =filename;  
         
            a.href = URL.createObjectURL(Blb);
            a.target = '_blank';
            a.click();
            document.body.removeChild(a);
            
        })
      }


  handlefileInput(e: any)
  {

this.fileToUpload= <File>e?.target?.files[0];
//this.url=e.target.value; 


  }

  fileChange(event, item) {
    
    item.binary = event;
    var r = new FileReader();
    r.onload = function(e) { 
      item.binary = r.result
    }
    r.readAsArrayBuffer(event.target.files[0]);
  }
  // editIconClick(e) {  
  //   this.router.navigateByUrl('/app/pages/stock-management/library/'+ e.row.data.id);  
  //  }  
  deleteItem(id: number) {
    
    abp.message.confirm((""),
    undefined,
        // (result: boolean) => {
        //     if (result) {
        //       this._purchaseOrderService.DeletePO(id).subscribe(() => {   
        //           abp.message.info("Deleted successfully", "Status", {});
        //       });
        //     }
        //   }
     ) }


  onSubmit(): void 
  {
debugger
    

//     var MesseageError="";
// if(this.FileUploading.get('ParentAgencyId').value ==null ||this.FileUploading.get('ParentAgencyId').value==undefined|| this.FileUploading.get('ParentAgencyId').value=="")
// {  abp.message.error("Agency is Empty","Alert")
// return
// }

  //  LBId: String;
 
  const foData:FormData = new FormData();

  // foData.append('ParentAgencyId',this.FileUploading.get('ParentAgencyId').value);
  // if (this.id != undefined && this.id != null && this.id>0) {

  //   foData.append("Id",this.id.toString());
  // }
  // else
  // {
  //   if(this.fileToUpload ==null ||this.fileToUpload==undefined )
  //   {  abp.message.error("Document File is Empty","Alert")
  //   return
  //   // MesseageError="Document File is Empty";
  //   }
  // }

  // console.log(localStorage.getItem("userId"));
  //   var userId = localStorage.getItem('userId');
  //   console.log(userId);
    
  // foData.append("Id",this.id.toString());
  foData.append('AgencyId',this.FileUploading.get('ParentAgencyId').value);


   foData.append('Description',this.FileUploading.get('Description').value);
 
   foData.append('ToUserId',this.FileUploading.get('ParentUserId').value);
 

  foData.append('File',this.fileToUpload);


  var userId = localStorage.getItem('userId');
    // this.Client.CreatorUserId = parseInt(userId);
    foData.append("FromUserId",userId)
 
    this._FileUploadingService.create(foData).subscribe((Response)=>{
      
      abp.message.info(Response.message)

     
      if(Response.message == "Successfully Inserted!"){

        this.FileUploading.reset();
        this.fileuploadList()
      }
     
     })

    }

   


      //this.LibraryResourceService.create(this.item).subscribe((Response)=>{
      
 
 
//  this.LibraryResourceService.PostItemReturnListPagination(formData).subscribe((event) => 
//  {
//   if (event.type === HttpEventType.UploadProgress)
//       this.progress = Math.round((100 * event.loaded) / event.total);
//   else if (event.type === HttpEventType.Response) {
//       this.message = 'Download success.';
//       this.downloadFile(event);
//   }
// });

//}

// private downloadFile(data: HttpResponse<Blob>) {
//   const downloadedFile = new Blob([data.body], { type: data.body.type });
//   const a = document.createElement('a');
//   a.setAttribute('style', 'display:none;');
//   document.body.appendChild(a);
//   a.download =this.fileToUpload ;
//   a.href = URL.createObjectURL(downloadedFile);
//   a.target = '_blank';
//   a.click();
//   document.body.removeChild(a);
// }

deletebtn(e) {

  debugger
   
 // var authorizerID =localStorage.getItem('authorizer');
  //  var tt= this.authorizer;
  var User_Id =localStorage.getItem('userId');
  //  var tt= this.authorizer;
    
   if (parseInt(e.row.data.fromUserId)== parseInt(User_Id))
    {
    return !e.row.isEditing;
    
  }
    else
    {
      
      return e.row.isEditing;
    }
 
  
}
delete(e) {
  debugger
     abp.message.confirm((""),
     undefined,
         (result: boolean) => {
             if (result) {
               // this.SecUserService.Deleteuser(e.row.data.id).subscribe() 
               //     abp.message.info("Deleted successfully", "Status", {});
 
                   this._FileUploadingService.Delete(e.row.data.id).subscribe((Response)=>{
  
                     abp.message.info(Response.message)
                     this.onSearch();
                    
                    })
                   
             }
           }
      )}
}
