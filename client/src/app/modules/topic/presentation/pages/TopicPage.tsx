import { Box } from '@mui/material';
import TopicAttachmentInfoContainer from '../containers/TopicAttachmentInfoContainer';
import TopicInfoContainer from '../containers/TopicInfoContainer';
import TopicWorkFlowContainer from '../containers/TopicWorkFlowContainer';
import { useEffect, useState } from 'react';
import { useGlobalContext } from '~/main/app';
import { useParams } from 'react-router-dom';
import { Topic } from '../../domain/models/Topic';
import { selectTopicById } from '../../infrastructure/store/selectors';
import { useAppSelector } from '~/app/stores/hooks';
import { defaultTopicService } from '~/app/modules/shared/common';

export default function TopicPage() {
  const { setTitle } = useGlobalContext();
  const [topic, setTopic] = useState<Topic>();

  const { id: topicId } = useParams<{ id: string }>();
  const selectTopic = selectTopicById(Number(topicId));
  const topicStoreSelect = useAppSelector(selectTopic);

  useEffect(() => {
    if (!topicId) return;

    if (topicStoreSelect) {
      setTopic(topicStoreSelect);
      return;
    }

    // IFFE
    (async () => {
      try {
        const data: Topic = await defaultTopicService.getTopicsById(Number(topicId));
        setTopic(data);
      } catch (error) {
        console.log('Failed to fetch topic details', error);
      }
    })();
  }, [topicId, topicStoreSelect]);

  useEffect(() => {
    setTitle('Thông tin đề tài');
  }, []);

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        flexWrap: 'wrap',
        gap: 3,
      }}
    >
      <TopicInfoContainer topic={topic} />
      <TopicAttachmentInfoContainer topic={topic} />
      <TopicWorkFlowContainer topic={topic} />
    </Box>
  );
}
