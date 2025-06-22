import { Component, inject, OnInit, signal } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProblemService } from '../../_services/problem.service';
import {
  ProblemDTO,
  TemplateCodeDTO,
  TestCaseDTO,
} from '../../_models/problem/problem-detal.response.dto';
import { ActionFlagEnum } from '../../_utils/action-flag.enum';
import { setInputInvalid } from '../../_utils/dom-utils';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { ProgrammingLanguageService } from '../../_services/programming-language.service';
import { GetProgrammingLanguageResponseDTO } from '../../_models/programming-language/programming-language.response.dto';
import { combineLatest } from 'rxjs';
@Component({
  selector: 'app-update-problem',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CKEditorModule,
    RouterModule,
    MatIcon,
  ],
  templateUrl: './update-problem.component.html',
  styleUrl: './update-problem.component.css',
})
export class UpdateProblemComponent implements OnInit {
  ActionFlagEnum = ActionFlagEnum;
  problemForm!: FormGroup;
  problemId!: string;
  problemDTO!: ProblemDTO;
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private problemService = inject(ProblemService);
  toastr = inject(ToastrService);
  public Editor: any = ClassicEditor;
  showDescriptionEditor = false;
  private router = inject(Router);
  programmingLanguageService = inject(ProgrammingLanguageService);
  languages: GetProgrammingLanguageResponseDTO[] = [];
  selectedLanguageId = signal<string>('');
  loading = signal(false);

  errorMessages: { [key: string]: string | null } = {
    title: null,
    description: null,
    // templateCode: null,
    timeLimit: null,
    memoryLimit: null,
    testCases: null,
  };

  ngOnInit(): void {
    this.problemId = this.route.snapshot.paramMap.get('id')!;
    this.initializeForm();
  }

  initializeForm(): void {
    this.loading.set(true);

    // Load both data in parallel
    combineLatest([
      this.programmingLanguageService.getAllProgrammingLanguage(),
      this.problemService.getProblemById(this.problemId),
    ]).subscribe({
      next: ([langResponse, problemResponse]) => {
        this.languages = langResponse.data;
        this.problemDTO = problemResponse.data;

        // Set default selected language
        if (this.languages.length > 0) {
          this.selectedLanguageId.set(this.languages[0].id);
        }

        // Initialize the reactive form
        this.problemForm = this.fb.group({
          id: [this.problemDTO.id],
          title: [this.problemDTO.title, Validators.required],
          description: [this.problemDTO.description],
          memoryLimit: [this.problemDTO.memoryLimit, Validators.required],
          timeLimit: [this.problemDTO.timeLimit, Validators.required],
          difficulty: [this.problemDTO.difficulty, Validators.required],
          testCases: this.fb.array(
            this.problemDTO.testCases.map((tc) => this.createTestCaseForm(tc))
          ),
          templateCodes: this.fb.array(
            this.createMergedTemplateCodes(this.problemDTO)
          ),
        });

        this.loading.set(false);
      },
      error: (err) => {
        this.toastr.error('Failed to load problem or programming languages');
        console.error(err);
        this.loading.set(false);
      },
    });
  }

  createMergedTemplateCodes(problem: ProblemDTO): FormGroup[] {
    const existingLangIds = problem.templateCodes.map(
      (tc) => tc.programmingLanguageId
    );

    const existingForms = problem.templateCodes.map((tplc) =>
      this.createTemplateCodeForm(tplc)
    );

    const missingForms = this.languages
      .filter((lang) => !existingLangIds.includes(lang.id))
      .map((lang) => this.newTemplateCode(lang.id));

    return [...existingForms, ...missingForms];
  }
  get testCases(): FormArray {
    return this.problemForm.get('testCases') as FormArray;
  }
  get templateCodes(): FormArray {
    return this.problemForm.get('templateCodes') as FormArray;
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

  newTemplateCode(languageId: string): FormGroup {
    return this.fb.group({
      programmingLanguageId: [languageId, Validators.required],
      userTemplateCode: ['', Validators.required],
      hiddenTemplateCode: ['', Validators.required],
      actionFlag: [ActionFlagEnum.Insert],
    });
  }

  createTestCaseForm(testCase: TestCaseDTO): FormGroup {
    return this.fb.group({
      id: [testCase.id],
      input: [testCase.input],
      expectedOutput: [testCase.expectedOutput],
      inputDisplay: [testCase.inputDisplay],
      isHidden: [testCase.isHidden],
      expectedOutputDisplay: [testCase.expectedOutputDisplay],
      actionFlag: [ActionFlagEnum.NoAction],
    });
  }

  createTemplateCodeForm(templateCode: TemplateCodeDTO): FormGroup {
    return this.fb.group({
      id: [templateCode.id],
      userTemplateCode: [templateCode.userTemplateCode],
      hiddenTemplateCode: [templateCode.hiddenTemplateCode],
      programmingLanguageId: [templateCode.programmingLanguageId],
      actionFlag: [ActionFlagEnum.NoAction],
    });
  }

  onLanguageChange(event: Event) {
    const select = event.target as HTMLSelectElement | null;
    if (select) {
      this.selectedLanguageId.set(select.value);
    }
  }

  toggleDescriptionEditor() {
    this.showDescriptionEditor = !this.showDescriptionEditor;
    // if (this.showDescriptionEditor) {
    //   this.showTemplateCodeEditor = false;
    // }
  }

  addTestCase() {
    this.testCases.push(
      this.fb.group({
        id: ['00000000-0000-0000-0000-000000000000'], // empty for new ones
        input: [''],
        expectedOutput: [''],
        inputDisplay: [''],
        expectedOutputDisplay: [''],
        isHidden: [false],
        actionFlag: [ActionFlagEnum.Insert],
      })
    );
  }

  removeTestCase(index: number) {
    const control = this.testCases.at(index);
    const id = control.get('id')?.value;
    const currentFlag = control.get('actionFlag')?.value;
    if (id && id != '00000000-0000-0000-0000-000000000000') {
      if (currentFlag === ActionFlagEnum.Delete) {
        const original = this.problemDTO.testCases.find((tc) => tc.id === id);
        if (original) {
          control.patchValue({
            input: original.input,
            expectedOutput: original.expectedOutput,
            inputDisplay: original.inputDisplay,
            expectedOutputDisplay: original.expectedOutputDisplay,
            isHidden: original.isHidden,
          });
        }
        control.patchValue({ actionFlag: ActionFlagEnum.NoAction });
      } else {
        control.patchValue({ actionFlag: ActionFlagEnum.Delete });
      }
    } else {
      this.testCases.removeAt(index);
    }
  }

  onTestCaseChange(index: number) {
    const control = this.testCases.at(index);
    const actionFlag = control.get('actionFlag')?.value;
    if (actionFlag === ActionFlagEnum.NoAction) {
      control.patchValue({ actionFlag: ActionFlagEnum.Update });
    }
  }

  onTemplateCodeChange(index: number) {
    console.log('template code changed');
    console.log(index);
    console.log(this.templateCodes.at(index));
    const control = this.templateCodes.at(index);
    const actionFlag = control.get('actionFlag')?.value;
    if (actionFlag === ActionFlagEnum.NoAction) {
      control.patchValue({ actionFlag: ActionFlagEnum.Update });
    }
  }

  submit(): void {
    if (!this.validationBeforeInsert()) {
      return;
    }
    if (this.problemForm.valid) {
      const payload = {
        ...this.problemForm.value,
        difficulty: Number(this.problemForm.value.difficulty),
      };
      console.log('payload: ');
      console.log(payload);
      this.problemService.updateProblem(payload).subscribe({
        next: (response) => {
          if (response.success) {
            Swal.fire({
              icon: 'success',
              title: 'Thành công',
              text: response.message,
            }).then(() => {
              this.router.navigate([`/problem/detail/${this.problemId}`]);
            });
          }
        },
      });
    }
  }

  private validationBeforeInsert(): boolean {
    const descriptionControl = this.problemForm.get('description');
    // const templateCodeControl = this.problemForm.get('templateCode');

    if (descriptionControl && descriptionControl.value?.trim() === '') {
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
      if (testCase.get('actionFlag')?.value === ActionFlagEnum.Delete) continue;
      const input = testCase.get('input')?.value?.trim();
      const expectedOutput = testCase.get('expectedOutput')?.value?.trim();
      const inputDisplay = testCase.get('inputDisplay')?.value?.trim();
      const expectedOutputDisplay = testCase
        .get('expectedOutputDisplay')
        ?.value?.trim();

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
    const control = this.problemForm.get('title');
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
    const control = this.problemForm.get('timeLimit');
    if (!control) return;
    const value = control.value;
    if (value <= 0) {
      this.errorMessages['timeLimit'] = 'Giới hạn thời gian phải lớn hơn 0';
      setInputInvalid('timeLimit');
    } else {
      this.errorMessages['timeLimit'] = null;
    }
  }

  validateMemoryLimit() {
    const control = this.problemForm.get('memoryLimit');
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
