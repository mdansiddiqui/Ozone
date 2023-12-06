import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { NgxPaginationModule } from 'ngx-pagination'; // <-- import the module
import { Observable } from 'rxjs';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { ActivatedRoute, Router } from '@angular/router';
import repaintFloatingActionButton from 'devextreme/ui/speed_dial_action/repaint_floating_action_button';
import { DxListModule } from "devextreme-angular";
import { DxDataGridModule, DxDataGridComponent, DxSpeedDialActionModule, DxSelectBoxModule } from 'devextreme-angular';
import { DxTreeListModule } from "devextreme-angular";
import { Employee, EmployeesService } from '@app/pages/sales/employees.service';
import DataSource from 'devextreme/data/data_source';
import { BrowserModule } from '@angular/platform-browser';
import { DxContextMenuModule } from "devextreme-angular";
import { IndentRequestService } from '@shared/Services/indent-request-service';
import { AuditDocumentsModel } from '@shared/Dto/Audit-Documents-model';
import { DocumentTypeService } from 'shared/Services/document-type';
import { AuditorDocumentsService } from 'shared/Services/Auditor-Documents-service';


@Component({
  selector: 'app-audit-document',
  templateUrl: './audit-document.component.html',
  styleUrls: ['./audit-document.component.css']
})
export class AuditDocumentComponent implements OnInit {
  public item: AuditDocumentsModel = new AuditDocumentsModel();
  AuditDocumentForm = new FormGroup({
    Id: new FormControl(''),
    Code: new FormControl(''),
    Name: new FormControl(''),
    Description: new FormControl(''),
    IsActive: new FormControl(''),
  });
  //AuditDocumentForm: FormGroup; 
  submitted = false;
  get f() { return this.AuditDocumentForm.controls; }
  public pagedDto: PagedRequestModel = new PagedRequestModel()
  public totalCount: number
  auditdocumentsList: any[] = [];
  @Input()formName: string;
  public StatusList: any[] = [];
  public keyword: string = ''
  secRoleForm
  // public AuditdocumentsList = []
  public moduleList = [];
  public isEditShown : boolean  
  public isViewShown : boolean  
  public isAddShown : boolean  
  fileToUpload: any;

  
 


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
    private AuditorDocumentsService: AuditorDocumentsService,
    private _makerAuthorizerFormService: MakerAuthorizerFormService,

  ) {
    this.edit = this.edit.bind(this);
    this.NewRecord = this.NewRecord.bind(this);
    this.delete = this.delete.bind(this);
  }


  ngOnInit(): void {
    
    this.loadSecRoleForm();
    this.LoadStatus();
    this.onSearch()
  }
  id: number

  edit(e) {
    this.id = e.row.data.id

    this.AuditDocumentForm.controls.Id.setValue(e.row.data.id);
    this.AuditDocumentForm.controls.Code.setValue(e.row.data.code);
    this.AuditDocumentForm.controls.Name.setValue(e.row.data.name);
    this.AuditDocumentForm.controls.Description.setValue(e.row.data.description);
    if (e.row.data.IsActive == true) {
      this.AuditDocumentForm.controls.IsActive.setValue(1);
    } else {
      this.AuditDocumentForm.controls.IsActive.setValue(0);
    }
  }
 
  onTableDataChange(event) {
    this.pagedDto.page = event;
    this.onSearch();
  }
  

  


  loadSecRoleForm() {



    this.formName = "AuditDocuments"
    this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {

      let formName = (this.formName == undefined ? localStorage.getItem('formName') : this.formName)

      this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)

    
      if(this.secRoleForm.authAllowed == true )
      {
        this.isViewShown = true
        if(this.secRoleForm.updateAllowed==true)
        {
        this.isEditShown = true
        }
        else
        {
          this.isEditShown = false
        }
        if(this.secRoleForm.insertAllowed==true)
        {
            this.isAddShown = true
            }
            else
            {
              this.isAddShown = false
            }
      }
      else{
        this.isViewShown = false
        this.isEditShown = false
      }
      //this.isViewShown = this.secRoleForm.authAllowed
    })

  }

  delete(e) {
    abp.message.confirm((""),
      undefined,
      (result: boolean) => {
        if (result) {
          this.AuditorDocumentsService.Delete(e.row.data.id).subscribe((Response) => {
            abp.message.info(Response.message)
            this.onSearch();
          })
        }
      })
  }




  // onSubmit() {
  //   this.submitted = true;
  //   var LoginUserId = localStorage.getItem('userId');
  //   const obj = {
  //     Id: this.AuditDocumentForm.get('Id').value,
  //     Code: this.AuditDocumentForm.get('Code').value,
  //     Name: this.AuditDocumentForm.get('Name').value,
  //     Description : this.AuditDocumentForm.get('Description').value,
  //     IsActive: this.AuditDocumentForm.get('IsActive').value,
  //     CreatedById: LoginUserId,
  //     LastModifiedById: LoginUserId,
  //   }  

  //   if (this.AuditDocumentForm.invalid) {
  //     abp.message.error("Some fields are required ");
  //     return;
  //   }

  // }

  onTableSizeChange(event): void {
    this.pagedDto.pageSize = event.target.value;
    this.onSearch();

   }

  onSubmit(): void {
    //debugger
    this.item = new AuditDocumentsModel();
    this.submitted = true;

    // stop here if form is invalid
    if (this.AuditDocumentForm.invalid) {
      abp.message.error("Some fields are required ");
      return;
    }
    if (this.id > 0 && this.id != null && this.id != undefined) {
      this.item.Id = this.id
    }

    if (this.id > 0) { this.item.Id = this.id }

    // this.item.Id=this.DocumentTypeForm.get('Id').value
    this.item.Name = this.AuditDocumentForm.get('Name').value
    this.item.Code = this.AuditDocumentForm.get('Code').value
    this.item.Description = this.AuditDocumentForm.get('Description').value

    if (this.AuditDocumentForm.get('IsActive').value == 1) {
      this.item.IsActive = true;

    }
    else {
      this.item.IsActive = false;

    }
    var userId = localStorage.getItem('userId');
    this.item.CreatedBy = parseFloat(userId);


    // this.auditdocumentsList.push(this.item);
    // this.item.IsDeleted=this.CertificateForm.get('IsDeleted').value



    this.AuditorDocumentsService.create(this.item).subscribe((Response) => {

    abp.message.info(Response.message)
  
   
  
      this.reloadGrid()
    
      debugger
      this.NewRecord()

    })
  }



  reloadGrid() {
    debugger
    window.location.reload();
    this.pagedDto.page =1
    this.onSearch()
  }

  onSearch() {
debugger
    this.auditdocumentsList == null ;
    this.pagedDto.keyword = this.keyword
    this.pagedDto.authAllowed = true;
    //this.pagedDto.pageSize = 3
    this.AuditorDocumentsService.Get(this.pagedDto).subscribe((Response) => {

     debugger
      this.totalCount = Response.totalCount
      this.auditdocumentsList = Response.auditDocumetTypeModel
    })
  }

  NewRecord() {

    this.id = 0;

    this.AuditDocumentForm.controls.Name.setValue('');
    this.AuditDocumentForm.controls.Code.setValue('');
    this.AuditDocumentForm.controls.Description.setValue('');
    this.AuditDocumentForm.controls.IsActive.setValue('');

  }

  LoadStatus(): void {
    const item = {
      id: 1,
      name: "Active",
    };
    this.StatusList.push(item)

    const item2 = {
      id: 0,
      name: 'InActive',
    }
    this.StatusList.push(item2)
  }
}
