import { Component, OnInit, ɵINJECTOR_IMPL__POST_R3__ } from '@angular/core';
import { FormControl, FormGroup ,FormBuilder} from '@angular/forms';

@Component({
  selector: 'app-audit-findings',
  templateUrl: './audit-findings.component.html',
  styleUrls: ['./audit-findings.component.css']
})
export class AuditFindingsComponent implements OnInit {
  // SummarOfAuditFindings: FormGroup;
  gridData: any[] = [];

  SummaryOfAuditFindings = new FormGroup({
    Selectdata:new FormControl(''),
    MajorNCs: new FormControl(''),
    MinorNCs: new FormControl(''),
    CheckedThisVisit: new FormControl(''),
    CriticalNCs: new FormControl(''),
    TimeBoundNCs: new FormControl(''),
    Ofi: new FormControl(''),
    CheckNextVisit: new FormControl(''),
    Remarks: new FormControl(''),
    Stage1Observations: new FormControl(''),
   
  })
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {

    // this.SummarOfAuditFindings = this.fb.group({
    //   selectedField: [''],
    //   checkedThisVisit: [''],
    //   majorNCs: [''],
    //   minorNCs: [''],
    //   criticalNCs: [''],
    //   timeBoundNCs: [''],
    //   stage1Observations: [''],
    //   ofi: [''],
    //   checkNextVisit: [''],
    //   remarks: ['']
    // });
  }
 statusList:any=[
  {name:'Top Management',id:0},
  {name:'Workers interview and Group Interview: Clause 1 to 9',id:1},
  {name:'Child Labour & PI',id:2},
  {name:'Forced or Compulsory Labour & PI',id:3},
  {name:'Health and Safety & PI',id:0},
  {name:'Freedom of Association & Right to Collective Bargaining & PI',id:4},
  {name:'Discrimination & PI',id:5},
  {name:'Disciplinary Practices & PI',id:6},
  {name:'Working Hours & PI',id:7},
  {name:'Remuneration and Basic Needs Wage & PI',id:8},
  {name:'Management Systems - Policies, Procedures and Records & PI',id:9},
  {name:'Management Systems - Social Performance Team & PI',id:10},
  {name:'Management Systems - Identification and Assessment of Risks & PI',id:11},
  {name:'Management Systems-Monitoring & PI',id:12},
  {name:'Management Systems - Internal Involvement and Communication & PI',id:13},
  {name:'Management Systems - Complaint Management and Resolution & PI',id:14},
  {name:'Management Systems - External Verification and Stakeholder Engagement & PI',id:15},
  {name:'Management Systems - Corrective and Preventive Action & PI',id:16},
  {name:'Management Systems - Training and Capacity Building & PI',id:17},
  {name:'Management Systems-Management of Suppliers and Contractors & PI',id:18},
  {name:'Review of Previous Audit Findings',id:19},
  {name:'Social Fingerprint Review & Completion of independent evaluation on social finger print',id:20},
  {name:'Use of SAAS/CB Logo',id:21},
 ];

checkVisitList :any = [
  {
    id: 0,
    name:'Yes',
  },
  {
    id: 1,
    name:'No',
    }
];




  onSubmit(): void {
    debugger
    // Get values from form controls
    const selectedField = this.SummaryOfAuditFindings.get('Selectdata').value;
    const checkedThisVisit = this.SummaryOfAuditFindings.get('CheckedThisVisit').value;
    const majorNCs = this.SummaryOfAuditFindings.get('MajorNCs').value;
    const minorNCs = this.SummaryOfAuditFindings.get('MinorNCs').value;
    const criticalNCs = this.SummaryOfAuditFindings.get('CriticalNCs').value;
    const timeBoundNCs = this.SummaryOfAuditFindings.get('TimeBoundNCs').value;
    const stage1Observations = this.SummaryOfAuditFindings.get('Stage1Observations').value;
    const ofi = this.SummaryOfAuditFindings.get('Ofi').value;
    const checkNextVisit = this.SummaryOfAuditFindings.get('CheckNextVisit').value;
    const remarks = this.SummaryOfAuditFindings.get('Remarks').value;
let griddata={selectedField: selectedField,
   checkedThisVisit:checkedThisVisit,
   majorNCs:majorNCs,minorNCs:minorNCs,criticalNCs:criticalNCs,timeBoundNCs:timeBoundNCs,
   stage1Observations:stage1Observations,ofi:ofi,checkNextVisit:checkNextVisit,remarks:remarks
  };

  console.log('let grid data show',griddata);
  
 

debugger
    // Push values to grid data
    this.gridData.push(griddata);
    console.log('last data',this.gridData);

    this.newRecord();
    
  }
  newRecord(){
    this.SummaryOfAuditFindings.controls.Selectdata.setValue('');
    this.SummaryOfAuditFindings.controls.CheckedThisVisit.setValue('');
    this.SummaryOfAuditFindings.controls.MajorNCs.setValue('');
    this.SummaryOfAuditFindings.controls.MinorNCs.setValue('');
    this.SummaryOfAuditFindings.controls.CriticalNCs.setValue('');
    this.SummaryOfAuditFindings.controls.TimeBoundNCs.setValue('');
    this.SummaryOfAuditFindings.controls.Stage1Observations.setValue('');
    this.SummaryOfAuditFindings.controls.Ofi.setValue('');
    this.SummaryOfAuditFindings.controls.CheckNextVisit.setValue('');
    this.SummaryOfAuditFindings.controls.Remarks.setValue('');

  }

}
