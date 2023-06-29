import { useState, useEffect } from 'react';
import { defaultUserService } from '../modules/shared/common';
import { User } from '../modules/user/domain/models/User';
import { selectUserById } from '../modules/user/infrastructure/store/selectors';
import { useAppSelector } from '../stores/hooks';

export const useUserDetails = (userId: string) => {
  const selectUser = selectUserById(userId);
  const userStoreSelect = useAppSelector(selectUser);
  const [user, setUser] = useState<User | undefined>();

  useEffect(() => {
    if (!userId) return;

    if (userStoreSelect) {
      setUser(userStoreSelect);
      return;
    }

    (async () => {
      try {
        const data = await defaultUserService.getUsersById(userId);
        setUser(data);
      } catch (error) {
        console.log('Failed to fetch user details', error);
      }
    })();
  }, [userId, userStoreSelect]);

  return user;
};
