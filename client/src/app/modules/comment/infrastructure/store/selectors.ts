import { RootState } from '~/app/stores/store';
import { createSelector } from '@reduxjs/toolkit';
import { CommentState } from './types';

export const selectComments = (state: RootState): CommentState => state.comment;
export const selectCommentLoading = (state: RootState) => state.comment.isLoading;
export const selectCommentFilter = (state: RootState) => state.comment.filter;
export const selectCommentPagination = (state: RootState) => state.comment.pagination;
export const selectError = (state: RootState) => state.comment.error;

export const selectCommentById = (id: string) =>
  createSelector(selectComments, (comments) => comments.data.find((comment) => comment.id === Number(id)));

export const selectCommentsByDateRange = createSelector(
  selectComments,
  (comments) => (start: Date, end: Date) =>
    comments.data.filter((comment) => {
      const commentDate = new Date(comment.date);
      return commentDate >= start && commentDate <= end;
    })
);
