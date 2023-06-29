import { Stack } from '@mui/material';
import TopicPostCard from './TopicPostCard';
import PrimaryPagination from '~/app/modules/shared/presentation/components/Pagination';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import { TopicState } from '../../infrastructure/store/types';
import { useEffect, ChangeEvent } from 'react';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';

type TopicPostListProps = {
  topicState: TopicState;
  fetchTopics: (model: TopicFilterModel) => void;
  setFilter: (model: TopicFilterModel) => void;
};

export default function TopicPostList({ topicState, fetchTopics, setFilter }: TopicPostListProps) {
  const { data: topics, filter, pagination } = topicState;

  useEffect(() => {
    fetchTopics(filter);
  }, [fetchTopics, filter]);

  useEffect(() => {
    setFilter({ pageNumber: 1, pageSize: 10 } as TopicFilterModel);
  }, [setFilter]);

  const handlePageChange = (e: ChangeEvent<unknown>, page: number) => {
    setFilter({ ...filter, pageNumber: page } as TopicFilterModel);
  };

  return (
    <BoxFlexCenter direction='column' className='gap-y-8'>
      <Stack spacing={3}>
        {topics &&
          topics.length > 0 &&
          topics.map((topic, i) => <TopicPostCard topic={topic} key={i} />)}
      </Stack>
      <PrimaryPagination
        count={Math.ceil((pagination?.totalItemCount ?? 0) / (pagination?.pageSize ?? 1))}
        page={(pagination?.pageIndex ?? 0) + 1}
        onChange={handlePageChange}
      />
    </BoxFlexCenter>
  );
}
