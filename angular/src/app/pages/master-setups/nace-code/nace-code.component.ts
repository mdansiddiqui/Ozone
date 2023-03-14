import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {Injectable} from '@angular/core';
import {HttpResponse} from '@angular/common/http';
import {NgxPaginationModule} from 'ngx-pagination'; // <-- import the module
import {Observable} from 'rxjs';
import { PagedRequestModel } from '@shared/Dto/paged-Request-model';
import { MakerAuthorizerFormService } from '@shared/Services/maker-authorizer-form.service';
import { ActivatedRoute, Router} from '@angular/router';
import repaintFloatingActionButton from 'devextreme/ui/speed_dial_action/repaint_floating_action_button';
import { DxListModule } from "devextreme-angular";
import {  DxDataGridModule,DxDataGridComponent,DxSpeedDialActionModule,DxSelectBoxModule} from 'devextreme-angular';
import { DxTreeListModule } from "devextreme-angular";
import { Employee, EmployeesService } from '@app/pages/sales/employees.service';
import DataSource from 'devextreme/data/data_source';
import { BrowserModule } from '@angular/platform-browser';
import { DxContextMenuModule } from "devextreme-angular";
import { IndentRequestService } from '@shared/Services/indent-request-service';
import { NaceCodeModel } from '@shared/Dto/nace-code-model';
import {NaceCodeService} from 'shared/Services/Nace-Code-service';

import { ToastrService } from 'ngx-toastr';
import { UserStandardService } from '@shared/Services/User-Standard-service';
@Component({
  selector: 'app-nace-code',
  templateUrl: './nace-code.component.html',
  styleUrls: ['./nace-code.component.css']
})
export class NaceCodeComponent implements OnInit {
  public item: NaceCodeModel = new NaceCodeModel();
  NaceCodeForm = new FormGroup({
    // Id: new FormControl(''),
    Id: new FormControl(''),
    Name: new FormControl(''),
    IsActive: new FormControl(''),
    Description: new FormControl(''),
    Code: new FormControl(''),
    EaCodeId: new FormControl(''),
    RiskLevelId: new FormControl(''),
  })

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
  public NaceCodeList = []
  public moduleList = [];
  public EAcodeList=[];
  public RiskLevelList=[];
  public StatusList=[];
  fileToUpload: any;
  submitted = false;

  get f() { return this.NaceCodeForm.controls; }
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
    private _UserStandardService: UserStandardService,
    private _makerAuthorizerFormService: MakerAuthorizerFormService,
    private _toster: ToastrService,
    private route: ActivatedRoute,
     public NaceCodeService: NaceCodeService,
     private router : Router) 
      { this.edit = this.edit.bind(this); 
      this.NewRecord=this.NewRecord.bind(this);
      this.delete=this.delete.bind(this);}

      ngOnInit(): void {
        
        
        this.loadSecRoleForm();
        this.loadStatus();
        this.onSearch();
        this.loadEaCode();
        this.loadRiskLevel();
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
      this.NaceCodeForm.controls.Id.setValue(e.row.data.id);
      this.NaceCodeForm.controls.Name.setValue(e.row.data.name);
      this.NaceCodeForm.controls.Code.setValue(e.row.data.code);
      this.NaceCodeForm.controls.Description.setValue(e.row.data.description); 
      this.NaceCodeForm.controls.EaCodeId.setValue(e.row.data.eaCodeId);
      this.NaceCodeForm.controls.RiskLevelId.setValue(e.row.data.riskLevelId);
    
    // this.CertificateForm.controls.StartDate.setValue(e.row.data.startdate);
      //let req = new Date(this.datePipe.transform(e.row.data.startDate, 'yyyy/MM/dd'))
    
       //this.CertificateForm.controls.StartDate.setValue(this.datePipe.transform(req, 'yyyy/MM/dd'))
    
      if(e.row.data.isActive==true){
      this.NaceCodeForm.controls.IsActive.setValue(1);
      }else{this.NaceCodeForm.controls.IsActive.setValue(0);}
      
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
     
                       this.NaceCodeService.Delete(e.row.data.id).subscribe((Response)=>{
      
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
    debugger
      
        
      this.formName = "NaceCode"
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
        debugger
        this.item=new NaceCodeModel();
        this.submitted = true;
    
        // stop here if form is invalid
        if (this.NaceCodeForm.invalid) {
          abp.message.error("Some fields are required ");
          return;
        }
      if(this.id>0){this.item.Id=this.id}
    
    // this.item.Id=this.DocumentTypeForm.get('Id').value
    this.item.Name=this.NaceCodeForm.get('Name').value
    this.item.Code=this.NaceCodeForm.get('Code').value
    this.item.Description=this.NaceCodeForm.get('Description').value
    this.item.EaCodeId=this.NaceCodeForm.get('EaCodeId').value
    this.item.RiskLevelId=this.NaceCodeForm.get('RiskLevelId').value
    
    if(this.NaceCodeForm.get('IsActive').value==1)
    {
      this.item.IsActive=true;
    
    }
    else
    { 
      this.item.IsActive=false;
    
    }
    var userId =localStorage.getItem('userId');
    this.item.CreatedBy=parseFloat(userId);
    // this.item.IsDeleted=this.CertificateForm.get('IsDeleted').value
    
          this.NaceCodeService.create(this.item).subscribe((Response)=>{
     
        abp.message.info(Response.message)
        this.reloadGrid();
        this.NewRecord();
       
       })
    }
    
    onSearch(){
      
        debugger
      this.pagedDto.keyword = this.NaceCodeForm.get('EaCodeId').value;
      this.pagedDto.authAllowed = this.secRoleForm.authAllowed
      //this.pagedDto.pageSize = 3
      this.NaceCodeService.Get(this.pagedDto).subscribe((Response) => {
                  
      
        this.totalCount = Response.totalCount
        this.NaceCodeList = Response.naceCodeModel
        console.log(this.NaceCodeList)
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
      this.NaceCodeForm.controls.Id.setValue('');
      this.NaceCodeForm.controls.Name.setValue('');
      this.NaceCodeForm.controls.Code.setValue('');
      this.NaceCodeForm.controls.Description.setValue(''); 
      this.NaceCodeForm.controls.IsActive.setValue(''); 
      this.NaceCodeForm.controls.EaCodeId.setValue('');
      this.NaceCodeForm.controls.RiskLevelId.setValue('');
    }
    
    loadEaCode(): void {
      
      this.NaceCodeService.getAllEACode().subscribe((Response)=>{
        this.EAcodeList = Response

        this.EAcodeList.push({id: 0 , name: "-----Select ALL-----" })
          
      })
    }

    loadRiskLevel(): void {
      
      this.NaceCodeService.getAllRiskLevel().subscribe((Response)=>{
        this.RiskLevelList = Response
          
      })
    }
    
    
    
    
    }
