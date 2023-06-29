import { Outlet, RouteObject } from 'react-router-dom';
import UserForm from '../../user/presentation/components/UserSignUpForm';
import TopicListPage from './pages/TopicListPage';
import TopicPage from './pages/TopicPage';
import TopicPostListPage from './pages/TopicPostListPage';
import TopicPostPage from './pages/TopicPostPage';
import TopicRankPage from './pages/TopicRankPage';
import TopicRegisterDraft from './components/TopicRegisterDraft';

export enum TopicRoutes {
  TOPICS = 'topics',
  REGISTRATION = 'registration',
  IDParams = ':topicId',
  YearParams = ':year',
  DRAFT = 'draft',
  RANKS = 'ranks',
}

export const topicPublicRoutes: RouteObject[] = [
  {
    path: TopicRoutes.TOPICS,
    element: <Outlet />,
    children: [
      {
        index: true,
        element: <TopicPostListPage />,
      },
      {
        path: TopicRoutes.IDParams,
        element: <TopicPostPage />,
      },
    ],
  },
];

export const topicPrivateRoutes: RouteObject[] = [
  {
    path: TopicRoutes.TOPICS,
    element: <Outlet />,
    children: [
      {
        index: true,
        element: <TopicPostListPage />,
      },
      {
        path: TopicRoutes.IDParams,
        element: <TopicPage />,
      },
      {
        path: TopicRoutes.REGISTRATION,
        element: <TopicListPage />,
      },
      {
        path: TopicRoutes.DRAFT,
        element: <TopicRegisterDraft />,
      },
      {
        path: TopicRoutes.RANKS,
        element: <Outlet />,
        children: [
          {
            path: TopicRoutes.YearParams,
            element: <TopicRankPage />,
          },
        ]
      },
    ],
  },
]
