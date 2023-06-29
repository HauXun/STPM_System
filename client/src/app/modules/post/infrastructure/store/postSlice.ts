import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { Post } from "../../domain/models/Post";
import { PostFilterModel } from "../../domain/models/PostFilterModel";
import { PostState } from "./types";

const initialState: PostState = {
  data: [],
  pagination: null,
  filter: {} as PostFilterModel,
  isLoading: false,
  error: null,
};

const postSlice = createSlice({
  name: 'post',
  initialState,
  reducers: {
    fetchPosts(state, action: PayloadAction<PostFilterModel>) {
      state.isLoading = true;
    },
    fetchPostsSuccess(state, action: PayloadAction<PaginationResponse<Post[]>>) {
      state.data = action.payload.items;
      state.pagination = action.payload.metadata;
      state.isLoading = false;
      state.error = null;
    },
    fetchPostsError(state, action: PayloadAction<string>) {
      state.error = action.payload;
      state.isLoading = false;
    },
    setFilter(state, action: PayloadAction<PostFilterModel>) {
      state.filter = action.payload;
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    setFilterWithDebounce(state, action: PayloadAction<PostFilterModel>) {},
  },
});

// Actions
export const postActions = postSlice.actions;

// Reducer
const postReducer = postSlice.reducer;
export default postReducer;