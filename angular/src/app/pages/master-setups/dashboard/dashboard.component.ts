import { Component, Injector, ChangeDetectionStrategy, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { SecUserService } from '@shared/Services/sec-user.service';
import { DashboardService } from '@shared/Services/Dashboard-service';

import {  Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { ClientService } from '@shared/Services/Client-Service';
import { DxDataGridComponent } from 'devextreme-angular';
import { ProjectLedgerModel } from '@shared/Dto/Project-Ledger-model';


import { FormControl, FormGroup } from '@angular/forms';
import {Injectable} from '@angular/core'; 
import {HttpResponse} from '@angular/common/http';
import {NgxPaginationModule} from 'ngx-pagination'; // <-- import the module
import {Observable} from 'rxjs';

import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { ActivatedRoute, } from '@angular/router';
import repaintFloatingActionButton from 'devextreme/ui/speed_dial_action/repaint_floating_action_button';
import { DxListModule } from "devextreme-angular";

import { DxTreeListModule } from "devextreme-angular";
import { Employee, EmployeesService } from '@app/pages/sales/employees.service';
import DataSource from 'devextreme/data/data_source';
import { BrowserModule } from '@angular/platform-browser';
import { DxContextMenuModule } from "devextreme-angular";
import { IndentRequestService } from '@shared/Services/indent-request-service';
import { AccreditationModel } from '@shared/Dto/accreditation-model';
import {AccreditationService} from 'shared/Services/Accreditation-service';
import { ToastrService } from 'ngx-toastr';
import { SA8000Service } from '@shared/Services/project-SA8000-service';
import { SlcpService } from '@shared/Services/project-slcp-service';
import { HiggService } from '@shared/Services/project-higg-service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  ProjectForm = new FormGroup({
   
    
  })


  CardForm = new FormGroup({
    ActivityLogId: new FormControl(''),
    ApprovalStatusId: new FormControl(""),
    Remarks: new FormControl(""),
    NewValues: new FormControl("")
  })

  ProjectRemarks = new FormGroup({
    ActivityLogId: new FormControl(''),
    ApprovalStatusId: new FormControl(""),
    Remarks: new FormControl(""),
    NewValues: new FormControl("")
  })

  ClientSitesRemarks = new FormGroup({
    ActivityLogId: new FormControl(''),
    ApprovalStatusId: new FormControl(""),
    Remarks: new FormControl(""),
    NewValues: new FormControl("")
  })
  // constructor(injector: Injector, private _SecUserService:SecUserService,
  //   private _DashboardService : DashboardService,
  //   public _ClientService :  ClientService,
  //   private router: Router) {
      
      
      
  //   super(injector);

  //    this.review = this.review.bind(this);
  //    this.review1=this.review1.bind(this);
  //    this.onRowClick=this.onRowClick.bind(this);
  //    this.onRowContractClick=this.onRowContractClick.bind(this);
  //    this.onRowClick1=this.onRowClick1.bind(this);
     
  // }


  constructor(service: EmployeesService,
    private _SecUserService:SecUserService,
    private indentRequestService : IndentRequestService,
    private _makerAuthorizerFormService: MakerAuthorizerFormService,
    private _toster: ToastrService,
    private _SA8000Service: SA8000Service,
    private route: ActivatedRoute,
     public AccreditationService: AccreditationService,
     private _DashboardService : DashboardService,
     public _ClientService :  ClientService,
     private _SlcpService: SlcpService,
     private _HiggService: HiggService,
    
     private router : Router) 
      { this.review = this.review.bind(this);
         this.review1=this.review1.bind(this);
         this.onpopup = this.onpopup.bind(this);
         this.openPopup = this.openPopup.bind(this);
         this.ProjectReviewpopup = this.ProjectReviewpopup.bind(this);
         this.ClientSitesReviewpopup = this.ClientSitesReviewpopup.bind(this);
         this.Projectonpopup = this.Projectonpopup.bind(this);
         this.RemarksPopup = this.RemarksPopup.bind(this);
         this.ProjectRemarksHistoryPopup = this.ProjectRemarksHistoryPopup.bind(this);
         this.ClientSitesRemarksHistoryPopup = this.ClientSitesRemarksHistoryPopup.bind(this);
         this.openPopupEdit = this.openPopupEdit.bind(this);
         this.openPopupProjectEdit = this.openPopupProjectEdit.bind(this);
         this.openPopupClientSitesEdit = this.openPopupClientSitesEdit.bind(this);
         this.ClientSitesMainPopup = this.ClientSitesMainPopup.bind(this);
          this.onRowClick=this.onRowClick.bind(this);
           this.onRowContractClick=this.onRowContractClick.bind(this);
          this.onRowClick1=this.onRowClick1.bind(this);
          this.onWindowPeriod=this.onWindowPeriod.bind(this);
           this.OnManageVisit=this.OnManageVisit.bind(this);
           this.Downloadfile = this.Downloadfile.bind(this);
           this.onRowPrepared = this.onRowPrepared.bind(this);
          //  this.onCellPrepared1 = this.onCellPrepared1.bind(this);
           this.ProjectChangeRequestDownloadfile = this.ProjectChangeRequestDownloadfile.bind(this);
           this.ClientSitesDownloadfile = this.ClientSitesDownloadfile.bind(this);
           this.OldValuesDownloadfile = this.OldValuesDownloadfile.bind(this);
           this.NewValuesDownloadfile = this.NewValuesDownloadfile.bind(this);
           this.ProjectchangeRequestRows=this.ProjectchangeRequestRows.bind(this);
           this.MarkComplete=this.MarkComplete.bind(this);
          }

  tableSizes = [3, 5, 10, 15, 20, 25];
  public totalCount: number
  public pagedDto: PagedRequestModel = new PagedRequestModel()
  public keyword : string = ''
  public DocumentsList=[]
  pageNumber : number = 1
  pageSize : number = 5
  public isEditShown : boolean  
  public authorizer:boolean=false
  public Recordeview:boolean=false
  public FileUploadingGrid:boolean=false
  public ProjectChangeGrid:boolean=false
  public WithOutDropdown:boolean=false
  public Dropdown:boolean=false
  public ReviewChangeRequest:boolean=false
  public RecordeviewClientSite:boolean=false
  public editRecord:boolean=false
  public isViewShown : boolean  
  public isAddShown : boolean  
  public ManageVisit:boolean=false
  public editClientSite:boolean=false
  public ProjectStatus = []
  public ContractStatus = []
  public ClientChangeStatus = []
  public ProjectChangeRequestStatus = []
  public ClientSiteChangeRequestStatus = []
  public ProjectChangeRequestStatuslist = []
  public ProjectChangeRequestFileUploadlist = []
  public ClientSitesChangeRequestStatuslist = []
  public AuditStatus = []
  public ApprovedStatus=[]
  public ProjectsList = [];
  public ContractList = [];
  public AuditList =[]
  public ClientApprovalList = []
  public OrganizationId: number
  public ActivityLogId: number
  public NewValuesList = [];
  public WindowPeriod = [];
public projectgrid:boolean= false
public contractgrid:boolean=false
public clientChangegrid:boolean=false
public projectChangegrid:boolean=false
public clientSitesChangegrid:boolean=false
public RecordeviewClient:boolean=false
public auditgrid:boolean=false
public windowPeriodgrid:boolean=false
public editClientrecord:boolean=false
  public CurrentName: string
  public UpdatedName:string
  public ContractName:string
  public ProjectChangeDetailList = [];
  public clientChangeDataList = [];
  public clientChangeRemarksList = [];
  public projectChangeRemarksList = [];
  public clientSitesChangeRemarksList = [];
  public standardId: number
  public clientChangeRequestList = [];
  public projectChangeRequestList = [];
  public CertifiedClientModel=[];
  public clientSiteChangeRequestList = [];
  public IntimationdateSurv_1 = [];
  public Windowperiod_start_Surv_1 = [];
  public Windowperiod_Start_FUP_1 = [];
  public IntimationdateSurv_2 = [];
  public Windowperiod_start_Surv_2 = [];
  public Windowperiod_Sart_FUP_2 = [];
  public Recertification_Windowperiod_start_Surv_2 = [];
  public Followup_Recert_Start = [];
   public CertifiedClientList=[];

  
  ngOnInit() : void{
    debugger
    this.loadSecRoleForm();
    this.UpdateProject();
    this.loadSecRoleForm1();
     
    this.onSearch();
   
 
  //this.pagedDto.page =1;
  // this.pageSize=3;
  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));

 


// this.pagedDto.page =1;
// //this.pagedDto.pageSize =3;
// var OrganizationId = parseInt(localStorage.getItem('organizationId'));
// this._DashboardService.GetDashboardData(OrganizationId,this.pagedDto).subscribe(data => {
//  console.log("service");
//  
// this.totalCount = 3;
//  this.ProjectStatus = data.projectDashboardStatusModel;
//  this.ContractStatus = data.contractDashboardStatusModel;
//  this.AuditStatus = data.auditsDashboardStatusModel;
//  this.pagedDto.page =1;
//   } )
  }
  ngAfterViewInit() : void {
    //this.UpdateProject()
  
    
   
  }
  review1(e)
  { 
   
    ///#/app/pages/sales/audit-plan?ProjectId=' + this.AllVisitDataList[_i].projectId + "&StandardId=" + this.AllVisitDataList[_i].standardId + "&ClientId=" + this.AllVisitDataList[_i].clientId + "&AuditVisitId=" + this.AllVisitDataList[_i].id
    // this.router.navigateByUrl('/app/pages/sales/audit-plan?'+e.row.data.projectId);
    this.router.navigateByUrl('/app/pages/sales/audit-plan?ProjectId=' + e.row.data.projectId + "&StandardId=" + e.row.data.standardId + "&ClientId=" + e.row.data.clientId + "&AuditVisitId=" + e.row.data.id);
    
  }


  displayStyle = "none"

  onpopup(e) {
     

    this.displayStyle = "block"
console.log(e);

    var name =  "Client Change Request"

  this.CurrentName= name +"   "+"( "+ e.row.data.approvalStatusName +" )"
  // console.log(e.data.id)

  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
  this.pagedDto.statusId = parseInt(e.row.data.approvalStatusId)
  // this.pagedDto.filePath = e.row.data.filePath;
  this.totalCount =0;

  if(parseInt(localStorage.getItem('organizationId'))== 1 &&  (parseInt(e.row.data.approvalStatusId) ==2 || parseInt(e.row.data.approvalStatusId) ==3))
  {
    this.RecordeviewClient = false;
    this.editClientrecord = false;
  
  }
  else if(parseInt( localStorage.getItem('organizationId')) > 1 &&  (parseInt(e.row.data.approvalStatusId) ==1 || parseInt(e.row.data.approvalStatusId) ==2 || parseInt(e.row.data.approvalStatusId) == 10002))
  {

    this.RecordeviewClient = false;
    this.editClientrecord = false;
  }
  else
  {
    this.RecordeviewClient = true
    if(parseInt( localStorage.getItem('organizationId')) > 1)
    {
      this.editClientrecord = true;

    }
  }

  
// if( this.pagedDto.organizationId > 1)
// {
//   this.RecordeviewClient = false
//   this.editClientrecord = true

// }
// else{
//   this.RecordeviewClient = true
//   this.editClientrecord =false
// }
// if(this.pagedDto.statusId ==2){
//   this.RecordeviewClient = false
//   this.editClientrecord = false

// }
// else{
//   this.RecordeviewClient = true

// }
// if(this.pagedDto.statusId ==3 && this.pagedDto.organizationId == 1)
// {
//   this.RecordeviewClient = false
//   this.editClientrecord = false

// }

   
  this._DashboardService.GetClientChangeDataById(parseInt(e.row.data.id),this.pagedDto).subscribe((Response) => {
   
    this.clientChangeDataList = Response
 
    
    //this .Liststandard=this.StandardList;
  })
  }


  closePopup() {
    this.displayStyle = "none"
  }

  projectdisplayStyle="none"

  Projectonpopup(e) {
     

    this.projectdisplayStyle = "block"
console.log(e);

    var name =  "Project Change Request"
    
    this.pagedDto.keyword = e.row.data.tableName;
  this.CurrentName= name +"   "+"( "+ e.row.data.approvalStatusName +" )"
  // console.log(e.data.id)

  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
  this.pagedDto.statusId = parseInt(e.row.data.approvalStatusId)
  // this.pagedDto.filePath = e.row.data.filePath;
  this.totalCount =0;

  if(parseInt(localStorage.getItem('organizationId'))== 1 &&  (parseInt(e.row.data.approvalStatusId) ==2 || parseInt(e.row.data.approvalStatusId) ==3))
  {
    this.Recordeview = false
    this.editRecord =false
  
  }
  else if(parseInt( localStorage.getItem('organizationId')) > 1 &&  (parseInt(e.row.data.approvalStatusId) ==1 || parseInt(e.row.data.approvalStatusId) ==2 || parseInt(e.row.data.approvalStatusId) == 10002))
  {
    this.Recordeview = false
    this.editRecord =false
  }
  else
  {
    this.Recordeview = true
    if(parseInt( localStorage.getItem('organizationId')) > 1)
    {
      this.editRecord = true;

    }
  }
//   if(this.pagedDto.organizationId >1){
//     this.Recordeview = false
//     this.editRecord = true
//   }else{
//   this.Recordeview = true
//   this.editRecord =false
// }

// if(this.pagedDto.statusId ==2 )
// {
//   this.Recordeview = false
//   this.editRecord = false
// }
// else{
// this.Recordeview = true

// }


// if(this.pagedDto.statusId ==3 && this.pagedDto.organizationId == 1)
// {
//   this.Recordeview = false
//   this.editRecord = false
// }
this.ProjectChangeRequestFileUploadlist=[];
this.ProjectChangeRequestFileUploadlist=[];

  this._DashboardService.GetProjectChangeDataById(parseInt(e.row.data.id),this.pagedDto).subscribe((Response) => {

   
     let count =  Response.filter((item) => item.affectedColumns != "ContractFilePath").length 
     let countfile =Response.filter((item) => item.affectedColumns == "ContractFilePath").length; 
   
    if (countfile > 0 ){
        this.FileUploadingGrid = true
      
        this.ProjectChangeRequestFileUploadlist=Response.filter((item) => item.affectedColumns == "ContractFilePath") 
    }
    else{
      this.FileUploadingGrid = false
    }
 if(count > 0)
    {
     
      this.ProjectChangeGrid = true
      this.ProjectChangeRequestStatuslist=Response.filter((item) => item.affectedColumns != "ContractFilePath") 

    }else{
   
      this.ProjectChangeGrid = false 
    }

 
    
    //this .Liststandard=this.StandardList;
  })

}

  projectclosePopup(){
this.projectdisplayStyle="none"
  }


  display = "none";

  openPopup(e){
     

    this.CardForm.controls.ActivityLogId.setValue(e.row.data.id);
   
    this.displayStyle = "none"
this.display = "block"
  }


  closepopup(){
    this.display = "none"
    this.displayStyle = "block"
  }

  ProjectRemarksdisplay="none"
  ProjectReviewpopup(e){
    this.ProjectRemarks.controls.ActivityLogId.setValue(e.row.data.id);
    this.ProjectRemarksdisplay="block"
    this.projectdisplayStyle="none"

  }

  ProjectReviewclose(){
  this.ProjectRemarksdisplay="none"
  this.projectdisplayStyle="block"

  }



  ClientSitesRemarksdisplay="none"
  ClientSitesReviewpopup(e){
    this.ClientSitesRemarks.controls.ActivityLogId.setValue(e.row.data.id);
this.ClientSitesRemarksdisplay="block"
  }
  ClientSitesReviewclose(){
this.ClientSitesRemarksdisplay="none"
  }



  RemarksDisplayStyle= "none"

  RemarksPopup(e){
     
  this.RemarksDisplayStyle= "block"
  this.displayStyle = "none"
  this.display = "none"


  this.totalCount =0;
   
  this._DashboardService.GetClientChangeRemarksDataById(e.row.data.id).subscribe((Response) => {
   
    this.clientChangeRemarksList = Response
 
    
    //this .Liststandard=this.StandardList;
  })

  }

  RemarksClosepopup(){
    this.RemarksDisplayStyle= "none"
    this.display = "none"
    this.displayStyle = "block"

  }


  RemarksHistoryDisplay="none"
  ProjectRemarksHistoryPopup(e){
    this.RemarksHistoryDisplay="block"
    this.ProjectRemarksdisplay="none"
    this.projectdisplayStyle="none"

    this.totalCount =0;
     
    this._DashboardService.GetProjectSA8000RemarksDataById(e.row.data.id).subscribe((Response) => {
     
      this.projectChangeRemarksList = Response
   
      
      //this .Liststandard=this.StandardList;
    })

  }
  RemarksHistoryClosepopup(){
    this.RemarksHistoryDisplay="none"
    this.ProjectRemarksdisplay="none"
    this.projectdisplayStyle="block"

  }

  ClientSitesRemarksHistoryDisplay="none"
  ClientSitesRemarksHistoryPopup(e){
this.ClientSitesRemarksHistoryDisplay="block"
this.ClientSitesRemarksdisplay="none"
this.ClientSitesMainPopupDisplay="none"


this.totalCount =0;
     
    this._DashboardService.GetClientSitesChangeRemarksDataById(e.row.data.id).subscribe((Response) => {
     
      this.clientSitesChangeRemarksList = Response
   
      
      //this .Liststandard=this.StandardList;
    })
  }


  ClientSitesRemarksHistoryClosepopup(){
    this.ClientSitesRemarksHistoryDisplay="none"
    this.ClientSitesRemarksdisplay="none"
this.ClientSitesMainPopupDisplay="block"

  }



  editdisplay="none"
  openPopupEdit(e){
     
    this.RemarksDisplayStyle= "none"
    this.displayStyle = "none"
    this.display = "none"
this.editdisplay="block"

this.CardForm.controls.ActivityLogId.setValue(e.row.data.id);


this.CardForm.controls.NewValues.setValue(e.row.data.newValues);

  }

  editclosepopup(){
    this.editdisplay="none"
    this.RemarksDisplayStyle= "none"
    this.display = "none"
    this.displayStyle = "block"
    
  }

  editProjectdisplay="none"
  openPopupProjectEdit(e){
     
    if(e.row.data.formId == 22){
this.standardId = 7
    }
    if(e.row.data.formId == 10037){
this.standardId = 6
    }
    if(e.row.data.formId == 37){
this.standardId = 3
    }

    this.RemarksHistoryDisplay="none"
    this.ProjectRemarksdisplay="none"
    this.projectdisplayStyle="none"
    this.editProjectdisplay="block"

    this.ProjectRemarks.controls.ActivityLogId.setValue(e.row.data.id);

    if(e.row.data.affectedColumns == "SurveillanceMethodId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadGetALlSurveillanceMethod()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "SurveillanceVisitFrequencyId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadSurveillanceVisitFrequency()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "ModuleVersionId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadModuleVersion()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "ModuleShareId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadModuleShare()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "RequestOfSiteId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadRequestOfSite()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "ServicesTypeId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadServiceType()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "MethodologyId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadMethodology()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "AssessmentCompletedId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadAssessmentCompleted()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "AccreditationId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadAccreditation()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "CompletedStepId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.LoadCompletedSetup()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "CompletedModuleId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadCompletedModule()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }
    else if(e.row.data.affectedColumns == "EffluentTreatmentPlantId"){
      this.Dropdown = true
      this.WithOutDropdown = false
      this.loadEffluentTreatmentPlant()
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
      
    }



    else{
      this.WithOutDropdown = true
      this.Dropdown = false
      //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
    }

  }

  loadGetALlSurveillanceMethod(): void {

    this._SA8000Service.GetALlSurveillanceMethod().subscribe((Response) => {
      this.NewValuesList = Response

    })
  }

  loadSurveillanceVisitFrequency(): void {

    this._SA8000Service.SurveillanceVisitFrequencyList().subscribe((Response) => {
      this.NewValuesList = Response

    })
  }

  loadModuleVersion(): void {
      
    
  
        this._SlcpService.GetALLModuleVersion(this.standardId).subscribe((Response) => {
          this.NewValuesList = Response
    
    
        })
      }

      loadRequestOfSite(): void {

        this._SlcpService.GetALLRequestOfSite().subscribe((Response) => {
          this.NewValuesList = Response
    
    
        })
      }

     loadModuleShare(): void {
    
        this._SlcpService.GetALLModuleShare().subscribe((Response) => {
          this.NewValuesList = Response
    
    
        })
      }


      loadServiceType(): void {

        this._HiggService.GetALLServicesType().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }

      loadMethodology(): void {

        this._HiggService.GetALLMethodology().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }

      loadAssessmentCompleted(): void {

        this._HiggService.GetALLAssessmentCompleted().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }

      loadAccreditation(): void {

        this._SlcpService.GetALLAccreditation().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }


      LoadCompletedSetup(): void {

        this._SlcpService.GetALLCompletedSetup().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }

      loadCompletedModule(): void {

        this._HiggService.GetALLCompletedModule().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }

      loadEffluentTreatmentPlant(): void {

        this._HiggService.GetALLEffluentTreatmentPlant().subscribe((Response) => {
          this.NewValuesList = Response
    
        })
      }

      loadCountries(): void {
    

        this._SecUserService.getCountries().subscribe((Response) => {
          this.NewValuesList = Response
          // let countryId=0;
          // this.loadState(countryId);
        })
      }
    
      loadState(countryId): void {
        this._SecUserService.getStateByCountryId(countryId).subscribe((Response) => {
          this.NewValuesList = Response
        })
      }

  editProjectclosepopup(){
    this.RemarksHistoryDisplay="none"
    this.ProjectRemarksdisplay="none"
    this.projectdisplayStyle="block"
    this.editProjectdisplay="none"

  }



  editClientSitesdisplay="none"
  openPopupClientSitesEdit(e){
this.editClientSitesdisplay="block"
this.ClientSitesMainPopupDisplay="none"
this.ClientSitesRemarksdisplay="none"
this.ClientSitesRemarksHistoryDisplay="none"

this.ClientSitesRemarks.controls.ActivityLogId.setValue(e.row.data.id);
   // this.ClientSitesRemarks.controls.NewValues.setValue(e.row.data.newValues);
  //  if(e.row.data.affectedColumns == "cityId"){
  //   this.Dropdown = true
  //   this.WithOutDropdown = false
  //   this.loadCountries()
  //   //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
    
  // }
  // else if(e.row.data.affectedColumns == "stateId"){
  //   this.Dropdown = true
  //   this.WithOutDropdown = false
  //  this.loadCountries()
  //   //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
    
  // }
  // else if(e.row.data.affectedColumns == "CountryId"){
  //   this.Dropdown = true
  //   this.WithOutDropdown = false
  //   this.loadModuleVersion()
  //   //this.ProjectRemarks.controls.NewValues.setValue(e.row.data.newValues);
    
  // }

}

editClientSitesclosepopup(){
this.editClientSitesdisplay="none"
this.ClientSitesMainPopupDisplay="none"
this.ClientSitesRemarksdisplay="none"
this.ClientSitesRemarksHistoryDisplay="block"

}



  public statusList = [
   

    {id:"2",name:"Approved"  },
    {id:"3",name:"Reject"},
    
    
    
  ]


  NewValuesEditSubmit(){
 
    if (
     
      this.CardForm.get("Remarks").value != null &&
      this.CardForm.get("Remarks").value != "" &&
      this.CardForm.get("Remarks").value != undefined &&
      this.CardForm.get("Remarks").value != isNaN
    ) 
    
    
    {
      const ClientSitesModel = {
        Id: this.CardForm.get("ActivityLogId").value,
        ApprovalStatusId: "10002",
        Remarks: this.CardForm.get("Remarks").value,
        NewValues: this.CardForm.get("NewValues").value,
        CreatedBy: localStorage.getItem("userId"),

        
      };
      let ActivitylogId= parseInt(this.CardForm.get("ActivityLogId").value);
       ;
      this._DashboardService.ApprovedClientChange(ClientSitesModel).subscribe(
        (Response) => {
          abp.message.info(Response.message);
          if(Response.message=="Successfully Submited..")
          {

            this.clientChangeDataList=  this.clientChangeDataList.filter(element => element.id !== ActivitylogId);
          }
          this.editdisplay = "none";
          this.displayStyle = "block";
          this.UpdateProject();
          //this.reloadGrid();
          //this.router.navigateByUrl('/app/home');
        }
      );
    } else {
      abp.message.error(
        "New Values and Remarks can not be Empty",
        "Alert"
      );
    }
  }

  ResubmitProjectRequest(){
 
    var newvalue=this.ProjectRemarks.get("NewValues").value;
    if (
     
      this.ProjectRemarks.get("Remarks").value != null &&
      this.ProjectRemarks.get("Remarks").value != "" &&
      this.ProjectRemarks.get("Remarks").value != undefined &&
      this.ProjectRemarks.get("Remarks").value != isNaN &&
         
      this.ProjectRemarks.get("NewValues").value != null &&
      this.ProjectRemarks.get("NewValues").value != "" &&
      this.ProjectRemarks.get("NewValues").value != undefined &&
      this.ProjectRemarks.get("NewValues").value != isNaN
    ) 
    
    
    {
      const projectSitesModel = {
        Id: this.ProjectRemarks.get("ActivityLogId").value,
        ApprovalStatusId: "10002",
        Remarks: this.ProjectRemarks.get("Remarks").value,
        NewValues: this.ProjectRemarks.get("NewValues").value,
        CreatedBy: localStorage.getItem("userId"),

        
      };
      let ActivitylogId= parseInt(this.ProjectRemarks.get("ActivityLogId").value);

       ;
      this._DashboardService.ApprovedProjectChange(projectSitesModel).subscribe(
        (Response) => {
          abp.message.info(Response.message);
          if(Response.message=="Successfully Submited.." || Response.message=="successfully Submited..")
          {

            this.ProjectChangeRequestStatuslist=  this.ProjectChangeRequestStatuslist.filter(element => element.id !== ActivitylogId);
          }
          this.editProjectdisplay = "none";
          this.projectdisplayStyle = "block";
          // this.editProjectdisplay="block"
          this.UpdateProject();
          // this.reloadGrid();
          //this.router.navigateByUrl('/app/home');
        }
      );
    } else {
      abp.message.error(
        "New Value And Remarks can not be Empty",
        "Alert"
      );
    }
  }



  ResubmitClientSitesRequest(){
    if (
     
      this.ClientSitesRemarks.get("Remarks").value != null &&
      this.ClientSitesRemarks.get("Remarks").value != "" &&
      this.ClientSitesRemarks.get("Remarks").value != undefined &&
      this.ClientSitesRemarks.get("Remarks").value != isNaN
    ) 
    
    
    {
      const clientSitesResubmitModel = {
        Id: this.ClientSitesRemarks.get("ActivityLogId").value,
        ApprovalStatusId: "10002",
        Remarks: this.ClientSitesRemarks.get("Remarks").value,
        NewValues: this.ClientSitesRemarks.get("NewValues").value,
        CreatedBy: localStorage.getItem("userId"),

        
      };

       ;
      this._DashboardService.ApprovedClientSitesChange(clientSitesResubmitModel).subscribe(
        (Response) => {
          abp.message.info(Response.message);
          this.editClientSitesdisplay = "none";
          // this.editProjectdisplay="block"
          this.UpdateProject();
          // this.reloadGrid();
          //this.router.navigateByUrl('/app/home');
        }
      );
    } else {
      abp.message.error(
        "Input Field And Remarks can not be Empty",
        "Alert"
      );
    }
  }



  Downloadfile(e): void {
 
    this.id=e.row.data.id;
    var fullpath=e.row.data.filePath;
   var filename =  fullpath.replace(/^.*[\\\/]/, '')
 
    this._DashboardService.downloadFile(this.id).subscribe((result:Blob)=>{
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


  OldValuesDownloadfile(e): void {
 
    this.id=e.row.data.id;
    var fullpath=e.row.data.oldValues;
   var filename =  fullpath.replace(/^.*[\\\/]/, '')
  //  FilePath_D:\OzoneLibraryDocuments\ProjectsSA8000\139_Receipt-Datewise-Report.xlsx_ContentType_application
  //  /vnd.openxmlformats-officedocument.spreadsheetml.sheet
 

  // D:\OzoneLibraryDocuments\ProjectHiggDocument\100_Zain Raza .pdf
    this._DashboardService.oldValuesDownloadfile(this.id).subscribe((result:Blob)=>{
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


  NewValuesDownloadfile(e): void {
 
    this.id=e.row.data.id;
    var fullpath=e.row.data.newValues;
 
   
    //var a = fullpath.Split("FilePath_D:\OzoneLibraryDocuments\Sa8000Documents\6a360691-352b-4d78-86df-554f1b9f5e3f_");
   var filepath=  fullpath.split("_ContentType_");
var filename = filepath[0].replace(/^.*[\\\/]/, '');
   // FilePath_D:\OzoneLibraryDocuments\Sa8000Documents\6a360691-352b-4d78-86df-554f1b9f5e3f_Resume.pdf
  //  _ContentType_application/pdf
 
    this._DashboardService.NewValuesDownloadfile(this.id).subscribe((result:Blob)=>{
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



  ProjectChangeRequestDownloadfile(e): void {
     
        this.id=e.row.data.id;
        var fullpath=e.row.data.filePath;
       var filename =  fullpath.replace(/^.*[\\\/]/, '')
     
        this._DashboardService.ProjectChangeRequestDownloadfile(this.id).subscribe((result:Blob)=>{
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


ClientSitesDownloadfile(e):void{
 
    this.id=e.row.data.id;
    var fullpath=e.row.data.filePath;
   var filename =  fullpath.replace(/^.*[\\\/]/, '')
 
    this._DashboardService.ClientSitesdownloadFile(this.id).subscribe((result:Blob)=>{
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





  OnApprovalSubmit(): void {
     ;
    

    if (
      this.CardForm.get("ApprovalStatusId").value > 0 &&
      this.CardForm.get("Remarks").value != null &&
      this.CardForm.get("Remarks").value != "" &&
      this.CardForm.get("Remarks").value != undefined &&
      this.CardForm.get("Remarks").value != isNaN
    ) 
    
    
    {
      const ClientSitesModel = {
        Id: this.CardForm.get("ActivityLogId").value,
        ApprovalStatusId: this.CardForm.get("ApprovalStatusId").value,
        Remarks: this.CardForm.get("Remarks").value,
        CreatedBy: localStorage.getItem("userId"),

        
      };
     let ActivitylogId= parseInt(this.CardForm.get("ActivityLogId").value);
       ;
      this._DashboardService.ApprovedClientChange(ClientSitesModel).subscribe(
        (Response) => {
           
          abp.message.info(Response.message);
          if(Response.message=="Successfully Submited..")
          {

            this.clientChangeDataList=  this.clientChangeDataList.filter(element => element.id !== ActivitylogId);
          }
          this.display = "none";
          this.displayStyle = "block";
          this.UpdateProject();
          //this.reloadGrid();
          //this.router.navigateByUrl('/app/home');
        }
      );
    } else {
      abp.message.error(
        "Approval Status and Remarks can not be Empty",
        "Alert"
      );
    }
  }


  OnProjectApprovalSubmit(): void{
     ;
    

    if (
      this.ProjectRemarks.get("ApprovalStatusId").value > 0 &&
      this.ProjectRemarks.get("Remarks").value != null &&
      this.ProjectRemarks.get("Remarks").value != "" &&
      this.ProjectRemarks.get("Remarks").value != undefined &&
      this.ProjectRemarks.get("Remarks").value != isNaN
    ) 
    
    
    {
      const projectSitesModel = {
        Id: this.ProjectRemarks.get("ActivityLogId").value,
        ApprovalStatusId: this.ProjectRemarks.get("ApprovalStatusId").value,
        Remarks: this.ProjectRemarks.get("Remarks").value,
        CreatedBy: localStorage.getItem("userId")

      };
      let ActivitylogId= parseInt(this.ProjectRemarks.get("ActivityLogId").value);
       ;
      this._DashboardService.ApprovedProjectChange(projectSitesModel).subscribe(
        (Response) => {
          abp.message.info(Response.message);

          if(Response.message=="Successfully Submited.." || Response.message=="successfully Submited..")
          {

            this.ProjectChangeRequestStatuslist=  this.ProjectChangeRequestStatuslist.filter(element => element.id !== ActivitylogId);
          }
          
          this.ProjectRemarksdisplay = "none";
          this.projectdisplayStyle = "block";
          this.UpdateProject();
          // this.reloadGrid();
          //this.router.navigateByUrl('/app/home');
        }
      );
    } else {
      abp.message.error(
        "Approval Status and Remarks can not be Empty",
        "Alert"
      );
    }
  }



  OnClientSitesApprovalSubmit(): void{
    
    if (
      this.ClientSitesRemarks.get("ApprovalStatusId").value > 0 &&
      this.ClientSitesRemarks.get("Remarks").value != null &&
      this.ClientSitesRemarks.get("Remarks").value != "" &&
      this.ClientSitesRemarks.get("Remarks").value != undefined &&
      this.ClientSitesRemarks.get("Remarks").value != isNaN
    ) 
    
    
    {
      const clientSitesModel = {
        Id: this.ClientSitesRemarks.get("ActivityLogId").value,
        ApprovalStatusId: this.ClientSitesRemarks.get("ApprovalStatusId").value,
        Remarks: this.ClientSitesRemarks.get("Remarks").value,
        CreatedBy: localStorage.getItem("userId"),

        
      };
      let ActivitylogId= parseInt(this.ClientSitesRemarks.get("ActivityLogId").value);
       ;
      this._DashboardService.ApprovedClientSitesChange(clientSitesModel).subscribe(
        (Response) => {
          abp.message.info(Response.message);

          if(Response.message=="Successfully Submited.."|| Response.message=="successfully Submited..")
          {

            this.ClientSitesChangeRequestStatuslist=  this.ClientSitesChangeRequestStatuslist.filter(element => element.id !== ActivitylogId);
          }
          this.ClientSitesRemarksdisplay = "none";
          this.ClientSitesMainPopupDisplay = "block";
          
          this.UpdateProject();
          // this.reloadGrid();
          //this.router.navigateByUrl('/app/home');
        }
      );
    } else {
      abp.message.error(
        "Approval Status and Remarks can not be Empty",
        "Alert"
      );
    }
  }

  review(e)
  {
    
     
   this._ClientService.GetProjectFormUrlById(e.row.data.standardId).subscribe((Response) => {
               
   
     var FormPth  = Response.path
     
    //  localStorage.removeItem('clientId');
    //  localStorage.setItem('clientId',this.Clientid.toString());
   
     //this.router.navigateByUrl(FormPth + e.row.data.id +"&"+ e.row.data.clientId);
     this.router.navigateByUrl(FormPth+"ProjectId="+ e.row.data.id +"&StandardId="+e.row.data.standardId+"&ClientId="+e.row.data.clientId );
    // this.router.navigateByUrl(FormPth + e.row.data.id);
      //this .Liststandard=this.StandardList;
    })
  }
  id:number
  onRowClick(e)
  
  {  
       
//this.pagedDto.page=1;
//this.pagedDto.pageSize=7;
this.projectgrid=true;
this.auditgrid=false;
this.contractgrid=false;
this.clientChangegrid=false;
this.projectChangegrid=false;
this.clientSitesChangegrid=false;
this.windowPeriodgrid=false;
  this.id=e.data.id;
var name = "Project Applications"

    this.CurrentName= name +"   "+"( "+e.data.name +" )"
 
    this.projectListData(this.id)
    // console.log(e.data.id)
    

  }

  ProjectchangeRequestRows(e){
     

    this.projectgrid=false;
    this.auditgrid=false;
    this.contractgrid=false;
    this.clientChangegrid=false;
    this.projectChangegrid=true;
    this.clientSitesChangegrid=false;
    this.windowPeriodgrid=false;
    

  var name =  "Project Change Request"

  this.CurrentName= name +"   "+"( "+e.data.name +" )"
  console.log(e.data.id)

  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
  this.totalCount =0;
  this._DashboardService.GetAllProjectChangeRequest( e.data.id,this.pagedDto).subscribe((Response) => {
              
   

  this.projectChangeRequestList = Response.activityLogModel
 
    
    //this .Liststandard=this.StandardList;
  })
    
  }




  ClientSitesMainPopupDisplay="none"

  ClientSitesMainPopup(e){
this.ClientSitesMainPopupDisplay="block"

var name =  "Client Sites Change Request"

this.CurrentName= name +"   "+"( "+ e.row.data.approvalStatusName +" )"
// console.log(e.data.id)

this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
this.pagedDto.statusId = parseInt(e.row.data.approvalStatusId)
// this.pagedDto.filePath = e.row.data.filePath;
this.totalCount =0;

if(parseInt(localStorage.getItem('organizationId'))== 1 &&  (parseInt(e.row.data.approvalStatusId) ==2 || parseInt(e.row.data.approvalStatusId) ==3))
  {
    this.RecordeviewClientSite = false
     this.editClientSite = false
  
  }
  else if(parseInt( localStorage.getItem('organizationId')) > 1 &&  (parseInt(e.row.data.approvalStatusId) ==1 || parseInt(e.row.data.approvalStatusId) ==2 || parseInt(e.row.data.approvalStatusId) == 10002))
  {
    this.RecordeviewClientSite = false
     this.editClientSite = false
  
  }
  else
  {
    this.RecordeviewClientSite = true
    if(parseInt( localStorage.getItem('organizationId')) > 1)
    {
      this.editClientSite = true;

    }
  }
// if( this.pagedDto.organizationId > 1){
  
//   this.RecordeviewClientSite = false
//   this.editClientSite = true

// }else{
//   this.RecordeviewClientSite = true
//   this.editClientSite = false
// }

// if( this.pagedDto.statusId ==2){
//   this.RecordeviewClientSite = false
//   this.editClientSite = false

// }else{
//   this.RecordeviewClientSite = true


// }
if(this.pagedDto.statusId ==3 && this.pagedDto.organizationId == 1)
{
  this.RecordeviewClientSite = false
  this.editClientSite = false

}


 
this._DashboardService.GetClientSitesChangeDataById(parseInt(e.row.data.id),this.pagedDto).subscribe((Response) => {
 
  this.ClientSitesChangeRequestStatuslist = Response
 
  
  //this .Liststandard=this.StandardList;
})
  }
  ClientSitesMainClosePopup(){
    this.ClientSitesMainPopupDisplay="none"
  }





  ClientSiteschangeRequestRows(e){
     

    this.projectgrid=false;
    this.auditgrid=false;
    this.contractgrid=false;
    this.clientChangegrid=false;
    this.projectChangegrid=false;
    this.clientSitesChangegrid=true;
    this.windowPeriodgrid=false;
    

  var name =  "Client Sites Change Request"

  this.CurrentName= name +"   "+"( "+e.data.name +" )"
  console.log(e.data.id)

  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
  this.totalCount =0;
  this._DashboardService.GetAllClientSitesChangeRequest( e.data.id,this.pagedDto).subscribe((Response) => {
              
   


    
    this.clientSiteChangeRequestList = Response.activityLogModel
 
    
    //this .Liststandard=this.StandardList;
  })
  }


  ClientChangeRowClick(e)
  
  {  
     

    this.projectgrid=false;
    this.auditgrid=false;
    this.contractgrid=false;
    this.clientChangegrid=true;
    this.projectChangegrid=false;
    this.clientSitesChangegrid=false;
    this.windowPeriodgrid=false;
    

  var name =  "Client Change Request"

  this.CurrentName= name +"   "+"( "+e.data.name +" )"
  console.log(e.data.id)

  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
  this.totalCount =0;
  this._DashboardService.GetAllClientChange( e.data.id,this.pagedDto).subscribe((Response) => {
              
   


    
    this.clientChangeRequestList = Response.activityLogModel
 
    
    //this .Liststandard=this.StandardList;
  })
    

  }

projectListData(PId:number)
{

  this.pagedDto.roleId = parseInt(localStorage.getItem('roleId'));
  this.pagedDto.userId = parseInt(localStorage.getItem('userId'));
  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
  this.totalCount =0;
  this._DashboardService.GetAllProjects(PId,this.pagedDto).subscribe((Response) => {
              
   
 
    this.totalCount = Response.totalCount
    this.ProjectsList = Response.clientProjectModel
   // var a = loc_array.slice(-1)[0]
  })
}
  onRowContractClick(e)
  
  {  
       

      this.projectgrid=false;
      this.auditgrid=false;
      this.contractgrid=true;
      this.windowPeriodgrid=false;

this.clientChangegrid=false;
this.projectChangegrid=false;
this.clientSitesChangegrid=false;

      // this.clientgrid=true;

  
    

    this.clientChangegrid=false;
    this.projectChangegrid=false;
    this.clientSitesChangegrid=false;

    var name =  "Client Contracts"

    this.CurrentName= name +"   "+"( "+e.data.name +" )"
    console.log(e.data.id)
  
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    this.totalCount =0;
    this._DashboardService.GetAllProjects( e.data.id,this.pagedDto).subscribe((Response) => {
                
     


      this.totalCount = Response.totalCount
      this.ContractList = Response.clientProjectModel
      
 
      
      //this .Liststandard=this.StandardList;
    })

  }

  onRowClick1(e)
  
  {  
        

this.projectgrid=false;
this.auditgrid=true;
this.contractgrid=false;
this.clientChangegrid=false;
this.projectChangegrid=false;
this.clientSitesChangegrid=false;
this.windowPeriodgrid=false;
var name =  "Audits Details"

      this.CurrentName= name +"  "+"("+e.data.name +")"
    console.log(e.data.id)
  
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    this.totalCount =0;
    this._DashboardService.GetAllAudits( e.data.id,this.pagedDto).subscribe((Response) => {
                
     
       

      this.totalCount = Response.totalCount
      this.AuditList = Response.clientAuditVisitModel
      //this .Liststandard=this.StandardList;
    })

  }
  

  
  reloadGrid()
 
{
 
  this.pagedDto.page =1;
  this.projectListData(this.id);

}
onTableDataChangeApplication(event) {
  this.pagedDto.page = event;
  this.onRowClick(this.id);

}
onTableSizeChange(event): void {
  this.pagedDto.pageSize = event.target.value;
  this.onRowClick1(this.id);


 }
 public Clientid:number
 onSearch(){
    
   console.log("organizationId")
 // this.pagedDto.keyword = this.keyword
  // this.pagedDto.authAllowed = true
  // this.pagedDto.pageSize=5
  // this.pagedDto.page = 1;

  // if(this.authorizer==true)
  // {
  //   this.pagedDto.keyword = this.keyword;
  //   this.pagedDto.authAllowed = true;
    //this.Clientid=7;
    //this.pagedDto.page =1;
    // this.pageSize=3;
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    // this._DashboardService.GetAllProjects(this.pagedDto).subscribe((Response) => {
                
    //  
    //   this.totalCount = Response.totalCount
    //   this.ProjectsList = Response.clientProjectModel
    //   //this .Liststandard=this.StandardList;
    // })
  
 }

 UpdateProject() {    
    
   this.onSearch();
   //this.reloadGrid()
    this.pagedDto.page =1;
   //this.pagedDto.pageSize =3;
   var roleId = parseInt(localStorage.getItem('roleId'));
this.pagedDto.userId = parseInt(localStorage.getItem('userId'));
this.pagedDto.organizationId = parseInt(localStorage.getItem('organizationId'));
  this._DashboardService.GetDashboardData(roleId,this.pagedDto).subscribe(data => {
    console.log("service");
 
   //this.totalCount = 3;
    this.ProjectStatus = data.projectDashboardStatusModel;
    this.ContractStatus = data.contractDashboardStatusModel;
    this.AuditStatus = data.auditsDashboardStatusModel;
    this.ClientChangeStatus = data.clientDashboardStatusModel;
    this.clientChangeDataList = data.GetClientChangeDataById;
   this.ProjectChangeRequestStatus= data.projectRequestDashboardStatusModel;
   this.ClientSiteChangeRequestStatus= data.clientSitetRequestDashboardStatusModel;
   this.WindowPeriod=data.windiwPeriodModel;
   this.CertifiedClientModel=data.certifiedClientModel;

   debugger
   console.log(data);
   this.IntimationdateSurv_1 = data.intimationdateSurv_1;
   this.Windowperiod_start_Surv_1 = data.windowperiod_start_Surv_1;
   this.Windowperiod_Start_FUP_1 = data.windowperiod_Start_FUP_1;
   this.Windowperiod_start_Surv_2 = data.windowperiod_start_Surv_2;
   this.Windowperiod_Sart_FUP_2 =data.windowperiod_Sart_FUP_2
   this.Recertification_Windowperiod_start_Surv_2 = data.recertification_Windowperiod_start_Surv_2;
   this.Followup_Recert_Start = data.followup_Recert_Start;
    //this.reloadGrid()
    
    //this.pagedDto.page =1;
    this.onSearch();
    this.reloadGrid();




   
   


  });

}
// onCellPrepared1(e){
//    
// var text ="Zain"
//   if (e.columnIndex === 3 && e.rowType === "data") {
//     // Set the cell text to a link
//     e.cellElement.html("zain");
// }
//   // if (e.data.affectedColumns === "ContractFilePath")
//   //  {
//   //    
//   //       e.rowElement.style.backgroundColor = "#099299";
//   //       e.rowElement.style.color = "white";

//   // }
// }

  // }

// displayStyle = "none";
// approvedProjects(e)
// {
//   this.totalCount=2;
//   this.ProjectStatus=e.row.data.id;
 
//  this.displayStyle = "block";

// //this.router.navigateByUrl('/app/pages/stock-management/library?'+this.id);  
// }


onTableDataChange(event) {
   
  this.pagedDto.page = event;
  this.projectListData(this.id)
}

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
  @Input() formName: string
  secRoleForm
  loadSecRoleForm() {

    

    this.formName = "dashboard"
    this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {

      let formName = (this.formName == undefined ? localStorage.getItem('dashboard') : this.formName)

      this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)
      if (this.secRoleForm.manageAllowed == true) {
        this.ManageVisit=true
        localStorage.removeItem('manageAllowed');
        localStorage.setItem('manageAllowed','1');
      }
      else {
        this.ManageVisit = false
        localStorage.removeItem('manageAllowed');
        localStorage.setItem('manageAllowed','0');
      }
    })

  }
  OnManageVisit(e)
  {
  
    this.router.navigateByUrl('/app/pages/sales/client-add-visit?'+"ProjectId="+ e.row.data.id +"&StandardId="+e.row.data.standardId );
  }

  ManageAuditVisit(e) {
   
  
    console.log(e.row.data.approvalStatusId)
   var manageAllowed =localStorage.getItem('manageAllowed');
    
   if (e.row.data.approvalStatusId=="7"  && manageAllowed=='1')
    {
    
       return !e.row.isEditing;
    
    
   
  }else if(e.row.data.approvalStatusId=="3" || e.row.data.approvalStatusId=="13"){
    return !e.row.isEditing;
  }
  else
  {
     return e.row.isEditing;
  }
  
  }

  ReciewForApproval(e) {

     
    return e.row.isEditing;
   // var authorizerID =localStorage.getItem('authorizer');
    //  var tt= this.authorizer;
   // var Insert_Allow =localStorage.getItem('InsertAllow');
    //  var tt= this.authorizer;
  //     if(Insert_Allow =="1")
  //     {
  //    if (e.row.data.approvalStatusId=="4" ||e.row.data.approvalStatusId=="2")
  //     {
  //     return !e.row.isEditing;
      
  //   }
  //     else
  //     {
        
  //       return e.row.isEditing;
  //     }
  //   }
  //   else
  //   {
  //      return e.row.isEditing;
  //   }
    
  }


  onRowPrepared(e) {
     
    if (e.rowType === "data") {
      if (e.row.data === "ContractFilePath") {
        e.rowElement.style.backgroundColor = "#099299";
        e.rowElement.style.color = "white";
      }
   
    }
  }

  editViaible(e) {
   
     
    var Insert_Allow =localStorage.getItem('InsertAllow');
  //  var tt= this.authorizer;
    if(Insert_Allow =="1")
    {
   if (e.row.data.approvalStatusId=="1" || e.row.data.approvalStatusId=="4" ||e.row.data.approvalStatusId=="2"||e.row.data.approvalStatusId=="7" ||e.row.data.approvalStatusId=="8"||e.row.data.approvalStatusId=="9"||e.row.data.approvalStatusId=="10")
    {
    return e.row.isEditing;
  }
  
    else
    {
      
      return !e.row.isEditing;
    }
  }
  else
  {
     return e.row.isEditing;
  }
  
  }


  loadSecRoleForm1() {
     
    // let secRoleForm = JSON.parse(localStorage.getItem('secRoleForm'))
    // let permission = secRoleForm.find(x => x.formCode != null && x.formCode == this.formCode)

    // this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {

    //   let formName = (this.formName == undefined ? localStorage.getItem('formName') : this.formName)
    //   this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)

    var formName = "ReviewChangeRequest"
    this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {

      this.secRoleForm = data.find(x => x.formName == formName)
     
    
      if (this.secRoleForm.insertAllowed == true) 
      {

     this.ReviewChangeRequest = true;
     
      this.statusList = [{id:"10002",name:"Resubmited"}, ]

      
    }
    if (this.secRoleForm.authAllowed == true) 
    {

   this.ReviewChangeRequest = true;
   
   
    
  }
   

    })

  }



  
  onWindowPeriod(e)
  
  {  
        

this.projectgrid=false;
this.auditgrid=false;
this.contractgrid=false;
this.clientChangegrid=false;
this.projectChangegrid=false;
this.clientSitesChangegrid=false;
this.windowPeriodgrid=true

var name =  "Window Period"

      this.CurrentName=  e.data.name;
    console.log(e.data.id)
  
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    this.totalCount =0;
    this.CertifiedClientList=[];
     if(e.data.id==1)
     {
      this.CertifiedClientList=this.IntimationdateSurv_1;
     }
     if(e.data.id==2)
     {
      this.CertifiedClientList=this.Windowperiod_start_Surv_1;
     }
     if(e.data.id==3)
     {
      this.CertifiedClientList=this.Windowperiod_Start_FUP_1;
     }
     if(e.data.id==4)
     {
      this.CertifiedClientList=this.Windowperiod_start_Surv_2;
     }
     if(e.data.id==5)
     {
      this.CertifiedClientList=this.Windowperiod_Sart_FUP_2;
     }
     if(e.data.id==6)
     {
      this.CertifiedClientList=this.Recertification_Windowperiod_start_Surv_2;
     }
     if(e.data.id==7)
     {
      this.CertifiedClientList=this.Followup_Recert_Start;
     }

    

      this.totalCount = this.CertifiedClientList.length;
     
  

  }


  MarkComplete(e)
  {
    const windowperiodModel = {
      Id: e.row.data.projectId,
      type: this.CurrentName,
      CreatedById: localStorage.getItem("userId"),

      
    };
  //  alert(e.row.data.projectId);
   this._DashboardService.windowperiod(windowperiodModel).subscribe(data => {
debugger

if (data.message=="1")
{
  this.UpdateProject();
  this.windowPeriodgrid=false;
  abp.message.info("Record Marked");

}
else 
{
  abp.message.error("Record Not Marked");
}


   });

  }
}
