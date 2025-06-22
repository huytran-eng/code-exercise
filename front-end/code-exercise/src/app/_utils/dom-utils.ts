import { FormGroup } from '@angular/forms';

export function setInputInvalid(id: string) {
  setTimeout(() => {
    const input = document.getElementById(id) as HTMLElement;
    if (input) {
      input.classList.add('is-invalid');

      // remove invalid state when the input is changed
      input.addEventListener(
        'focus',
        () => {
          input.classList.remove('is-invalid');
        },
        { once: true }
      );
    }
  });
}

export function clearAllInvalidInputs() {
  const invalidInputs = document.querySelectorAll('.is-invalid');
  invalidInputs.forEach((input) => input.classList.remove('is-invalid'));
}

export function setFormInputError(
  formGroup: FormGroup,
  controlName: string,
  error: string
) {
  const control = formGroup.get(controlName);
  if (control) {
    control.setErrors({ [error]: true });
    control.markAsTouched();
    control.markAsDirty();
    control.updateValueAndValidity();
  }
}
