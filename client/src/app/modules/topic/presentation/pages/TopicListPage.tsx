import { Stack } from '@mui/material';
import { ChangeEvent, useEffect } from 'react';
import SearchFilter from '~/app/modules/shared/presentation/components/SearchFilter';
import { useGlobalContext } from '~/main/app';
import TopicListContainer from '../containers/TopicListContainer';

export default function TopicListPage() {
  const { setTitle } = useGlobalContext();

  useEffect(() => {
    setTitle('Danh sách đề tài đăng ký');
  }, [setTitle]);
  
  return (
    <Stack spacing={3}>
      <SearchFilter
        placeholder="Tìm kiếm đề tài"
        onChange={(event: ChangeEvent<HTMLInputElement>) => console.log(event.target.value)}
      />
      <TopicListContainer />
    </Stack>
  );
}
