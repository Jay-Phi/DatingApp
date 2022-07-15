import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

  //make request to our API
  //This is a singleton service :
  //   It is destroyed when the window is closed
export class AccountService {
  //Ci-dessous, on utilise "="et non ":" car on assigne directement une valeur à la variable.
  //":" permets d'itianiliser et de préciser le type
  baseUrl = 'https://localhost:5001/api/';
  //Construction d'un observable "Buffer" ci-dessous
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable(); //Le dollar signifie que c'est un observable

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      //Ci-dessous obtention de la réponse du serveur
      map((response: User) => {

        //Obtention du user dans la réponse
        const user = response;
        if (user) {
          //"localStorage" se situe dans le moteur de recherche (Chrome, Edge, etc...)
          //setItem, récupère un élément JSON puis le met à jour
          localStorage.setItem('user', JSON.stringify(user));
          //On défini la nouvelle valeur du buffer
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model)
      .pipe(
        map((user: User) => {
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
        })
      )
  }

  //méthode helper
  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }
  
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
