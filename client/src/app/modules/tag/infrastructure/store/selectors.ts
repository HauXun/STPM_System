import { RootState } from '~/app/stores/store';
import { createSelector } from '@reduxjs/toolkit';
import { TagState } from './types';

export const selectTags = (state: RootState): TagState => state.tag;
export const selectTagLoading = (state: RootState) => state.tag.isLoading;
export const selectTagFilter = (state: RootState) => state.tag.filter;
export const selectTagPagination = (state: RootState) => state.tag.pagination;
export const selectError = (state: RootState) => state.tag.error;

export const selectTagById = (id: string) =>
  createSelector(selectTags, (tags) => tags.data.find((tag) => tag.id === Number(id)));
