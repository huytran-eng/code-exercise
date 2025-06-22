import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);
  return next(req).pipe(
    catchError((err) => {
      const apiMessage =
        err.error?.message || 'Đã xảy ra lỗi. Vui lòng thử lại.';
      if (err.status === 400) {
        toastr.error(apiMessage, 'Error');
      }
      else if (err.status === 500) {
        toastr.error(apiMessage, 'Error');
      }else if (err.status === 401) {
        // Clear cache or local storage
        localStorage.clear();
        sessionStorage.clear();
        // Show a message and redirect to the login screen
        toastr.warning('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.', 'Unauthorized');
        router.navigate(['/login']); // Replace '/login' with your actual login route
      } else {
        toastr.error(apiMessage);
      }
      throw err;
    })
  );
};
