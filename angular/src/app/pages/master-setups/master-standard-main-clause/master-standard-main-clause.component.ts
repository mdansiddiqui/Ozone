

import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { LibraryResourceService } from 'shared/Services/library-Resource_service';
import { MsMainClauseService } from 'shared/Services/ms-main-clause.service';


@Component({
  selector: 'app-master-standard-main-clause',
  templateUrl: './master-standard-main-clause.component.html',
  styleUrls: ['./master-standard-main-clause.component.css']
})
export class MasterStandardMainClauseComponent implements OnInit{
public CertificationList : any =[];
gridData: any[] = [];
public pagedDto: PagedRequestModel = new PagedRequestModel()
public keyword : string = ''
public mainClauseList = [];
public totalCount: number


  StandardMainClause = new FormGroup({
    
    Standard: new FormControl(''),
    MainClause: new FormControl(''),
    Headings: new FormControl(''),
    Requirement: new FormControl(''),
   
  })
griddata: any;
     constructor(
     public LibraryResourceService: LibraryResourceService,
     public msMainClauseService :MsMainClauseService,
     ) 
  {
    this.edit = this.edit.bind(this); 
    this.clearForm=this.clearForm.bind(this);
     this.delete=this.delete.bind(this);
  }

  ngOnInit(): void {
    debugger
    this.loadAllQcStandard();
    this.onSearch()
  };

  
  loadAllQcStandard(): void {
    debugger
    this.LibraryResourceService.getAllCertification().subscribe((Response) => {
      console.log('CertificationList', Response);
      
      this.CertificationList = Response
      console.log('this.CertificationList',this.CertificationList);
      
    })
  }


  submit(){
    debugger
let MainClauseId=0
    if(this.id>0 && this.id!=null && this.id!=undefined && !Number.isNaN(this.id))
    {
      MainClauseId = this.id
    }

    const standard = Number(this.StandardMainClause.get('Standard').value);
    const mainClause = this.StandardMainClause.get('MainClause').value;
    const headings =  this.StandardMainClause.get('Headings').value;
    const  Requirement = this.StandardMainClause.get('Requirement').value;

   // const standardnumber = Number(standard)

    let griddata={ Id: MainClauseId,
      StandardId:standard,MainClauseName:mainClause,Heading:headings,Requirement:Requirement
    }

   // this.gridData.push(griddata);
    //console.log('gridData to show', this.gridData);
debugger
    this.msMainClauseService.submitData(griddata).subscribe(res=>{
      debugger
      console.log(res);

      abp.message.info(res.message)
      this.reloadGrid();
      this.clearForm();
      

    })
    
    
  }
  reloadGrid()
 
  {
  
    this.pagedDto.page =1;
    this.onSearch();
  }
  onSearch(){
    debugger
      
    this.pagedDto.keyword = this.keyword
  //  if(this.secRoleForm.authAllowed==true || this.secRoleForm.authAllowed==false)
  //  {
     //this.pagedDto.authAllowed = this.secRoleForm.authAllowed
  // }
    //this.pagedDto.pageSize = 3
    this.msMainClauseService.Get(this.pagedDto).subscribe((Response) => {

      // console.log('msMainClauseService',Response);

      // console.log('msMainClauseService',this.gridData);

      this.gridData = Response.mainClauseModel

      
                
    debugger
      this.totalCount = Response.totalCount
      this.mainClauseList = Response.qcDocumentsListModel
    })
  }

  clearForm(){
    this.id =0;
    this.StandardMainClause.controls.Standard.setValue('');
    this.StandardMainClause.controls.MainClause.setValue('');
    this.StandardMainClause.controls.Headings.setValue('');
    this.StandardMainClause.controls.Requirement.setValue('');

  }
  id: any;
  edit(e) {
    debugger
    this.id = e.row.data.id;
    // console.log('e.row.data.id', e.row.data.id);
    this.StandardMainClause.controls.MainClause.setValue(e.row.data.mainClauseName);

     this.StandardMainClause.controls.Headings.setValue(e.row.data.heading);
    this.StandardMainClause.controls.Requirement.setValue(e.row.data.requirement);
    this.StandardMainClause.controls.Standard.setValue(e.row.data.standardId);

    console.log('this.StandardMainClause',e.row.data);
    
}

  delete(e) {
    debugger
       abp.message.confirm((""),
       undefined,
           (result: boolean) => {
            debugger
               if (result) {
                     this.id = e.row.data.id
                     this.msMainClauseService.Delete(e.row.data.id).subscribe((Response) => {
                      abp.message.info(Response.message)
                      
    
                       abp.message.info(Response.message)
                       this.onSearch();
                     
                      })
                     
               }
             }
       )
        }

}
