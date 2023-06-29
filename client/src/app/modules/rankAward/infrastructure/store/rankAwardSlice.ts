import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RankAward } from '../../domain/models/RankAward';
import { RankAwardFilterModel } from '../../domain/models/RankAwardFilterModel';
import { RankAwardState } from './types';
import { PaginationResponse } from '~/app/modules/config/api/api';

const initialState: RankAwardState = {
  data: [],
  pagination: null,
  filter: {} as RankAwardFilterModel,
  isLoading: false,
  error: null,
};

const rankAwardSlice = createSlice({
  name: 'rankAward',
  initialState,
  reducers: {
    fetchRankAwards(state, action: PayloadAction<RankAwardFilterModel>) {
      state.isLoading = true;
    },
    fetchRankAwardsSuccess(state, action: PayloadAction<PaginationResponse<RankAward[]>>) {
      state.data = action.payload.items;
      state.isLoading = false;
      state.error = null;
    },
    fetchRankAwardsError(state, action: PayloadAction<string>) {
      state.error = action.payload;
      state.isLoading = false;
    },
    setFilter(state, action: PayloadAction<RankAwardFilterModel>) {
      state.filter = action.payload;
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    setFilterWithDebounce(state, action: PayloadAction<RankAwardFilterModel>) {},
  },
});

// Actions
export const rankAwardActions = rankAwardSlice.actions;

// Reducer
const rankAwardReducer = rankAwardSlice.reducer;
export default rankAwardReducer;
