import { Component, inject, OnInit } from '@angular/core';
import { ProblemService } from '../../_services/problem.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { clearAllInvalidInputs, setInputInvalid } from '../../_utils/dom-utils';
import { GetProblemsRequestDTO } from '../../_models/problem/get-problems.request.dto';
import { DatePipe } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-problem-list',
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
  templateUrl: './problem-list.component.html',
  styleUrl: './problem-list.component.css',
})
export class ProblemListComponent implements OnInit {
  problemService = inject(ProblemService);
  private router = inject(Router);
  toastr = inject(ToastrService);
  pageNumber = 1;
  pageSize = 10;
  private fb = inject(FormBuilder);

  problemForm: FormGroup = this.fb.group({
    title: [''],
    fromDate: [''],
    toDate: [''],
    orderBy: [null],
  });

  
  ngOnInit(): void {
    this.loadProblems();
  }

  loadProblems() {
    clearAllInvalidInputs();
    this.problemService.paginatedResult.set(null);
    const formValue = this.problemForm.value;
    if (!this.validationBeforeSearch()) return;

    const filter: GetProblemsRequestDTO = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      title: formValue.title?.trim() || undefined,
      fromDate: formValue.fromDate ? new Date(formValue.fromDate) : undefined,
      toDate: formValue.toDate ? new Date(formValue.toDate) : undefined,
      orderBy: formValue.orderBy || null,
    };
    this.problemService.getProblems(filter).subscribe({
      next: (response) => {
        this.problemService.paginatedResult.set(response);
      },
      error: (error) => {
        console.error('Error loading tags:', error);
      },
    });
  }
  private validationBeforeSearch(): boolean {
    const nameControl = this.problemForm.get('name');
    const displayNameControl = this.problemForm.get('displayName');
    const fromDateControl = this.problemForm.get('fromDate');
    const toDateControl = this.problemForm.get('toDate');

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

  onPageSizeChange() {
    this.pageNumber = 1;
    this.loadProblems();
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadProblems();
    }
  }

  onSearch() {
    this.pageNumber = 1;

    this.problemService.paginatedResult.set(null);
    if (!this.validationBeforeSearch()) {
      return;
    }

    this.pageNumber = 1;
    this.loadProblems();
  }
  onClear(): void {
    clearAllInvalidInputs();
    this.problemForm.reset({
      name: '',
      displayName: '',
      fromDate: null,
      toDate: null,
      orderBy: null,
    });
  }

  onRowClick(problemId: string) {
    this.router.navigate(['/problem/detail', problemId]);
  }
}
