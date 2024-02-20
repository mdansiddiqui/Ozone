import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { LibraryResourceService } from "@shared/Services/library-Resource_service";
import { MsSubClauseService } from "@shared/Services/ms-sub-clause.service";
import { PagedRequestModel } from "@shared/Dto/paged-Request-model";

@Component({
  selector: "app-master-standard-sub-clause",
  templateUrl: "./master-standard-sub-clause.component.html",
  styleUrls: ["./master-standard-sub-clause.component.css"],
})
export class MasterStandardSubClauseComponent implements OnInit {
  public CertificationList: any = [];
  public totalCount: number;
  public pagedDto: PagedRequestModel = new PagedRequestModel();
  public keyword: string = "";
  gridData: any[] = [];
  public subClauseList = [];
  public MainClauseList = [];


  StandardSubClause = new FormGroup({
    Standard: new FormControl(""),
    MainClause: new FormControl(""),
    SubCaluse: new FormControl(""),
    Requirement: new FormControl(""),
  });
  constructor(
    public LibraryResourceService: LibraryResourceService,
    public msSubClauseService: MsSubClauseService
  ) {
    this.edit = this.edit.bind(this); 
    this.clearForm=this.clearForm.bind(this);
     this.delete=this.delete.bind(this);
    //  this.GetMainClause=this.GetMainClause.bind(this);
  }

  ngOnInit(): void {
    this.loadAllQcStandard();
    this.onSearch();
    // this.GetMainClause();
  }

  id: any;
  submit() {
    debugger;
    let subClauseId = 0;
    if (
      this.id > 0 &&
      this.id != null &&
      this.id != undefined &&
      !Number.isNaN(this.id)
    ) {
      subClauseId = this.id;
    }
    const starndard = Number(this.StandardSubClause.get("Standard").value);
    const mainClause = Number(this.StandardSubClause.get("MainClause").value);
    const subclause = this.StandardSubClause.get("SubCaluse").value;
    const requirement = this.StandardSubClause.get("Requirement").value;

    let dataGrid = {
      Id: subClauseId,
      StandardId: starndard,
      MainClauseId: mainClause,
      SubClauseName: subclause,
      Requirement: requirement,
    };

    this.msSubClauseService.submitData(dataGrid).subscribe((res) => {
      console.log("res", res);

      abp.message.info(res.message);
      this.clearForm();
      this.reloadGrid();
    });

    console.log("dataGrid", dataGrid);
  }

  reloadGrid()
 
  {
  
    this.pagedDto.page =1;
    this.onSearch();
  }

  loadAllQcStandard(): void {
    debugger;
    this.LibraryResourceService.getAllCertification().subscribe((Response) => {
      console.log("CertificationList", Response);

      this.CertificationList = Response;
      console.log("this.CertificationList", this.CertificationList);

    });
  }
  edit(e) {
    debugger;
    this.id = e.row.data.id;
    // console.log('e.row.data.id', e.row.data.id);
    this.StandardSubClause.controls.MainClause.setValue(e.row.data.mainClauseId);

    this.StandardSubClause.controls.SubCaluse.setValue(e.row.data.subClauseName);
    this.StandardSubClause.controls.Requirement.setValue(e.row.data.requirement);
    this.StandardSubClause.controls.Standard.setValue(e.row.data.standardId);

    console.log("this.StandardMainClause", e.row.data);
  }

  onSearch() {
    debugger;

    this.pagedDto.keyword = this.keyword;
    //  if(this.secRoleForm.authAllowed==true || this.secRoleForm.authAllowed==false)
    //  {
    //this.pagedDto.authAllowed = this.secRoleForm.authAllowed
    // }
    //this.pagedDto.pageSize = 3
    this.msSubClauseService.Get(this.pagedDto).subscribe((Response) => {
      console.log('msMainClauseService',Response);

      // console.log('msMainClauseService',this.gridData);

      this.gridData = Response.subClauseModel;
      console.log('Response.mainClauseModel',this.gridData);
      

      debugger;
      this.totalCount = Response.totalCount;
      this.subClauseList = Response.qcDocumentsListModel;
    });
  }

  clearForm() {
    this.id = 0;
    this.StandardSubClause.controls.Standard.setValue("");
    this.StandardSubClause.controls.MainClause.setValue("");
    this.StandardSubClause.controls.SubCaluse.setValue("");
    this.StandardSubClause.controls.Requirement.setValue("");
  }

  delete(e) {
    debugger
       abp.message.confirm((""),
       undefined,
           (result: boolean) => {
            debugger
               if (result) {
                     this.id = e.row.data.id
                     this.msSubClauseService.Delete(e.row.data.id).subscribe((Response) => {
                      abp.message.info(Response.message)
                      
    
                       abp.message.info(Response.message)
                       this.onSearch();
                     
                      })
                     
               }
             }
       )
        }

        GetMainClause(e): void {
          debugger
          this.msSubClauseService.GetMainClause(Number(e)).subscribe((Response) => {
            this.MainClauseList = Response

            console.log('this.StateList', Response);
            
          })
        }

    
}
