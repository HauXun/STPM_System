import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "~/app/stores/store";
import { UserState } from "./types";

export const selectUsers = (state: RootState): UserState => state.user;
export const selectUserLoading = (state: RootState) => state.user.isLoading;
export const selectUserFilter = (state: RootState) => state.user.filter;
export const selectUserPagination = (state: RootState) => state.user.pagination;
export const selectError = (state: RootState) => state.user.error;

export const selectUserById = (id: string) =>
  createSelector(selectUsers, (users) => users.data.find((user) => user.id === Number(id)));