import { Component, inject } from '@angular/core';
import { ProblemService } from '../../_services/problem.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MatIcon } from '@angular/material/icon';
import { GetProblemsRequestDTO } from '../../_models/problem/get-problems.request.dto';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-problem-list',
  standalone: true,
  imports: [
    PaginationModule,
    FormsModule,
    ReactiveFormsModule,
    MatIcon,
    RouterModule,
    MatTableModule,
    MatInputModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    CommonModule,
  ],
  templateUrl: './problem-list.component.html',
  styleUrl: './problem-list.component.css',
})
export class ProblemListComponent {
  problemService = inject(ProblemService);
  private router = inject(Router);
  toastr = inject(ToastrService);
  pageNumber = 1;
  pageSize = 10;
  private fb = inject(FormBuilder);

  problemForm: FormGroup = this.fb.group({
    title: [''],
    orderBy: [null],
  });

  ngOnInit(): void {
    this.loadProblems();
  }

  loadProblems() {
    this.problemService.paginatedResult.set(null);
    const formValue = this.problemForm.value;

    const filter: GetProblemsRequestDTO = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      title: formValue.title?.trim() || undefined,
      orderBy: formValue.orderBy || null,
    };
    this.problemService.getProblems(filter).subscribe({
      next: (response) => {
        console.log(response)
        this.problemService.paginatedResult.set(response);
      },
      error: (error) => {
        console.error('Error loading problems:', error);
      },
    });
  }

  onSearch() {
    this.pageNumber = 1;

    this.problemService.paginatedResult.set(null);
    this.pageNumber = 1;
    this.loadProblems();
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

  onRowClick(problemId: string) {
    this.router.navigate(['/problem', problemId]);
  }

  statusText(status: number): string {
    return status === 1 ? 'Solved' : status === 2 ? 'Attempted' : 'Unsolved';
  }

  statusColor(status: number): 'primary' | 'accent' | 'warn' | undefined {
    return status === 1 ? 'primary' : status === 2 ? 'warn' : undefined;
  }

  difficultyColor(difficulty: string): string {
    switch (difficulty) {
      case 'Easy':
        return 'easy-chip';
      case 'Medium':
        return 'medium-chip';
      case 'Hard':
        return 'hard-chip';
      default:
        return '';
    }
  }
}
