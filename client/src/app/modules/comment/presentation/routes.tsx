import { Outlet, RouteObject } from 'react-router-dom';

export enum CommentRoutes {
  COMMENTS = 'comments',
}

export const userRoutes: RouteObject[] = [
  {
    path: CommentRoutes.COMMENTS,
    element: <Outlet />,
    children: [
      // {
      //   index: true,
      //   element: <CommentListPage />,
      // },
    ],
  },
];
