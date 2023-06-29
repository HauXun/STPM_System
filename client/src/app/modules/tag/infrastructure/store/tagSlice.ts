import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { Tag } from "../../domain/models/Tag";
import { TagFilterModel } from "../../domain/models/TagFilterModel";
import { TagState } from "./types";

const initialState: TagState = {
  data: [],
  pagination: null,
  filter: {} as TagFilterModel,
  isLoading: false,
  error: null,
};

const tagSlice = createSlice({
  name: 'tag',
  initialState,
  reducers: {
    fetchTags(state, action: PayloadAction<TagFilterModel>) {
      state.isLoading = true;
    },
    fetchTagsSuccess(state, action: PayloadAction<PaginationResponse<Tag[]>>) {
      state.data = action.payload.items;
      state.pagination = action.payload.metadata;
      state.isLoading = false;
      state.error = null;
    },
    fetchTagsError(state, action: PayloadAction<string>) {
      state.error = action.payload;
      state.isLoading = false;
    },
    setFilter(state, action: PayloadAction<TagFilterModel>) {
      state.filter = action.payload;
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    setFilterWithDebounce(state, action: PayloadAction<TagFilterModel>) {},
  },
});

// Actions
export const tagActions = tagSlice.actions;

// Reducer
const tagReducer = tagSlice.reducer;
export default tagReducer;