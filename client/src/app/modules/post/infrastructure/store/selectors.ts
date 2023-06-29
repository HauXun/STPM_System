import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "~/app/stores/store";
import { PostState } from "./types";

export const selectPosts = (state: RootState): PostState => state.post;
export const selectPostLoading = (state: RootState) => state.post.isLoading;
export const selectPostFilter = (state: RootState) => state.post.filter;
export const selectPostPagination = (state: RootState) => state.post.pagination;
export const selectError = (state: RootState) => state.post.error;

export const selectPostById = (id: string) =>
  createSelector(selectPosts, (posts) => posts.data.find((post) => post.id === Number(id)));

export const selectPostsByDateRange = createSelector(
  selectPosts,
  (posts) => (start: Date, end: Date) =>
    posts.data.filter((post) => {
      const postDate = new Date(post.postedDate);
      return postDate >= start && postDate <= end;
    })
);
