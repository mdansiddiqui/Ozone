import { VisitLevel } from "./../../../../shared/Dto/visit-level.model";
import { ClientAuditVisitService } from "./../../../../shared/Services/Client-Audit-visit-service";
import { Component, OnInit } from "@angular/core";
import { PagedRequestModel } from "@shared/Dto/paged-Request-model";
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators,
} from "@angular/forms";

@Component({
  selector: "app-visit-level",
  templateUrl: "./visit-level.component.html",
  styleUrls: ["./visit-level.component.css"],
})
export class VisitLevelComponent implements OnInit {
  public VisitLevelData = [];

  public item: VisitLevel = new VisitLevel();
  VisitLevelForm = new FormGroup({
    Id: new FormControl(""),
    Name: new FormControl(""),
    Description: new FormControl(""),
    IsActive: new FormControl(""),
    Code: new FormControl(""),
  });
  submitted = false;
  id: number;
  mode: boolean;
  store: number;
  check: boolean;
  checkFields() {
    if (
      this.VisitLevelForm.get("Name").value == null &&
      this.VisitLevelForm.get("Name").value == '' &&
      this.VisitLevelForm.get("Name").value == undefined
    ) {
      this.check == true;
    } else {
      this.check == false;
    }
  }
  get f() { return this.VisitLevelForm.controls; }
  onSubmit(): void {
    this.checkFields();
    this.item = new VisitLevel();
    this.submitted = true;
    if (this.VisitLevelForm.invalid || this.check === true) {
      abp.message.error("Some fields are required ");
      return;
    }
    if (this.mode == true) {
      console.log(this.store);
      this.item.Id = this.store;
      this.item.Name = this.VisitLevelForm.get("Name").value;
      this.item.Code = this.VisitLevelForm.get("Code").value;
      this.item.Description = this.VisitLevelForm.get("Description").value;
      console.log(this.VisitLevelForm.get("IsActive").value);
      if (this.VisitLevelForm.get("IsActive").value == 1) {
        this.item.IsActive = true;
      } else {
        this.item.IsActive = false;
      }
      this._ClientAuditVisitService
        .CreateVisitLevel(this.item)
        .subscribe((Response) => {
          abp.message.info(Response.message);
          this.reloadGrid();
        });
    } else {
      if (
        this.id > 0 &&
        this.id != null &&
        this.id != undefined &&
        this.id != NaN
      ) {
        this.item.Id = this.id;
      }
      this.item.Name = this.VisitLevelForm.get("Name").value;
      this.item.Code = this.VisitLevelForm.get("Code").value;
      this.item.Description = this.VisitLevelForm.get("Description").value;
      console.log(this.VisitLevelForm.get("IsActive").value);
      if (this.VisitLevelForm.get("IsActive").value == 1) {
        this.item.IsActive = true;
      } else {
        this.item.IsActive = false;
      }
      this._ClientAuditVisitService
        .CreateVisitLevel(this.item)
        .subscribe((Response) => {
          abp.message.info(Response.message);
          this.reloadGrid();
        });
    }
    this.resetFormControls();
    setTimeout(() => {
      this.GetAllVisitLevel();
    }, 5000);
  }
  AddClientVisitLevel () {
    this.mode = false
    this.resetFormControls();
  }
  StatusList = [
    { id: 1, name: "Active" },
    { id: 2, name: "Not Active" },
  ];
  formFields = [
    {
      name: "Code",
      label: "Code",
      type: "text",
      options: null,
    },
    {
      name: "Name",
      label: "Name",
      type: "text",
      options: null,
    },
    {
      name: "Description",
      label: "Description",
      type: "text",
      options: null,
    },

    // ... add more fields here
  ];

  resetFormControls() {
    for (const controlName of Object.keys(this.VisitLevelForm.controls)) {
      const control = this.VisitLevelForm.get(controlName);
      control.setValue("");
    }
  }
  constructor(
    private _ClientAuditVisitService: ClientAuditVisitService,
    private fb: FormBuilder
  ) {
    this.editClientData = this.editClientData.bind(this);
    this.deleteClientData = this.deleteClientData.bind(this);
  }

  ngOnInit(): void {
    this.check == false;
    this.mode = false;
    this.GetAllVisitLevel();
  }
  public totalCount: number;
  public pagedDto: PagedRequestModel = new PagedRequestModel();
  public AllClientVisit = [];
  public isAddShown: boolean;
  public keyword : string = ''


  editClientData(e) {
    this.mode = true;
    this.store = e.row.data.id;
    this.VisitLevelForm.get("Id").setValue(e.row.data.id);
    this.VisitLevelForm.get("Code").setValue(e.row.data.code);
    this.VisitLevelForm.get("Description").setValue(e.row.data.description);
    this.VisitLevelForm.get("Name").setValue(e.row.data.name);
    if (e.row.data.isActive == true) {
      this.VisitLevelForm.get("IsActive").setValue(1);
    } else {
      this.VisitLevelForm.get("IsActive").setValue(2);
    }
  }
  GetAllVisitLevel() {
    this.pagedDto.keyword = this.keyword
    this.pagedDto.organizationId = parseInt( localStorage.getItem('organizationId'));

    this._ClientAuditVisitService.GetALLVisitLevel().subscribe((Response) => {
      this.VisitLevelData = Response;
      console.log(Response);
    });
  }
  deleteClientData(e) {
    abp.message.confirm("", undefined, (result: boolean) => {
      if (result) {
        this._ClientAuditVisitService
          .DeleteVisitLevelById(e.row.data.id)
          .subscribe((Response) => {
            abp.message.info(Response.message);
          });
          setTimeout(() => {
            this.GetAllVisitLevel();
          }, 2000)
      }
    });
  }
  reloadGrid() {
    this.pagedDto.page = 1;
    this.GetAllVisitLevel();
  }
  onTableDataChange(event) {
    this.pagedDto.page = event;
    this.GetAllVisitLevel();
  }
}
