import { RootState } from '~/app/stores/store';
import { createSelector } from '@reduxjs/toolkit';
import { TopicState } from './types';

export const selectTopics = (state: RootState): TopicState => state.topic;
export const selectTopicLoading = (state: RootState) => state.topic.isLoading;
export const selectTopicFilter = (state: RootState) => state.topic.filter;
export const selectTopicPagination = (state: RootState) => state.topic.pagination;
export const selectError = (state: RootState) => state.topic.error;

export const selectTopicList = createSelector(selectTopics, selectTopicFilter, (topics, filter) => {
  if (filter === null) return topics.data;
  return topics.data.filter((topic) => {
    if (filter.keyword !== '') return topic.topicName.includes(filter.keyword);

    return topic;
  });
});

export const selectTopicById = (id: number) =>
  createSelector(selectTopics, (topics) => topics.data.find((topic) => topic.id === id));

export const selectTopicsByDateRange = createSelector(
  selectTopics,
  (topics) => (start: Date, end: Date) =>
    topics.data.filter((topic) => {
      const topicDate = new Date(topic.regisDate);
      return topicDate >= start && topicDate <= end;
    })
);
