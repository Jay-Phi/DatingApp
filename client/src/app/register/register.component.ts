import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validator, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter(); // From child to parent
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService
    , private toastr: ToastrService
    , private fb: FormBuilder
    , private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required,
                Validators.minLength(4)]],
      confirmPassword: ['', [Validators.required,
                        this.matchValues('password')]]
    })

    //Ci-dessous une manière de controler la validité du mdp après la situation suivante:
    //  Le mot de passe et de confirmations de mot de passe sont valides et égaux
    //  Un caractère d'un des 2 champs viennent d'être changé. Le comportement attendu est que le mot de passe soit invalide
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  //Méthode pour confirmer les mots de passes
  //ValidatorFn pour Validator Function
  matchValues(matchTo: string): ValidatorFn {
    //AbstractControl englobe les valeurs de l'objets FormControl
    return (control: AbstractControl) => {
      //controls permets d'accéder à toutes les valeurs du Formulaire,
      //  matchTo est la précision de quelle valeur du formulaire on sélectionne
      // control.value est le champ où cette méthode est appelée, donc'confirm password'
      //Si les valeurs password et confirm password sont identiques,
      //  on renvoie null contrairement aux notations
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isMatching: true}
    }
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, error => {
      this.validationErrors = error;
    });
  }

  cancel() {
    //Turn off the register mode in our home component
    this.cancelRegister.emit(false);
  }
}
