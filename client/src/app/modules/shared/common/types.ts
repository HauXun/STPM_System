export type Order = 'asc' | 'desc';

export interface HeadCell {
  label: string;
  align: 'left' | 'center' | 'right' | 'justify' | 'inherit';
  minWidth?: number | string;
}

export interface HeadCellWithId<Key extends keyof never> extends HeadCell {
  id: Key;
}