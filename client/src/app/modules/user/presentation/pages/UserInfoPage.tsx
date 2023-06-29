import { Box } from '@mui/material';
import { useEffect } from 'react';
import { useGlobalContext } from '~/main/app';
import UserInfoContainer from '../containers/UserInfoContainer';
import UserAchievementContainer from '../containers/UserAchievementContainer';
import UserTopicJoinedContainer from '../containers/UserTopicJoinedContainer';
import UserPostContributeContainer from '../containers/UserPostContributeContainer';
import { useParams } from 'react-router-dom';

export default function UserInfoPage() {
  const { setTitle } = useGlobalContext();
  const { id: userId } = useParams<{ id: string }>();

  useEffect(() => {
    setTitle('Danh sách tài khoản');
  }, [setTitle]);

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: '850px 1fr',
        gridTemplateRows: 'auto 1fr',
        gap: 4,
      }}
    >
      <UserInfoContainer userId={userId || ''} />
      <UserAchievementContainer userId={userId || ''} />
      <UserTopicJoinedContainer userId={userId || ''} />
      <UserPostContributeContainer userId={userId || ''} />
    </Box>
  );
}
