import { Outlet, RouteObject } from 'react-router-dom';
import UserListPage from './pages/UserListPage';

export enum UserRoutes {
  INDEX = '',
  USERS = 'users',
}

export const userRoutes: RouteObject[] = [
  {
    path: UserRoutes.USERS,
    element: <Outlet />,
    children: [
      {
        path: UserRoutes.INDEX,
        element: <UserListPage />,
      },
    ],
  },
];
