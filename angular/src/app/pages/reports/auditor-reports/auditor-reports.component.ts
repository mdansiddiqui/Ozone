import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { ProjectAmountReportsService } from 'shared/Services/Project-Amount-Reports-service';
import { AuditorService } from 'shared/Services/Auditor-Service';
import { LibraryResourceService } from '@shared/Services/library-Resource_service';
//import { exportDataGrid } from 'devextreme/pdf_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver-es';
// Our demo infrastructure requires us to use 'file-saver-es'. We recommend that you use the official 'file-saver' package in your applications.
import { exportDataGrid } from 'devextreme/excel_exporter';
//import { jsPDF } from 'jspdf';
import { DxDataGridModule, DxDataGridComponent, DxSpeedDialActionModule, DxSelectBoxModule } from 'devextreme-angular';
@Component({
  selector: 'app-auditor-reports',
  templateUrl: './auditor-reports.component.html',
  styleUrls: ['./auditor-reports.component.css']
})
export class AuditorReportsComponent implements OnInit {

  // auditorReports = new FormGroup({
  
  //   OrganizationId: new FormControl('')
  // })
  @ViewChild('dataGridVar', { static: false }) dataGrid: DxDataGridComponent;
  @Input() AuditDetail = []
  public pagedDto: PagedRequestModel = new PagedRequestModel()
  public OrganizationList = [];

  public AuditorReport = []
  public AuditorReportDetailsList = []
  public ClientProjectsReport = []
  public AuditorListsReport = []
  public AuditorScheduleList = []
  public CertificationList = [];
  public ScheduleOfAuditList = [];
  public ImpartialityReviewList = [];
  public CertifiedClientList = [];
  // public ReportsList = [];
  public standardList = [];
  public totalCount: number
  public AuditReport: boolean=false
  public ClientProjectReport: boolean=false
  public Status: boolean=false

  public AuditorListReport: boolean=false
  public AuditorValidationScheduleReport: boolean=false
  public ScheduleOfAuditReport: boolean=false
  public CertifiedClientReport: boolean=false
  public ImpartialityReviewReport: boolean=false
  public ReportName: string=""
  filterForm = new FormGroup({
  fromdate: new FormControl(''),
  todate: new FormControl(''),
  OrganizationId: new FormControl(''),
  StandardId: new FormControl(''),
  ReportId: new FormControl(''),
  StatusId: new FormControl(''),
});


  constructor(
     public ProjectAmountReportsService: ProjectAmountReportsService,
     public _AuditorService: AuditorService,
     public LibraryResourceService: LibraryResourceService
     ) { }

  ngOnInit(): void {
    this.loadAgency();
    this.loadAllCertification();
    // this.loadReportsList();
    var roleId = localStorage.getItem("roleId");
    var organizationId = localStorage.getItem("organizationId");
    if(parseInt(roleId) !=2 && parseInt(roleId)!=21)
    {
      this.filterForm.controls.OrganizationId.setValue(organizationId);
      this.filterForm.get('OrganizationId').disable();
    }
  
    
  }
  
  onExporting(e) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet(this.ReportName);

    exportDataGrid({
      component: e.component,
      worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), this.ReportName+'.xlsx');
      });
    });
    e.cancel = true;
  }
  // onExporting(e) {
  //   const doc = new jsPDF();
  //   exportDataGrid({
  //     jsPDFDocument: doc,
  //     component: e.component,
  //     indent: 5,
  //   }).then(() => {
  //     doc.save('Companies.pdf');
  //   });
  // }

 public ReportsList = [
    {id:1 , name:' Auditor Audits '},
    {id:2, name:' Client Projects'},
    {id:3, name:' Auditor List'},
    {id:4, name:' Auditor Validation Schedule'},
    {id:5, name:' Schedule Of Audits'},
    {id:6, name:' Impartiality Review'},
    {id:7, name:' Window Period monitoring'},
  ]
 public StatusList = [
    {id:0,name:' In-Active '},
    {id:1, name:'Active'}
   
  ]



  loadStatus(a){
     
    if( a == 2)
    { 
      this.Status = true
    }
    else{
      this.Status = false
    }
  }
 				

  loadAgency(): void {
     
    this.ProjectAmountReportsService.getAllAgency().subscribe((Response) => {
      this.OrganizationList = Response

      const item2 = {
        id: 0,
        name:'--- ALL ---',
     };
      
        this.OrganizationList.push(item2);
        this.OrganizationList= this.OrganizationList.sort((a, b) => {
         return a.id - b.id;
       });
         
    })
  }

//   onSearch(){
      
//      
//   //this.pagedDto.keyword = "";
//   //this.pagedDto.authAllowed = this.secRoleForm.authAllowed
//   this.pagedDto.pageSize = 3
//   this._AuditorService.Get(this.pagedDto).subscribe((Response) => {
              
//    
//      this.totalCount = Response.totalCount
//     this.AuditorReport = Response.leadAuditorReportsModel
//     //console.log(this.NaceCodeList)
//   })
// }
reloadGrid() {

  this.pagedDto.page = 1;
  //this.onSearch();
}
Search() {
 
   
  if( parseInt(this.filterForm.get('ReportId').value) >0)
  {  

  
  if( parseInt(this.filterForm.get('ReportId').value) == 1)
{  
  this.AuditReport = true
  this.ClientProjectReport = false
  this.AuditorListReport = false
  this.AuditorValidationScheduleReport = false
  this.ScheduleOfAuditReport = false
  this.CertifiedClientReport=false
  this.ImpartialityReviewReport = false
  this.AuditorReportSearch();
}
 else if( parseInt(this.filterForm.get('ReportId').value) == 2)
{  
 
  this.AuditReport = false
  this.ClientProjectReport = true
  this.AuditorListReport = false
  this.AuditorValidationScheduleReport = false
  this.ScheduleOfAuditReport = false
  this.CertifiedClientReport=false
  this.ImpartialityReviewReport = false
 this.ClientProjectReportSearch();
}
 else if( parseInt(this.filterForm.get('ReportId').value) == 3)
{  
  this.AuditReport = false
  this.ClientProjectReport = false
  this.AuditorListReport = true
  this.AuditorValidationScheduleReport = false
  this.ScheduleOfAuditReport = false
  this.ImpartialityReviewReport = false
this.AuditorListReportSearch();
}
 else if( parseInt(this.filterForm.get('ReportId').value) == 4)
{  
  this.AuditReport = false
  this.ClientProjectReport = false
  this.AuditorListReport = false
  this.AuditorValidationScheduleReport = true
  this.ScheduleOfAuditReport = false
  this.CertifiedClientReport=false
  this.ImpartialityReviewReport = false
 this.AuditorValidationScheduleReportSearch();
}
 else if( parseInt(this.filterForm.get('ReportId').value) == 5)
{  
  this.AuditReport = false
  this.ClientProjectReport = false
  this.AuditorListReport = false
  this.AuditorValidationScheduleReport = false
  this.ScheduleOfAuditReport = true
  this.CertifiedClientReport=false
  this.ImpartialityReviewReport = false
  this.ScheduleOfAuditReportSearch();
}
 else if( parseInt(this.filterForm.get('ReportId').value) == 6)
{  
  this.AuditReport = false
  this.ClientProjectReport = false
  this.AuditorListReport = false
  this.AuditorValidationScheduleReport = false
  this.ScheduleOfAuditReport = false
  this.CertifiedClientReport=false
  this.ImpartialityReviewReport = true
  this.ImpartialityReviewReportSearch();
}
else if( parseInt(this.filterForm.get('ReportId').value) == 7)
{  
  this.AuditReport = false
  this.ClientProjectReport = false
  this.AuditorListReport = false
  this.AuditorValidationScheduleReport = false
  this.ScheduleOfAuditReport = false
  this.CertifiedClientReport=true
  this.ImpartialityReviewReport = false
  this.CertifiedClientSearch();
}
}
else
{
  abp.message.error("Please Select Report Name","Alert")
}
}


ClientProjectReportSearch(){
   
  if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
  {  abp.message.error("From-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
  {  abp.message.error("To-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(this.filterForm.get('StatusId').value ==null ||this.filterForm.get('StatusId').value==undefined|| this.filterForm.get('StatusId').value=="")
  {  abp.message.error("Status can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
     
    this.pagedDto.pageSize = 100
    this.ClientProjectsReport =[];
    this.totalCount =0;
    this.ReportName="Client Projects";
    this._AuditorService.GetClientProjectReport(this.filterForm.value).subscribe((Response) => {
                
     
       this.totalCount = Response.totalCount
      this.ClientProjectsReport = Response.clientProjectReportModel;
   
      //console.log(this.NaceCodeList)
    })
   
}


AuditorReportSearch() {
  if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
{  abp.message.error("From-Date can not be Empty","Alert")
return
// MesseageError="Version is Empty";
}
if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
{  abp.message.error("To-Date can not be Empty","Alert")
return
// MesseageError="Version is Empty";
}
   
  this.pagedDto.pageSize = 100
  this.AuditorReport=[];
  this.totalCount =0;
  this.ReportName="Auditor Audits";
  this._AuditorService.GetAuditorReport(this.filterForm.value).subscribe((Response) => {
              
   
   // this.totalCount = Response.totalCount
    this.AuditorReport = Response
    console.log(Response)
    //this.AuditorReportDetails();
   
    //console.log(this.NaceCodeList)
  })
  this.AuditorReportDetails();
  
}

AuditorReportDetails() {
  if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
{  abp.message.error("From-Date can not be Empty","Alert")
return
// MesseageError="Version is Empty";
}
if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
{  abp.message.error("To-Date can not be Empty","Alert")
return
// MesseageError="Version is Empty";
}
   
  this.pagedDto.pageSize = 100
  this.AuditorReport=[];
  this.totalCount =0;
  //this.ReportName="Auditor Audits";

  
  const Param= {
    fromdate: this.filterForm.get('fromdate').value ,
    todate: this.filterForm.get('todate').value,
    OrganizationId:this.filterForm.get('OrganizationId').value,
    StandardId:this.filterForm.get('StandardId').value,
    //LeadAuditorId:this.filterForm.get('LeadAuditorId').value,
 };
  
  this._AuditorService.GetAuditorReportDetails(Param).subscribe((Response) => {
              
   
   // this.totalCount = Response.totalCount
    this.AuditorReportDetailsList = Response
    console.log(Response);
   
    //console.log(this.NaceCodeList)

  })
 

}
AuditorListReportSearch(){
   
  // if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
  // {  abp.message.error("From-Date can not be Empty","Alert")
  // return
  // // MesseageError="Version is Empty";
  // }
  // if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
  // {  abp.message.error("To-Date can not be Empty","Alert")
  // return
  // // MesseageError="Version is Empty";
  // }
     
    this.pagedDto.pageSize = 100
    this.AuditorListsReport =[];
    this.ReportName="Auditor List";
    this._AuditorService.GetAuditorListReport(this.filterForm.value).subscribe((Response) => {
                
     
       this.totalCount = Response.totalCount
      this.AuditorListsReport = Response.sp_AuditorListReportModel;
      
      //console.log(this.NaceCodeList)
    })
}

AuditorValidationScheduleReportSearch(){
   
  if(parseInt(this.filterForm.get('StandardId').value) > 0 )
  { 

     
    this.pagedDto.pageSize = 100
    this.AuditorScheduleList=[];
    this.ReportName="Auditor Validation Schedule";
    this._AuditorService.GetAuditorScheduleReport(this.filterForm.value).subscribe((Response) => {
                
     
       this.totalCount = Response.totalCount
      this.AuditorScheduleList = Response.sp_AuditorScheduleReportModel;
   
      //console.log(this.NaceCodeList)
    })

  }
  else { abp.message.error("Standard can not be Empty and All","Alert")
  return
  // MesseageError="Version is Empty";
  }
}

ScheduleOfAuditReportSearch(){
   
  if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
  {  abp.message.error("From-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
  {  abp.message.error("To-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(parseInt(this.filterForm.get('StandardId').value) == 7 )
  {  
     
    this.pagedDto.pageSize = 100
    this.ScheduleOfAuditList =[];
    this.ReportName="Schedule Of Audit";
    this._AuditorService.GetScheduleOfAuditReport(this.filterForm.value).subscribe((Response) => {
                
     
       this.totalCount = Response.totalCount
      this.ScheduleOfAuditList = Response.sp_ScheduleOfAuditReportModel;
     
      //console.log(this.NaceCodeList)
    })
  }
  else{
    abp.message.error("This Report generated For SA8000","Please Select Standard SA8000 ")
    return
    // MesseageError="Version is Empty";
    }
}

ImpartialityReviewReportSearch(){
   
  if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
  {  abp.message.error("From-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
  {  abp.message.error("To-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }

     
    this.pagedDto.pageSize = 100
    this.ImpartialityReviewList = [];
    this.ReportName="Impartiality Review";
    this._AuditorService.GetImpartialityReviewReport(this.filterForm.value).subscribe((Response) => {
                
     
       this.totalCount = Response.totalCount
      this.ImpartialityReviewList = Response.sp_ImpartialityReviewReportModel;
    
      //console.log(this.NaceCodeList)
    })
}


loadAllCertification(): void {
      
  this.LibraryResourceService.getAllCertification().subscribe((Response)=>{
    this.CertificationList = Response

     const item2 = {
     id: 0,
     name:'--- ALL ---',
  };
   
     this.CertificationList.push(item2);
     this.standardList= this.CertificationList.sort((a, b) => {
      return a.id - b.id;
    });
      
  })
}

// loadReportsList() : void 
// {

//   const item2 = {
//     id: 1,
//     name:'--- Auditor Audit Report ---',
//  };
//  this.ReportsList.push(item2);
// }
addItem(newItem: []) {
    
  this.AuditorReportDetailsList = newItem;
  this.dataGrid.instance.refresh()

}



CertifiedClientSearch(){
   
  if(this.filterForm.get('fromdate').value ==null ||this.filterForm.get('fromdate').value==undefined|| this.filterForm.get('fromdate').value=="")
  {  abp.message.error("From-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(this.filterForm.get('todate').value ==null ||this.filterForm.get('todate').value==undefined|| this.filterForm.get('todate').value=="")
  {  abp.message.error("To-Date can not be Empty","Alert")
  return
  // MesseageError="Version is Empty";
  }
  if(parseInt(this.filterForm.get('StandardId').value) == 7 )
  {  
     
    this.pagedDto.pageSize = 100
    this.ScheduleOfAuditList =[];
    this.ReportName="Certified Client Monitoring";
    this._AuditorService.GetCertifiedClientReport(this.filterForm.value).subscribe((Response) => {
                

       
      this.CertifiedClientList = Response;
      console.log(Response);
     
      //console.log(this.NaceCodeList)
    })
  }
  else{
    abp.message.error("This Report generated For SA8000","Please Select Standard SA8000 ")
    return
    // MesseageError="Version is Empty";
    }
}
}
