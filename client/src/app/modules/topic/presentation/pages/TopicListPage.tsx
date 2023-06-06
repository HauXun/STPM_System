import { Box } from '@mui/material';
import { useEffect } from 'react';
import { useGlobalContext } from '~/main/app';
import TopicListContainer from '../containers/TopicListContainer';

export default function TopicListPage() {
  const { setTitle } = useGlobalContext();

  useEffect(() => {
    setTitle('Danh sách đề tài');

  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  
  return (
    <Box>
      <TopicListContainer />
    </Box>
  );
}
