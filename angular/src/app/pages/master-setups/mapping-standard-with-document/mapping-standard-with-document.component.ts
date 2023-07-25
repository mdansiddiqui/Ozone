import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { AuditReportService } from '@shared/Services/Audit-report-service';
import { ClientAuditVisitService } from '@shared/Services/Client-Audit-visit-service';
import { DocumentTypeService } from '@shared/Services/document-type';
import { UserStandardService } from '@shared/Services/User-Standard-service';
import { MappingStandardDocumentModel } from "./../../../../shared/Dto/mapping-standard-document-model";
@Component({
  selector: 'app-mapping-standard-with-document',
  templateUrl: './mapping-standard-with-document.component.html',
  styleUrls: ['./mapping-standard-with-document.component.css']
})
export class MappingStandardWithDocumentComponent implements OnInit {

  public item: MappingStandardDocumentModel = new MappingStandardDocumentModel();
  public pagedDto: PagedRequestModel = new PagedRequestModel()
  MapForm = new FormGroup({
    StandardId: new FormControl(''),
    DocumentTypeId: new FormControl(''),
    VisitLevelId: new FormControl(''),
    IsActive: new FormControl(''),
    IsRequired: new FormControl(''),
    DocumentForReviewer:new FormControl(''),
    DocumentAssignId:new FormControl(''),
  })

  public StandardList = [];
  public DocumentsTypeList = [];
  public StatusList = [];
  public RequireStatusList = [];
  public VisitLevelList = [];
  public GridData = [];
  public documentfor=[];
  public documentAssignfor=[];
  submitted = false;
  public isAddShown: boolean;
  id: number;

  constructor(
    private formBuilder: FormBuilder,
    private _UserStandardService: UserStandardService,
    public _AuditReportService: AuditReportService,
    public _DocumentTypeService: DocumentTypeService,
    private _ClientAuditVisitService: ClientAuditVisitService,

    ) {
      this.deleteMWSD=this.deleteMWSD.bind(this);
      this.editMSWD=this.editMSWD.bind(this);
      this.NewRecord=this.NewRecord.bind(this);
  }

  ngOnInit(): void {
   this.loadStandard()
   this.GetAllDocumentsType()
   this.loadStatus()
   this.onSearch()
   this.LoadVisitLevel()
  }
  NewRecord() {
   this.id = 0;
   this.MapForm.controls.StandardId.setValue('');
   this.MapForm.controls.DocumentTypeId.setValue('');
   this.MapForm.controls.VisitLevelId.setValue('');
   this.MapForm.controls.IsActive.setValue('');
   this.MapForm.controls.IsRequired.setValue('');

 }
  loadStandard(): void {

    this._UserStandardService.getAllStandard().subscribe((Response) => {
      this.StandardList = Response
      console.log(this.StandardList)

    })
  }
  GetAllDocumentsType(): void {

    this._AuditReportService.GetAllDocumentsType().subscribe((Response) => {

      this.DocumentsTypeList = Response

    })
  }

  loadStatus(): void {

    const item = {
      id: 1,
      name:'Active',
    };
   this.StatusList.push(item);
   const item2 = {
    id: 0,
    name:'NotActive',
    };
  this.StatusList.push(item2);

    const item3 = {
      id: 1,
      name:'Required',
    };
  this.RequireStatusList.push(item3);
  const item4 = {
    id: 0,
    name:'Not Required',
    };
  this.RequireStatusList.push(item4);



  const document = {
    id: 1,
    name:'Reviewer',
  };
 this.documentfor.push(document);
 const document1 = {
  id: 0,
  name:'Auditor',
  };
this.documentfor.push(document1);



const documentAssign = {
  id: 1,
  name:'Auditor',
};
this.documentAssignfor.push(documentAssign);
const documentAssign2 = {
id: 2,
name:'Reviewer',
};
this.documentAssignfor.push(documentAssign2);
   

    const documentAssign3 = {
      id: 3,
      name:'Manager',
      };
      this.documentAssignfor.push(documentAssign3);
          }

  get f() { return this.MapForm.controls; }

  deleteMWSD(e) {

    abp.message.confirm((""),
    undefined,
        (result: boolean) => {
            if (result) {

                  this._DocumentTypeService.DeleteMSWD(e.row.data.id).subscribe((Response)=>{

                    abp.message.info(Response.message)
                    this.onSearch();

                   })

            }
          }
     )}

  onSubmit(): void {
debugger
console.log(this.id)

      this.item = new MappingStandardDocumentModel ();
      this.submitted = true;

      // stop here if form is invalid
      if (this.MapForm.invalid) {
        abp.message.error("Some fields are required ");
        return;
      }

      if(this.id>0 && this.id!=null && this.id!=undefined && !Number.isNaN(this.id))
      {
            this.item.Id = this.id
            console.log(this.item.Id)
      }

      // this.item.Id=this.MapForm.get('Id').value
      this.item.StandardId=this.MapForm.get('StandardId').value
      this.item.DocumentTypeId=this.MapForm.get('DocumentTypeId').value
      this.item.VisitLevelId=this.MapForm.get('VisitLevelId').value
      this.item.DocumentAssignId=this.MapForm.get('DocumentAssignId').value
      
      if(this.MapForm.get('IsActive').value==1)
      {
        this.item.IsActive=true;

      }
      else
      {
        this.item.IsActive=false;

      }
      if(this.MapForm.get('IsRequired').value==1)
      {
        this.item.IsRequired=true;

      }
      else
      {
        this.item.IsRequired=false;

      }
      // if(this.MapForm.get('DocumentForReviewer').value==1)
      // {
      //   this.item.DocumentForReviewer=true;

      // }
      // else
      // {
      //   this.item.DocumentForReviewer=false;

      // }
      
// this.item.IsDeleted=this.CertificateForm.get('IsDeleted').value

      this._DocumentTypeService.createMSWD(this.item).subscribe((Response)=>{

    abp.message.info(Response.message)
    this.reloadGrid();
    this.NewRecord();
    this.onSearch()
   })
}
onTableDataChange(event) {
  this.pagedDto.page = event;
  this.onSearch();
}
editMSWD(e) {
  debugger
  this.id=e.row.data.id
    console.log(this.id)
    // this.MapForm.controls.Id.setValue(e.row.data.id);
    this.MapForm.controls.StandardId.setValue(e.row.data.standardId);
    this.MapForm.controls.DocumentTypeId.setValue(e.row.data.documentTypeId);
    this.MapForm.controls.VisitLevelId.setValue(e.row.data.visitLevelId);

    if(e.row.data.isActive==true){
    this.MapForm.controls.IsActive.setValue(1);
    }else{this.MapForm.controls.IsActive.setValue(0);}
    if(e.row.data.IsRequired==true){
      this.MapForm.controls.IsRequired.setValue(1);
      }else{this.MapForm.controls.IsRequired.setValue(0);}
      
      if(e.row.data.documentForReviewer==true){
        this.MapForm.controls.documentForReviewer.setValue(1);
        }else{this.MapForm.controls.documentForReviewer.setValue(0);}

  }

reloadGrid () {
  this.onSearch();
}
LoadVisitLevel(): void {


  this._ClientAuditVisitService.GetALLVisitLevel().subscribe((Response) => {
    this.VisitLevelList = Response

  })
}
onSearch(){


 // this.pagedDto.keyword = this.keyword
  this.pagedDto.authAllowed = true;
  this.pagedDto.pageSize = 3
  this._DocumentTypeService.GetMSWD(this.pagedDto).subscribe((Response) => {
  console.log(Response)
    this.GridData = Response
    // this.totalCount = Response.totalCount
    // this.DocumentList = Response.documentsTypeModel
  })
}
onTableSizeChange(event): void {
  this.pagedDto.pageSize = event.target.value;
  this.onSearch();
 }

}
