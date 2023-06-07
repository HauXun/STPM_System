import { Outlet, RouteObject } from 'react-router-dom';
import TopicPage from './pages/TopicPage';
import TopicRegisterDraft from './components/TopicRegisterDraft';
import TopicListPage from './pages/TopicListPage';

export enum TopicRoutes {
  INDEX = '',
  TOPICS = 'topics',
  TOPIC = ':id',
  DRAFT = 'draft',
}

export const topicRoutes: RouteObject[] = [
  {
    path: TopicRoutes.TOPICS,
    element: <Outlet />,
    children: [
      {
        path: TopicRoutes.INDEX,
        element: <TopicListPage />,
      },
      {
        path: TopicRoutes.TOPIC,
        element: <TopicPage />,
      },
      {
        path: TopicRoutes.DRAFT,
        element: <TopicRegisterDraft />,
      },
    ],
  },
];
