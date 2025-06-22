export interface MenuItem {
  id: string;
  menuName: string;
  menuDisplayName: string;
  menuUrl: string | null;
  isActive: number;
  orderNo: number;
  childItems: MenuItem[];
  parentItemId: string;
}
