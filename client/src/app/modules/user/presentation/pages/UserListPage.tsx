import { Stack } from '@mui/material';
import { ChangeEvent, useEffect } from 'react';
import SearchFilter from '~/app/modules/shared/presentation/components/SearchFilter';
import { useGlobalContext } from '~/main/app';
import UserListContainer from '../containers/UserListContainer';

export default function UserListPage() {
  const { setTitle } = useGlobalContext();

  useEffect(() => {
    setTitle('Danh sách tài khoản');
  }, [setTitle]);

  return (
    <Stack spacing={3}>
      <SearchFilter
        placeholder="Tìm kiếm tài khoản"
        onChange={(event: ChangeEvent<HTMLInputElement>) => console.log(event.target.value)}
      />
      <UserListContainer />
    </Stack>
  );
}
