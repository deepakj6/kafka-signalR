export interface Inventory {
  books: Book[];
  bookCollections: BookCollection[];
}

export interface Book {
  id: string;
  title: string;
  author: string;
  description: string;
  price: number;
  quantity: number;
  bookCollectionId: string;
  addedToCart: boolean;
}

export interface BookCollection {
  id: string;
  name: string;
}
