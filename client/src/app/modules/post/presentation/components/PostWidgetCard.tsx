import { Box, Paper, PaperProps, Typography } from '@mui/material';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { darkerBoxShadow } from '~/app/modules/shared/utils';
import { Post } from '../../domain/models/Post';

type PostWidgetCardProps = PaperProps & {
  post: Post;
  direction?: 'column' | 'row';
  columnWidth?: number | string;
  rowWidth?: number | string;
};

export default function PostWidgetCard({
  sx,
  post,
  className,
  direction = 'row',
  columnWidth,
  rowWidth,
  ...props
}: PostWidgetCardProps) {
  return (
    <Paper
      className={`${className}`}
      elevation={0}
      sx={{
        borderRadius: 3,
        width: direction === 'column' ? columnWidth ?? 400 : rowWidth ?? 280,
        boxShadow: COMPONENT_SHADOW,
        cursor: 'pointer',
        transition: 'all .15s ease-in-out',
        '&:hover': {
          opacity: 0.9,
          boxShadow: darkerBoxShadow(COMPONENT_SHADOW, 0.3),
        },
        ...sx,
      }}
    >
      <Box
        sx={{
          display: 'grid',
          width: 'inherit',
          ...(direction === 'column'
            ? {
                gridTemplateColumns: 'auto 1fr',
              }
            : {
                gridTemplateRows: 'auto 1fr',
              }),
        }}
      >
        <Box
          component="img"
          sx={{
            ...(direction === 'column'
              ? {
                  width: 100,
                  aspectRatio: '1 / 1',
                  borderRadius: 2,
                }
              : {
                  width: 'inherit',
                  aspectRatio: '3.2 / 2',
                  borderTopLeftRadius: '8px',
                  borderTopRightRadius: '8px',
                }),
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
            my: 1,
            mx: 2,
            '& .MuiTypography-root': {
              overflow: 'hidden',
              display: '-webkit-box',
              WebkitBoxOrient: 'vertical',
            },
          }}
        >
          <Typography
            color="var(--primary-green)"
            className="text-xl font-bold"
            variant="body1"
            sx={{
              WebkitLineClamp: 1,
            }}
          >
            {post.title}
          </Typography>
          <Typography
            variant="caption"
            sx={{
              WebkitLineClamp: 2,
            }}
          >
            {post.shortDescription}
          </Typography>
        </Box>
      </Box>
    </Paper>
  );
}
