export interface User {
  id: number;
  fullName: string;
  userName: string;
  imageUrl: string;
  urlSlug: string;
  joinedDate: Date;
  mssv: string;
  gradeName: string;
  email: string;
  phoneNumber: string;
  lockEnable: boolean;
  roles: string[];

  postCount: number;
  topicCount: number;
  notifyCount: number;
  commentCount: number;
  topicRatingCount: number;
}
