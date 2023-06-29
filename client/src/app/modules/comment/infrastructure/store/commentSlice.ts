import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { Comment } from "../../domain/models/Comment";
import { CommentFilterModel } from "../../domain/models/CommentFilterModel";
import { CommentState } from "./types";

const initialState: CommentState = {
  data: [],
  pagination: null,
  filter: {} as CommentFilterModel,
  isLoading: false,
  error: null,
};

const commentSlice = createSlice({
  name: 'comment',
  initialState,
  reducers: {
    fetchComments(state, action: PayloadAction<CommentFilterModel>) {
      state.isLoading = true;
    },
    fetchCommentsSuccess(state, action: PayloadAction<PaginationResponse<Comment[]>>) {
      state.data = action.payload.items;
      state.pagination = action.payload.metadata;
      state.isLoading = false;
      state.error = null;
    },
    fetchCommentsError(state, action: PayloadAction<string>) {
      state.error = action.payload;
      state.isLoading = false;
    },
    setFilter(state, action: PayloadAction<CommentFilterModel>) {
      state.filter = action.payload;
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    setFilterWithDebounce(state, action: PayloadAction<CommentFilterModel>) {},
  },
});

// Actions
export const commentActions = commentSlice.actions;

// Reducer
const commentReducer = commentSlice.reducer;
export default commentReducer;