import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TagService } from '../../_services/tag.service';
import { TagDetailDTO } from '../../_models/tag/detail-tag.response.dto';
import { DatePipe } from '@angular/common';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

@Component({
  selector: 'app-tag-detail',
  standalone: true,
  imports: [DatePipe, RouterModule, CKEditorModule],
  templateUrl: './tag-detail.component.html',
  styleUrl: './tag-detail.component.css',
})
export class TagDetailComponent {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  tagService = inject(TagService);
  tag: TagDetailDTO | null = null;
  public Editor: any = ClassicEditor;

  loading = signal(true);

  ngOnInit(): void {
    const tagId = this.route.snapshot.paramMap.get('id');
    if (tagId) {
      this.loadTagDetails(tagId);
    }
  }

  loadTagDetails(tagId: string): void {
    this.tagService.getTagById(tagId).subscribe({
      next: (response) => {
        this.tag = response.data;
        this.loading.set(false);
      },
    });
  }
}
