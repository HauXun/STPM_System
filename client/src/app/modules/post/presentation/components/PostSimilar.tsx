import { Stack, Typography, Box } from '@mui/material';
import { useState, useEffect } from 'react';
import { Post } from '../../domain/models/Post';
import { defaultPostService } from '~/app/modules/shared/common';
import PostWidgetCard from './PostWidgetCard';

type PostSimilarProps = {};

export default function PostSimilar({}: PostSimilarProps) {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    (async () => {
      try {
        const data = await defaultPostService.getRandomPosts(3);
        setPosts(data);
      } catch (error) {
        console.log('Failed to fetch similar posts', error);
      }
    })();
  }, []);

  return (
    <Stack spacing={3}>
      <Typography className="text-3xl font-bold">Chủ đề tương tự</Typography>
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: '1fr 1fr 1fr',
          columnGap: 3,
        }}
      >
        {posts &&
          posts.length > 0 &&
          posts.map((post, i) => <PostWidgetCard key={i} post={post} rowWidth="100%" />)}
      </Box>
    </Stack>
  );
}
