import { Component, inject, Inject } from '@angular/core';
import { BreadcrumbService } from '../../_services/breadcrumb.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.css',
})
export class BreadcrumbComponent {
  breadcrumbs: Array<{ label: string; url: string }> = [];
  breadcrumbService = inject(BreadcrumbService);

  ngOnInit(): void {
    this.breadcrumbService.breadcrumbs$.subscribe((bcs) => {
      this.breadcrumbs = bcs;
      console.log('Breadcrumbs:', this.breadcrumbs);
    });
  }
}
