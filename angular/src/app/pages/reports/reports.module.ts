import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AppRoutingModule } from '@app/app-routing.module';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AbpModalHeaderComponent } from '@shared/components/modal/abp-modal-header.component';
import { DxBulletModule, DxDataGridModule, DxSpeedDialActionModule, DxTemplateModule } from 'devextreme-angular';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuditorReportsComponent } from './auditor-reports/auditor-reports.component';
import { InvoiceComponent } from './invoice/invoice.component';
import { AuditorAuditDetailsComponent } from './auditor-audit-details/auditor-audit-details.component';



@NgModule({
    declarations: [
    
    AuditorReportsComponent,
          InvoiceComponent,
          AuditorAuditDetailsComponent
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
    ]
  })

  export class ReportsModule { }