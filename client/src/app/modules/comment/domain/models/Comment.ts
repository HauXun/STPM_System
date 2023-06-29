import { User } from '~/app/modules/user/domain/models/User';
export interface Comment {
  id: number;
  content: string;
  date: Date;
  modifiedDate?: Date;
  userId: number;
  user: User;
}
