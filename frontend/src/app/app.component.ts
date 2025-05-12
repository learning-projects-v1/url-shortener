import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { HttpClientService } from '../http-cilent.service';
import { take } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  imports:[[FormsModule]],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent {
  title = 'url shortener';
  shortenedUrl: string = "";
  originalUrl: string = "";
  constructor(private httpService: HttpClientService, private toastr: ToastrService) {  }

  shorten(){
    this.httpService.getShortenedUrl(this.originalUrl).pipe(
      take(1)
    ).subscribe((res: any) => {
      if(res?.url){
          this.shortenedUrl = res?.url ?? "no url";
      }
      else{
        this.toastr.show(res?.message ?? "No link received")
      }
    })
  }
}
