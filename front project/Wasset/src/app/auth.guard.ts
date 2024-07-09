import { CanActivateFn } from '@angular/router';
import { Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = new Router();
  if(localStorage.getItem('userToken')!== null)
  {
    return true;
  }
  else{
    router.navigate(['/home']);
    return false;
  }
  
};
