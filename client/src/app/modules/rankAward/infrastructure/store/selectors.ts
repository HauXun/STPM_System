import { RootState } from '~/app/stores/store';
import { createSelector } from '@reduxjs/toolkit';
import { RankAwardState } from './types';

export const selectRankAwards = (state: RootState): RankAwardState => state.rankAward;
export const selectRankAwardLoading = (state: RootState) => state.rankAward.isLoading;
export const selectRankAwardFilter = (state: RootState) => state.rankAward.filter;
export const selectRankAwardPagination = (state: RootState) => state.rankAward.pagination;
export const selectError = (state: RootState) => state.rankAward.error;

export const selectRankAwardById = (id: string) =>
  createSelector(selectRankAwards, (rankAwards) =>
    rankAwards.data.find((rankAward) => rankAward.id === Number(id))
  );

export const selectRankAwardsByYearRange = (year: number) =>
  createSelector(selectRankAwards, (rankAwards) => {
    return rankAwards.data.map((rankAward) => {
      const updatedSpecificAwards = rankAward.specificAwards.filter((s) => s.year === year);
      return { ...rankAward, specificAwards: updatedSpecificAwards };
    });
  });
