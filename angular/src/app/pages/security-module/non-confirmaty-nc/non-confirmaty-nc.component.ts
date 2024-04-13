import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-non-confirmaty-nc',
  templateUrl: './non-confirmaty-nc.component.html',
  styleUrls: ['./non-confirmaty-nc.component.css']
})
export class NonConfirmatyNCComponent implements OnInit {


  NonConfirmatyNC = new FormGroup({

  })

  CertificationList:[]=[];
  constructor() { }

  ngOnInit(): void {
  }

}
