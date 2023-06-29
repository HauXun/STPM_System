import { Outlet, RouteObject } from 'react-router-dom';
import PostListPage from './pages/PostListPage';
import PostPage from './pages/PostPage';

export enum PostRoutes {
  POSTS = 'posts',
  IDParams = ':postId',
}

export const postPublicRoutes: RouteObject[] = [
  {
    path: PostRoutes.POSTS,
    element: <Outlet />,
    children: [
      {
        index: true,
        element: <PostListPage />,
      },
      {
        path: PostRoutes.IDParams,
        element: <PostPage />,
      },
    ],
  },
];
