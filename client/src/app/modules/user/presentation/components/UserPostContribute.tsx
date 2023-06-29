import { Paper, Typography, Divider, Box, PaperProps } from '@mui/material';
import { useEffect } from 'react';
import { CustomScrollbar } from '~/app/modules/core/presentation/components/CustomScrollbar';
import { PostFilterModel } from '~/app/modules/post/domain/models/PostFilterModel';
import { PostState } from '~/app/modules/post/infrastructure/store/types';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';

type Props = PaperProps & {
  userId: string;
  postState: PostState;
  fetchPosts: (model: PostFilterModel) => void;
  setFilter: (model: PostFilterModel) => void;
};

export default function UserPostContribute({
  sx,
  userId,
  postState,
  fetchPosts,
  setFilter,
}: Props) {
  const { data, filter } = postState;

  useEffect(() => {
    fetchPosts(filter);
  }, [fetchPosts, filter]);

  useEffect(() => {
    setFilter({ userId: Number(userId) } as PostFilterModel);
  }, [setFilter, userId]);

  return (
    <Paper
      sx={{
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        p: 2,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
        rowGap: 1.5,
      }}
    >
      <Typography color="var(--primary-green)" className="text-lg font-bold" variant="body1">
        Bài viết đóng góp
      </Typography>
      <Divider />
      <CustomScrollbar
        // eslint-disable-next-line tailwindcss/no-custom-classname
        className="custom-scrollbar"
        autoHide
        autoHideTimeout={1000}
        autoHideDuration={200}
      >
        {data &&
          data.length > 0 &&
          data.map((post, i) => (
            <Box
              key={i}
              sx={{
                display: 'grid',
                placeItems: 'center',
                width: 'inherit',
                columnGap: 2,
                gridTemplateColumns: 'auto 1fr auto',
                '&:not(:first-of-type)': {
                  mt: 2,
                },
              }}
            >
              <Box
                component="img"
                sx={{
                  width: 60,
                  aspectRatio: '1 / 1',
                  borderRadius: 2,
                }}
                src={
                  post.postPhotos[0]
                    ? post.postPhotos[0].imageUrl
                    : 'https://q-dance-network-images.akamaized.net/41347/1666098985-0001_20220625165639_kevin_453a5245-edit.jpg?auto=compress&fit=max?w=1200'
                }
              />
              <Box
                sx={{
                  display: 'flex',
                  flexDirection: 'column',
                  '& .MuiTypography-root': {
                    WebkitLineClamp: 1,
                    overflow: 'hidden',
                    display: '-webkit-box',
                    WebkitBoxOrient: 'vertical',
                  },
                }}
              >
                <Typography className="text-xl font-bold" variant="body1">
                  {post.title}
                </Typography>
                <Typography variant="body2">{post.shortDescription}</Typography>
              </Box>
              <Typography color="var(--primary-green)" variant="body2">
                {new Date(post.postedDate).toLocaleDateString()}
              </Typography>
            </Box>
          ))}
      </CustomScrollbar>
    </Paper>
  );
}
