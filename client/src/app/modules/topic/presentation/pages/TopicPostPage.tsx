import { Box } from '@mui/material';
import TopicPostContainer from '../containers/TopicPostContainer';

type Props = {};

export default function TopicPostPage({}: Props) {
  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: '1fr',
      }}
    >
      <TopicPostContainer />
    </Box>
  );
}
