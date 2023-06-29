import { Box } from '@mui/material';
import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTopicDetails } from '~/app/hooks';
import { useGlobalContext } from '~/main/app';
import TopicAttachmentInfoContainer from '../containers/TopicAttachmentInfoContainer';
import TopicInfoContainer from '../containers/TopicInfoContainer';
import TopicWorkFlowContainer from '../containers/TopicWorkFlowContainer';

export default function TopicPage() {
  const { setTitle } = useGlobalContext();
  const { topicId } = useParams<{ topicId: string }>();
  const topic = useTopicDetails(topicId || '');

  useEffect(() => {
    setTitle('Thông tin đề tài');
  }, [setTitle]);

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: 'auto 1fr',
        gridTemplateRows: '1fr auto',
        gridTemplateAreas: `
          'h1 h2' 'h3 h3'
        `,
        gap: 3,
      }}
    >
      <TopicInfoContainer topic={topic} />
      <TopicAttachmentInfoContainer topic={topic} />
      <TopicWorkFlowContainer topic={topic} />
    </Box>
  );
}
