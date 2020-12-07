import { Injectable } from '@angular/core';
import { ActiveToast, ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  constructor(private toastr: ToastrService) {
  }
  pop(type: string, message: string, title?: string): ActiveToast<any> {
    switch (type) {
      case 'error':
        return this.error(message, title);
      case 'success':
        return this.success(message, title);
      case 'warning':
        return this.warning(message, title);
      case 'info':
        return this.info(message, title);
      case 'danger':
        return this.danger(message, title);
      default:
        return this.primary(message, title);
    }
  }

  show(message?: string, title?: string, type?: string): ActiveToast<any> {
    return this.toastr.show(message, title, {}, type);
  }

  success(message?: string, title?: string): ActiveToast<any> {
    return this.toastr.success(message, title);
  }

  error(message?: string, title?: string): ActiveToast<any> {
    return this.toastr.error(message, title);
  }

  info(message?: string, title?: string): ActiveToast<any> {
    return this.toastr.info(message, title);
  }

  warning(message?: string, title?: string): ActiveToast<any> {
    return this.toastr.warning(message, title);
  }

  danger(message?: string, title?: string): ActiveToast<any> {
    return this.toastr.error(message, title);
  }

  primary(message?: string, title?: string): ActiveToast<any> {
    return this.show(message, title, 'primary');
  }

  default(message?: string, title?: string): ActiveToast<any> {
    return this.show(message, title, 'default');
  }

}
