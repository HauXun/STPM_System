import { Role } from "~/app/modules/role/domain/models/Role";

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
  lockoutEnabled: boolean;
  role: Role;
}
