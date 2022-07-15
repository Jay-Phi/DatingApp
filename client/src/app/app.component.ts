import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

//Décorateur
//ci-dessous ce sont des métadonnées angular qui définnissent comment les composants Angular doivent être organisés, interagir, instanciés et utilisées
//app-root : balise contenant le code html principal pour toutes applications angular
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

  //OnInit est un module "cyclique" executé après que Angular ait terminé de charger ses modules natives
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users: any;

  constructor(/*private http: HttpClient,*/ private accountService: AccountService) { }

  ngOnInit() {
    //this.getUsers();
    this.setCurrentUser();
  }

  //Look at in our LocalStorage and see if we've got a key or an object for the key of the User
  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  //getUsers() {
  //  //Return the variable "Observable"
  //  this.http.get('https://localhost:5001/api/users').subscribe(response => {
  //    //set our users' class property
  //    this.users = response;
  //  }, error => {
  //    console.log(error);
  //  })

  //}
}
