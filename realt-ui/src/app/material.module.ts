import { NgModule } from '@angular/core';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  imports: [
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule
  ],
  exports: [
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule
  ],
  providers: [ MatDatepickerModule ]
})

export class MaterialModule {}
