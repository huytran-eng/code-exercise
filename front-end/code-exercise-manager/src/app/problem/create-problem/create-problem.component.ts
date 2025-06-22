import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { setInputInvalid } from '../../_utils/dom-utils';
import { ToastrService } from 'ngx-toastr';
import { ProblemService } from '../../_services/problem.service';
import Swal from 'sweetalert2';
import { ProgrammingLanguageService } from '../../_services/programming-language.service';
import { GetProgrammingLanguageResponseDTO } from '../../_models/programming-language/programming-language.response.dto';

@Component({
  selector: 'app-create-problem',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CKEditorModule,
    RouterModule,
    MatIcon,
  ],
  templateUrl: './create-problem.component.html',
  styleUrl: './create-problem.component.css',
})
export class CreateProblemComponent {
  private router = inject(Router);
  public Editor: any = ClassicEditor;
  form!: FormGroup;
  private fb = inject(FormBuilder);
  showDescriptionEditor = false;
  languages: GetProgrammingLanguageResponseDTO[] = [];
  loading = signal(false);
  problemService = inject(ProblemService);
  programmingLanguageService = inject(ProgrammingLanguageService);
  errorMessages: { [key: string]: string | null } = {
    title: null,
    description: null,
    // templateCode: null,
    timeLimit: null,
    memoryLimit: null,
    testCases: null,
  };
  toastr = inject(ToastrService);
  selectedLanguageId = signal<string>('');

  ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', [Validators.required]],
      description: [''],
      // templateCode: [''],
      timeLimit: ['', Validators.required],
      memoryLimit: ['', Validators.required],
      difficulty: [1, Validators.required],
      testCases: this.fb.array([this.newTestCase()]),
      templateCodes: this.fb.array([]),
    });

    this.loadLanguages();
  }

  loadLanguages() {
    this.programmingLanguageService.getAllProgrammingLanguage().subscribe({
      next: (res) => {
        this.languages = res.data;

        // Set default selected language
        if (this.languages.length > 0) {
          this.selectedLanguageId.set(this.languages[0].id);
        }

        // Create template codes for each language
        this.languages.forEach((lang) => {
          this.templateCodes.push(this.newTemplateCode(lang.id));
        });
      },
      error: (err) => {
        this.toastr.error('Failed to load programming languages');
        console.error(err);
      },
    });
  }

  onLanguageChange(event: Event) {
    const select = event.target as HTMLSelectElement | null;
    if (select) {
      this.selectedLanguageId.set(select.value);
    }
  }

  get templateCodes(): FormArray {
    return this.form.get('templateCodes') as FormArray;
  }

  get testCases(): FormArray {
    return this.form.get('testCases') as FormArray;
  }

  newTemplateCode(languageId: string): FormGroup {
    return this.fb.group({
      programmingLanguageId: [languageId, Validators.required],
      userTemplateCode: ['', Validators.required],
      hiddenTemplateCode: ['', Validators.required],
    });
  }

  newTestCase(): FormGroup {
    return this.fb.group({
      input: ['', Validators.required],
      expectedOutput: ['', Validators.required],
      isHidden: [false],
      inputDisplay: ['', Validators.required],
      expectedOutputDisplay: ['', Validators.required],
    });
  }

  addTestCase(): void {
    this.testCases.push(this.newTestCase());
  }

  removeTestCase(index: number): void {
    this.testCases.removeAt(index);
  }

  submit(): void {
    if (!this.validationBeforeInsert()) {
      return;
    }
    if (this.form.valid) {
      const payload = {
        ...this.form.value,
        difficulty: Number(this.form.value.difficulty),
      };
      this.problemService.createProblem(payload).subscribe({
        next: (response) => {
          console.log(response);
          if (response.success) {
            Swal.fire({
              icon: 'success',
              title: 'Thành công',
              text: response.message,
            }).then(() => {
              this.router.navigate(['/problem/list']);
            });
          }
        },
      });
    }
  }

  toggleDescriptionEditor() {
    this.showDescriptionEditor = !this.showDescriptionEditor;
    // if (this.showDescriptionEditor) {
    //   this.showTemplateCodeEditor = false;
    // }
  }

  // toggleTemplateCodeEditor() {
  //   this.showTemplateCodeEditor = !this.showTemplateCodeEditor;
  //   if (this.showTemplateCodeEditor) {
  //     this.showDescriptionEditor = false;
  //   }
  // }

  private validationBeforeInsert(): boolean {
    const descriptionControl = this.form.get('description');
    // const templateCodeControl = this.form.get('templateCode');

    if (descriptionControl && descriptionControl.value.trim() === '') {
      this.toastr.error('Nội dung bài tập không được để trống', 'Error');
      return false;
    }

    // if (templateCodeControl && templateCodeControl.value.trim() === '') {
    //   this.toastr.error('Code mẫu không được để trống', 'Error');
    //   return false;
    // }

    const testCasesArray = this.testCases;
    if (testCasesArray.length === 0) {
      this.toastr.error('Bài tập phải có ít nhất một test case', 'Error');
      return false;
    }

    for (let i = 0; i < testCasesArray.length; i++) {
      const testCase = testCasesArray.at(i) as FormGroup;
      const input = testCase.get('input')?.value.trim();
      const expectedOutput = testCase.get('expectedOutput')?.value.trim();
      const inputDisplay = testCase.get('inputDisplay')?.value.trim();
      const expectedOutputDisplay = testCase
        .get('expectedOutputDisplay')
        ?.value.trim();

      if (
        !input ||
        !expectedOutput ||
        !inputDisplay ||
        !expectedOutputDisplay
      ) {
        this.toastr.error(
          `Test case ${i + 1} không hợp lệ. Vui lòng điền đầy đủ các trường.`,
          'Lỗi'
        );
        return false;
      }
    }

    return true;
  }

  validateTitle() {
    const control = this.form.get('title');
    if (!control) return;
    const value = control.value?.trim();
    if (value.length > 100) {
      this.errorMessages['title'] =
        'Tiêu đề bài tập không được vượt quá 100 ký tự';
      setInputInvalid('title');
    } else if (value.length === 0) {
      this.errorMessages['title'] = 'Tiêu đề bài tập không được để trống';
      setInputInvalid('title');
    } else if (value.length < 3) {
      this.errorMessages['title'] = 'Tiêu đề bài tập phải lớn hơn 3 ký tự';
      setInputInvalid('title');
    } else {
      this.errorMessages['title'] = null;
    }
  }

  validateTimeLimit() {
    const control = this.form.get('timeLimit');
    if (!control) return;
    const value = control.value;
    console.log(value);
    if (value <= 0) {
      this.errorMessages['timeLimit'] = 'Giới hạn thời gian phải lớn hơn 0';
      setInputInvalid('timeLimit');
    } else {
      this.errorMessages['timeLimit'] = null;
    }
  }

  validateMemoryLimit() {
    const control = this.form.get('memoryLimit');
    if (!control) return;
    const value = control.value;
    if (value <= 0) {
      this.errorMessages['memoryLimit'] = 'Giới hạn bộ nhớ phải lớn hơn 0';
      setInputInvalid('memoryLimit');
    } else {
      this.errorMessages['memoryLimit'] = null;
    }
  }

  hasErrors(): boolean {
    return Object.values(this.errorMessages).some((error) => error !== null);
  }
}
