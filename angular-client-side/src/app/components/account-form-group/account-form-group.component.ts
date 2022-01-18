import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LinkedAccount } from '@models/linked_acount';
import { UserRestService } from '@services/user-rest/user-rest.service';

@Component({
  selector: 'app-account-form-group',
  templateUrl: './account-form-group.component.html',
  styleUrls: ['./account-form-group.component.css']
})
export class AccountFormGroupComponent implements OnInit {
  @Input() value!:string;
  @Input() formFields!:any;
  @Input() accountFlag!:any;
  username = '';
  rank = '';
  region = '';
  listValues: string[] = [];
  form!: FormGroup;
  message!: string;
  
  constructor(public router: Router, private restService : UserRestService) {
  }
  
  ngOnInit(): void {
    this.form = new FormGroup({
      username: new FormControl('', [Validators.required]),
      region: new FormControl(null, [Validators.required]),
    });
  }

  onSubmitAccount(): void {
    const data = new LinkedAccount();
    this.formFields.forEach((input:any) => {
      if(input.type != 'select') {
        // (data as any)[input.name!] = input.model;
        (data as any)[input.name!] = this.form.get(input.name)?.value
      }
      else {
        this.region = this.form.get(input.value)?.value;
      }
    });
    this.accountFlag = "view";
    if(data == undefined) {
      this.addAccount(data);
    }
    else {
      this.editAccount(data);
    }
  }

  addAccount(account: LinkedAccount): void {
    this.username = account.username!;

    this.restService.addAccount(this.username, this.region).subscribe({
      next: () => window.location.reload(),
      error: (err) => console.log(err)
    });
  }

  editAccount(account: LinkedAccount): void {
    this.username = account.username!;

    this.restService.updateRiotAccount(this.username, this.region).subscribe({
      next: () => window.location.reload(),
      error: (err) => console.log(err)
    });
  }

  getErrorMessage(name: string) {
    if (this.form.get(name)?.hasError('required')) {
      return 'You must enter a value';
    }
    
    switch (name) {
      case 'username':
        this.message = "Not a valid username";
      break;
      case 'region':
        this.message = "Region doesn't exist";
      break;
    }

    return this.form.get(name)?.hasError(name) ? this.message : '';
  }
}