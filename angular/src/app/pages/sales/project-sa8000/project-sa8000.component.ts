//T.S

import { ActivatedRoute } from "@angular/router";

//for test file
import {
  HttpClient,
  HttpEventType,
  HttpHeaders,
  HttpResponse,
} from "@angular/common/http";
//import { EventEmitter, Injectable, InjectionToken } from "@angular/core";
//import { AppConsts } from "../AppConsts";
//import { LibraryResourceModel } from "../Dto/Library-Resource_model";
import { BehaviorSubject, Observable } from "rxjs";
import { TagContentType } from "@angular/compiler";
import { Content } from "@angular/compiler/src/render3/r3_ast";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
  OnChanges,
} from "@angular/core";
import { create } from "lodash-es";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { RoleFormService } from "@shared/Services/sec-role-service";
import { RoleFormModel } from "@shared/Dto/role-form-model";
import { Router } from "@angular/router";
import { SecRoleFormModel } from "@shared/Dto/sec-role-form-model";
import { MakerAuthorizerFormService } from "@shared/Services/maker-authorizer-form.service";
import { TabsetComponent } from "ngx-bootstrap/tabs";
import { UserStandardService } from "shared/Services/User-Standard-service";
import { ToastrService } from "ngx-toastr";
import { DxListModule } from "devextreme-angular";
import {
  DxDataGridModule,
  DxDataGridComponent,
  DxSpeedDialActionModule,
  DxSelectBoxModule,
} from "devextreme-angular";
import { DxTreeListModule } from "devextreme-angular";
import { Employee, EmployeesService } from "@app/pages/sales/employees.service";
import DataSource from "devextreme/data/data_source";
import { BrowserModule } from "@angular/platform-browser";
import { DxContextMenuModule } from "devextreme-angular";
import { PagedRequestModel } from "@shared/Dto/paged-Request-model";
import dxNumberBox from "devextreme/ui/number_box";
import { DatePipe } from "@angular/common";
import { UserAuditModel } from "@shared/Dto/user-audit-model";
import { SA8000Service } from "@shared/Services/project-SA8000-service";
import { ClientService } from "@shared/Services/Client-Service";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import pdfmake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";
import { Injector, QueryList, ViewChildren } from "@angular/core";
import {
  animate,
  state,
  style,
  transition,
  trigger,
} from "@angular/animations";
import { max } from "rxjs/operators";
import { SlcpService } from "@shared/Services/project-slcp-service";
import { HiggService } from "@shared/Services/project-higg-service";
import { ClientAuditVisitService } from "@shared/Services/Client-Audit-visit-service";
import { log } from "console";

@Component({
  selector: "app-project-sa8000",
  templateUrl: "./project-sa8000.component.html",
  styleUrls: ["./project-sa8000.component.css"],
})
export class ProjectSA8000Component implements OnInit {
  @Input() AuditId: number;
  // public UserAudit: UserAuditModel = new UserAuditModel();
  SAForm = new FormGroup({
    Code: new FormControl(""),
    ClientSiteId: new FormControl(""),
    LegalStatus: new FormControl(""),
    OutsourceProductionProcessess: new FormControl(""),
    TotalEmployees: new FormControl(""),
    ShiftTimings: new FormControl(""),
    VerificationTypeId: new FormControl(""),
    Address: new FormControl(""),
    // CountryId: new FormControl(''),
    // StateId: new FormControl(''),
    // CityId: new FormControl(''),
    ProjectTypeId: new FormControl(""),
    ProjectTypeName: new FormControl(""),
    // RiskId: new FormControl(''),
    // EacodeId: new FormControl(''),
    // NaceCodeId: new FormControl(''),
    ConsultantId: new FormControl(""),
    AccreditationId: new FormControl(""),
    Scope: new FormControl(""),
    PreAuditDuration: new FormControl(""),
    DurationStage1: new FormControl(""),
    DurationStage2: new FormControl(""),
    DurationSurvVisit: new FormControl(""),
    SurveillanceVisitFrequencyId: new FormControl(""),
    SurveillanceMethodId: new FormControl(""),
    NoOfSurveillanceVisits: new FormControl(""),
    Applicatonfee: new FormControl(""),
    PreAuditfee: new FormControl(""),
    Assessmentfee: new FormControl(""),
    Survfee: new FormControl(""),
    ExpensesId: new FormControl(""),
    Remarks: new FormControl(""),
    ApprovalStatusId: new FormControl(""),
    ProjectCode: new FormControl(""),
    ApplicationFee: new FormControl(""),
    AuditFee: new FormControl(""),
    SurveillanceAudits: new FormControl(""),
    tax: new FormControl(""),
    ContractStatusId: new FormControl(""),
    ContractRemarks: new FormControl(""),
    OverAllEmployees: new FormControl(""),
    RiskName: new FormControl(""),
    Eacodename: new FormControl(""),
    NaceCodeName: new FormControl(""),
    ProjectRemarks: new FormControl(""),
    VisitStatusId: new FormControl(""),
    ConsultancyName: new FormControl(""),
  });

  SAClientChangeRequest = new FormGroup({
    Scope: new FormControl(""),
    DurationStage2: new FormControl(""),
    SurveillanceVisitFrequencyId: new FormControl(""),
    SurveillanceMethodId: new FormControl(""),
    NoOfSurveillanceVisits: new FormControl(""),
    Assessmentfee: new FormControl(""),
    DurationSurvVisit: new FormControl(""),
    Survfee: new FormControl(""),
    ConsultantId: new FormControl(""),
  });

  get f() {
    return this.SAForm.controls;
  }
  datePipe = new DatePipe("en-US");

  @Input() formName: string;
  submitted = false;
  secRoleForm;
  tableSizes = [3, 5, 10, 15, 20, 25];
  public totalCount: number;
  public pagedDto: PagedRequestModel = new PagedRequestModel();
  pageNumber: number = 1;
  pageSize: number = 10;
  public SiteCount: string = "";
  // public EACodeName: string = ''
  // public NaceCodeName: string = ''
  // public RiskName: string = ''

  public isEditShown: boolean;
  public isViewShown: boolean;
  public isAddShown: boolean;
  public IsContract: boolean;
  public keyword: string = "";
  public savebtn: boolean = true;
  public deletebtn: boolean = false;
  public btnApproval: boolean = false;
  public btnChnageRequest: boolean = false;
  public RoleBaseHide: boolean = false;
  public ApprovalList = [];
  public NaceCodeList = [];
  public savedownload: boolean = false;
  public ClientSiteList = [];
  public VerificationTypeList = [];
  public Sa8000ProjectTypeList = [];
  public ProjectTypeList = [];
  public RiskList = [];
  public EACodeList = [];
  public NaceCodeId = [];
  public ProjectCode: string;
  public ApplicationfileName: string;
  public ContarctFileName: string;
  public ProjectStatus: string;
  public ConsultantId: number;

  public Consultancy: boolean = false;
  //public CompletedSetupList = [];
  //public MethodologyList = [];
  public ConsultantList = [];
  public AccreditationList = [];
  public SurveillanceVisitFrequencyList = [];
  public SurveillanceMethodList = [];
  public ExpensesList = [];
  public ContractApprovalList = [];
  public OrganizationId: number;
  public standardId: number;
  public ProjectId: number;
  public ClientId: number;
  public SiteId: number = 0;
  public isShown: boolean = false;
  public authorizer: boolean = false;
  public Multisite: boolean = false;
  fileToUpload: any;
  fileToUploadSa8000: any;
  ContractfileToUploadSa8000: any;
  clientinfo: any;
  projectinfo: any;
  clientsite: any;

  // public ProjectCode:string
  public TransferProjectList = [
    { id: "1", code: "Yes" },
    { id: "0", code: "No" },
  ];
  public UserStatusList = [];
  public btnContract: boolean = false;
  public btnContractSave: boolean = false;
  public btnContractDownload: boolean = false;
  public Inserted: boolean = false;
  public Revieweruser: boolean = false;
  public Registration: string;

  // public reviwerData :any
  readonly allowedPageSizes = [5, 10, "all"];
  readonly displayModes = [
    { text: "Display Mode 'full'", value: "full" },
    { text: "Display Mode 'compact'", value: "compact" },
  ];
  displayMode = "full";
  showPageSizeSelector = true;
  showInfo = true;
  showNavButtons = true;
  private userUpdateId: number;

  customizeColumns(columns) {
    columns[0].width = 70;
  }
  get isCompactMode() {
    return this.displayMode === "compact";
  }

  constructor(
    //  private http: HttpClient,
    private _HiggService: HiggService,
    private _UserStandardService: UserStandardService,
    private _SA8000Service: SA8000Service,
    private _ClientService: ClientService,
    private _toster: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private _makerAuthorizerFormService: MakerAuthorizerFormService,
    private _SlcpService: SlcpService,
    private _ClientAuditVisitService: ClientAuditVisitService
  ) //public StandardService: StandardService
  {
    // pdfmake.vfs = pdfFonts.pdfmake.vfs;
    //this.Pdf=this.Pdf.bind(this);
    // this.edit = this.edit.bind(this);
  }
  RoleId: number;
  ngOnInit(): void {
    var roleId = localStorage.getItem("roleId");
    this.RoleId = parseInt(roleId);

    if (+roleId === 2) {
      this.RoleBaseHide = true;
    }
    this.editProject();
    //  this.loadSecRoleForm()
    this.CheckReviewer();
    this.loadStatus();
    //this.editProject()
    //this.loadNaceCode();
    this.loadEaCode();
    //this.onSearch();
    this.loadProjectType();
    this.loadVerificationType();
    this.loadRisk();
    this.loadAccreditation();
    this.loadGetALlSurveillanceMethod();
    this.loadSurveillanceVisitFrequency();
    this.loadExpences();
    this.loadAllApprovalStatus();

    this.loadClientSites();
  }
  ngAfterViewInit(): void {
    if (this.ProjectId > 0) {
      this.editSA8000User();
      this.UpdateProject();
    } else {
      this.loadConsultant();
    }
  }
  displayStyle = "none";
  datadisplay = "none";

  Popup() {
    this.datadisplay = "block";
  }

  close() {
    this.datadisplay = "none";
  }

  openPopup() {
    this.displayStyle = "block";
  }

  closePopup() {
    this.displayStyle = "none";
  }

  editSA8000User() {
    this._SA8000Service
      .GetProjectSA8000BYId(this.ProjectId)
      .subscribe((data) => {
        debugger;
        var ProjectSaDataSa = data.projectSA8000Model;
        debugger;
        console.log("Change Request    " + ProjectSaDataSa);

        this.SAClientChangeRequest.controls.DurationStage2.setValue(
          ProjectSaDataSa.durationStage2
        );
        this.SAClientChangeRequest.controls.NoOfSurveillanceVisits.setValue(
          ProjectSaDataSa.noOfSurveillanceVisits
        );
        this.SAClientChangeRequest.controls.Scope.setValue(
          ProjectSaDataSa.scope
        );
        this.SAClientChangeRequest.controls.SurveillanceMethodId.setValue(
          ProjectSaDataSa.surveillanceMethodId
        );
        this.SAClientChangeRequest.controls.SurveillanceVisitFrequencyId.setValue(
          ProjectSaDataSa.surveillanceVisitFrequencyId
        );
        this.SAClientChangeRequest.controls.Survfee.setValue(
          ProjectSaDataSa.survfee
        );
        this.SAClientChangeRequest.controls.Assessmentfee.setValue(
          ProjectSaDataSa.assessmentfee
        );
        this.SAClientChangeRequest.controls.DurationSurvVisit.setValue(
          ProjectSaDataSa.durationSurvVisit
        );
        this.SAClientChangeRequest.controls.ConsultantId.setValue(
          ProjectSaDataSa.consultantId
        );
        
      });
  }
  ChangeStatus(): void {
    console.log(
      "ProjectRemarks value:",
      this.SAForm.get("ProjectRemarks").value
    );
    console.log("VisitStatusId value:", this.SAForm.get("VisitStatusId").value);

    if (
      this.SAForm.get("VisitStatusId").value == null ||
      this.SAForm.get("VisitStatusId").value == undefined ||
      this.SAForm.get("VisitStatusId").value == "" ||
      this.SAForm.get("VisitStatusId").value == "" ||
      this.SAForm.get("VisitStatusId").value == isNaN
    ) {
      abp.message.error("Project Status is required", "Please Select Status");
      return;
    }

    var LoginUserId = localStorage.getItem("userId");
    const status: FormData = new FormData();

    if (this.ProjectId > 0) {
      status.append("Id", this.ProjectId.toString());
    }
    status.append("LastModifiedById", LoginUserId);
    status.append("Remarks", this.SAForm.get("ProjectRemarks").value);
    status.append("ApprovalStatusId", this.SAForm.get("VisitStatusId").value);

    this._HiggService.ProjectStatusChange(status).subscribe((Response) => {
      // abp.message.info(Response.message)
      if (Response.message == "1") {
        abp.message.info("Successfully Saved!");
        this.router.navigateByUrl("/app/home");
      } else if (Response.message == "2") {
        abp.message.info(
          "Project Status not set in Status Amount form for this Standard!"
        );
        ///this.router.navigateByUrl('/app/pages/sales/all-projects?'+this.ClientId);
      } else if (Response.message == "0") {
        abp.message.error("Not Inserted!");
      }
      //this.router.navigateByUrl('/app/pages/sales/all-projects?'+this.ClientId);
    });
  }
  onSubmitChangeRequest() {
    this.submitted = true;

    if (this.SAClientChangeRequest.invalid) {
      return;
    }
    this.SA8000ChangeRequest();
  }

  SA8000ChangeRequest(): void {
    const foData: FormData = new FormData();
    // const ProjectSA8000CreateModel = {
    //   Id:this.id,
    //   DurationStage2: this.SAClientChangeRequest.get("DurationStage2").value,
    //   NoOfSurveillanceVisits: this.SAClientChangeRequest.get("NoOfSurveillanceVisits").value,
    //   Scope: this.SAClientChangeRequest.get("Scope").value,
    //   SurveillanceMethodId: this.SAClientChangeRequest.get("SurveillanceMethodId").value,
    //   SurveillanceVisitFrequencyId: this.SAClientChangeRequest.get("SurveillanceVisitFrequencyId").value,
    //   Survfee: this.SAClientChangeRequest.get("Survfee").value,
    //   Assessmentfee: this.SAClientChangeRequest.get("Assessmentfee").value,
    //   CreatedById: localStorage.getItem("userId"),
    //   OrganizationId: localStorage.getItem('organizationId'),
    //   FormName:"ProjectSA8000",
    //   File:this.fileToUploadSa8000

    // };

    Object.keys(this.SAClientChangeRequest.controls).forEach((key) => {
      if (
        this.SAClientChangeRequest.controls[key].value != null &&
        this.SAClientChangeRequest.controls[key].value != "" &&
        this.SAClientChangeRequest.controls[key].value != undefined &&
        this.SAClientChangeRequest.controls[key].value != isNaN
      ) {
        var sname = key;
        //var sname= this.SAForm.controls[key].;
        var val = this.SAClientChangeRequest.controls[key].value;

        foData.append(sname, val);
      }
    });

    foData.append("Id", this.id.toString());
    // foData.append("DurationStage2",this.SAClientChangeRequest.get('DurationStage2').value);
    // foData.append("NoOfSurveillanceVisits",this.SAClientChangeRequest.get('NoOfSurveillanceVisits').value);
    // foData.append("Scope",this.SAClientChangeRequest.get('Scope').value);
    // foData.append("SurveillanceMethodId",this.SAClientChangeRequest.get('SurveillanceMethodId').value);
    // foData.append("SurveillanceVisitFrequencyId",this.SAClientChangeRequest.get('SurveillanceVisitFrequencyId').value);
    // foData.append("Survfee",this.SAClientChangeRequest.get('Survfee').value);
    // foData.append("Assessmentfee",this.SAClientChangeRequest.get('Assessmentfee').value);
    foData.append("FormName", "ProjectSA8000");
    foData.append("File", this.fileToUploadSa8000);
    foData.append("ContractForm", this.ContractfileToUploadSa8000);

    var OrgId = localStorage.getItem("organizationId");

    foData.append("OrganizationId", OrgId);

    var userId = localStorage.getItem("userId");

    foData.append("CreatedById", userId);

    this._SA8000Service.SA8000ChangeRequest(foData).subscribe((Response) => {
      if (Response.message == "1") {
        abp.message.info(
          "Please Submit For Review",
          "Change Request Successfully Saved.!"
        );
        this.router.navigateByUrl(
          "/app/pages/sales/all-projects?" + this.ClientId
        );
      } else {
        abp.message.error(Response.message);
      }
    });
  }

  contractDownload(): void {
    // this.ProjectId=e.row.data.id;
    // var fillename=e.row.data.title;
    var fillename = "Document File";

    if (this.ContarctFileName != undefined && this.ContarctFileName != null) {
      fillename = this.ContarctFileName.replace(/^.*[\\\/]/, "");

      // var  fillename2=[...fillename];
      // fillename2.splice(0,37,"").join('');
    }
    this._SA8000Service
      .downloadContract(this.ProjectId)
      .subscribe((result: Blob) => {
        const Blb = new Blob([result], { type: result.type });
        // const url=window.URL.createObjectURL(Blb);
        // window.open(url);
        // console.log("success");

        const a = document.createElement("a");
        a.setAttribute("style", "display:none;");
        document.body.appendChild(a);
        a.download = fillename;
        // const fileName =

        //="farooq";
        a.href = URL.createObjectURL(Blb);
        a.target = "_blank";
        a.click();
        document.body.removeChild(a);
      });
  }

  contractApproved(): void {
    if (
      this.SAForm.get("ContractStatusId").value == null ||
      this.SAForm.get("ContractStatusId").value == undefined ||
      this.SAForm.get("ContractStatusId").value == "" ||
      this.SAForm.get("ContractStatusId").value == "" ||
      this.SAForm.get("ContractStatusId").value == isNaN
    ) {
      abp.message.error("Contract Status is required", "Please Select Status");
      return;
    }

    var LoginUserId = localStorage.getItem("userId");
    const foData: FormData = new FormData();

    if (this.ProjectId > 0) {
      foData.append("Id", this.ProjectId.toString());
    }
    foData.append("LastModifiedById", LoginUserId);

    foData.append("Remarks", this.SAForm.get("ContractRemarks").value);
    foData.append(
      "ApprovalStatusId",
      this.SAForm.get("ContractStatusId").value
    );

    this._SA8000Service.ContractApproval(foData).subscribe((Response) => {
      //abp.message.info(Response.message)

      if (Response.message == "1") {
        abp.message.info("Successfully Saved!");
        this.router.navigateByUrl("/app/home");
      } else if (Response.message == "2") {
        abp.message.info(
          "Project amount not set in Project Amount form for this Standard!"
        );
        ///this.router.navigateByUrl('/app/pages/sales/all-projects?'+this.ClientId);
      } else if (Response.message == "0") {
        abp.message.error("Not Inserted!");
      }
      //this.router.navigateByUrl('/app/pages/sales/all-projects?'+this.ClientId);
    });
  }
  CheckReviewer() {
    const UserModel = {
      StandardId: this.standardId,
      UserId: parseInt(localStorage.getItem("userId")),
    };

    this._SlcpService.GetReviewerByStandard(UserModel).subscribe((Response) => {
      var reviwerData = Response;
      if (
        reviwerData == null ||
        reviwerData == undefined ||
        reviwerData == "" ||
        reviwerData == "" ||
        reviwerData == isNaN
      ) {
        this.Revieweruser = false;
      } else if (
        parseInt(reviwerData.userId) == parseInt(localStorage.getItem("userId"))
      ) {
        this.Revieweruser = true;
      } else {
        this.Revieweruser = false;
      }
      this.loadSecRoleForm();
    });
  }
  loadSecRoleForm() {
    // let secRoleForm = JSON.parse(localStorage.getItem('secRoleForm'))
    // let permission = secRoleForm.find(x => x.formCode != null && x.formCode == this.formCode)

    // this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {

    //   let formName = (this.formName == undefined ? localStorage.getItem('formName') : this.formName)
    //   this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)

    var formName = "ProjectSA8000";
    this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {
      this.secRoleForm = data.find((x) => x.formName == formName);
      this.isShown = this.secRoleForm.authAllowed;

      if (this.Revieweruser == true && this.RoleId != 21) {
        //this.secRoleForm.insertAllowed = false
        this.secRoleForm.authAllowed = true;
      } else {
        this.secRoleForm.authAllowed = false;
      }
      if (this.secRoleForm.insertAllowed == false) {
        this.Inserted = false;
        this.SAForm.get("ClientSiteId").disable();
        //this.SAForm.get('VerificationTypeId').disable();
        this.SAForm.get("ProjectTypeName").disable();
        this.SAForm.get("ProjectTypeId").disable();
        // this.SAForm.get('RiskId').disable();
        // this.SAForm.get('EacodeId').disable();

        // this.SAForm.get('NaceCodeId').disable();
        this.SAForm.get("ConsultantId").disable();
        this.SAForm.get("AccreditationId").disable();

        this.SAForm.get("Scope").disable();
        this.SAForm.get("PreAuditDuration").disable();
        this.SAForm.get("DurationStage1").disable();
        this.SAForm.get("DurationStage2").disable();
        this.SAForm.get("DurationSurvVisit").disable();
        this.SAForm.get("SurveillanceVisitFrequencyId").disable();
        this.SAForm.get("SurveillanceMethodId").disable();
        this.SAForm.get("NoOfSurveillanceVisits").disable();
        this.SAForm.get("Applicatonfee").disable();
        this.SAForm.get("PreAuditfee").disable();
        this.SAForm.get("Assessmentfee").disable();
        this.SAForm.get("Survfee").disable();
        this.SAForm.get("ExpensesId").disable();
        //this.SAForm.get('Remarks').disable();
      }
      // if(this.secRoleForm.authAllowed ==true)
      // {
      //   this.btnApproval=true
      //   this.authorizer=true}
      // else{
      //   this.authorizer=false

      // }

      // if (this.secRoleForm.authAllowed == true) {

      //   if (this.ProjectId > 0) {
      //     this.btnApproval = true
      //   }
      //   else {
      //     this.btnApproval = false
      //   }
      //   this.authorizer = true
      // }
      // else {
      //   this.authorizer = false

      // }
      // if (this.secRoleForm.insertAllowed == true) {

      //   this.Inserted = true
      // }

      var roleId = localStorage.getItem("roleId");
      if (this.secRoleForm.insertAllowed == true) {
        this.Inserted = true;
        this.btnApproval = false;
        this.btnContractSave = false;
        this.SAForm.get("ContractStatusId").disable();
        this.SAForm.get("ApprovalStatusId").disable();
      } else if (this.Revieweruser == true) {
        this.Inserted = false;
        this.savebtn = false;
        this.IsContract = false;
        this.deletebtn = false;
        this.savebtn = false;
        if (this.ProjectId > 0 && parseInt(roleId) != 21) {
          debugger;
          //alert("652");
          this.btnApproval = true;
        } else {
          this.btnApproval = false;
        }
        this.authorizer = true;
      }
    });
  }

  editProject() {
    var ur;
    ur = window.location.href.split("/")[7];
    var com = ([] = ur.split("?")[1]);
    if (com != undefined && com != null) {
      var Parameter1 = com.split("&")[0];
      var Parameter2 = com.split("&")[1];
      var Parameter3 = com.split("&")[2];

      if (Parameter1.split("=")[0] == "ProjectId") {
        this.ProjectId = Parameter1.split("=")[1];
      } else if (Parameter1.split("=")[0] == "StandardId") {
        this.standardId = Parameter1.split("=")[1];
      } else if (Parameter1.split("=")[0] == "ClientId") {
        this.ClientId = Parameter1.split("=")[1];
      }

      if (Parameter2.split("=")[0] == "StandardId") {
        this.standardId = Parameter2.split("=")[1];
      } else if (Parameter2.split("=")[0] == "ProjectId") {
        this.ProjectId = Parameter2.split("=")[1];
      } else if (Parameter2.split("=")[0] == "ClientId") {
        this.ClientId = Parameter2.split("=")[1];
      }

      if (Parameter3.split("=")[0] == "StandardId") {
        this.standardId = Parameter2.split("=")[1];
      } else if (Parameter3.split("=")[0] == "ProjectId") {
        this.ProjectId = Parameter2.split("=")[1];
      } else if (Parameter3.split("=")[0] == "ClientId") {
        this.ClientId = Parameter3.split("=")[1];
      }
    }
    if (this.ProjectId == 0) {
      this.btnChnageRequest = false;
      this.ProjectId = 0;
      this.btnApproval = false;
      this.ProjectStatus = "New Project";
    }
    this.OrganizationId = parseInt(localStorage.getItem("organizationId"));
    if (this.ProjectId > 0 && this.OrganizationId > 1 && this.RoleId == 6) {
      this.btnChnageRequest = true;
    }

    // else {
    //   this.ProjectId = 0;
    //   this.btnApproval = false
    this.ClientData();
    // }

    this.OrganizationId = parseInt(localStorage.getItem("organizationId"));

    this.savebtn = true;
  }

  UpdateProject() {
    this._SA8000Service
      .GetProjectSA8000BYId(this.ProjectId)
      .subscribe((data) => {
       // alert("735 Update");
        debugger;
        console.log("Testing Sa800");
        console.log("Testing Sa800");
        this.Registration = data.clientProjectModel.registration_no;
        console.log(this.Registration);
        console.log(this.Registration);
        console.log(data);
        var ProjectSaData = data.projectSA8000Model;
        this.projectinfo = data.clientSitesModel;
        var ClientProjectMod = data.clientProjectModel;
        this.ConsultantId = ProjectSaData.consultantId;
        //this.UserAuditList= data
        this.ApplicationfileName = ProjectSaData.applicationFormPath;
        this.ContarctFileName = ClientProjectMod.contractFilePath;
        this.ProjectCode = ClientProjectMod.projectCode;
        this.standardId = ClientProjectMod.standardId;
        this.ClientId = ClientProjectMod.clientId;
        this.OrganizationId = ClientProjectMod.agencyId;

        if (
          data.clientSitesModel != null &&
          data.clientSitesModel != "" &&
          data.clientSitesModel != "" &&
          data.clientSitesModel != undefined &&
          data.clientSitesModel != isNaN
        ) {
          var ClientSiteData = data.clientSitesModel;
          this.clientsite = data.clientSitesModel;
          this.SAForm.controls.Address.setValue(ClientSiteData.address);
          this.SAForm.controls.OutsourceProductionProcessess.setValue(
            ClientSiteData.outsourceProductionProcessess
          );
          this.SAForm.controls.ShiftTimings.setValue(
            ClientSiteData.shiftTimings
          );

          this.SAForm.controls.LegalStatus.setValue(ClientSiteData.legalStatus);
          this.SAForm.controls.TotalEmployees.setValue(
            ClientSiteData.totalEmployees
          );

          this.SAForm.controls.ClientSiteId.setValue(ClientSiteData.id);
          this.SiteId = ClientSiteData.id;
        }

        //this.SAForm.controls.VerificationTypeId.setValue(ClientProjectMod.verificationTypeId);
        //this.SAForm.controls.ProjectTypeName.setValue(ClientProjectMod.projectTypeName);
        // this.SAForm.controls.ProjectTypeId.setValue(ClientProjectMod.projectTypeId);
        // this.SAForm.controls.ProjectTypeId.setValue(ClientProjectMod.projectTypeId);
        if (ClientProjectMod.projectTypeId == 1) {
          this.SAForm.controls.ProjectTypeId.setValue(
            ClientProjectMod.projectTypeId
          );
        } else {
          this.SAForm.controls.ProjectTypeId.setValue(0);
        }

        console.log(ClientProjectMod);
        this.id = ProjectSaData.id;
        this.SAForm.controls.ProjectTypeName.setValue(
          ClientProjectMod.projectTypeName
        );
        this.SAForm.controls.AccreditationId.setValue(
          ProjectSaData.accreditationId
        );
        this.SAForm.controls.Applicatonfee.setValue(
          ProjectSaData.applicatonfee
        );
        this.SAForm.controls.Assessmentfee.setValue(
          ProjectSaData.assessmentfee
        );
        //this.SAForm.controls.ConsultantId.setValue(ProjectSaData.consultantId)

        this.ConsultantList = [];
        debugger;
        //this._SA8000Service.GetAllConsultantList(ClientProjectMod.agencyId).subscribe((Response) => {
        this.ConsultantList = data.consultantModels;
        this.SAForm.controls.ConsultantId.setValue(ProjectSaData.consultantId);
        this.SAForm.controls.ConsultancyName.setValue(
          ProjectSaData.consultantName
        );
        // this.Consultancy=true;
        if (
          ClientProjectMod.approvalStatusId != 5 &&
          ClientProjectMod.approvalStatusId != 6
        ) {
          this.Consultancy = true;
        }
        var RoleId = parseInt(localStorage.getItem("roleId"));
        if (RoleId == 2 || RoleId == 21 || RoleId == 12) {
          this.Consultancy = true;
        }

        //   if(this.ProjectId>0)
        //   {
        //   this.UpdateProject();
        // }
        //})

        this.SAForm.controls.DurationStage1.setValue(
          ProjectSaData.durationStage1
        );
        this.SAForm.controls.DurationStage2.setValue(
          ProjectSaData.durationStage2
        );
        this.SAForm.controls.DurationSurvVisit.setValue(
          ProjectSaData.durationSurvVisit
        );

        // this.SAForm.controls.EacodeId.setValue(ProjectSaData.eacodeId);
        // this.loadNaceCode(ProjectSaData.eacodeId);
        this.SAForm.controls.ExpensesId.setValue(ProjectSaData.expensesId);
        // this.SAForm.controls.NaceCodeId.setValue(ProjectSaData.naceCodeId)
        //this.loadRisk(ProjectSaData.naceCodeId);
        this.SAForm.controls.NoOfSurveillanceVisits.setValue(
          ProjectSaData.noOfSurveillanceVisits
        );

        this.SAForm.controls.PreAuditDuration.setValue(
          ProjectSaData.preAuditDuration
        );
        this.SAForm.controls.PreAuditfee.setValue(ProjectSaData.preAuditfee);
        this.SAForm.controls.Remarks.setValue(ProjectSaData.remarks);
        // this.SAForm.controls.RiskId.setValue(ProjectSaData.riskId);
        this.SAForm.controls.Scope.setValue(ProjectSaData.scope);

        this.SAForm.controls.SurveillanceMethodId.setValue(
          ProjectSaData.surveillanceMethodId
        );

        this.SAForm.controls.SurveillanceVisitFrequencyId.setValue(
          ProjectSaData.surveillanceVisitFrequencyId
        );
        this.SAForm.controls.ApprovalStatusId.setValue(
          ClientProjectMod.approvalStatusId
        );
        this.ProjectStatus = "  " + ClientProjectMod.approvalStatusName;
        this.SAForm.controls.Survfee.setValue(ProjectSaData.survfee);
        this.SAForm.controls.ProjectCode.setValue(ClientProjectMod.projectCode);
        this.ProjectCode = ClientProjectMod.projectCode;

        // alert("Update"+ProjectSaData.clientId);
        this._ClientService
          .GeClientDatabyId(this.ClientId)
          .subscribe((Response) => {
            this.clientinfo = Response;
          });

        if (
          this.AuditId != null &&
          this.AuditId != undefined &&
          this.AuditId > 0
        ) {
          this.btnApproval = false;
          this.btnContractSave = false;
          this.savebtn = false;
          this.IsContract = false;
          this.deletebtn = false;
          this.Inserted = false;
          this.authorizer = false;
          this.btnContractDownload = true;
          //this.savebtn=false
        } else {
          if (
            ClientProjectMod.contractFilePath != null &&
            ClientProjectMod.contractFilePath != undefined &&
            ClientProjectMod.contractFilePath != ""
          ) {
            this.btnContractDownload = true;
          }
          if (
            ClientProjectMod.approvalStatusId == 7 ||
            ClientProjectMod.approvalStatusId == 8 ||
            ClientProjectMod.approvalStatusId == 9 ||
            ClientProjectMod.approvalStatusId == 10 ||
            ClientProjectMod.approvalStatusId == 3
          ) {
            this.btnApproval = false;
          }
          //  if(ClientProjectMod.approvalStatusId==4 ||ClientProjectMod.approvalStatusId==2 )
          //  {
          //    this.savebtn=false

          //  }
          if (this.Inserted == true) {
            if (
              ClientProjectMod.approvalStatusId == 8 ||
              ClientProjectMod.approvalStatusId == 1
            ) {
              this.IsContract = true;
              this.SAForm.get("ApprovalStatusId").disable();
            }
          }
          if (this.Inserted == true && this.RoleId != 21) {
            if (
              ClientProjectMod.approvalStatusId == 8 ||
              ClientProjectMod.approvalStatusId == 7 ||
              ClientProjectMod.approvalStatusId == 1 ||
              ClientProjectMod.approvalStatusId == 10 
            ) {
              this.btnContract = true;
              this.btnContractSave = false;
              this.SAForm.get("ContractStatusId").disable();
              this.SAForm.get("ApprovalStatusId").disable();
              this.SAForm.controls.ApprovalStatusId.setValue(1);
              if (ClientProjectMod.approvalStatusId != 1) {
                this.SAForm.controls.ContractRemarks.setValue(
                  ClientProjectMod.remarks
                );
                this.SAForm.controls.ContractStatusId.setValue(
                  ClientProjectMod.approvalStatusId
                );
              }
            }
            // if (this.authorizer == true && ClientProjectMod.approvalStatusId == 9 || ClientProjectMod.approvalStatusId == 10) {
            //   this.btnContract = true
            //   this.btnContractSave = true
            //   this.SAForm.get('ApprovalStatusId').disable();
            //   this.SAForm.get('Remarks').disable();
            // }
          } else if (this.authorizer == true && this.RoleId != 21) {
            if (
              ClientProjectMod.approvalStatusId == 9 ||
              ClientProjectMod.approvalStatusId == 10
            ) {
              this.btnContract = true;
              this.btnContractSave = true;
              this.SAForm.get("ApprovalStatusId").disable();
              this.SAForm.get("Remarks").disable();
            }
          }
          if (
            ClientProjectMod.approvalStatusId == 1 ||
            ClientProjectMod.approvalStatusId == 5
          ) {
            this.btnApproval = false;
          }

          if (this.Inserted == true) {
            if (
              ClientProjectMod.approvalStatusId == 5 ||
              ClientProjectMod.approvalStatusId == 6
            ) {
              this.savebtn = true;
            } else {
              this.savebtn = false;
            }
          } else {
            this.savebtn = false;
          }
          if (
            ClientProjectMod.approvalStatusId == undefined ||
            ClientProjectMod.approvalStatusId == "" ||
            ClientProjectMod.approvalStatusId == null ||
            ClientProjectMod.approvalStatusId == 2 ||
            ClientProjectMod.approvalStatusId == 4
          ) {
            this.Inserted = false;
          }

          if (
            (this.ProjectId > 0 &&
              this.Inserted == true &&
              ClientProjectMod.approvalStatusId == 5) ||
            ClientProjectMod.approvalStatusId == 6
          ) {
            this.deletebtn = true;
          } else {
            this.deletebtn = false;
          }

          if (
            ProjectSaData.applicationFormPath != null &&
            ProjectSaData.applicationFormPath != undefined &&
            ProjectSaData.applicationFormPath != ""
          ) {
            this.savedownload = true;
          }

          if (
            parseInt(ClientProjectMod.approvalStatusId) == 1 ||
            parseInt(ClientProjectMod.approvalStatusId) > 6
          ) {
            this.SAForm.get("Remarks").disable();
            this.SAForm.controls.ApprovalStatusId.setValue(1);
            this.SAForm.get("ApprovalStatusId").disable();
          }
          if (this.Inserted == true) {
            this.btnApproval = false;
            this.btnContractSave = false;
            this.SAForm.get("ContractStatusId").disable();
            this.SAForm.get("ApprovalStatusId").disable();
          } else if (this.Revieweruser == true) {
            this.savebtn = false;
            this.IsContract = false;
            this.deletebtn = false;
            this.savebtn = false;
          }
        }

        if (
          this.AuditId != null &&
          this.AuditId != undefined &&
          this.AuditId > 0
        ) {
          this.btnApproval = false;
          this.btnContractSave = false;
          this.savebtn = false;
          this.IsContract = false;
          this.deletebtn = false;
          this.Inserted = false;
          this.authorizer = false;
          //this.savebtn=false
        }
        // if(ClientProjectMod.approvalStatusId==1)
        // {
        //   this.btnApproval=false
        //  }
        //    if(ClientProjectMod.approvalStatusId==5 ||ClientProjectMod.approvalStatusId==6 && this.authorizer==false)
        //    {
        //      this.savebtn=true
        //     }
        //     else
        //     {
        //       this.savebtn=false
        //      }

        //  if(this.ProjectId>0 && this.authorizer==false && ClientProjectMod.approvalStatusId==5 || ClientProjectMod.approvalStatusId==6 )
        //  {
        //    this.deletebtn=true;

        //  }
        //  else
        //  {
        //    this.deletebtn=false;
        //  }

        //    if(ProjectSaData.applicationFormPath!=null && ProjectSaData.applicationFormPath!=undefined && ProjectSaData.applicationFormPath!="")
        //    {
        //      this.savedownload=true;
        //    }
        this.loadClientSites();

        this._ClientAuditVisitService
          .GetProjectStatus(this.ProjectId)
          .subscribe((Response) => {
            if (Response != "0") {
              this.ProjectStatus = Response;
              this.SAForm.disable();
              if(RoleId==2)
              {
                this.SAForm.get("ProjectRemarks").enable();
                this.SAForm.get("VisitStatusId").enable();
              }

              //alert(Response);
            }
          });
          if(RoleId==2)
          {
            this.SAForm.get("ProjectRemarks").enable();
            this.SAForm.get("VisitStatusId").enable();
          }
        debugger;
        //this.SAForm.controls.ApprovalStatusId.setValue(ProjectSaData.approvalStatusId);
      });
    debugger;
  }
  loadNaceCode(eacodeId): void {
    this._UserStandardService
      .getAllNaceCodeByEaCode(eacodeId)
      .subscribe((Response) => {
        this.NaceCodeList = Response;
      });
  }

  loadAllApprovalStatus(): void {
    var roleId = localStorage.getItem("roleId");
    this._SA8000Service.GetALLProjectStatus(roleId).subscribe((Response) => {
      this.ApprovalList = Response;
    });
  }
  loadEaCode(): void {
    this._UserStandardService.getAllEACode().subscribe((Response) => {
      this.EACodeList = Response;
      let eacodeId = 0;
      this.loadNaceCode(eacodeId);
    });
  }

  loadClientSites(): void {
    if (
      this.ClientId > 0 &&
      this.ClientId != null &&
      this.ClientId != undefined
    ) {
      this.ClientSiteList = null;
      this._SA8000Service
        .GetAllClientSites(this.ClientId)
        .subscribe((Response) => {
          this.ClientSiteList = Response;
          if (this.SiteId > 0) {
            this.SAForm.controls.ClientSiteId.setValue(this.SiteId);
          }
        });
    }
  }
  loadProjectType(): void {
    this._SA8000Service.GetAllProjectType().subscribe((Response) => {
      this.ProjectTypeList = Response;
    });
  }
  loadRisk(): void {
    this._SA8000Service.GetAllRisk().subscribe((Response) => {
      this.RiskList = Response;
    });
  }

  loadAccreditation(): void {
    this._SA8000Service.GetAllAccreditation().subscribe((Response) => {
      this.AccreditationList = Response;
    });
  }
  loadVerificationType(): void {
    this._SA8000Service.GetAllVerificationType().subscribe((Response) => {
      this.VerificationTypeList = Response;
    });
  }

  loadSurveillanceVisitFrequency(): void {
    this._SA8000Service
      .SurveillanceVisitFrequencyList()
      .subscribe((Response) => {
        this.SurveillanceVisitFrequencyList = Response;
      });
  }
  loadGetALlSurveillanceMethod(): void {
    this._SA8000Service.GetALlSurveillanceMethod().subscribe((Response) => {
      this.SurveillanceMethodList = Response;
    });
  }
  loadConsultant(): void {
    debugger;
    this.OrganizationId = parseInt(localStorage.getItem("organizationId"));
    this._SA8000Service
      .GetAllConsultantList(this.OrganizationId)
      .subscribe((Response) => {
        //this.ConsultantList = Response
        if (this.ConsultantId > 0) {
          this.SAForm.controls.ConsultantId.setValue(this.ConsultantId);
        } else {
          this.ConsultantList = Response;
        }
        //   if(this.ProjectId>0)
        //   {
        //   this.UpdateProject();
        // }
      });
  }
  loadExpences(): void {
    this._SA8000Service.GetALlExpences().subscribe((Response) => {
      this.ExpensesList = Response;
    });
  }
  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.SAForm.invalid) {
      return;
    }
    this.onSubmit1();
    // display form values on success
    //alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.SAForm.value, null, 4));
  }
  onSubmit1(): void {
    var LoginUserId = localStorage.getItem("userId");
    const foData: FormData = new FormData();

    if (this.ProjectId > 0) {
      foData.append("Id", this.ProjectId.toString());
    } else {
      if (
        this.fileToUpload != null &&
        this.fileToUpload != "" &&
        this.fileToUpload != undefined &&
        this.fileToUpload != undefined &&
        this.fileToUpload != isNaN
      ) {
      } else {
        abp.message.error(
          "Please Upload Application Form!",
          "Application Form is required"
        );
        return;
      }
    }

    foData.append("LastModifiedById", LoginUserId);

    foData.append("ClientId", this.ClientId.toString());
    foData.append("CreatedById", LoginUserId);
    foData.append("StandardId", this.standardId.toString());
    foData.append("ApplicationForm", this.fileToUpload);

    Object.keys(this.SAForm.controls).forEach((key) => {
      if (
        this.SAForm.controls[key].value != null &&
        this.SAForm.controls[key].value != "" &&
        this.SAForm.controls[key].value != undefined &&
        this.SAForm.controls[key].value != isNaN
      ) {
        var sname = key;
        //var sname= this.SAForm.controls[key].;
        var val = this.SAForm.controls[key].value;

        foData.append(sname, val);
      }
    });

    // foData.append('ClientSiteId',this.SAForm.get('ClientSiteId').value);
    //  foData.append('VerificationTypeId',this.SAForm.get('VerificationTypeId').value);
    //  foData.append('ProjectTypeId',this.SAForm.get('ProjectTypeId').value);
    //  foData.append('RiskId',this.SAForm.get('RiskId').value);
    //  foData.append('EacodeId',this.SAForm.get('EacodeId').value);
    //  foData.append('NaceCodeId',this.SAForm.get('NaceCodeId').value);
    //  foData.append('ConsultantId',this.SAForm.get('ConsultantId').value);
    //  foData.append('AccreditationId',this.SAForm.get('AccreditationId').value);

    //   foData.append('Scope',this.SAForm.get('Scope').value);
    //  foData.append('PreAuditDuration',this.SAForm.get('PreAuditDuration').value);
    //  foData.append('DurationStage1',this.SAForm.get('DurationStage1').value);
    //  foData.append('DurationStage2',this.SAForm.get('DurationStage2').value);
    //  foData.append('SurveillanceVisitFrequencyId',this.SAForm.get('SurveillanceVisitFrequencyId').value);
    //  foData.append('SurveillanceMethodId',this.SAForm.get('SurveillanceMethodId').value);
    //  foData.append('NoOfSurveillanceVisits',this.SAForm.get('NoOfSurveillanceVisits').value);
    //  foData.append('Applicatonfee',this.SAForm.get('Applicatonfee').value);
    //  foData.append('PreAuditfee',this.SAForm.get('PreAuditfee').value);
    //  foData.append('Assessmentfee',this.SAForm.get('Assessmentfee').value);
    //  foData.append('Survfee',this.SAForm.get('Survfee').value);
    //  foData.append('ExpensesId',this.SAForm.get('ExpensesId').value);
    //  foData.append('Remarks',this.SAForm.get('Remarks').value);

    this._SA8000Service.CreateSA8000WithFile(foData).subscribe((Response) => {
      if (Response.message == "1") {
        abp.message.info("Successfully Saved!");
        this.router.navigateByUrl(
          "/app/pages/sales/all-projects?" + this.ClientId
        );
      } else if (Response.message == "2") {
        abp.message.info("Successfully Updated!");
        this.router.navigateByUrl(
          "/app/pages/sales/all-projects?" + this.ClientId
        );
      } else if (Response.message == "0") {
        abp.message.error("Not Inserted!");
      }

      //this.router.navigateByUrl('/app/pages/sales/all-projects?'+this.ClientId);
    });
  }

  Back(): void {
    this.router.navigateByUrl("/app/pages/sales/all-projects?" + this.ClientId);
  }
  dashboard(): void {
    this.router.navigateByUrl("/app/home");
  }

  id: number;
  // edit(e) {
  //
  //   // var List = [];
  //   // List=this.Liststandard                                                                             ;
  //   // this.router.navigateByUrl('/app/pages/stock-management/library');
  //   this.ProjectId=e.row.data.id
  //   // var updateDate =this.StandardList.find(x => x.id == this.ProjectId );

  //   // this._StandardService.GetStandardById(this.ProjectId).subscribe((res) =>
  //   // {
  //
  //       this.UserAuditForm.controls.Organization.setValue(e.row.data.organization);
  //       this.UserAuditForm.controls.StandardId.setValue(e.row.data.standardId);
  //       this.UserAuditForm.controls.DurationDays.setValue(e.row.data.durationDays);
  //       this.UserAuditForm.controls.Year.setValue(e.row.data.year);
  //       this.UserAuditForm.controls.NaceCodeId.setValue(e.row.data.naceCodeId)
  //       this.UserAuditForm.controls.EacodeId.setValue(e.row.data.eacodeId);
  //       this.UserAuditForm.controls.AuditTypeId.setValue(e.row.data.auditTypeId);
  //       this.UserAuditForm.controls.CertificationBodyId.setValue(e.row.data.CertificationBodyId);

  //  }

  onTableSizeChange(event): void {
    this.pagedDto.pageSize = event.target.value;
  }

  //  NewRecord()

  //  {
  //   this.UserAuditForm.controls.Organization.setValue('');
  //   this.UserAuditForm.controls.StandardId.setValue('');
  //   this.UserAuditForm.controls.DurationDays.setValue('');
  //   this.UserAuditForm.controls.Year.setValue('');
  //   this.UserAuditForm.controls.NaceCodeId.setValue('')
  //   this.UserAuditForm.controls.EacodeId.setValue('');
  //   this.UserAuditForm.controls.AuditTypeId.setValue('');
  //   this.UserAuditForm.controls.CertificationBodyId.setValue('');

  //   this.ProjectId=null;
  //   //  window.location.reload();
  //   // this.ModuleForm.controls.Name.setValue('');
  //   //   this.ModuleForm.controls.Description.setValue('');
  //   //   this.ModuleForm.controls.Code.setValue('');

  //   //  let currentUrl = this.router.url;
  //   // this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  //   // this.router.onSameUrlNavigation = 'reload';
  //   // this.router.navigate([currentUrl]);
  //
  //  // this.router.navigateByUrl('/app/pages/certification-setups/module');

  // }

  editRecord(e) {
    // var userId=item;
    var urlink = e;
    this.router.navigateByUrl(e + this.ClientId);
  }

  handlefileInput(e: any) {
    this.fileToUpload = <File>e?.target?.files[0];
    //this.url=e.target.value;
  }

  handlefileInputSA8000(e: any) {
    this.fileToUploadSa8000 = <File>e?.target?.files[0];
  }

  handleContractfileInputSA8000(e: any) {
    this.ContractfileToUploadSa8000 = <File>e?.target?.files[0];
  }

  onOptionsSelected(id: number) {
    console.log("the selected value is " + id);
  }

  DiscountTypeChange($event) {
    abp.message.info("the selected value is " + $event.value);
  }

  onChange(deviceValue) {
    this._ClientService.GetSiteById(deviceValue).subscribe((Response) => {
      this.SAForm.controls.Address.setValue(Response.address);
      this.SAForm.controls.OutsourceProductionProcessess.setValue(
        Response.outsourceProductionProcessess
      );
      this.SAForm.controls.ShiftTimings.setValue(Response.shiftTimings);

      this.SAForm.controls.LegalStatus.setValue(Response.legalStatus);
      this.SAForm.controls.TotalEmployees.setValue(Response.totalEmployees);
      //this.SAForm.controls.ProjectTypeName.setValue(Response.projectTypeName)
    });
  }

  OnApprovalSubmit(): void {
    var LoginUserId = localStorage.getItem("userId");

    if (
      this.SAForm.get("ApprovalStatusId").value > 0 &&
      this.SAForm.get("Remarks").value != null &&
      this.SAForm.get("Remarks").value != "" &&
      this.SAForm.get("Remarks").value != undefined &&
      this.SAForm.get("Remarks").value != "" &&
      this.SAForm.get("Remarks").value != isNaN
    ) {
      const ClientSitesModel = {
        Id: this.ProjectId,
        ProjectId: this.ProjectId,
        ApprovalStatusId: this.SAForm.get("ApprovalStatusId").value,
        Remarks: this.SAForm.get("Remarks").value,
        RemarksById: LoginUserId.toString(),
      };

      this._SA8000Service
        .ApprovedProject(ClientSitesModel)
        .subscribe((Response) => {
          abp.message.info(Response.message);
          //this.reloadGrid();
          this.router.navigateByUrl("/app/home");
        });
    } else {
      abp.message.error(
        "Approval Status and Remarks can not be Empty",
        "Alert"
      );
    }
  }

  DownloadApplicationForm(): void {
    // this.ProjectId=e.row.data.id;
    // var fillename=e.row.data.title;
    var fillename = "Document File";

    if (
      this.ApplicationfileName != undefined &&
      this.ApplicationfileName != null
    ) {
      fillename = this.ApplicationfileName.replace(/^.*[\\\/]/, "");
    }
    this._SA8000Service
      .downloadApplicationForm(this.ProjectId)
      .subscribe((result: Blob) => {
        const Blb = new Blob([result], { type: result.type });
        // const url=window.URL.createObjectURL(Blb);
        // window.open(url);
        // console.log("success");

        const a = document.createElement("a");
        a.setAttribute("style", "display:none;");
        document.body.appendChild(a);
        a.download = fillename;
        // const fileName =

        //="farooq";
        a.href = URL.createObjectURL(Blb);
        a.target = "_blank";
        a.click();
        document.body.removeChild(a);
      });
  }
  delete(): void {
    if (
      this.ProjectId != 0 &&
      this.ProjectId != null &&
      this.ProjectId != undefined
    ) {
      abp.message.confirm("", undefined, (result: boolean) => {
        if (result) {
          // this.SecUserService.Deleteuser(e.row.data.id).subscribe()
          //     abp.message.info("Deleted successfully", "Status", {});

          this._SA8000Service
            .DeleteProject(this.ProjectId)
            .subscribe((Response) => {
              abp.message.info(Response.message);
            });
        }
      });
    }
  }
  // Pdf() {
  //

  //     var pdfList = [];

  //     var item =[];
  //     var noOfPatient=[]
  //     var internal=[]
  //     var filteredDepart =[]
  //     var total:number
  //     var date:string
  //     var filteredWithoutAdmission = []

  //

  //       filteredDepart.push({id: 13 , name: "Surgical Admission" },
  //       {id : 14, name : "Medical Admission"})
  //       filteredWithoutAdmission = filteredDepart.filter(x=>x.name != "Admission")
  //       if(filteredWithoutAdmission == undefined)
  //       filteredWithoutAdmission = filteredDepart

  //   var dd = {
  //     content: [

  //         {
  //           text: 'Appointment List',
  //           alignment: "center",
  //           fontSize: 14,
  //           margin: [0, 0, 0, 10]
  //         },
  //         {
  //           text: date,
  //           alignment: "center",

  //           fontSize: 12,
  //           margin: [0, 0, 0, 10]
  //         },
  //           {
  //               table: {
  //                   widths: [25,'*','*'],
  //                   heights: [0, 0, 0],
  //                   body: pdfList,
  //                 }
  //           },
  //   ]
  //   }
  //   pdfmake.createPdf(dd).open();
  // }

  Pdf() {
    let Today_Date = new Date(
      this.datePipe.transform(Date.now(), "yyyy/MM/dd")
    );

    let TodayDate = this.datePipe.transform(Today_Date, "yyyy-MM-dd");

    //let completestep = this.CompletedSetupList.find(x => x.id == this.SAForm.get('CompletedStepId').value)
    //let methodology = this.MethodologyList.find(x => x.id == this.SAForm.get('MethodologyId').value)

    let clientSite = this.ClientSiteList.find(
      (x) => x.id == this.SAForm.get("ClientSiteId").value
    );
    var test = this.clientinfo;
    var test1 = this.projectinfo;
    // Font bold = Font font = new Font(FontFamily.HELVETICA, 12, Font.BOLD);
    // Phrase p = new Phrase("NAME: ", bold);
    //   this._ClientService.GeClientDatabyId(this.ClientId).subscribe((Response) =>
    // {

    //   this.clientinfo=Response;

    // });

    var documentDefinition = {
      info: {
        title: "SA8000:2014 Proposal Weaving" + "/" + this.clientinfo.name,
      },
      // pageMargins: [ 10, 70, 10, 20 ],
      pageSize: "A4",
      pageMargins: [50, 50, 50, 50],
      background: [
        {
          image:
            "data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QNtaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6OTZGQjhGODgyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6OTZGQjhGODcyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTBERkYyRERGRDI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94Onht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BAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEH//Z",

          width: 500,
          opacity: 0.2,
          margin: [20, 280, 0, 0],
        },
      ],
      content: [
        {
          columns: [
            {
              text:
                "Code: " +
                this.SAForm.get("Code").value +
                "\n" +
                "Date: " +
                TodayDate,
            },

            {
              image:
                "data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QNtaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6OTZGQjhGODgyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6OTZGQjhGODcyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTBERkYyRERGRDI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94Onht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BAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEH//Z",
              alignment: "center",
              margin: [0, -30, 0, -10],
              width: 150,
              height: 120,
            },
          ],
        },
        {
          text: "SA8000:CERTIFICATION",
          alignment: "center",
          fontSize: 16,
          //decoration: "underline" ,
          bold: true,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 10]
        },

        {
          canvas: [
            { type: "line", x1: 160, y1: 0, x2: 335, y2: 0, lineWidth: 3 },
          ],
        },

        {
          text: "PROPOSAL",
          alignment: "center",

          bold: true,

          margin: [0, 7, 0, 10],
        },

        // {
        //   text: 'OZONE SUSTAINABILITY',
        //   alignment: "center",
        //   fontSize: 16,
        //   //decoration: "underline" ,
        //   bold : true,

        //   //textTransform: 'uppercase',
        //   // background:
        //   // 'assets/img/ozone-group-logo.jpg',

        //   margin: [0, 0, 0, 10]
        // },

        // {canvas: [ { type: 'line', x1: 170, y1: 0, x2: 345, y2: 0, lineWidth: 3 } ]},

        // {
        //   text: 'PROPOSAL',
        //   alignment: "center",

        //   bold : true,

        //   margin: [0, 7, 0, 10]
        // },

        // {

        //   table: {
        //     widths: [500],
        //     heights: [20],
        //     body: [
        //       // ['row 1 with height 20'],
        //       // ['row 2 with height 50'],
        //       [{
        //         text: 'OZONE SUSTAINABILITY'+"\n"+ "________________________"+'PROPOSAL',

        //         color: 'white',
        //         fillColor: '#4CAF50',
        //         border: [false, true, false, true],
        //         margin: [0, 5, 0, 5],
        //         textTransform: 'uppercase',
        //         alignment: "center",
        //         fontSize: 16,
        //         //decoration: "underline" ,
        //         bold : true,
        //       },

        //     ]
        //     ]
        //   }
        // },
        {
          text: "Company Representative",
          alignment: "left",
          fontSize: 14,
          //decoration: "underline" ,
          bold: true,

          textTransform: "uppercase",
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 20, 0, 0],
        },

        {
          canvas: [
            { type: "line", x1: 0, y1: 0, x2: 495, y2: 0, lineWidth: 1 },
          ],
        },

        {
          text:
            this.clientinfo.contactPerson +
            "  (" +
            this.clientinfo.position +
            ")",
          //alignment: "left",
          fontSize: 11,
          //decoration: "underline" ,
          //bold : true,
          margin: [0, 5, 0, 0],

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 0]
        },
        {
          text: this.clientinfo.name,
          //alignment: "left",
          fontSize: 11,
          //decoration: "underline" ,
          //bold : true,
          margin: [0, 0, 0, 0],

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 0]
        },
        {
          text: this.clientinfo.address1,
          //alignment: "left",
          fontSize: 11,
          //decoration: "underline" ,
          //bold : true,
          margin: [0, 0, 0, 0],
          lineHeight: 1.5,
          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 0]
        },

        //   {
        //     canvas: [ {  type: 'line', x1: 0, y1: 0, x2: 500, y2: 0, lineWidth: 3 } ]

        // },
        {
          text: "SUB: Certification in Standards:",
          alignment: "left",
          fontSize: 14,
          //decoration: "underline" ,
          bold: true,

          //pageBreak: 'before',

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 0]
        },
        {
          canvas: [
            { type: "line", x1: 0, y1: 0, x2: 495, y2: 0, lineWidth: 1 },
          ],
        },

        {
          text: "SA8000:2014 VERIFICATION",
          alignment: "left",
          fontSize: 14,
          //decoration: "underline" ,
          bold: true,
          margin: [0, 5, 0, 0],
          //pageBreak: 'before',

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 0]
        },
        {
          text:
            "Thank you for choosing us for taking an opportunity to present us certification SA8000:2014 in your esteemed organisation." +
            "\n \n " +
            "Among the range of certifications, The Ozone has accredited scheme certification of SA8000:2014 thru SAAS. We have approval as HIG Index Trainer & Verifier Body and Social Labour Convergence Project (SLCP) as Trainer & Verifier Body in Environmental and Social Assessments in Industry. We are doing assessments and certifications in ISO 9001:2015 for Quality Management Systems, ISO 14001:2015 for Environmental Management Systems, OHSAS 18001 for Health & Safety, HACCP." +
            "\n \n" +
            "The Ozone fosters a ‘listening philosophy ‘ to be administered by its full support before and after the certification by working with you, as you further improve your systems, stream line your processes & strengthen your position as a world class organization. Also as a value addition various training programs and workshops are conducted to enhance better understanding of the standard and imbibing it to seamless and integrated organizational development. " +
            "\n \n" +
            "We are enclosing our proposal for certification with SA8000:2014 with all details. " +
            "\n \n" +
            "We are pleased to respond your queries in this regard." +
            "\n \n" +
            "Regards, " +
            "\n \n" +
            "Ozone Group",

          fontSize: 12,
          alignment: "justify",
          margin: [0, 11, 0, 0],
          //decoration: "underline" ,
          //bold : true,

          //pageBreak: 'before',

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          //margin: [0, 0, 0, 0]
        },

        {
          margin: [350, 0, 0, 0],
          pageBreak: "before",
          columns: [
            {
              image:
                "data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QNtaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6OTZGQjhGODgyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6OTZGQjhGODcyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTBERkYyRERGRDI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94Onht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BAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEH//Z",
              alignment: "center",
              margin: [0, -40, 0, 0],
              width: 150,
              height: 100,
            },
          ],
        },
        // {
        //   text: 'OZONE SUSTAINABILITY',
        //   alignment: "left",
        //   fontSize: 16,
        //   //decoration: "underline" ,
        //   bold: true,
        //   pageBreak: 'before',

        //   //textTransform: 'uppercase',
        //   // background:
        //   // 'assets/img/ozone-group-logo.jpg',

        //   margin: [0, 10, 0, 0]
        // },

        // {canvas: [ { type: 'line', x1: 170, y1: 0, x2: 345, y2: 0, lineWidth: 3 } ]},

        // {
        //   text: 'PROPOSAL',
        //   alignment: "left",

        //   bold: true,

        //   margin: [0, 3, 0, 10]
        // },

        // {
        //   text: 'ORGANIZATION DETAILS:',
        //   alignment: "left",
        //   fontSize: 14,
        //   //decoration: "underline" ,
        //   bold: true,

        //   //pageBreak: 'before',

        //   //textTransform: 'uppercase',
        //   // background:
        //   // 'assets/img/ozone-group-logo.jpg',

        //   //margin: [0, 0, 0, 0]
        // },
        {
          canvas: [
            { type: "line", x1: 0, y1: 0, x2: 495, y2: 0, lineWidth: 1 },
          ],
        },

        {
          margin: [0, 15, 0, 0],
          //style: 'tableExample',

          table: {
            headerRows: 1,
            widths: [150, "*"],
            heights: [10, 10, 0],
            //fontSize: 11,
            body: [
              [
                {
                  text: "Organization Details ",
                  style: "tableHeader",
                  colSpan: 2,
                  alignment: "center",
                  color: "white",
                  fillColor: "#4258ad",
                  fontSize: 11,
                },
                {},
              ],

              [
                { text: "Scope", style: "tableHeader", alignment: "left" },
                {
                  text: this.projectinfo.scope,
                  style: "tableHeader",
                  alignment: "left",
                },
              ],

              [
                {
                  text: "No of Employees",
                  style: "tableHeader",
                  alignment: "left",
                },
                {
                  text: this.clientsite.totalEmployees,
                  style: "tableHeader",
                  alignment: "left",
                },
              ],

              [
                { text: "No of Site", style: "tableHeader", alignment: "left" },
                { text: "1", style: "tableHeader", alignment: "left" },
              ],

              // [{ text: 'Particulars', style: 'tableHeader', colSpan: 2, alignment: 'center' }, {}, { text: 'Amount ', style: 'tableHeader', alignment: 'center' }],

              // ['Service Type', 'Management System Certification Audit', ''],

              // ['Standard/Code', 'ISO 9001:2015' + '\n' + 'ISO 14001:2015' + '\n' + 'ISO 45001:2018', ''],

              // ['Site Name', '', ''],
              // ['Charge For', 'ISO 9001:2015, ISO 14001:2015, ISO 45001:2018', ''],
              // ['VAT & AIT', 'Excluded', ''],
              // ['Grand Total (PKR)', { colSpan: 3, rowSpan: 3, text: 'Both:\n100,000' }, ''],

              // [{ text: '', style: 'tableHeader', colSpan: 2, alignment: 'center' }, {}, { text: '', style: 'tableHeader', alignment: 'center' }],
            ],
          },
        },
        // {
        //   //alignment: 'justify',
        //   columns: [

        //     {
        //       width: 200,
        //       text: 'Total no of site:',
        //       bold: true,
        //       margin: [0, 5, 0, 0]
        //     },
        //     {
        //       text: '01',
        //       margin: [0, 5, 0, 0]
        //     },
        //   ],

        // },
        // {
        //   columns: [

        //     {
        //       width: 200,
        //       text: 'Number of employees:',
        //       bold: true,
        //       margin: [0, 2, 0, 0]
        //     },
        //     {
        //       text: this.SAForm.get('TotalEmployees').value,
        //       margin: [0, 2, 0, 0]
        //     },
        //   ],
        // },
        // {
        //   columns: [

        //     {
        //       width: 200,
        //       text: 'Completed Step:',
        //       bold: true,
        //       margin: [0, 2, 0, 0]
        //     },
        //     {
        //       text:'',
        //       margin: [0, 2, 0, 0]
        //     },
        //   ],
        // },
        {
          columns: [
            {
              width: 200,
              text: "",
              bold: true,
              margin: [0, 2, 0, 0],
            },
            {
              text: "",
              margin: [0, 2, 0, 0],
            },
          ],
        },

        {
          text: "ORGANIZATION SITES COVERED UNDER CERTIFICATION",
          alignment: "center",
          fontSize: 14,
          //decoration: "underline" ,
          bold: true,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 30, 0, 10],
        },

        {
          text: "SITE TO BE AUDITED: ",
          alignment: "left",
          fontSize: 12,
          decoration: "underline",
          bold: true,
          width: 50,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },
        {
          text: clientSite.siteName + ",  " + clientSite.address,
          //alignment: "left",
          fontSize: 11,

          //bold : true,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },

        // {
        //   columns: [

        //     {
        //       width: 120,
        //       text: 'SITE TO BE AUDITED: ',
        //       bold: true,
        //       margin: [0, 5, 0, 0],
        //       alignment: "left",
        //      fontSize: 12,

        //     },
        //     {
        //       text: this.clientinfo.name +",  "+ this.clientinfo.address1 ,
        //       margin: [0, 5, 0, 0],
        //       fontSize: 11,
        //     },
        //   ],
        // },
        //   {
        //     canvas: [ {  type: 'line', x1: 0, y1: 0, x2: 515, y2: 0, lineWidth: 1 } ]

        // },

        {
          text: "COMMERCIAL ASPECTS OF PROPOSAL",
          alignment: "left",
          fontSize: 14,
          //decoration: "underline" ,
          bold: true,

          //pageBreak: 'before',

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 10, 0, 0],
        },
        {
          canvas: [
            { type: "line", x1: 0, y1: 0, x2: 495, y2: 0, lineWidth: 1 },
          ],
        },

        {
          table: {
            heights: [40, 25, 25],
            // widths: [370, 120],
            widths: ["75%", "25%"],
            margin: [0, 10, 0, 0],
            //alignment: "center",
            body: [
              [
                {
                  text: "Service Type",
                  alignment: "center",
                  color: "white",
                  margin: [0, 10, 0, 0],
                  fillColor: "#4258ad",
                },
                {
                  text: "Charges" + "\n" + "USD/-",
                  alignment: "center",
                  color: "white",
                  fillColor: "#4258ad",
                  margin: [0, 3, 0, 0],
                },
              ],
              [
                {
                  text: "Application fee  ",
                  fontSize: 11,
                  margin: [0, 3, 0, 0],
                },
                { text: this.SAForm.get("ApplicationFee").value },
              ],
              [
                {
                  text: "Audit fee",
                  fontSize: 11,
                  bold: false,
                  margin: [0, 3, 0, 0],
                },
                { text: this.SAForm.get("AuditFee").value },
              ],
              [
                {
                  text: "Cost of Each Annual Surveillance Audits",
                  fontSize: 11,
                  bold: false,
                  margin: [0, 3, 0, 0],
                },
                { text: this.SAForm.get("SurveillanceAudits").value },
              ],
              [
                { text: "GST/Local tax", fontSize: 11, margin: [0, 3, 0, 0] },
                { text: this.SAForm.get("tax").value },
              ],
            ],
            //alignment: "center",
          },
          layout: {
            hLineWidth: function (i, node) {
              return i === 0 || i === node.table.body.length ? 2 : 1;
            },
            vLineWidth: function (i, node) {
              return i === 0 || i === node.table.widths.length ? 2 : 1;
            },
            hLineColor: function (i, node) {
              return "black";
            },
            vLineColor: function (i, node) {
              return "black";
            },
            hLineStyle: function (i, node) {
              if (i === 0 || i === node.table.body.length) {
                return null;
              }
              return { dash: { length: 4, space: 4 } };
            },
            vLineStyle: function (i, node) {
              if (i === 0 || i === node.table.widths.length) {
                return null;
              }
              return { dash: { length: 4 } };
            },
            // paddingLeft: function(i, node) { return 4; },
            // paddingRight: function(i, node) { return 4; },
            // paddingTop: function(i, node) { return 2; },
            // paddingBottom: function(i, node) { return 2; },
            // fillColor: function (i, node) { return null; }
          },
        },

        "\n",
        "\n",

        {
          margin: [350, 0, 0, 0],
          pageBreak: "before",
          columns: [
            {
              image:
                "data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QNtaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MCA2MS4xMzQ3NzcsIDIwMTAvMDIvMTItMTc6MzI6MDAgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6OTZGQjhGODgyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6OTZGQjhGODcyOTAwMTFFODkxOTY5QkU5M0ZFRjNGRkIiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6QTBERkYyRERGRDI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6RTcxNjFENUNFQTI4RTgxMUIyNkRGQjk2N0Y4ODI2NzQiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94Onht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BAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEH//Z",
              alignment: "center",
              margin: [0, -40, 0, 0],
              width: 150,
              height: 100,
            },
          ],
        },
        // {
        //   text: 'OZONE SUSTAINABILITY',
        //   alignment: "left",
        //   fontSize: 16,
        //   //decoration: "underline" ,
        //   bold: true,
        //   pageBreak: 'before',

        //   //textTransform: 'uppercase',
        //   // background:
        //   // 'assets/img/ozone-group-logo.jpg',

        //   margin: [0, 10, 0, 0]
        // },

        // {canvas: [ { type: 'line', x1: 170, y1: 0, x2: 345, y2: 0, lineWidth: 3 } ]},

        // {
        //   text: 'PROPOSAL',
        //   alignment: "left",

        //   bold: true,

        //   margin: [0, 3, 0, 10]
        // },
        {
          text: "(Company Seal):",
          alignment: "left",
          fontSize: 12,
          decoration: "underline",
          bold: true,
          width: 50,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },
        {
          text: "Client organization’s Signature means validation and acceptance of proposal by Applicant client Organization for the Certification/Verification Scheme(s) mentioned in proposal.",
          ul: [
            "Client organization’s Signature means validation and acceptance of proposal by Applicant client Organization for the Certification/Verification Scheme(s) mentioned in proposal.",
          ],
          alignment: "justify",
          fontSize: 11,
          //decoration: "underline" ,
          //bold : true,

          //pageBreak: 'before',

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 20, 0, 0],
        },

        {
          text: "Notes: ",
          alignment: "left",
          fontSize: 12,
          decoration: "underline",
          bold: true,
          width: 50,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },
        {
          text:
            "•	All Surveillance audits shall be Semi Announced." +
            "\n" +
            "•	There are 2 Nos of Annual Surveillance audits will be scheduled in certification cycle of 3 years." +
            "\n" +
            "•	1st Surveillance audit will be scheduled prior to 6 months from the certification audit. 2nd Annual Surveillance audit will be scheduled prior to 18 months from the Transfer/Recertification audit.",

          alignment: "justify",
          fontSize: 11,
          margin: [0, 20, 0, 0],
        },

        {
          text: "Terms & Conditions:",
          alignment: "left",
          fontSize: 12,
          decoration: "underline",
          bold: true,
          width: 50,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },
        {
          ul: [
            "• All quotations are firm for the first 3 years of registration." +
              "\n" +
              "•	All prices are in Local currency." +
              "\n" +
              "•	Auditors/Specialist Traveling, Boarding & Lodging Expenses will be charged at cost and includes Taxi, Train or Air Fare, Boarding & Lodging expenses, Local Conveyance etc." +
              "\n" +
              "•	Should any changes occur within your organization, Ozone Group reserves the right to reissue the quotation. Examples of changes affecting the quote are an increase in the number of employees, a change in the scope of registration (i.e. addition of a product line) etc." +
              "\n" +
              "•	In case of during the active certification cycle, If Accreditation board, SAAS, may choose the organization for onsite assessment covered by their accreditation program, the factory shall be visited as per SAAS procedure requirements." +
              "\n" +
              "•	In case of Special audits or Visits as per procedural requirements, then the fee will be charged extra with Manday fee quoted above and other expenses related to that visit." +
              "\n" +
              "•	Our quotation is valid for acceptance for 30 days (Thirty Days) from the quotation date.",
          ],
          alignment: "justify",
          fontSize: 11,
          margin: [10, 2, 0, 0],
        },
        {
          text: "THE Ozone GROUP reserves the right to accept / reject an “Acceptance of Proposal” submitted outside of the proposal validity period.",
          alignment: "justify",
          fontSize: 11,
          // decoration: "underline" ,
          // bold : true,
          //width:50,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 10, 0, 0],
        },
        +"\n\n\n",

        {
          text: "Notes:",
          alignment: "left",
          fontSize: 12,
          decoration: "underline",
          bold: true,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },
        {
          ul: [
            "•	The Social Fingerprint/Self assessment is required to be done before Stage 1/transfer/recertification Audit take place. The Social Fingerprint is done and purchased online at www.sa-intl.org. Failure to do this, following audit shall not be conducted.",
          ],
          alignment: "justify",
          fontSize: 11,
          margin: [10, 2, 0, 0],
        },
        {
          text: "ACCEPTANCE OF QUOTE:",
          alignment: "left",
          fontSize: 12,
          decoration: "underline",
          bold: true,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 5, 0, 0],
        },
        {
          type: "none",
          ul: [
            "If this quotation is acceptable by Applicant Client Organization, then please send a confirmation by email referencing the quotation number and attach the signed and stamped copy of this quotation.",
            "\n" +
              "By signing this quotation, it means the client organization is accepting this quotation and follow “we confirm the information contained above is correct and agree to comply with the OSMS (Ozone Group) Scheme Rules and agree to pay all fees relating to the provision of certification/Verification services.”",
          ],
          fontSize: 11,
          alignment: "justify",
        },
        {
          text: "The organization shall update Ozone Group if there are any changes in legal status, organization and management including number of employees, addresses and sites, scope of operation and major changes to the management system / processes.",
          //alignment: "left",
          fontSize: 11,
          alignment: "justify",
          // decoration: "underline" ,
          // bold : true,
          //width:50,

          //textTransform: 'uppercase',
          // background:
          // 'assets/img/ozone-group-logo.jpg',

          margin: [0, 10, 0, 0],
        },

        // {
        //   columns: [

        //     { canvas: [{ type: 'line', x1: 0, y1: 90, x2: 150, y2: 90, lineWidth: 1, }], width: 370 },

        //     { canvas: [{ type: 'line', x1: 0, y1: 90, x2: 150, y2: 90, lineWidth: 1 }] },
        //   ],
        // },
        {
          margin: [0, 130, 0, 0],
          columns: [
            {
              canvas: [
                { type: "line", x1: 0, y1: 0, x2: 150, y2: 0, lineWidth: 1 },
              ],
              width: 340,
            },

            {
              canvas: [
                { type: "line", x1: 0, y1: 0, x2: 150, y2: 0, lineWidth: 1 },
              ],
            },
          ],
        },
        {
          columns: [
            {
              text: "Ozone Sustainability",
              alignment: "left",
              fontSize: 11,
              //decoration: "underline" ,
              bold: true,
              width: 340,
              margin: [0, 5, 0, 0],
              color: "#338fdb",
            },
            {
              text: "CLIENT DETAILS",
              alignment: "left",
              fontSize: 11,
              //decoration: "underline" ,
              bold: true,
              margin: [0, 5, 0, 0],
              color: "#338fdb",
            },
          ],
        },

        {
          columns: [
            {
              text: "Office Coordinator",
              alignment: "left",
              fontSize: 10,
              //decoration: "underline" ,
              bold: true,
              width: 340,
              margin: [0, 5, 0, 0],
            },
            {
              text:
                this.clientinfo.contactPerson +
                "\n" +
                this.clientinfo.position +
                "\n" +
                "Cell #:" +
                this.clientinfo.mobileNumber +
                "\n" +
                "Email:" +
                this.clientinfo.email,
              alignment: "left",
              fontSize: 10,
              //decoration: "underline" ,
              bold: true,
              margin: [0, 5, 0, 0],
            },
          ],
        },

        // {
        //   columns: [

        //   ],
        // },
      ],
    };

    const pdfFile = pdfmake.createPdf(
      documentDefinition,
      null,
      null,
      pdfFonts.pdfMake.vfs
    );
    pdfFile.open();
    //pdfFile.download('SLCP Proposal Weaving.pdf');
    //pdfFile.download();
    //pdfmake.createPdf(documentDefinition).open();
    //pdfmake.createPdf(documentDefinition).download();
  }
  onContractSubmit(): void {
    try {
      var fileName = this.fileToUpload.name.split("_")[0];

      //if (fileName == this.ProjectCode) {

      if (
        this.fileToUpload == null ||
        this.fileToUpload == undefined ||
        this.fileToUpload == "" ||
        this.fileToUpload == "" ||
        this.fileToUpload == isNaN
      ) {
        abp.message.error(
          "Contract file is required",
          "Please Upload Contract file"
        );
        return;
      }
      var LoginUserId = localStorage.getItem("userId");
      const foData: FormData = new FormData();

      if (this.ProjectId > 0) {
        foData.append("Id", this.ProjectId.toString());
      }
      foData.append("LastModifiedById", LoginUserId);

      foData.append("ContractForm", this.fileToUpload);
      foData.append("Remarks", this.SAForm.get("ContractRemarks").value);
      // foData.append('ApprovalStatusId',this.SLCPForm.get('ContractStatusId').value);

      this._SA8000Service.ContarctsSubmit(foData).subscribe((Response) => {
        abp.message.info(Response.message);

        this.router.navigateByUrl(
          "/app/pages/sales/all-projects?" + this.ClientId
        );
      });

      // }
      // else {
      //   //this.SLCPForm.controls.File.setValue("");
      //   abp.message.error("File Name Is Incorrect, Project code must be include in File name. File name must be  " + this.ProjectCode + "_Contract File", "Please Upload Correct File")
      // }
    } catch (error) {
      // this.SLCPForm.controls.File.setValue("");
      abp.message.error("File Name Is Incorrect", "Please Upload Correct File");
    }

    console.log(this.fileToUpload.name);
  }

  loadStatus(): void {
    const item = {
      id: 7,
      name: "Contract Approved",
    };
    this.ContractApprovalList.push(item);
    const item2 = {
      id: 8,
      name: "Contract Reject",
    };
    this.ContractApprovalList.push(item2);
  }

  ClientData(): void {
    //alert(this.ClientId);
    this._ClientService
      .GeClientDatabyId(this.ClientId)
      .subscribe((Response) => {
        this.clientinfo = Response;

        this.Multisite = Response.multisite;
        if (this.Multisite == true) {
          this.SiteCount = Response.siteCount + "   Sites";
        } else {
          this.SiteCount = "Only One Site";
        }

        this.SAForm.controls.OverAllEmployees.setValue(
          Response.overAllEmployees
        );
        this.SAForm.controls.RiskName.setValue(Response.riskName);
        this.SAForm.controls.Eacodename.setValue(Response.eacodename);
        this.SAForm.controls.NaceCodeName.setValue(Response.naceCodeName);
        console.log(this.clientinfo);
      });
  }
}

//NaceCodeId
//EacodeId
//RiskId
