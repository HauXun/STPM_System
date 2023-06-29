import { Outlet, RouteObject } from 'react-router-dom';
import UserListPage from './pages/UserListPage';
import UserInfoPage from './pages/UserInfoPage';

export enum UserRoutes {
  USERS = 'users',
  IDParams = ':id',
}

export const userPublicRoutes: RouteObject[] = [
  {
    path: UserRoutes.USERS,
    element: <Outlet />,
    children: [
      {
        path: UserRoutes.IDParams,
        element: <UserInfoPage />,
      },
    ],
  },
];

export const userPrivateRoutes: RouteObject[] = [
  {
    path: UserRoutes.USERS,
    element: <Outlet />,
    children: [
      {
        index: true,
        element: <UserListPage />,
      },
      {
        path: UserRoutes.IDParams,
        element: <UserInfoPage />,
      },
    ],
  },
];
