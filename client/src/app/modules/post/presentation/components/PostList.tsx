import { Box } from '@mui/material';
import { ChangeEvent, useEffect } from 'react';
import PrimaryPagination from '~/app/modules/shared/presentation/components/Pagination';
import { PostFilterModel } from '../../domain/models/PostFilterModel';
import { PostState } from '../../infrastructure/store/types';
import PostCard from './PostCard';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';

type PostListProps = {
  postState: PostState;
  fetchPosts: (model: PostFilterModel) => void;
  setFilter: (model: PostFilterModel) => void;
};

export default function PostList({ postState, fetchPosts, setFilter }: PostListProps) {
  const { data: posts, filter, pagination } = postState;

  useEffect(() => {
    fetchPosts(filter);
  }, [fetchPosts, filter]);

  useEffect(() => {
    setFilter({ pageNumber: 1, pageSize: 7 } as PostFilterModel);
  }, [setFilter]);

  const handlePageChange = (e: ChangeEvent<unknown>, page: number) => {
    setFilter({ ...filter, pageNumber: page } as PostFilterModel);
  };

  return (
    <BoxFlexCenter direction='column' className='gap-y-8'>
      <Box
        sx={{
          display: 'grid',
          gridTemplateAreas: `
            'h1 h1 h2' 
            'h3 h4 h5'
            'h6 h7 h7'
          `,
          gap: 5,
        }}
      >
        {posts &&
          posts.length > 0 &&
          posts.map((post, i) => (
            <PostCard
              post={post}
              key={i}
              sx={{
                ...(i === 0 ? { gridArea: 'h1', width: '100%' } : ''),
                ...(i === 6 ? { gridArea: 'h7', width: '100%' } : ''),
              }}
            />
          ))}
      </Box>
      <PrimaryPagination
        count={Math.ceil((pagination?.totalItemCount ?? 0) / (pagination?.pageSize ?? 1))}
        page={(pagination?.pageIndex ?? 0) + 1}
        onChange={handlePageChange}
      />
    </BoxFlexCenter>
  );
}
