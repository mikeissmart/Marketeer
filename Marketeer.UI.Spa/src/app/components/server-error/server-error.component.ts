import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ApiHttpService } from '../../services/http/api-http/api-http.service';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss'],
})
export class ServerErrorComponent implements OnInit {
  error: SafeHtml | null = null;

  constructor(
    private readonly apiHttpService: ApiHttpService,
    private readonly sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    if (
      this.apiHttpService.serverErrorMessage != null &&
      this.apiHttpService.serverErrorMessage.length > 0
    ) {
      this.error = this.sanitizer.bypassSecurityTrustHtml(
        this.apiHttpService.serverErrorMessage
      );
    }
  }
}
