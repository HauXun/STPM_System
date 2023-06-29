import { Badge, Chip, Stack, Typography } from '@mui/material';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import PostWidgetCard from './PostWidgetCard';
import { useState, useEffect } from 'react';
import { defaultPostService } from '~/app/modules/shared/common';
import { Post } from '../../domain/models/Post';

type Props = {};

export default function PostWidget({}: Props) {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    (async () => {
      try {
        const data = await defaultPostService.getPopularPosts(3);
        setPosts(data);
      } catch (error) {
        console.log('Failed to fetch similar posts', error);
      }
    })();
  }, []);

  return (
    <Stack spacing={4}>
      <Stack spacing={1}>
        <Typography variant="h6" className="font-bold">
          Tags
        </Typography>
        <BoxFlexCenter justifyContent="flex-start" className="flex-wrap gap-2" >
          <Chip
            className="bg-orange-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="warning" variant="dot" />}
            label="Học máy"
          />
          <Chip
            className="bg-blue-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="info" variant="dot" />}
            label="Phần mềm"
          />
          <Chip
            className="bg-red-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="error" variant="dot" />}
            label="Ứng dụng"
          />
          <Chip
            className="bg-gray-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="error" variant="dot" />}
            label="Data"
          />
          <Chip
            className="bg-green-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="primary" variant="dot" />}
            label="Thiết bị"
          />
          <Chip
            className="bg-slate-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="secondary" variant="dot" />}
            label="Chế tạo"
          />
          <Chip
            className="bg-orange-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="warning" variant="dot" />}
            label="Giám sát"
          />
          <Chip
            className="bg-green-100"
            sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
            icon={<Badge color="success" variant="dot" />}
            label="Nhận dạng"
          />
        </BoxFlexCenter>
      </Stack>
      <Stack spacing={1}>
        <Typography variant="h6" className="font-bold">
          Bài viết mới
        </Typography>
        <BoxFlexCenter direction="column" className="flex-wrap gap-y-4">
          {posts &&
            posts.length > 0 &&
            posts.map((post, i) => (
              <PostWidgetCard
                key={i}
                post={post}
                direction="column"
                rowWidth="100%"
                sx={{ boxShadow: 'unset', backgroundColor: 'transparent' }}
              />
            ))}
        </BoxFlexCenter>
      </Stack>
    </Stack>
  );
}
