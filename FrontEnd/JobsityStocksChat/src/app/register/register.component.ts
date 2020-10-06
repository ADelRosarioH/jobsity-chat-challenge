import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    passwordRepeat: new FormControl('', [Validators.required]),
  });

  errors: any[];

  doesPasswordsMatch: boolean = false;

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {
    this.errors = [];
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['']);
    }
  }

  doRegister() {
    const { userName, email, password, passwordRepeat } = this.registerForm.value;

    if (password != passwordRepeat) {
      this.toastr.error("Please confirm that password match.", "Oops!");
      this.doesPasswordsMatch = false;
      return;
    } else {
      this.doesPasswordsMatch = true;
    }

    this.authService.register({ userName, email, password }).subscribe(data => {
      this.router.navigate([""])
    }, (err) => {
      this.toastr.error(err.error.message, "Oops!");
      for (let i = 0; i < err.error.errors.length; i++) {
        this.errors.push(err.error.errors[i].description);
      }
    });
  }

}
