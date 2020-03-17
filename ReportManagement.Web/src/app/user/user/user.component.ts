import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  constructor(private translate: TranslateService) {
    let userLanguage;
    userLanguage = navigator.language;
    console.log(userLanguage);

    if(userLanguage === "en-US") {
      userLanguage = "en"
    }
    if(userLanguage === "ja") {
      userLanguage = "jp"
    }
    console.log(userLanguage);
    translate.use(userLanguage);
   }

  ngOnInit() {
  }

}
