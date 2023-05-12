import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/Services/authentication.service';
import RegisterDto from 'src/app/types/RegisterDto';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  form = new FormGroup({
    username: new FormControl<string>('user'),
    password: new FormControl<string>('user123_'),
    email: new FormControl<string>('user@gmail.com'),
  });

  constructor(
    private readonly authService: AuthenticationService,
    private readonly router: Router
  ) {}

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.form.invalid) return;

    var userData = new RegisterDto();
    userData.email = this.form.value.email ?? '';
    userData.username = this.form.value.username ?? '';
    userData.password = this.form.value.password ?? '';

    this.authService.register(userData).subscribe({
      next: () => {
        this.router.navigate(['/authentication/login']);
      },
      error: () => {
        alert('Something went wrong');
      },
    });
  }
}
