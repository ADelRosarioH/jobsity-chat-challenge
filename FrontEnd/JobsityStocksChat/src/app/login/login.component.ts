import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['']);
    }
  }

  doLogin() {
    const { userName, password } = this.loginForm.value;
    this.authService.logIn(userName, password).subscribe((data: LoginResponse) => {
      localStorage.setItem("token", data.token);
      this.router.navigate([""]);
    }, (err) => {
      this.toastr.error(err.error.message, "Oops!");
    });
  }

}

class LoginResponse {
  token: string
}