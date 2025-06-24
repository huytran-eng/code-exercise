import {
  Component,
  computed,
  effect,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProblemService } from '../../_services/problem.service';
import {
  ProblemDetailDTO,
  TemplateCodeDTO,
} from '../../_models/problem/problem-detail.response.dto';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
@Component({
  selector: 'app-problem-detail',
  standalone: true,
  imports: [MonacoEditorModule, ReactiveFormsModule],
  templateUrl: './problem-detail.component.html',
  styleUrl: './problem-detail.component.css',
})
export class ProblemDetailComponent implements OnInit {
  @ViewChild('editor') editor: any;
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  problemService = inject(ProblemService);
  problem: ProblemDetailDTO | null = null;
  selectedCaseIndex = 0;

  get selectedCase() {
    return this.problem?.testCases?.[this.selectedCaseIndex];
  }

  selectCase(index: number) {
    this.selectedCaseIndex = index;
  }

  objectKeys(obj: any): string[] {
    return Object.keys(obj);
  }
  languages: {
    programmingLanguageId: string;
    displayName: string;
    name: string;
  }[] = [];

  selectedLanguageId = signal<string>('');
  selectedLanguageName = signal<string>('');

  editorOptions = {  language: 'go' };

  editorCode = computed(() => {
    const langId = this.selectedLanguageId();
    return (
      this.problem?.templateCodes.find(
        (tc) => tc.programmingLanguageId === langId
      )?.userTemplateCode || ''
    );
  });

  codeControl = new FormControl('');

  loading = signal(true);

  ngOnInit(): void {
    const slug = this.route.snapshot.paramMap.get('slug');
    if (slug) {
      this.loadProblemDetails(slug);
    }
  }

  loadProblemDetails(slug: string): void {
    this.problemService.getProblemBySlug(slug).subscribe({
      next: (response) => {
        this.problem = response.data;
        console.log('response data:');
        console.log(response.data);

        this.languages = this.problem.templateCodes.map((tc) => ({
          programmingLanguageId: tc.programmingLanguageId,
          displayName: tc.programmingLanguageDisplayName,
          name: tc.programmingLanguageName,
        }));
        console.log(this.languages);
        // Optionally set default selected language
        if (this.languages.length > 0) {
          this.selectedLanguageId.set(this.languages[0].programmingLanguageId);
          this.selectedLanguageName.set(this.languages[0].name);
          console.log(this.selectedLanguageName());
          console.log(this.problem.templateCodes[0].userTemplateCode);
          this.codeControl.setValue(
            this.problem.templateCodes[0].userTemplateCode
          );
        }
        this.loading.set(false);
      },
    });
  }

  onLanguageChange(event: Event) {
    const select = event.target as HTMLSelectElement | null;
    if (!select) return;

    const selectedId = select.value;
    this.selectedLanguageId.set(selectedId);

    const selectedLang = this.languages.find(
      (lang) => lang.programmingLanguageId === selectedId
    );
    console.log('selected lang');
    console.log(selectedLang);
  }

  ngAfterViewInit(): void {
    const resizer = document.querySelector('.resizer') as HTMLElement;
    const leftPane = document.querySelector('.left-pane') as HTMLElement;

    const hResizer = document.querySelector(
      '.resizer-horizontal'
    ) as HTMLElement;
    const codeArea = document.querySelector('.code-area') as HTMLElement;
    const testcaseArea = document.querySelector(
      '.testcase-area'
    ) as HTMLElement;

    let isResizing = false;

    // Horizontal resizer (between left and right)
    resizer.addEventListener('mousedown', () => {
      isResizing = true;
      document.body.style.cursor = 'col-resize';
    });

    document.addEventListener('mousemove', (e) => {
      if (!isResizing) return;
      const newLeftWidth = e.clientX;
      leftPane.style.width = `${newLeftWidth}px`;
      this.editor?._editor?.layout(); // update Monaco layout
    });

    document.addEventListener('mouseup', () => {
      isResizing = false;
      document.body.style.cursor = 'default';
    });

    // Vertical resizer (between code and testcases)
    let isHResizing = false;

    hResizer.addEventListener('mousedown', () => {
      isHResizing = true;
      document.body.style.cursor = 'row-resize';
    });

    document.addEventListener('mousemove', (e) => {
      if (!isHResizing) return;

      const containerTop = codeArea.parentElement!.getBoundingClientRect().top;
      const newHeight = e.clientY - containerTop;

      codeArea.style.height = `${newHeight}px`;
      this.editor?._editor?.layout(); // update Monaco layout
    });

    document.addEventListener('mouseup', () => {
      isHResizing = false;
      document.body.style.cursor = 'default';
    });
  }
}
