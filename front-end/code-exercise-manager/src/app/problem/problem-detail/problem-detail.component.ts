import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProblemService } from '../../_services/problem.service';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ProblemDTO } from '../../_models/problem/problem-detal.response.dto';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-problem-detail',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './problem-detail.component.html',
  styleUrl: './problem-detail.component.css',
})
export class ProblemDetailComponent {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  problemService = inject(ProblemService);
  problem: ProblemDTO | null = null;
  public Editor: any = ClassicEditor;
  languages = [
    {
      id: '41f87e81-f906-4a38-a2e7-3101391685d3',
      name: 'go',
      label: 'Go',
    },
    {
      id: '2972aa31-38ec-4c14-b76c-f5593ce5125f',
      name: 'csharp',
      label: 'C#',
    },
  ];
  selectedLanguageId = signal<string>('41f87e81-f906-4a38-a2e7-3101391685d3');
  loading = signal(true);

  ngOnInit(): void {
    const problemId = this.route.snapshot.paramMap.get('id');
    if (problemId) {
      this.loadProblemDetails(problemId);
    }
  }

  onLanguageChange(event: Event) {
    const select = event.target as HTMLSelectElement | null;
    if (select) {
      this.selectedLanguageId.set(select.value);
    }
  }

  loadProblemDetails(problemId: string): void {
    this.problemService.getProblemById(problemId).subscribe({
      next: (response) => {
        this.problem = response.data;
        this.loading.set(false);
      },
    });
  }

  onDeleteProblem(problemId: string) {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn xóa bài tập này?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Xóa',
      cancelButtonText: 'Hủy',
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
    }).then((result) => {
      if (result.isConfirmed) {
        this.problemService.deleteProblem(problemId).subscribe({
          next: () => {
            Swal.fire(
              'Đã xóa!',
              'Bài tập đã được xóa thành công.',
              'success'
            ).then(() => {
              this.router.navigate(['/problem/list']);
            });
          },
          error: () => {
            Swal.fire(
              'Lỗi',
              'Xóa bài tập thất bại. Vui lòng thử lại.',
              'error'
            );
          },
        });
      }
    });
  }
}
