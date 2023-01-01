import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SecurityApiService } from '../../services/api/security-api.service';
import { ToasterService } from '../../services/toaster/toaster.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  userName = '';
  password = '';
  returnUrl: string | null = null;
  showErrorMessage = false;

  constructor(
    private readonly securityService: SecurityApiService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.returnUrl = params['returnUrl'];
    });
  }

  login(): void {
    this.securityService.login(
      this.userName,
      this.password,
      (result) => {
        if (this.returnUrl !== null) {
          this.showErrorMessage = false;
          this.router.navigateByUrl(this.returnUrl);
        }
      },
      () => {
        this.showErrorMessage = true;
      }
    );
  }
}
