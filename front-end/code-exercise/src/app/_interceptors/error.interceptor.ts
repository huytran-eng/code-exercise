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
        toastr.warning('Bạn cần đăng nhập để sử dụng tính năng này', 'Chưa đăng nhập');
      } else {
        toastr.error(apiMessage);
      }
      throw err;
    })
  );
};
