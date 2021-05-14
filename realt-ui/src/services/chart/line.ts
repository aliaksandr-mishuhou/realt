import { Item } from 'src/services/item';

export interface Line {
  label: string;
  mapFunc: (item: Item) => any;
  hidden?: boolean;
}
