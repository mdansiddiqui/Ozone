import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-good-practices',
  templateUrl: './good-practices.component.html',
  styleUrls: ['./good-practices.component.css']
})
export class GoodPracticesComponent implements OnInit {

  CertificationList:[]=[];

  GoodPractices = new FormGroup({
    DescriptionofGoodPractices : new FormControl(''),
    ObjectiveEvidence : new FormControl(''),
    UploadEvidence : new FormControl('')
  });

  constructor() { }

  ngOnInit(): void {
  }

}
