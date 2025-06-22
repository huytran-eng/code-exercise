import { Component, inject, input, OnInit, output } from '@angular/core';
import { MenuItem } from '../../_models/layout/menu-item.layout';
import { MenuService } from '../../_services/menu.service';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { MenuItemComponent } from '../menu-item/menu-item.component';
@Component({
  selector: 'app-side-menu',
  standalone: true,
  imports: [MatListModule, MatIconModule, RouterModule, MenuItemComponent],
  templateUrl: './side-menu.component.html',
  styleUrl: './side-menu.component.css',
})
export class SideMenuComponent implements OnInit {
  private menuService = inject(MenuService);
  menuItems: MenuItem[] = [];

  childItemClicked = output();
  nestedMenuOpen = input(false);

  ngOnInit(): void {
    this.menuService.getMenuItems().subscribe((data: MenuItem[]) => {
      this.menuItems = data;
    });
  }

  onChilItemClicked() {
    this.childItemClicked.emit(); // Bubble up to LayoutComponent
  }
}
