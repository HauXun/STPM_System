import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { Topic } from "../../domain/models/Topic";
import { TopicState } from "./types";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { TopicFilterModel } from "../../domain/models/TopicFilterModel";

const initialState: TopicState = {
  data: [],
  pagination: null,
  filter: {} as TopicFilterModel,
  isLoading: false,
  error: null,
};

const topicSlice = createSlice({
  name: 'topic',
  initialState,
  reducers: {
    fetchTopics(state, action: PayloadAction<TopicFilterModel>) {
      state.isLoading = true;
    },
    fetchTopicsSuccess(state, action: PayloadAction<PaginationResponse<Topic[]>>) {
      state.data = action.payload.items;
      state.pagination = action.payload.metadata;
      state.isLoading = false;
      state.error = null;
    },
    fetchTopicsError(state, action: PayloadAction<string>) {
      state.error = action.payload;
      state.isLoading = false;
    },
    setFilter(state, action: PayloadAction<TopicFilterModel>) {
      state.filter = action.payload;
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    setFilterWithDebounce(state, action: PayloadAction<TopicFilterModel>) {},
  },
});

// Actions
export const topicActions = topicSlice.actions;

// Reducer
const topicReducer = topicSlice.reducer;
export default topicReducer;