import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { AppAuthService } from '../../../services/AppAuthService';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  imports: [RouterLink],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent implements OnInit {
  authService = inject(AppAuthService);

  isAuthenticated = computed<boolean>(() => this.authService.isAuthenticated());

  ngOnInit(): void {
  }

  logout(event: Event) {
    event.preventDefault();
    this.authService.logout();
  }
}
