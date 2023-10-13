//import { Component, OnInit } from '@angular/core';
//import { Component, OnInit } from '@angular/core';
import { AfterViewChecked, AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild } from '@angular/core';
// import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { create } from 'lodash-es';
import { FormControl, FormGroup } from '@angular/forms';

import { RoleFormService } from '@shared/Services/sec-role-service'
import { RoleFormModel } from '@shared/Dto/role-form-model';
import { Router } from '@angular/router';
import { SecRoleFormModel } from '@shared/Dto/sec-role-form-model';
import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

import { LibraryResourceService } from '@shared/Services/library-Resource_service';
import { AgencyModel } from '@shared/Dto/Agency-model';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';

import { HttpClient, HttpEventType, HttpHeaders, HttpResponse } from "@angular/common/http";
//import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
//import { AppConsts } from "../AppConsts";
//import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import { ToastrService } from 'ngx-toastr';
import { TagContentType } from '@angular/compiler';
import { Content } from '@angular/compiler/src/render3/r3_ast';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { SecUserService } from '@shared/Services/sec-user.service';
import { ClientService } from '@shared/Services/Client-Service';
import { ClientModel } from '@shared/Dto/Client-model';
import { BrowserModule } from '@angular/platform-browser';
import { UserStandardService } from '@shared/Services/User-Standard-service';
import { SA8000Service } from '@shared/Services/project-SA8000-service';
import { nullSafeIsEquivalent, THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';


@Component({
  selector: 'app-task-board',
  templateUrl: './task-board.component.html',
  styleUrls: ['./task-board.component.css']
})
export class TaskBoardComponent implements OnInit {
  @ViewChild('content', { static: false }) content: ElementRef;

  public pagedDto: PagedRequestModel = new PagedRequestModel()
  public id: number
  public MultisiteList = [];
  public StatusList = [];
  public PrefixList = [];
  public CountryList = [];
  public StateList = [];
  public CityList = [];
  public RiskList = [];
  public EACodeList = [];
  public NaceCodeId = [];
  public NaceCodeList = [];
  public ProjectChangeDetailList = [];
  submitted = false;
  change = false
  public totalCount: number
  formName:string
  secRoleForm
  message: string;
  progress: number;
  pipe = new DatePipe('en-US');
  datePipe = new DatePipe("en-US");
  isAuthAllowed: boolean = false
  isManageAllowed: boolean = false
  isShownDeclineRemarks: boolean = false
  isShownRejectOrAuthButton: boolean = false
  VisibleBtns: boolean = true
  saveBtns: boolean = true
  isMakerButtons: boolean = false
  isDeclineRemarks: boolean = false
  public isShown: boolean = false
  UserMaker: any;
  fileToUpload: File;
  private userUpdateId: number
  public Client: ClientModel = new ClientModel();

  ClientForm = new FormGroup({
    Code: new FormControl(''),
    Name: new FormControl(''),
    Address1: new FormControl(''),
    Address2: new FormControl(''),
    CountryId: new FormControl(''),
    CityId: new FormControl(''),
    StateId: new FormControl(''),
    PostalCode: new FormControl(''),
    PrefixId: new FormControl(''),
    ContactPerson: new FormControl(''),
    Date: new FormControl(''),
    Position: new FormControl(''),
    PhoneNumber: new FormControl(''),
    MobileNumber: new FormControl(''),
    Email: new FormControl(''),
    Website: new FormControl(''),
    Multisite: new FormControl(''),
    IsDeleted: new FormControl(''),
    IsActive: new FormControl(''),
    RiskId: new FormControl(''),
    EacodeId: new FormControl(''),
    NaceCodeId: new FormControl(''),


  })

  ClientEditForm = new FormGroup({
    name: new FormControl(''),
    address1: new FormControl(''),
    countryId: new FormControl(''),
    cityId: new FormControl(''),
    stateId: new FormControl(''),
    postalCode: new FormControl(''),
    email: new FormControl(''),
    prefixId: new FormControl(''),
    contactPerson: new FormControl(''),
    position: new FormControl(''),
  })



  constructor(public SecUserService: SecUserService,

    private _makerAuthorizerFormService: MakerAuthorizerFormService,
    private _UserStandardService: UserStandardService,
    private _SA8000Service: SA8000Service,
    public RoleFormService: RoleFormService,
    public _ClientService: ClientService,
    private router: Router) { }

  ngOnInit(): void {
    const RoleId = parseInt(localStorage.getItem('roleId'));
    console.log(RoleId)
    if(RoleId === 2 || RoleId === 21) {
      this.VisibleBtns = false;
      this.saveBtns= false;
    }
    this.loadSecRoleForm()
    this.loadSecRoleForm()
    //this.loadCities()
    this.loadCountries()
    this.loadPrefix()
    // this.loadState()
    this.loadStatus()
    this.loadMultisite()
    this.loadEaCode();
    this.loadRisk();

  }
  ngAfterViewInit(): void {
    this.editUser()
    this.editUser1()

  }
  loadSecRoleForm() {
    // let secRoleForm = JSON.parse(localStorage.getItem('secRoleForm'))
    // let permission = secRoleForm.find(x => x.formCode != null && x.formCode == this.formCode)

    // this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {

    //   let formName = (this.formName == undefined ? localStorage.getItem('formName') : this.formName)
    //   this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)

   this.formName = "Clients"
    this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {
      this.secRoleForm = data.find(x => x.formName == this.formName)
      this.isShown = this.secRoleForm.authAllowed
      if (this.secRoleForm.manageAllowed == true) {
        this.isManageAllowed = true

      }
      if (this.secRoleForm.authAllowed == false) {


        this.isShownDeclineRemarks = this.secRoleForm.authAllowed
        this.isShownRejectOrAuthButton = true
        this.isMakerButtons = false

        this.isAuthAllowed = false
      }
      else {
        this.isAuthAllowed = true
        this.isMakerButtons = true
        this.isDeclineRemarks = true
      }

    })

  }

  displayStyle = "none"

  onpopup() {
    this.displayStyle = "block"
  }


  closePopup() {
    this.displayStyle = "none"
  }

  editUser1() {
    debugger
    var ur;
    ur = window.location.href.split("/")[7];
    var com = [] = ur.split("?")[1];
    if (com != undefined && com != null) {
      var PId = com.split("=")[0];
      this.id = PId;
      this._ClientService.GeClientDatabyId(this.id).subscribe(data => {

        // this.ProjectChangeDetailList = data
        this.userUpdateId = data.id
        // this.ClientForm.controls.code.setValue(agencyData.code);
        var agencyData = data;
        if (this.userUpdateId != undefined) {

          this.ClientForm.get('Code').setValue(agencyData.code);

          this.ClientEditForm.get('name').setValue(agencyData.name);
          this.ClientEditForm.get('address1').setValue(agencyData.address1);
          this.ClientEditForm.get('countryId').setValue(agencyData.countryId);
          this.loadState(agencyData.countryId);


          this.ClientEditForm.get('cityId').setValue(agencyData.cityId);
          this.ClientEditForm.get('stateId').setValue(agencyData.stateId);
          this.loadCities(agencyData.stateId);
          this.ClientEditForm.get('postalCode').setValue(agencyData.postalCode);
          this.ClientEditForm.get('email').setValue(agencyData.email);

          this.ClientEditForm.get('prefixId').setValue(agencyData.prefixId);
          this.ClientEditForm.get('contactPerson').setValue(agencyData.contactPerson);
          this.ClientEditForm.get('position').setValue(agencyData.position);
        }
        
    
      
      })

    }

  }




  editUser() {
    debugger
    var ur;
    ur = window.location.href.split("/")[7];
    var com = [] = ur.split("?")[1];
    if (com != undefined && com != null) {
      var PId = com.split("=")[0];
      this.id = PId;
      this._ClientService.GeClientDatabyId(this.id).subscribe(data => {

        this.userUpdateId = data.id
        // this.UserMaker =
        var agencyData = data;
        if (this.userUpdateId != undefined) {
          //this.isResetButtonShow=true;
          // this.isResetPassword=false;
          // this.confrmPass = this.UserMaker.ConfirmPassword
          // this.pass = this.UserMaker.password


          //  this.AgencyForm.get('Id').setValue(agencyData.id);
          this.ClientForm.get('Code').setValue(agencyData.code);
          this.ClientForm.get('Name').setValue(agencyData.name);
          this.ClientForm.get('Address1').setValue(agencyData.address1);
          // this.AgencyForm.get('JoiningDate').setValue(agencyData.joiningDate);
          this.ClientForm.get('EacodeId').setValue(agencyData.eacodeId);
          this.loadNaceCode(agencyData.eacodeId);

          this.ClientForm.get('NaceCodeId').setValue(agencyData.naceCodeId);
          debugger
          this.ClientForm.get('RiskId').setValue(agencyData.riskId);

          this.ClientForm.get('Address2').setValue(agencyData.address2);
          // this.ClientForm.get('CountryId').setValue(agencyData.countryId);
          // this.ClientForm.get('CityId').setValue(agencyData.cityId);
          // this.ClientForm.get('StateId').setValue(agencyData.stateId);
          this.ClientForm.get('CountryId').setValue(agencyData.countryId);
          this.loadState(agencyData.countryId);
          this.ClientForm.get('StateId').setValue(agencyData.stateId);
          this.loadCities(agencyData.stateId);
          this.ClientForm.get('CityId').setValue(agencyData.cityId);
          this.ClientForm.get('PostalCode').setValue(agencyData.postalCode);
          this.ClientForm.get('PrefixId').setValue(agencyData.prefixId);
          this.ClientForm.get('ContactPerson').setValue(agencyData.contactPerson);
          this.ClientForm.get('Position').setValue(agencyData.position);
          this.ClientForm.get('PhoneNumber').setValue(agencyData.phoneNumber);
          let req = new Date(this.datePipe.transform(agencyData.date, 'yyyy/MM/dd'))

          this.ClientForm.get('Date').setValue(this.datePipe.transform(req, 'yyyy-MM-dd'))

          this.ClientForm.get('MobileNumber').setValue(agencyData.mobileNumber);
          this.ClientForm.get('Email').setValue(agencyData.email);
          this.ClientForm.get('Website').setValue(agencyData.website);
          //this.ClientForm.get('Multisite').setValue(agencyData.website);
          // this.ClientForm.get('Multisite').setValue(agencyData.multisite);
          //  this.ClientForm.get('IsActive').setValue(agencyData.isActive);

          if (agencyData.isActive == true) {
            this.ClientForm.get('IsActive').setValue(1);
          }
          else {
            this.ClientForm.get('IsActive').setValue(0);
          }
          if (agencyData.multisite == true) {
            this.ClientForm.get('Multisite').setValue('1');
          }
          else {
            this.ClientForm.get('Multisite').setValue('0');

          }
          if (agencyData.isProjectExist == true) {
            this.ClientForm.get('Code').disable();
            this.ClientForm.get('Name').disable();
            this.ClientForm.get('Address1').disable();
            // this.ClientForm.get('EacodeId').disable();
            // this.ClientForm.get('NaceCodeId').disable();
            this.ClientForm.get('RiskId').disable();
            this.ClientForm.get('Address2').disable();
            this.ClientForm.get('CountryId').disable();
            this.ClientForm.get('StateId').disable();
            this.ClientForm.get('CityId').disable();
            this.ClientForm.get('PostalCode').disable();
            // this.ClientForm.get('PrefixId').disable();
            // this.ClientForm.get('ContactPerson').disable();
            // this.ClientForm.get('Position').disable();
            // this.ClientForm.get('PhoneNumber').disable();
            this.ClientForm.get('Date').disable();
            // this.ClientForm.get('MobileNumber').disable();
            this.ClientForm.get('Email').disable();
            // this.ClientForm.get('Website').disable();
            this.ClientForm.get('IsActive').disable();
            this.ClientForm.get('Multisite').disable();
            this.ClientForm.disable();
            this.change = true
          
            this.saveBtns= false;

          } else {
            this.change = false
          }

          const RoleId = parseInt(localStorage.getItem('roleId'));
          if(RoleId === 2 || RoleId === 21) {
            this.VisibleBtns = false
            this.saveBtns= false;
          }

         
          // this._ClientService.GetAllProjects(this.id,this.pagedDto).subscribe((Response) => {
      
      
          //   this.totalCount = Response.totalCount
          //    var ProjectsList = Response.clientProjectModel
          //    if(ProjectsList.length>0)
          //    {
          //     this.VisibleBtns=false;
          //    }
          //   //this .Liststandard=this.StandardList;
          // })
          // this.AgencyForm.get('Password').disable();
          // this.AgencyForm.get('ConfirmPassword').disable();
          // this.AgencyForm.get('ConfirmPassword').setValue("Pakistan@123");
          // this.AgencyForm.get('Password').setValue("Pakistan@123");
          // this.AgencyForm.get('IsActive').setValue(this.agencyData.isActive);
          // this.AgencyForm.get('FirstName').setValue(this.agencyData.firstName);

          // this.SecUserForm.get('OrganizationId').setValue(this.agencyData.EmailForgotPassword);


          //   this.SecUserForm.get('Designation').setValue(this.agencyData.designation);
          //  this.SecUserForm.get('Remarks').setValue(this.agencyData.remarks);

          //  this.SecUserForm.get('SbpAllowed').setValue(this.agencyData.sbpAllowed);
          //  this.SecUserForm.get('SbpAllowed').setValue(this.agencyData.sbpAllowed);



        }

      })
      //  this.onSearch(this.userUpdateId);
    }

  }
  loadProject(): void {

  
}
  // loadCities(): void {
  //
  // this.SecUserService.getCities().subscribe((Response)=>{
  //   this.CityList = Response
  // })
  // }
  // loadCountries(): void {
  //
  //   this.SecUserService.getCountries().subscribe((Response)=>{
  //     this.CountryList = Response
  //   })
  //   }
  //   loadState(): void
  //    {
  //
  //     this.SecUserService.getState().subscribe((Response)=>{
  //       this.StateList = Response
  //     })
  //  }
  loadPrefix(): void {

    this.SecUserService.getPrefix().subscribe((Response) => {
      this.PrefixList = Response
    })
  }

  loadStatus(): void {

    const item = {
      id: 1,
      name: 'Active',


    };
    this.StatusList.push(item);
    const item2 = {
      id: 0,
      name: 'InActive',


    };
    this.StatusList.push(item2);

  }
  loadMultisite(): void {

    const item = {
      id: 1,
      name: 'Yes',


    };
    this.MultisiteList.push(item);
    const item2 = {
      id: 0,
      name: 'No',


    };
    this.MultisiteList.push(item2);

  }

  handlefileInput(e: any)
  {

this.fileToUpload= <File>e?.target?.files[0];
//this.url=e.target.value;


  }

  get f() { return this.ClientForm.controls; }

  ChangeRequestOnSubmit() {
    debugger
    this.submitted = true;

    // stop here if form is invalid
    if (this.ClientEditForm.invalid) {
      abp.message.error("Some fields are required ");
      return;
    }
    this.ChangeRequest();
  }


  ChangeRequest() {
    debugger
// if(this.ClientEditForm.get('stateId').value ==null || this.ClientEditForm.get('stateId').value=="" ||this.ClientEditForm.get('stateId').value==undefined || this.ClientEditForm.get('stateId').value=='' || this.ClientEditForm.get('stateId').value==isNaN )
// {
//   abp.message.error("State fields are required ");
//   return;

// }
// console.log(this.ClientEditForm.get('cityId').value);

// if(this.ClientEditForm.get('cityId').value ==null || this.ClientEditForm.get('cityId').value=="" ||this.ClientEditForm.get('cityId').value==undefined || this.ClientEditForm.get('cityId').value=='' || this.ClientEditForm.get('cityId').value==isNaN )
// {
//   abp.message.error("city fields are required ");
//   return;

// }
    const foData:FormData = new FormData();

    // if (this.id > 0) {
    //   this.Client.Id = this.id
    // }
    // this.Client.Name = this.ClientEditForm.get('name').value;
    // this.Client.Address1 = this.ClientEditForm.get('address1').value;
    // this.Client.CountryId = this.ClientEditForm.get('countryId').value;
    // this.Client.Email = this.ClientEditForm.get('email').value;
    // this.Client.CityId = this.ClientEditForm.get('cityId').value;
    // this.Client.StateId = this.ClientEditForm.get('stateId').value;
    // this.Client.PostalCode = this.ClientEditForm.get('postalCode').value;
    // this.Client.FormName=this.formName;
    // this.Client.File = this.fileToUpload;
    // if (this.ClientEditForm.get('isActive').value == '1') {
    //   this.Client.IsActive = true;
    // }
    // else {
    //   this.Client.IsActive = false;
    // }
    foData.append("Id",this.id.toString());
    foData.append("Name",this.ClientEditForm.get('name').value);
    foData.append("Address1",this.ClientEditForm.get('address1').value);
    // foData.append("CountryId",this.ClientEditForm.get('countryId').value);
    foData.append("Email",this.ClientEditForm.get('email').value);
    // foData.append("CityId",this.ClientEditForm.get('cityId').value);
    // foData.append("StateId",this.ClientEditForm.get('stateId').value);
    foData.append("PostalCode",this.ClientEditForm.get('postalCode').value);
    foData.append("FormName",this.formName);
    foData.append('File',this.fileToUpload);

    foData.append("PrefixId",this.ClientEditForm.get('prefixId').value);
    foData.append("ContactPerson",this.ClientEditForm.get('contactPerson').value);
    // foData.append("CountryId",this.ClientEditForm.get('countryId').value);
    foData.append("Position",this.ClientEditForm.get('position').value);
  

    console.log(this.fileToUpload);

    var OrgId = localStorage.getItem('organizationId');
    // this.Client.OrganizationId = parseInt(OrgId);
    foData.append("OrganizationId",OrgId)

    var userId = localStorage.getItem('userId');
    // this.Client.CreatorUserId = parseInt(userId);
    foData.append("CreatorUserId",userId)



    this._ClientService.CreateChangeClient(foData).subscribe((Response) => {
      if (Response.message == '1') {
              abp.message.info("Submited For Review","Change Request Successfully Saved.!")
              this.router.navigateByUrl('/app/pages/sales/task-board-list');
            }else {
              abp.message.error(Response.message)
            }


          })

  }





  onSubmit() {

    this.submitted = true;

    // stop here if form is invalid
    if (this.ClientForm.invalid) {
      abp.message.error("Some fields are required ");
      return;
    }
    this.onSubmit1();
    // if (this.fileToUpload != null && this.fileToUpload != "" && this.fileToUpload != undefined && this.fileToUpload != undefined && this.fileToUpload != NaN) {
    //   this.onSubmit1();
    // }
    // else {

    //   abp.message.error("Please Upload Application Form!", "Application Form is required ");
    // }
    // display form values on success
    //alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.SLCPForm.value, null, 4));
  }
  onSubmit1(): void {
    debugger
    var codelength = this.ClientForm.get('Code').value



    if (this.id > 0) {
      if (codelength.length != 12) {
        abp.message.error("Code Length should be 12 Character")
        return
      }
    }
    else {
      if (codelength.length != 8) {
        abp.message.error("Code Length should be 8 Character")
        return
      }
    }
    var MesseageError = "";

    // MesseageError="Module is Empty";

    // if(this.ClientForm.get('ContactPerson').value ==null ||this.ClientForm.get('ContactPerson').value==undefined|| this.ClientForm.get('ContactPerson').value=="")
    // {  abp.message.error("ContactPerson is Required","Alert")
    // return
    // // MesseageError="Title is Empty";
    // }
    // if(this.ClientForm.get('Email').value ==null ||this.ClientForm.get('Email').value==undefined|| this.ClientForm.get('Email').value=="")
    // {  abp.message.error("Email is Required","Alert")
    // return
    // // MesseageError="Version is Empty";
    // }
    // if(this.ClientForm.get('Address1').value ==null ||this.ClientForm.get('Address1').value==undefined|| this.ClientForm.get('Address1').value=="")
    // {  abp.message.error("Address1 is Required","Alert")
    // return
    // // MesseageError="Version is Empty";
    // }
    // if(this.ClientForm.get('Date').value ==null ||this.ClientForm.get('Date').value==undefined|| this.ClientForm.get('Date').value=="")
    // {  abp.message.error("Date is Required","Alert")
    // return
    // // MesseageError="Version is Empty";
    // }



    if (this.id > 0) {
      this.Client.Id = this.id
    }
    this.Client.Code = this.ClientForm.get('Code').value;
    this.Client.Name = this.ClientForm.get('Name').value;
    this.Client.Address1 = this.ClientForm.get('Address1').value;

    this.Client.RiskId = this.ClientForm.get('RiskId').value;
    this.Client.NaceCodeId = this.ClientForm.get('NaceCodeId').value;
    this.Client.EacodeId = this.ClientForm.get('EacodeId').value;


    this.Client.Address2 = this.ClientForm.get('Address2').value;
    this.Client.CountryId = parseInt(this.ClientForm.get('CountryId').value);
    this.Client.CityId = parseInt(this.ClientForm.get('CityId').value);
    this.Client.StateId = parseInt(this.ClientForm.get('StateId').value);
    this.Client.PostalCode = this.ClientForm.get('PostalCode').value;
    this.Client.PrefixId = parseInt(this.ClientForm.get('PrefixId').value);
    this.Client.ContactPerson = this.ClientForm.get('ContactPerson').value;
    this.Client.Position = this.ClientForm.get('Position').value;
    this.Client.PhoneNumber = this.ClientForm.get('PhoneNumber').value;
    if (this.ClientForm.get('Date').value != null && this.ClientForm.get('Date').value != "" && this.ClientForm.get('Date').value != undefined && this.ClientForm.get('Date').value != isNaN) { this.Client.Date = this.ClientForm.get('Date').value; }
    this.Client.MobileNumber = this.ClientForm.get('MobileNumber').value;
    this.Client.Email = this.ClientForm.get('Email').value;
    this.Client.Website = this.ClientForm.get('Website').value;



    if (this.ClientForm.get('Multisite').value == '1') {
      this.Client.Multisite = true;
    }
    else if (this.ClientForm.get('Multisite').value == '0') {

      this.Client.Multisite = false;
    }
    if (this.ClientForm.get('IsActive').value == '1') {
      this.Client.IsActive = true;
    }
    else {
      this.Client.IsActive = false;
    }



    var OrgId = localStorage.getItem('organizationId');
    this.Client.OrganizationId = parseInt(OrgId);

    var userId = localStorage.getItem('userId');
    this.Client.CreatorUserId = parseInt(userId);


    // this.Agncy.IsActive = this.AgencyForm.get('IsActive').value





    this._ClientService.create(this.Client).subscribe((Response) => {
      if (Response.message == '1') {
        abp.message.info("Successfully Saved!")
        this.router.navigateByUrl('/app/pages/sales/task-board-list');
      }
      else if (Response.message == '2') {
        abp.message.info("Successfully Updated!")
        this.router.navigateByUrl('/app/pages/sales/task-board-list');
      }
      else if (Response.message == '0') {
        abp.message.error("Client Code Already Exist")
      }
      else {
        abp.message.error(Response.message)
      }


    })

  }

  Back(): void {
    this.router.navigateByUrl('/app/pages/sales/task-board-list');

  }

  loadCities(stateId): void {

    this.CityList = null
    this.SecUserService.getCitiesByState(stateId).subscribe((Response) => {
      this.CityList = Response
    })
  }

  loadCountries(): void {

    this.SecUserService.getCountries().subscribe((Response) => {
      this.CountryList = Response
      let countryId = 0;
      this.loadState(countryId);
    })
  }

  loadState(countryId): void {
    this.StateList = [];
    this.SecUserService.getStateByCountryId(countryId).subscribe((Response) => {
      this.StateList = Response
      this.CityList = [];
    })
  }

  loadEaCode(): void {
    debugger
    this._UserStandardService.getAllEACode().subscribe((Response) => {
      this.EACodeList = Response
      let eacodeId = 0;
      this.loadNaceCode(eacodeId);

    })
  }

  loadNaceCode(eacodeId): void {
    debugger
    this._UserStandardService.getAllNaceCodeByEaCode(eacodeId).subscribe((Response) => {
      this.NaceCodeList = Response
      debugger
      // riskLevelId=
      // this.ClientForm.controls.RiskId.setValue(this.NaceCodeList[0].riskLevelId);
      console.log(this.NaceCodeList[0].riskLevelId);

    })
  }
  loadRisk(): void {

    this._SA8000Service.GetAllRisk().subscribe((Response) => {
      this.RiskList = Response

    })
  }

  RiskLevel(NaceCodeId): void {
    debugger


    let RiskLevel = this.NaceCodeList.find(x => x.id == NaceCodeId)

    console.log(RiskLevel);
    this.ClientForm.controls.RiskId.setValue(RiskLevel.riskLevelId);

  }
}
