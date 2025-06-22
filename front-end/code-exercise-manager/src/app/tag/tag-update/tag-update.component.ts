import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TagService } from '../../_services/tag.service';
import { TagDetailDTO } from '../../_models/tag/detail-tag.response.dto';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BaseResponseDTO } from '../../_models/common/base.response';
import { UpdateTagRequestDTO } from '../../_models/tag/update-tag.request.dto';
import { CommonModule } from '@angular/common';
import { setInputInvalid } from '../../_utils/dom-utils';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-tag-update',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, CKEditorModule],
  templateUrl: './tag-update.component.html',
  styleUrl: './tag-update.component.css',
})
export class TagUpdateComponent {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private tagService = inject(TagService);
  public Editor: any = ClassicEditor;

  tagForm!: FormGroup;
  tagId = '';
  loading = signal(true);
  errorMessages: { [key: string]: string | null } = {
    name: null,
    displayName: null,
  };

  ngOnInit(): void {
    this.tagId = this.route.snapshot.paramMap.get('id')!;
    this.tagService.getTagById(this.tagId).subscribe({
      next: (res: BaseResponseDTO<TagDetailDTO>) => {
        const tag = <TagDetailDTO>res.data;
        this.tagForm = this.fb.group({
          name: [tag.name, [Validators.required, Validators.maxLength(20)]],
          displayName: [
            tag.displayName,
            [Validators.required, Validators.maxLength(20)],
          ],
          description: [tag.description],
        });
        this.loading.set(false);
      },
      error: () => {
        this.router.navigate(['/tag/list']);
      },
    });
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
    const value = control.value?.trim();
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

  onSubmit(): void {
    if (this.tagForm.invalid) return;
    const updatedTag: UpdateTagRequestDTO = {
      id: this.tagId,
      ...this.tagForm.value,
    };

    this.tagService.updateTag(updatedTag).subscribe({
      next: (response) => {
        if (response.success) {
          Swal.fire({
            icon: 'success',
            title: 'Thành công',
            text: response.message,
          }).then(() => {
            this.router.navigate(['/tag/detail', this.tagId]);
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

  cancel(): void {
    this.router.navigate(['/tag']);
  }
}
