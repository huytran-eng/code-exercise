import { Component, inject, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { TagService } from '../../_services/tag.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { setInputInvalid } from '../../_utils/dom-utils';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import Swal from 'sweetalert2';
import { Router, RouterModule } from '@angular/router';
@Component({
  selector: 'app-create-tag',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, CKEditorModule, RouterModule],
  templateUrl: './create-tag.component.html',
  styleUrl: './create-tag.component.css',
})
export class CreateTagComponent {
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private tagService = inject(TagService);
  private toastr = inject(ToastrService);
  public Editor: any = ClassicEditor;

  errorMessages: { [key: string]: string | null } = {
    name: null,
    displayName: null,
  };

  tagForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(20)]],
    displayName: ['', [Validators.required, Validators.maxLength(20)]],
    description: [''],
  });

  onSubmit() {
    if (this.tagForm.valid) {
      this.tagService.createTag(this.tagForm.value).subscribe({
        next: (response) => {
          if (response.success) {
            Swal.fire({
              icon: 'success',
              title: 'Thành công',
              text: response.message,
            }).then(() => {
              this.router.navigate(['/tag/list']);
            });
          }
        },
        error: (err) => {
          if ((err.error.messageCode = 'TAG_NAME_EXIST')) {
            const nameControl = this.tagForm.controls['name'];
            nameControl.setErrors({ tagExists: true });
            nameControl.markAsTouched();
            setInputInvalid('name');
          }
        },
      });
    }
  }

  validateName() {
    const control = this.tagForm.get('name');
    if (!control) return;

    const value = control.value?.trim();
    if (value.length > 20) {
      this.errorMessages['name'] = 'Tên không được vượt quá 20 ký tự';
      setInputInvalid('name');
    } else if (value.length === 0) {
      this.errorMessages['name'] = 'Tên không được để trống';
      setInputInvalid('name');
    } else if (value.length < 3) {
      this.errorMessages['name'] = 'Tên phải lớn hơn 3 ký tự';
      setInputInvalid('name');
    } else {
      this.errorMessages['name'] = null;
    }
  }

  validateDisplayName() {
    const control = this.tagForm.get('displayName');
    if (!control) return;
    const value = control.value;
    if (value.length > 20) {
      this.errorMessages['displayName'] =
        'Tên hiển thị không được vượt quá 20 ký tự';
      setInputInvalid('displayName');
    } else if (value.length === 0) {
      this.errorMessages['displayName'] = 'Tên hiển thị không được để trống';
      setInputInvalid('displayName');
    } else if (value.length < 3) {
      this.errorMessages['displayName'] = 'Tên hiển thị phải lớn hơn 3 ký tự';
      setInputInvalid('displayName');
    } else {
      this.errorMessages['displayName'] = null;
    }
  }

  get form() {
    return this.tagForm.controls;
  }

  hasErrors(): boolean {
    return Object.values(this.errorMessages).some((error) => error !== null);
  }
}
