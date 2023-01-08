import { Component, TemplateRef } from '@angular/core';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-toaster',
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.scss'],
})
export class ToasterComponent {
  constructor(public toasterService: ToasterService) {}

  isTemplate(toast: any) {
    return toast.textOrTemplate instanceof TemplateRef;
  }
}
