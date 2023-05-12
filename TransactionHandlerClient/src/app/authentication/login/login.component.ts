import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/Services/authentication.service';
import LoginDto from 'src/app/types/LoginDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  form = new FormGroup({
    username: new FormControl<string>('user'),
    password: new FormControl<string>('user123_'),
  });

  constructor(
    private readonly authService: AuthenticationService,
    private readonly router: Router
  ) {}

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.form.invalid) return;

    var credentials = new LoginDto();
    credentials.username = this.form.value.username ?? '';
    credentials.password = this.form.value.password ?? '';

    this.authService.login(credentials).subscribe({
      next: (response) => {
        this.router.navigate(['/']);
      },
      error: () => {
        alert('Something went wrong');
      },
    });
  }
}
