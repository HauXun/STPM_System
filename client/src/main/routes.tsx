import { Outlet, RouteObject } from "react-router-dom";
import AppLayout from "~/app/modules/core/presentation/components/layouts";
import AdminLayout from "~/app/modules/core/presentation/components/layouts/admin";
import NotFoundPage from "~/app/modules/core/presentation/pages/NotFoundPage";
import { postPublicRoutes } from "~/app/modules/post/presentation/routes";
import TopicSignUpPage from "~/app/modules/topic/presentation/pages/TopicSignUpPage";
import { topicPrivateRoutes, topicPublicRoutes } from "~/app/modules/topic/presentation/routes";
import UserSignInPage from "~/app/modules/user/presentation/pages/UserSignInPage";
import UserSignUpPage from "~/app/modules/user/presentation/pages/UserSignUpPage";
import { userPrivateRoutes, userPublicRoutes } from "~/app/modules/user/presentation/routes";

export const rootRoutes: RouteObject[] = [
  {
    path: '',
    element: <Outlet />,
    children: [
      {
        path: 'sign-up',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <UserSignUpPage />,
          },
          {
            path: 'topic',
            element: <TopicSignUpPage />,
          },
        ],
      },
      {
        path: 'sign-in',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <UserSignInPage />,
          },
        ],
      },
      {
        path: '',
        element: <AppLayout />,
        children: [
          {
            index: true,
            element: <NotFoundPage />,
          },
          ...userPublicRoutes,
          ...topicPublicRoutes,
          ...postPublicRoutes
        ],
      },
      {
        path: 'admin',
        element: <AdminLayout />,
        children: [...topicPrivateRoutes, ...userPrivateRoutes],
      },
    ],
  },
  {
    path: '*',
    element: <NotFoundPage />,
  },
];