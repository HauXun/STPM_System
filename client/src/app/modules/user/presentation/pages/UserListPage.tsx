import { Box } from '@mui/material';
import UserListContainer from '../containers/UserListContainer';
import { useGlobalContext } from '~/main/app';
import { useEffect } from 'react';

export default function UserListPage() {
  const { setTitle } = useGlobalContext();

  useEffect(() => {
    setTitle('Danh sách tài khoản');
  }, []);

  return (
    <Box>
      <UserListContainer />
    </Box>
  );
}
