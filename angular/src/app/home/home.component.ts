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
@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase implements OnInit {
  //@ViewChild('dataGridVar', { static: false }) dataGrid: DxDataGridComponent;
  constructor(injector: Injector, private _SecUserService:SecUserService,
    private _DashboardService : DashboardService,
    public _ClientService :  ClientService,
    private router: Router) {
      
      
      
    super(injector);

     this.review = this.review.bind(this);
     this.review1=this.review1.bind(this);
     this.onRowClick=this.onRowClick.bind(this);
     this.onRowContractClick=this.onRowContractClick.bind(this);
     this.onRowClick1=this.onRowClick1.bind(this);
     
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
  public isViewShown : boolean  
  public isAddShown : boolean  
  public ProjectStatus = []
  public ContractStatus = []
  public AuditStatus = []
  public ApprovedStatus=[]
  public ProjectsList = [];
  public ContractList = [];
  public AuditList =[]
  public OrganizationId: number
public projectgrid:boolean= false
public contractgrid:boolean=false
public auditgrid:boolean=false
  public CurrentName: string
  public UpdatedName:string
  public ContractName:string

  
  ngOnInit() : void{

  //   this.UpdateProject();
  //   debugger
  //   this.onSearch();
   
  // this.checkPermission()
  // this.pagedDto.page =1;
  // // this.pageSize=3;
  // this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));

 


// this.pagedDto.page =1;
// //this.pagedDto.pageSize =3;
// var OrganizationId = parseInt(localStorage.getItem('organizationId'));
// this._DashboardService.GetDashboardData(OrganizationId,this.pagedDto).subscribe(data => {
//  console.log("service");
// debugger
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
   
    
     this.router.navigateByUrl('/app/pages/sales/audit-plan?'+e.row.data.projectId);
      
    
  }

  review(e)
  {
    
    debugger
   this._ClientService.GetProjectFormUrlById(e.row.data.standardId).subscribe((Response) => {
               
  debugger
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
      debugger
//this.pagedDto.page=1;
//this.pagedDto.pageSize=7;
this.projectgrid=true;
this.auditgrid=false;
this.contractgrid=false;
  this.id=e.data.id;
var name = "Projects Details"

    this.CurrentName= name +"   "+"( "+e.data.name +" )"
 
    this.projectListData(this.id)
    // console.log(e.data.id)
    

  }
projectListData(PId:number)
{

  this.pagedDto.roleId = parseInt(localStorage.getItem('roleId'));
  this.pagedDto.userId = parseInt(localStorage.getItem('userId'));
  this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));

  alert("Home 150" + PId);
  this._DashboardService.GetAllProjects(PId,this.pagedDto).subscribe((Response) => {
              
  debugger
   
    this.totalCount = Response.totalCount
    this.ProjectsList = Response.clientProjectModel
   // var a = loc_array.slice(-1)[0]
  })
}
  onRowContractClick(e)
  
  {  
      debugger

      this.projectgrid=false;
      this.auditgrid=false;
      this.contractgrid=true;
      

    var name =  "Contracts Details"

    this.CurrentName= name +"   "+"( "+e.data.name +" )"
    console.log(e.data.id)
  
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    alert("Home 176" + e.data.id);
    this._DashboardService.GetAllProjects( e.data.id,this.pagedDto).subscribe((Response) => {
                
    debugger


      this.totalCount = Response.totalCount
      this.ContractList = Response.clientProjectModel
debugger
      
      //this .Liststandard=this.StandardList;
    })

  }

  onRowClick1(e)
  
  {  
      debugger 

this.projectgrid=false;
this.auditgrid=true;
this.contractgrid=false;
var name =  "Audit Details"

      this.CurrentName= name +"  "+"("+e.data.name +")"
    console.log(e.data.id)
  
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    
    this._DashboardService.GetAllAudits( e.data.id,this.pagedDto).subscribe((Response) => {
                
    debugger
       

      this.totalCount = Response.totalCount
      this.AuditList = Response.clientAuditVisitModel
      //this .Liststandard=this.StandardList;
    })

  }
  // review(e)
  
  //  {
  //    debugger
     
  //   this._ClientService.GetProjectFormUrlById(e.row.data.standardId).subscribe((Response) => {
                
   
  //     var FormPth  = Response.path
      
  //     localStorage.removeItem('clientId');
  //     localStorage.setItem('clientId',this.Clientid.toString());
    
  //     //this.router.navigateByUrl(FormPth + e.row.data.id +"&"+ e.row.data.clientId);
  //     this.router.navigateByUrl(FormPth + e.row.data.id);
  //      //this .Liststandard=this.StandardList;
  //    })
  //  }

  checkPermission(): void{
    var LoginUserId = localStorage.getItem('userId');
    // const UserModel =

    // {

    //   Id: LoginUserId,
    //   // LastModifiedById: LoginUserId,
    //   // CreatedById: LoginUserId,
    //   // Password: this.ChangePasswordForm.get('NewPassword').value,
    //   //ConfirmPassword: this.ChangePasswordForm.get('NewConfirmPassword').value,
      
    // }

    this._SecUserService.CheckPermission(LoginUserId).subscribe((Response)=>{
 if(Response.message== "1")
 {
  abp.message.info("Your Password has been changed by another User!","Please reset your password")
 }
 if(Response.message== "3")
 {
  abp.message.info("Please reset your password")
  //this.router.navigate(['users/Change-password']);
  //this.router.navigateByUrl('/app/')
 }   
     })


  }

  reloadGrid()
 
{
debugger
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
   debugger
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
    this.pagedDto.page =1;
    // this.pageSize=3;
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));
    // this._DashboardService.GetAllProjects(this.pagedDto).subscribe((Response) => {
                
    // debugger
    //   this.totalCount = Response.totalCount
    //   this.ProjectsList = Response.clientProjectModel
    //   //this .Liststandard=this.StandardList;
    // })
  
 }

 UpdateProject() {    
   debugger
   this.onSearch();
   //this.reloadGrid()
   this.pagedDto.page =1;
   //this.pagedDto.pageSize =3;
   var roleId = parseInt(localStorage.getItem('roleId'));
this.pagedDto.userId = parseInt(localStorage.getItem('userId'));
this.pagedDto.organizationId = parseInt(localStorage.getItem('organizationId'));
  this._DashboardService.GetDashboardData(roleId,this.pagedDto).subscribe(data => {
    console.log("service");
debugger
this.totalCount = 3;
    this.ProjectStatus = data.projectDashboardStatusModel;
    this.ContractStatus = data.contractDashboardStatusModel;
    this.AuditStatus = data.auditsDashboardStatusModel;
   
    //this.reloadGrid()
    this.pagedDto.page =1;
    this.onSearch();
    this.reloadGrid();




   
   


  });

}
onCellPrepared(e){
  // debugger
  // if (e.rowType === "header")
  //  {
  //  // e.cellElement.css("color", "orange"); 
  //        e.cellElement.css("text-align", "right")

  // }
}
displayStyle = "none";
// approvedProjects(e)
// {
//   this.totalCount=2;
//   this.ProjectStatus=e.row.data.id;
 
//  this.displayStyle = "block";

// //this.router.navigateByUrl('/app/pages/stock-management/library?'+this.id);  
// }


onTableDataChange(event) {
  debugger
  this.pagedDto.page = event;
  this.projectListData(this.id)
}
}
