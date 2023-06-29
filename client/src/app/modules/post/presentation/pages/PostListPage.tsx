import { Stack } from '@mui/material';
import PostListContainer from '../containers/PostListContainer';
import SearchFilter from '~/app/modules/shared/presentation/components/SearchFilter';

export default function PostListPage() {
  return (
    <Stack spacing={5}>
      <SearchFilter placeholder='Tìm kiếm bài viết' />
      <PostListContainer />
    </Stack>
  );
}
