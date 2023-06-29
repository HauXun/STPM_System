import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { PaginationResponse } from "~/app/modules/config/api/api";
import { User } from "../../domain/models/User";
import { UserFilterModel } from "../../domain/models/UserFilterModel";
import { UserState } from "./types";

const initialState: UserState = {
  data: [],
  pagination: null,
  filter: {} as UserFilterModel,
  isLoading: false,
  error: null,
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    fetchUsers(state, action: PayloadAction<UserFilterModel>) {
      state.isLoading = true;
    },
    fetchUsersSuccess(state, action: PayloadAction<PaginationResponse<User[]>>) {
      state.data = action.payload.items;
      state.pagination = action.payload.metadata;
      state.isLoading = false;
      state.error = null;
    },
    fetchUsersError(state, action: PayloadAction<string>) {
      state.error = action.payload;
      state.isLoading = false;
    },
    setFilter(state, action: PayloadAction<UserFilterModel>) {
      state.filter = action.payload;
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    setFilterWithDebounce(state, action: PayloadAction<UserFilterModel>) {},
  },
});

// Actions
export const userActions = userSlice.actions;

// Reducer
const userReducer = userSlice.reducer;
export default userReducer;