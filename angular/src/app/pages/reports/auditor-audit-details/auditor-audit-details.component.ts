import { AuditorService } from "shared/Services/Auditor-Service";

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
import { ClientAuditVisitModel } from "@shared/Dto/Client-Audit-Visit-model";
import { SlcpService } from "@shared/Services/project-slcp-service";
import { ClientService } from "@shared/Services/Client-Service";
import { ClientAuditVisitService } from "@shared/Services/Client-Audit-visit-service";
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

import { AfterViewInit } from "@angular/core";
import ArrayStore from "devextreme/data/array_store";
import { QCCommentsModel } from "@shared/Dto/QC-Comments-model";
import { Workbook } from "exceljs";
import { exportDataGrid } from "devextreme/excel_exporter";

import { saveAs } from "file-saver-es";
// Our demo infrastructure requires us to use 'file-saver-es'. We recommend that you use the official 'file-saver' package in your applications.

@Component({
  selector: "app-auditor-audit-details",
  templateUrl: "./auditor-audit-details.component.html",
  styleUrls: ["./auditor-audit-details.component.css"],
})
export class AuditorAuditDetailsComponent implements AfterViewInit {
  @Input() key: number;
  @Input() auditorReportDetailsList = [];
  @Output() newItemEvent: EventEmitter<string[]> = new EventEmitter<string[]>();

  // @Output() newItemEvent = new EventEmitter<string>();
  // public Detaillist:any;
  tasksDataSource: DataSource;
  public QcdocumentsListId: number;
  public QCCommentsModel: QCCommentsModel = new QCCommentsModel();
  public QcCommentsId: number;
  public projectId: number;
  public UserName: string;
  public newrecords = [];
  public pagedDto: PagedRequestModel = new PagedRequestModel();
  public totalCount: number;
  public viewHistoryReportData = [];
  QcStatusList = [];
  submitted = false;
  // tasks:
  // public QCDetail=[];

  constructor(
    private _ClientAuditVisitService: ClientAuditVisitService,
    private auditorService: AuditorService
  ) {
    this.addComments = this.addComments.bind(this);
    this.delete = this.delete.bind(this);
    this.Downloadfile = this.Downloadfile.bind(this);
    this.viewHistoryReport = this.viewHistoryReport.bind(this);
    this.viewNCS=this.viewNCS.bind(this);
    // this.ViewHistory=this.ViewHistory.bind(this);

    // this.editButton=this.editButton(this);
  }
  //ngOnInit(): void {  this.QCHistory();}
  ngAfterViewInit() {
    //.QcStatusList=this.qcStatusList
    // this.QCHistory()
    debugger;
    this.tasksDataSource = new DataSource({
      store: new ArrayStore({
        data: this.auditorReportDetailsList,
        key: "id",
      }),
      filter: ["leadAuditorId", "=", this.key],
    });
    debugger;
  }

  onExporting(e) {
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet("Audit Report");

    exportDataGrid({
      component: e.component,
      worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(
          new Blob([buffer], { type: "application/octet-stream" }),
          "Audit Report.xlsx"
        );
      });
    });
    e.cancel = true;
  }
  completedValue(rowData) {
    return rowData.Status == "Completed";
  }

  // QCHistory(): void
  // {
  //   //let organizationId =parseInt(localStorage.getItem('organizationId'));
  //
  //  this._ClientAuditVisitService.QCHistory(this.projectId).subscribe((Response)=>{
  //    this.QCDetail = Response

  //  })
  // }
  // ResetPasswordForm = new FormGroup({
  //   NewPassword: new FormControl(''),
  //    NewConfirmPassword: new FormControl(''),
  //    EmailForgotPassword: new FormControl(''),
  //   })
  displayHistoryPopup = "none";
  displayNCSPopup = "none";
  viewHistoryReport(e) {
    console.log("Mission completed");
    //  this.userId=0;
    // this.userId=e.row.data.id;
    // this.userdata=e.row.data;
    // this.ResetPasswordForm.get('EmailForgotPassword').setValue(e.row.data.emailForgotPassword);
    // this.ResetPasswordForm.get('EmailForgotPassword').disable();
    this.displayHistoryPopup = "block";

    this.auditorService.GetAuditorReportHistory(e.row.data.id).subscribe((Response)=>{
     console.log(Response)
     this.viewHistoryReportData = Response

     })

    //this.router.navigateByUrl('/app/pages/stock-management/library?'+this.id);
  }
  

  closeHistoryPopup() {
    this.displayHistoryPopup = "none";
  }




  displayStyle = "none";


  viewNCS(e) {

    debugger
    //console.log("Mission completed");
    //  this.userId=0;
    // this.userId=e.row.data.id;
    // this.userdata=e.row.data;
    // this.ResetPasswordForm.get('EmailForgotPassword').setValue(e.row.data.emailForgotPassword);
    // this.ResetPasswordForm.get('EmailForgotPassword').disable();
    this.displayNCSPopup = "block";

    this.auditorService.AuditNCS(e.row.data.id).subscribe((Response)=>{
     console.log("ncs" +Response)
    // this.viewHistoryReportData = Response

     })

    //this.router.navigateByUrl('/app/pages/stock-management/library?'+this.id);
  }
  closeNCSPopup() {
    this.displayHistoryPopup = "none";
  }

  addComments(e) {
    e.row.data;

    const obj = this.auditorReportDetailsList.find(
      (item) => item.id === e.row.data.id
    );
    console.log(obj);
    console.log(this.auditorReportDetailsList);
    obj.remarks = "lol";
    console.log(this.auditorReportDetailsList);

    var Hstory = this.auditorReportDetailsList.filter((item) => {
      if (
        item.id != 0 &&
        item.qcdocumentsListId != e.row.data.qcdocumentsListId
      )
        return true;
    });

    //  this.auditorReportDetailsList=[] = this.auditorReportDetailsList.filter(item => item.id != 0 && item.qcdocumentsListId !=e.row.data.qcdocumentsListId);

    // this.tasksDataSource = new DataSource({
    //   store: new ArrayStore({
    //     data: this.auditorReportDetailsList,
    //     key: 'id',
    //   }),
    //   filter: ['qcdocumentsListId', '=', this.key],
    // });

    var test = this.tasksDataSource;

    this.QcCommentsId = null;
    this.QcdocumentsListId = null;
    this.projectId = null;
    this.QcCommentsId = e.row.data.id;
    this.QcdocumentsListId = e.row.data.qcdocumentsListId;
    this.projectId = e.row.data.projectId;

    this.displayStyle = "block";

    //this.router.navigateByUrl('/app/pages/stock-management/library?'+this.id);
  }
  closePopup() {
    this.displayStyle = "none";
  }
  QcCommentsForm = new FormGroup({
    Comment: new FormControl(""),

    QcStatusId: new FormControl(""),
  });

  get f() {
    return this.QcCommentsForm.controls;
  }

  saveComment(): void {
    this.submitted = true;

    if (
      this.QcCommentsForm.get("Comment").value !=
      this.QcCommentsForm.get("Comment").value
    ) {
      abp.message.error("Comment can not be empty", "Alert");
      return;
    }

    if (
      this.QcCommentsForm.get("QcStatusId").value !=
      this.QcCommentsForm.get("QcStatusId").value
    ) {
      abp.message.error("Please Select Status", "Alert");
      return;
    }
    const obj = this.auditorReportDetailsList.find(
      (item) =>
        item.id === this.QcCommentsId &&
        item.qcdocumentsListId === this.QcdocumentsListId
    );
    console.log(this.QcCommentsId);
    console.log(this.QcdocumentsListId);
    console.log(this.auditorReportDetailsList);
    console.log(obj);
    this.UserName = localStorage.getItem("userName").toString();
    var userStr = this.UserName.replace(/['"]+/g, "");
    obj.id;
    //console.log(someStr.replace(/['"]+/g, ''));
    //abp.message.info(someStr.replace(/['"]+/g, ''))
    //let LoginUserId = parseInt(localStorage.getItem('userId'));

    (obj.id = this.QcCommentsId),
      (obj.isDeleted = false),
      (obj.projectId = this.projectId),
      (obj.qcStatusId = parseInt(this.QcCommentsForm.get("QcStatusId").value)),
      (obj.qcdocumentsListId = this.QcdocumentsListId),
      (obj.remarks = this.QcCommentsForm.get("Comment").value),
      (obj.remarksBy = userStr),
      (obj.remarksById = parseInt(localStorage.getItem("userId"))),
      (obj.remarksDate = new Date()),
      (obj.remarksUpdateById = null),
      (obj.renarksUpdateDate = null);

    this.newItemEvent.emit(this.auditorReportDetailsList);
    this.tasksDataSource = new DataSource({
      store: new ArrayStore({
        data: this.auditorReportDetailsList,
        key: "id",
      }),
      filter: ["qcdocumentsListId", "=", this.key],
    });

    this.closePopup();

    //  let LoginUserId = parseInt(localStorage.getItem('userId'));

    //  if(this.QcCommentsId>0)
    //  {  this.QCCommentsModel.Id=this.QcCommentsId}

    //  this.QCCommentsModel.ProjectId=this.projectId
    //  this.QCCommentsModel.Remarks=this.QcCommentsForm.get('Comment').value,
    //  this.QCCommentsModel.QcdocumentsListId=this.QcdocumentsListId
    //  this.QCCommentsModel.RemarksById=LoginUserId;
  }

  RefreshGrid(): void {
    this.tasksDataSource = new DataSource({
      store: new ArrayStore({
        data: this.auditorReportDetailsList,
        key: "id",
      }),
      filter: ["qcdocumentsListId", "=", this.key],
    });
  }

  delete(e) {
    abp.message.confirm("", undefined, (result: boolean) => {
      if (result) {
        for (let i = 0; i < this.auditorReportDetailsList.length; ++i) {
          if (
            this.auditorReportDetailsList[i].id === e.row.data.id &&
            this.auditorReportDetailsList[i].qcdocumentsListId ===
              e.row.data.qcdocumentsListId
          ) {
            this.auditorReportDetailsList.splice(i, 1);
          }
          this.newItemEvent.emit(this.auditorReportDetailsList);
          //console.log(this.auditorReportDetailsList);
        }
        // this.SecUserService.Deleteuser(e.row.data.id).subscribe()
        //     abp.message.info("Deleted successfully", "Status", {});

        // this._ClientAuditVisitService.DeleteComment(e.row.data.id).subscribe((Response) => {

        //   abp.message.info(Response.message)

        //   this.RefreshGrid();

        // })
      }
    });
  }

  editButton(e) {
    var authorizerID = localStorage.getItem("authorizer");
    //  var tt= this.authorizer;

    if (e.row.data.id == "0") {
      return !e.row.isEditing;
    } else {
      return e.row.isEditing;
    }
  }

  Downloadfile(e): void {
    //this.id=e.row.data.DocumentContentType;
    // const source = `data:application/pdf;base64,${e.row.data.documentContent}`;
    const source =
      "data:" +
      e.row.data.documentContentType +
      ";base64," +
      e.row.data.documentContent;

    // console.log("ttttt");
    // console.log(source);
    // console.log(source2);
    const link = document.createElement("a");
    link.href = source;
    link.download = e.row.data.documentName;
    // link.download = this.FileName
    link.click();
  }

  DownloadViaible(e) {
    if (
      e.row.data.documentName != null &&
      e.row.data.documentName != undefined &&
      e.row.data.documentName != isNaN &&
      e.row.data.documentName != "" &&
      e.row.data.documentName != ""
    ) {
      return !e.row.isEditing;
    } else {
      return e.row.isEditing;
    }
  }
}

function saveAs(arg0: Blob, arg1: string) {
  throw new Error("Function not implemented.");
}
