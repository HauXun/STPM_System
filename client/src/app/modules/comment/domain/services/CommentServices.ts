export interface Comment {
  id: number;
  content: string;
  date: Date;
  modifiedDate?: Date;
  userId: number;
}
