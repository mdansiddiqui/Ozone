import { ClientAuditVisitService } from "@shared/Services/Client-Audit-visit-service";
import { Component, OnInit } from "@angular/core";
import { PagedRequestModel } from "@shared/Dto/paged-Request-model";
import { exportDataGrid } from "devextreme/excel_exporter";
import { Workbook } from "exceljs";
import { saveAs } from "file-saver-es";
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators,
} from "@angular/forms";
import { ProjectAmountReportsService } from 'shared/Services/Project-Amount-Reports-service';
import { LibraryResourceService } from '@shared/Services/library-Resource_service';
import { Router } from "@angular/router";
@Component({
  selector: "app-all-visit",
  templateUrl: "./all-visit.component.html",
  styleUrls: ["./all-visit.component.css"],
})
export class AllVisitComponent implements OnInit {
  // AllClientVisitForm = new FormGroup({
  //   StartDate: new FormControl(""),
  //   EndDate: new FormControl(""),
  //   VisitLevel: new FormControl(""),
  //   VisitStatus: new FormControl(""),
  // });
  filterForm = new FormGroup({
    fromdate: new FormControl(''),
    todate: new FormControl(''),
    OrganizationId: new FormControl(''),
    StandardId: new FormControl(''),
    StatusId: new FormControl(''),
    VisitLevelId: new FormControl(''),
    VisitStatusId: new FormControl(''),
  });
  constructor(private _ClientAuditVisitService: ClientAuditVisitService,
    public ProjectAmountReportsService: ProjectAmountReportsService,
    public LibraryResourceService: LibraryResourceService,
    private router : Router) {
    this.Downloadfile = this.Downloadfile.bind(this);
    this.review1=this.review1.bind(this);
  }

  ngOnInit(): void {
    this.LoadVisitLevel();
    this.LoadVisitStatus();
    this.loadAllCertification();
    this.loadAgency();
    var roleId = localStorage.getItem("roleId");
    var organizationId = localStorage.getItem("organizationId");
    if(parseInt(roleId) !=2 && parseInt(roleId)!=21)
    {
      this.filterForm.controls.OrganizationId.setValue(organizationId);
      this.filterForm.get('OrganizationId').disable();
      this.filterForm.controls.OrganizationId.setValue(organizationId);
    }
  }
  public totalCount: number;
  public pagedDto: PagedRequestModel = new PagedRequestModel();
  public AllClientVisit = [];
  public isAddShown: boolean;
  public formVisitLevelName: string;
  public formVisitStatusName: string;
  public formStartDate: string;
  public formEndDate: string;
  public TemporaryStorage = [];
  public VisitLevelList = [];
  public VisitStatusList = [];
  public OrganizationList = [];
  public standardList = [];
  public CertificationList = [];
  // OnSearch() {
  //   this.AllClientVisit = [];
  //   this.TemporaryStorage = [];
  //   this.formVisitLevelName = ''
  //   this.formVisitStatusName = ''
  //   this.formStartDate = ''
  //   this.formEndDate = ''
  //   this.formVisitLevelName =this.AllClientVisitForm.get("VisitLevel").value.toLowerCase();
  //   this.formVisitStatusName =this.AllClientVisitForm.get("VisitStatus").value.toLowerCase();
  //   this.formStartDate = new Date(this.AllClientVisitForm.get("StartDate").value).toLocaleDateString().toLowerCase();
  //   this.formEndDate = new Date(this.AllClientVisitForm.get("EndDate").value).toLocaleDateString().toLowerCase();

  //   if (this.AllClientVisitForm.invalid) {
  //     abp.message.error("All fields are required ");
  //     return;
  //   } else {
  //     let organizationId = parseInt(localStorage.getItem("organizationId"));
  //     this._ClientAuditVisitService
  //       .GetClientAuditVisitBySearch(this.AllClientVisitForm.value).subscribe((Response) => {
          
  //         this.AllClientVisitForm=Response;
          
  //       });

  //     this.resetFormControls();
  //   }
  // }
  
  review1(e)
  {debugger
   
    ///#/app/pages/sales/audit-plan?ProjectId=' + this.AllVisitDataList[_i].projectId + "&StandardId=" + this.AllVisitDataList[_i].standardId + "&ClientId=" + this.AllVisitDataList[_i].clientId + "&AuditVisitId=" + this.AllVisitDataList[_i].id
    // this.router.navigateByUrl('/app/pages/sales/audit-plan?'+e.row.data.projectId);
    this.router.navigateByUrl('/app/pages/sales/audit-plan?ProjectId=' + e.row.data.projectId + "&StandardId=" + e.row.data.standardId + "&ClientId=" + e.row.data.clientId + "&AuditVisitId=" + e.row.data.id);
    
  }
  LoadVisitLevel(): void {


    this._ClientAuditVisitService.GetALLVisitLevel().subscribe((Response) => {
      this.VisitLevelList = Response

    })
  }
  LoadVisitStatus(): void {
    

    this._ClientAuditVisitService.GetALLVisitStatus().subscribe((Response) => {
      this.VisitStatusList = Response
      const item2 = {
        id: 0,
        name:'--- ALL ---',
     };
     debugger
    //  this.VisitStatusList.push(item2);
    //     this.VisitStatusList.push(result);
        this.VisitStatusList.push(item2);
        this.VisitStatusList= this.VisitStatusList.sort((a, b) => {
         return a.id - b.id;
       });
    })
  }
  loadAgency(): void {
    debugger
    this.ProjectAmountReportsService.getAllAgency().subscribe((Response) => {
      this.OrganizationList = Response

      const item2 = {
        id: 0,
        name:'--- ALL ---',
     };
     debugger
        this.OrganizationList.push(item2);
        this.OrganizationList= this.OrganizationList.sort((a, b) => {
         return a.id - b.id;
       });
         
    })
  }
  loadAllCertification(): void {
      
    this.LibraryResourceService.getAllCertification().subscribe((Response)=>{
      this.CertificationList = Response
  
       const item2 = {
       id: 0,
       name:'--- ALL ---',
    };
    debugger
       this.CertificationList.push(item2);
       this.standardList= this.CertificationList.sort((a, b) => {
        return a.id - b.id;
      });
        
    })
  }
  Search(){
  debugger
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
  
    debugger
    //this.pagedDto.pageSize = 
    this.AllClientVisit =[];
    //this.totalCount =0;
    //this.ReportName="Client Projects";
    this._ClientAuditVisitService.GetClientAuditVisitBySearch(this.filterForm.value).subscribe((Response) => {
                
    debugger
      // this.totalCount = Response.totalCount
       this.AllClientVisit=Response;
   
      //console.log(this.NaceCodeList)
    })
   
}
  // resetFormControls() {
  //   for (const controlName of Object.keys(this.AllClientVisitForm.controls)) {
  //     const control = this.AllClientVisitForm.get(controlName);
  //     control.setValue("");
  //   }
  // }

  id: number;
  onExporting(e) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet("Client Audits List");

    exportDataGrid({
      component: e.component,
      worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(
          new Blob([buffer], { type: "application/octet-stream" }),
          "Client Audits List" + ".xlsx"
        );
      });
    });
    e.cancel = true;
  }

  Downloadfile(e): void {
    this.id = e.row.data.id;
    var fullpath = e.row.data.auditPlanFilePath;
    var filename = fullpath.replace(/^.*[\\\/]/, "");

    this._ClientAuditVisitService
      .clientAuditVisitDownloadFile(this.id)
      .subscribe((result: Blob) => {
        const Blb = new Blob([result], { type: result.type });

        const a = document.createElement("a");
        a.setAttribute("style", "display1:none;");
        document.body.appendChild(a);
        a.download = filename;

        a.href = URL.createObjectURL(Blb);
        a.target = "_blank";
        a.click();
        document.body.removeChild(a);
      });
  }
  reloadGrid() {
    this.pagedDto.page = 1;
  }
  onTableDataChange(event) {
    this.pagedDto.page = event;
  }
  ClientFields = [
    {
      name: "StartDate",
      label: "StartDate",
      type: "date",
      options: null,
    },
    {
      name: "EndDate",
      label: "EndDate",
      type: "date",
      options: null,
    },
    {
      name: "VisitLevel",
      label: "VisitLevel",
      type: "text",
      options: null,
    },
    {
      name: "VisitStatus",
      label: "VisitStatus",
      type: "text",
      options: null,
    },
    // ... add more fields here
  ];
}
