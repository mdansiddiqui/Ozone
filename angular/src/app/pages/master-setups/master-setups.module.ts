import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AppRoutingModule } from '@app/app-routing.module';
import { RouterModule } from '@angular/router';


import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AbpModalHeaderComponent } from '@shared/components/modal/abp-modal-header.component';
import { DxBulletModule, DxDataGridModule, DxSpeedDialActionModule, DxTemplateModule } from 'devextreme-angular';


import { NgxPaginationModule } from 'ngx-pagination';
import { DocumentTypeComponent } from './document-type/document-type.component';
import { CourseTypeComponent } from './course-type/course-type.component';
import {NaceCodeComponent } from './nace-code/nace-code.component';
import { EaCodeComponent } from './ea-code/ea-code.component';
import { LegislationComponent } from './legislation/legislation.component';
import { AudditorTypeComponent } from './audditor-type/audditor-type.component';
import { ProjectAmountComponent } from './project-amount/project-amount.component';
import { VisitLevelComponent } from './visit-level/visit-level.component';
import { MappingStandardWithDocumentComponent } from './mapping-standard-with-document/mapping-standard-with-document.component';
//import { DashboardComponent } from './dashboard/dashboard.component';
import { AuditDocumentComponent } from './audit-document/audit-document.component';




@NgModule({
  declarations: [
    DocumentTypeComponent,
    VisitLevelComponent,
    MappingStandardWithDocumentComponent,
    AuditDocumentComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ModalModule.forChild(),
    RouterModule,
    AppRoutingModule,
    ReactiveFormsModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
    DxSpeedDialActionModule,
    NgxPaginationModule,
    //ProjectAmountComponent,
    //DashboardComponent
  ]
})
export class MasterSetupsModule { }
