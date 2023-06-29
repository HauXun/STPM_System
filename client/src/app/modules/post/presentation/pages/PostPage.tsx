import { Box, Stack } from '@mui/material';
import CommentArea from '~/app/modules/comment/presentation/pages/CommentArea';
import PostContainer from '../containers/PostContainer';
import PostSimilarContainer from '../containers/PostSimilarContainer';
import PostWidgetContainer from '../containers/PostWidgetContainer';
import { useParams } from 'react-router-dom';

type Props = {};

export default function PostPage({}: Props) {
  const { postId } = useParams<{ postId: string }>();

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: '1fr 350px',
        columnGap: 5,
      }}
    >
      <Stack spacing={4}>
        <PostContainer postId={postId || ''} />
        <CommentArea />
        <PostSimilarContainer />
      </Stack>
      <PostWidgetContainer />
    </Box>
  );
}
