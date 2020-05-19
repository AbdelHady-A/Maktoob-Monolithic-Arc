import { Component, OnInit } from '@angular/core';
import { IAuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

  constructor(private authService: IAuthService) { }

  ngOnInit(): void {
  }

  public SignOutAsync() {
    this.authService.SignOutAsync();
  }

}
