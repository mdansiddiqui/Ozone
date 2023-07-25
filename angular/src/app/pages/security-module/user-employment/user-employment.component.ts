
//T.S

import { ActivatedRoute} from '@angular/router';



//for test file
import { HttpClient, HttpEventType, HttpHeaders, HttpResponse } from "@angular/common/http";
//import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
//import { AppConsts } from "../AppConsts";
//import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import { TagContentType } from '@angular/compiler';
import { Content } from '@angular/compiler/src/render3/r3_ast';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { create } from 'lodash-es';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { RoleFormService } from '@shared/Services/sec-role-service'
import { RoleFormModel } from '@shared/Dto/role-form-model';
import { Router } from '@angular/router';
import { SecRoleFormModel } from '@shared/Dto/sec-role-form-model';
import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';


import {UserStandardService} from 'shared/Services/User-Standard-service';

import { ToastrService } from 'ngx-toastr';
import { DxListModule } from "devextreme-angular";
import {  DxDataGridModule,DxDataGridComponent,DxSpeedDialActionModule,DxSelectBoxModule} from 'devextreme-angular';
import { DxTreeListModule } from "devextreme-angular";
import { Employee, EmployeesService } from '@app/pages/sales/employees.service';
import DataSource from 'devextreme/data/data_source';
import { BrowserModule } from '@angular/platform-browser';
import { DxContextMenuModule } from "devextreme-angular";
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import dxNumberBox from 'devextreme/ui/number_box';
import { DatePipe } from '@angular/common';
import{ UserEmploymentModel} from'@shared/Dto/userEmployment-model';
import { SecUserService } from '@shared/Services/sec-user.service';

@Component({
  selector: 'app-user-employment',
  templateUrl: './user-employment.component.html',
  styleUrls: ['./user-employment.component.css']
})
export class UserEmploymentComponent implements OnInit {

  public UserEmployment: UserEmploymentModel = new UserEmploymentModel();
  UserEmploymentForm = new FormGroup({
    // Id: new FormControl(''),
    JobTitle: new FormControl(''),
    Organization: new FormControl(''),
    BusinessScope: new FormControl(''),
    StartYear: new FormControl(''),


    EndYear: new FormControl(''),

    // IsDeleted: new FormControl(''),

  })
  datePipe = new DatePipe("en-US");
  public UserName:string;
  @Input() formName : string
  secRoleForm
  tableSizes = [3, 5, 10, 15, 20, 25];
  public totalCount: number
  public pagedDto: PagedRequestModel = new PagedRequestModel()
  pageNumber : number = 1
  pageSize : number = 10
  public isEditShown : boolean
  public isViewShown : boolean
  public isAddShown : boolean
  public keyword : string = ''
  public OID : number
  public StandardList = [];
  public UserEmploymentList = [];
  public ApprovalList = [];
  public StatusId:number
  submitted = false;


 get f() { return this.UserEmploymentForm.controls; }
  fileToUpload: any;

 public UserStatusList=[]

  readonly allowedPageSizes = [5, 10, 'all'];
  readonly displayModes = [{ text: "Display Mode 'full'", value: "full" }, { text: "Display Mode 'compact'", value: "compact" }];
  displayMode = "full";
  showPageSizeSelector = true;
  showInfo = true;
  showNavButtons = true;
  customizeColumns(columns) {
      columns[0].width = 70;
  }
  get isCompactMode() {
      return this.displayMode === "compact";
  }

  constructor(
  //  private http: HttpClient,
    private _UserStandardService: UserStandardService,
    // private route: Router,
     private _toster: ToastrService,
     public SecUserService: SecUserService,
     private router: Router,
     private route: ActivatedRoute,
    private _makerAuthorizerFormService: MakerAuthorizerFormService
     //public StandardService: StandardService
    )
    {    this.edit = this.edit.bind(this);
      this.delete = this.delete.bind(this);
      this.editRecord=this.editRecord.bind(this);
     }

  ngOnInit(): void {


   //this.loadApprovalStatus()
   //this.loadContractType()

    //this.onSearch();

  }
  ngAfterViewInit() : void {
    this.editUser()

  }
  // editVsible(e) {
  //   debugger
  //   var organizationId =  parseInt( localStorage.getItem('organizationId'));
  //   // console.log(roleId)
  //   let oid = parseInt(localStorage.getItem('UserOrganizationID'));
  //   if (organizationId === oid)
  //    {
  //    return !e.row.isEditing;
  //  }else {
  //   return e.row.isEditing;
  //  }

  //  }
  editVsible(e) {
    debugger
    var organizationId =  parseInt( localStorage.getItem('organizationId'));
    // console.log(roleId)
    let oid = parseInt(localStorage.getItem('UserOrganizationID'));
    var userstatusId =  parseInt( localStorage.getItem('userstatusId'));
    if(userstatusId==2)
    {
     return e.row.isEditing;
    }
    if (organizationId === oid)
     {
     return !e.row.isEditing;
   }else {
    return e.row.isEditing;
   }
   }
  Userid: number
  editUser()
  {

    var ur = window.location.href.split("/")[7];
    var com = ur.split("?")[1];

  if (com != undefined && com != null) {
    var PId = com.split("=")[0]
    // var org = com.split("&")[1]
    // var oid = org.split("=")[1]
    // this.OID=parseInt(oid);
    // var params = new URLSearchParams(com);
    // // var NId = params.get("NId");
    // var OId = params.get("OrganzationId");
    //   console.log(PId);
    //   console.log(OId);
    //   this.OID = +OId
    this.Userid= +PId;
    // localStorage.removeItem('UserOrganizationID');
    // localStorage.setItem('UserOrganizationID', this.OID.toString());
      this.SecUserService.GetUserbyId(this.Userid).subscribe(data => {
        this.UserName  = data.userName
        this.StatusId=data.approvelStatusId;
        this.OID=data.organizationId;
        localStorage.removeItem('UserOrganizationID');
        localStorage.setItem('UserOrganizationID', this.OID.toString());
        localStorage.removeItem('userstatusId');
        localStorage.setItem('userstatusId', this.StatusId.toString());

      })
      this.onSearch();

     var testid= this.route.snapshot.params.id;
    // this._UserStandardService.GetUserEmployment(this.Userid).subscribe(data => {

    //   this.UserEmploymentList= data

    // })
  //  this.onSearch(this.userUpdateId);
  }

  }


  onSubmit(): void {



    this.submitted = true;

    // stop here if form is invalid
    if (this.UserEmploymentForm.invalid) {
      abp.message.error("Some fields are required ");
      return;
    }
    this.UserEmployment= new UserEmploymentModel();




    if (this.id != undefined && this.id != null && this.id >0) {
      this.UserEmployment.Id=this.id;
    }
    this.UserEmployment.JobTitle=this.UserEmploymentForm.get('JobTitle').value
    this.UserEmployment.Organization=this.UserEmploymentForm.get('Organization').value
    this.UserEmployment.BusinessScope=this.UserEmploymentForm.get('BusinessScope').value
    this.UserEmployment.StartYear=parseInt(this.UserEmploymentForm.get('StartYear').value)
    this.UserEmployment.EndYear=parseInt(this.UserEmploymentForm.get('EndYear').value)





  var LoginUserId =localStorage.getItem('userId');
   this.UserEmployment.CreatedBy=parseInt(LoginUserId)
   this.UserEmployment.UserId=this.Userid



      this._UserStandardService.CreateUserEmployment(this.UserEmployment).subscribe((Response)=>{

    abp.message.info(Response.message)
    this.reloadGrid();
    this.NewRecord();
   })
}


id: number
  edit(e) {

    // var List = [];
    // List=this.Liststandard                                                                             ;
    // this.router.navigateByUrl('/app/pages/stock-management/library');
    this.id=e.row.data.id
    // var updateDate =this.StandardList.find(x => x.id == this.id );

    // this._StandardService.GetStandardById(this.id).subscribe((res) =>
    // {

        this.UserEmploymentForm.controls.JobTitle.setValue(e.row.data.jobTitle);
        this.UserEmploymentForm.controls.Organization.setValue(e.row.data.organization);
        this.UserEmploymentForm.controls.BusinessScope.setValue(e.row.data.businessScope);
        this.UserEmploymentForm.controls.StartYear.setValue(e.row.data.startYear);
        this.UserEmploymentForm.controls.EndYear.setValue(e.row.data.endYear)



   }


onTableDataChange(event) {
  this.pagedDto.page = event;
  this.onSearch();
}








onTableSizeChange(event): void {
  this.pagedDto.pageSize = event.target.value;
  this.onSearch();
}

onSearch(){


  this.pagedDto.keyword = this.Userid.toString();
  this.pagedDto.authAllowed = true;
  //this.pagedDto.pageSize = 3
  this._UserStandardService.GetUserEmployment(this.pagedDto).subscribe((Response) => {


    this.totalCount = Response.totalCount
    this.UserEmploymentList = Response.userEmploymentModel
    //this .Liststandard=this.StandardList;
  })
}


  reloadGrid()

 {

   this.pagedDto.page =1;
   this.onSearch();
 }

 NewRecord()


 {
  this.UserEmploymentForm.controls.JobTitle.setValue('');
        this.UserEmploymentForm.controls.Organization.setValue('');
        this.UserEmploymentForm.controls.BusinessScope.setValue('');
        this.UserEmploymentForm.controls.StartYear.setValue('');
        this.UserEmploymentForm.controls.EndYear.setValue('');
        this.id=0;
}
delete(e) {

     abp.message.confirm((""),
     undefined,
         (result: boolean) => {
             if (result) {
               // this.SecUserService.Deleteuser(e.row.data.id).subscribe()
               //     abp.message.info("Deleted successfully", "Status", {});

                   this._UserStandardService.DeleteUserEmployment(e.row.data.id).subscribe((Response)=>{

                     abp.message.info(Response.message)
                     this.onSearch();

                    })

             }
           }
      )}

  editRecord(e)
  {

    // var userId=item;
    var urlink=e;
    this.router.navigateByUrl(e+this.Userid)

  }






}
