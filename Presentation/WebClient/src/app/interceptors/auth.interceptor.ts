import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Get the token from localStorage
  const token = localStorage.getItem('auth_acces_token');
  
  // Only add the Authorization header if a token exists
  if (token) {
    // Clone the request and add the Authorization header
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    
    // Pass the cloned request to the next handler
    return next(authReq);
  }
  
  // If no token exists, proceed with the original request
  return next(req);
};