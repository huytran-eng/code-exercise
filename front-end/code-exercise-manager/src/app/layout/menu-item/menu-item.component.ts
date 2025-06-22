import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  output,
  signal,
} from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { Router, RouterModule } from '@angular/router';
import { MenuItem } from '../../_models/layout/menu-item.layout';

@Component({
  selector: 'app-menu-item',
  standalone: true,
  imports: [MatListModule, RouterModule, MatIcon],
  templateUrl: './menu-item.component.html',
  styleUrl: './menu-item.component.css',
})
export class MenuItemComponent implements OnInit {
  item = input.required<MenuItem>();
  level = input.required<number>();
  router = inject(Router);
  nestedMenuOpen = signal(false);
  hasActiveChild = signal(false);
  childItemClicked = output();

  toggleNested() {
    if (!this.item().childItems) {
      return;
    }
    this.nestedMenuOpen.set(!this.nestedMenuOpen());
  }

  ngOnInit(): void {
    const currentUrl = this.router.url;
    const childItems = this.item().childItems || [];
    const active = childItems.some((child) =>
      child.menuUrl ? currentUrl.includes(child.menuUrl) : false
    );

    this.hasActiveChild.set(active);

    if (active) {
      this.nestedMenuOpen.set(true);
    }
  }

  onChildItemClick() {
    this.childItemClicked.emit();
  }

  indentation = computed(() => `${10 + this.level() * 10}px`);
  fontsize = computed(() => `${Math.max(10, 10 - this.level())}px`);
}
