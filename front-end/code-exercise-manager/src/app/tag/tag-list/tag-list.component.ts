import { Component, inject, OnInit } from '@angular/core';
import { TagService } from '../../_services/tag.service';
import { GetTagsRequestDTO } from '../../_models/tag/get-tag.request.dto';
import { DatePipe } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { clearAllInvalidInputs, setInputInvalid } from '../../_utils/dom-utils';
import { ToastrService } from 'ngx-toastr';
import { MatIcon } from '@angular/material/icon';
@Component({
  selector: 'app-tag-list',
  standalone: true,
  imports: [
    DatePipe,
    PaginationModule,
    FormsModule,
    BsDatepickerModule,
    ReactiveFormsModule,
    MatIcon,
    RouterModule,
  ],
  templateUrl: './tag-list.component.html',
  styleUrl: './tag-list.component.css',
})
export class TagListComponent implements OnInit {
  tagService = inject(TagService);
  private router = inject(Router);
  toastr = inject(ToastrService);
  pageNumber = 1;
  pageSize = 10;
  private fb = inject(FormBuilder);

  tagForm: FormGroup = this.fb.group({
    name: [''],
    displayName: [''],
    fromDate: [''],
    toDate: [''],
    orderBy: [null],
  });

  ngOnInit(): void {
    this.loadTags();
  }

  loadTags() {
    clearAllInvalidInputs();
    this.tagService.paginatedResult.set(null);
    const formValue = this.tagForm.value;
    if (!this.validationBeforeSearch()) return;

    const filter: GetTagsRequestDTO = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      name: formValue.name?.trim() || undefined,
      displayName: formValue.displayName?.trim() || undefined,
      fromDate: formValue.fromDate ? new Date(formValue.fromDate) : undefined,
      toDate: formValue.toDate ? new Date(formValue.toDate) : undefined,
      orderBy: formValue.orderBy || null,
    };
    this.tagService.getTags(filter).subscribe({
      next: (response) => {
        this.tagService.paginatedResult.set(response);
      },
      error: (error) => {
        console.error('Error loading tags:', error);
      },
    });
  }

  onPageSizeChange() {
    this.pageNumber = 1; // Reset to the first page when page size changes
    this.loadTags();
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadTags();
    }
  }

  onSearch() {
    this.pageNumber = 1;

    this.tagService.paginatedResult.set(null);
    if (!this.validationBeforeSearch()) {
      return;
    }

    this.pageNumber = 1;
    this.loadTags();
  }

  onClear(): void {
    clearAllInvalidInputs();
    this.tagForm.reset({
      name: '',
      displayName: '',
      fromDate: null,
      toDate: null,
      orderBy: null,
    });
  }

  onRowClick(tagId: string) {
    this.router.navigate(['/tag/detail', tagId]);
  }

  private validationBeforeSearch(): boolean {
    const nameControl = this.tagForm.get('name');
    const displayNameControl = this.tagForm.get('displayName');
    const fromDateControl = this.tagForm.get('fromDate');
    const toDateControl = this.tagForm.get('toDate');

    if (nameControl?.value) {
      nameControl.setValue(nameControl.value.trim());
    }

    if (displayNameControl?.value) {
      displayNameControl.setValue(displayNameControl.value.trim());
    }

    const fromDate = fromDateControl?.value
      ? new Date(fromDateControl.value)
      : null;
    const toDate = toDateControl?.value ? new Date(toDateControl.value) : null;

    if (fromDate && toDate && fromDate > toDate) {
      setInputInvalid('fromDate');
      setInputInvalid('toDate');
      this.toastr.error(
        'Ngày bắt đầu tìm kiếm không thể lớn hơn ngày kết thúc',
        'Error'
      );
      return false;
    }

    return true;
  }

  onViewDetail(tagId: string) {
    this.router.navigate(['/tag/detail', tagId]);
  }

  onUpdateTag(tagId: string) {
    this.router.navigate(['/tag/update', tagId]);
  }
}
