import { defaultTopicService } from '~/app/modules/shared/common';
import { Topic } from '~/app/modules/topic/domain/models/Topic';
import { useAppSelector } from '~/app/stores/hooks';
import { useState, useEffect } from "react";
import { selectTopicById } from '~/app/modules/topic/infrastructure/store/selectors';

export const useTopicDetails = (topicId: string) => {
  const selectTopic = selectTopicById(topicId);
  const topicStoreSelect = useAppSelector(selectTopic);
  const [topic, setTopic] = useState<Topic | undefined>();

  useEffect(() => {
    if (!topicId) return;

    if (topicStoreSelect) {
      setTopic(topicStoreSelect);
      return;
    }

    (async () => {
      try {
        const data = await defaultTopicService.getTopicsById(topicId);
        setTopic(data);
      } catch (error) {
        console.log('Failed to fetch topic details', error);
      }
    })();
  }, [topicId, topicStoreSelect]);

  return topic;
};