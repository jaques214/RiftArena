import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from '@models/user';
import { RouterLink } from '@angular/router';
import { SharedFormGroupComponent } from '../shared-form-group/shared-form-group.component';
import {NgClass, NgOptimizedImage} from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: true,
  imports: [
    MatCardModule,
    NgClass,
    SharedFormGroupComponent,
    RouterLink,
    NgOptimizedImage,
  ],
})
export class LoginComponent implements OnInit {
  formFields: any = User.loginFields();
  title: string = 'Insert your account data';
  authForm: FormGroup = new FormGroup({
    nickname: new FormControl(''),
    email: new FormControl(''),
    password: new FormControl(''),
    new_password: new FormControl(''),
  });

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.authForm = this.formBuilder.group({
      nickname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }
}
