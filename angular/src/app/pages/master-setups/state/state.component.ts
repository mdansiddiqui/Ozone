import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee, EmployeesService } from '@app/pages/sales/employees.service';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { IndentRequestService } from '@shared/Services/indent-request-service';
import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { ToastrService } from 'ngx-toastr';
import { StateModel } from '@shared/Dto/state-model';
import {StateService} from 'shared/Services/state-service';
import { SecUserService } from '@shared/Services/sec-user.service';


@Component({
  selector: 'app-state',
  templateUrl: './state.component.html',
  styleUrls: ['./state.component.css']
})
export class StateComponent implements OnInit {
  
  public item: StateModel = new StateModel();
  StateForm = new FormGroup({
    // Id: new FormControl(''),
    Id: new FormControl(''),
    Name: new FormControl(''),
    CountryId: new FormControl(''),
    IsActive: new FormControl(''),
    Code: new FormControl(''),
   
  })
  submitted = false;
  get f() { return this.StateForm.controls; }

  employees: Employee[] = [];
  @Input() formName : string
  secRoleForm
  tableSizes = [3, 5, 10, 15, 20, 25];
  public totalCount: number
  public pagedDto: PagedRequestModel = new PagedRequestModel()
  pageNumber : number = 1
  pageSize : number = 10
  public isEditShown : boolean  
  public isViewShown : boolean  
  public isAddShown : boolean  
  public keyword : string = ''
  public StateList = []
  public CountryList=[]
  public moduleList = [];
  public StatusList=[];
  fileToUpload: any;
  LibraryForm = new FormGroup({
   
    ModuleId: new FormControl(''),
    
  
  })
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

  constructor(service: EmployeesService,
    private indentRequestService : IndentRequestService,
    private _makerAuthorizerFormService: MakerAuthorizerFormService,
    public SecUserService : SecUserService,
    private _toster: ToastrService,
    private route: ActivatedRoute,
     public StateService: StateService,
     private router : Router) 
      { this.edit = this.edit.bind(this); 
      this.NewRecord=this.NewRecord.bind(this);
      this.delete=this.delete.bind(this);}

  ngOnInit(): void  {
    
    
    this.loadSecRoleForm();
    this.loadStatus();
    this.onSearch();
    this.loadCountries()
  }
  loadCountries(): void {
    
    
    this.SecUserService.getCountries().subscribe((Response)=>{
      this.CountryList = Response
    })
    }
  id: number

  edit(e) {  
     
// var List = [];
// List=this.Liststandard                                                                             ; 
// this.router.navigateByUrl('/app/pages/stock-management/library');
this.id=e.row.data.id
// var updateDate =this.StandardList.find(x => x.id == this.id );

// this._StandardService.GetStandardById(this.id).subscribe((res) => 
// {

 // this.ModuleForm.controls.Code.setValue(e.row.data.code);
  this.StateForm.controls.Id.setValue(e.row.data.id);
  this.StateForm.controls.Name.setValue(e.row.data.name);
  this.StateForm.controls.Code.setValue(e.row.data.code);
  this.StateForm.controls.CountryId.setValue(e.row.data.countryId);
  

// this.CertificateForm.controls.StartDate.setValue(e.row.data.startdate);
  //let req = new Date(this.datePipe.transform(e.row.data.startDate, 'yyyy/MM/dd'))

   //this.CertificateForm.controls.StartDate.setValue(this.datePipe.transform(req, 'yyyy/MM/dd'))

  if(e.row.data.isActive==true){
  this.StateForm.controls.IsActive.setValue(1);
  }else{this.StateForm.controls.IsActive.setValue(0);}
  
  // });
  // this.router.navigate(['app/pages/stock-management/library']);
    //this.router.navigate(["account/login"]);
// this.router.navigateByUrl('/app/pages/certification-setups/Standard?'+this.id);  
}  


delete(e) {
  
     abp.message.confirm((""),
     undefined,
         (result: boolean) => {
             if (result) {
               // this.SecUserService.Deleteuser(e.row.data.id).subscribe() 
               //     abp.message.info("Deleted successfully", "Status", {});
 
                   this.StateService.Delete(e.row.data.id).subscribe((Response)=>{
  
                     abp.message.info(Response.message)
                     this.onSearch();
                    
                    })
                   
             }
           }
      )}

onTableDataChange(event) {
  this.pagedDto.page = event;
  this.onSearch();
}
loadSecRoleForm() {

  
    
  this.formName = "State"
  this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {
      
    let formName = (this.formName == undefined ? localStorage.getItem('formName') : this.formName)
      
    this.secRoleForm = data.find(x => x.formCode != null && x.formCode == this.formName)

  //this.isEditShown= this.secRoleForm.authAllowed
  // this.isViewShown = this.secRoleForm.authAllowed

  // var formName = "User"
  // this._makerAuthorizerFormService.getSecRoleForm().subscribe((data) => {
  //   this.secRoleForm = data.find(x => x.formName.toLocaleLowerCase() == formName.toLocaleLowerCase())
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


onTableSizeChange(event): void {
  this.pagedDto.pageSize = event.target.value;
  this.onSearch();

 }

  onSubmit(): void {
    
    this.submitted = true;

    // stop here if form is invalid
    if (this.StateForm.invalid) {
      abp.message.error("Some fields are required ");
      return;
    }

  if(this.id>0){this.item.Id=this.id}

// this.item.Id=this.DocumentTypeForm.get('Id').value
this.item.Name=this.StateForm.get('Name').value
this.item.Code=this.StateForm.get('Code').value
this.item.CountryId=this.StateForm.get('CountryId').value

if(this.StateForm.get('IsActive').value==1)
{
  this.item.IsActive=true;

}
else
{ 
  this.item.IsActive=false;

}
// var userId =localStorage.getItem('userId');
// this.item.CreatedBy=parseFloat(userId);


// this.item.IsDeleted=this.CertificateForm.get('IsDeleted').value

      this.StateService.create(this.item).subscribe((Response)=>{
 
    abp.message.info(Response.message)
    this.reloadGrid();
    this.NewRecord();
   
   })
}

onSearch(){
  
    
  this.pagedDto.keyword = this.keyword
  this.pagedDto.authAllowed = this.secRoleForm.authAllowed
  //this.pagedDto.pageSize = 3
  this.StateService.Get(this.pagedDto).subscribe((Response) => {
              
  
    this.totalCount = Response.totalCount
    this.StateList = Response.stateModel
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
  name:'InActive',
  };
this.StatusList.push(item2);

  } 

reloadGrid()
 
{

  this.pagedDto.page =1;
  this.onSearch();
}

NewRecord()
 {  
   this.id=0;
  this.StateForm.controls.Id.setValue('');
  this.StateForm.controls.Name.setValue('');
  this.StateForm.controls.Code.setValue('');
  this.StateForm.controls.CountryId.setValue('');
 this.StateForm.controls.IsActive.setValue('');
}

}
