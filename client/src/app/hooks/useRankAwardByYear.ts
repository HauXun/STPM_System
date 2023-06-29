import { useEffect, useState } from 'react';
import { RankAward } from '~/app/modules/rankAward/domain/models/RankAward';
import { RankAwardFilterModel } from '~/app/modules/rankAward/domain/models/RankAwardFilterModel';
import { selectRankAwardsByYearRange } from '~/app/modules/rankAward/infrastructure/store/selectors';
import { defaultRankAwardService } from '~/app/modules/shared/common';
import { useAppSelector } from '~/app/stores/hooks';

export const useRankAwardByYear = (year: number) => {
  const selectRankAward = selectRankAwardsByYearRange(Number(year));
  const rankAwardList = useAppSelector(selectRankAward);
  const [rankAwards, setRankAwards] = useState<RankAward[]>([]);

  useEffect(() => {
    if (!year) return;

    if (rankAwardList) {
      setRankAwards(rankAwardList);
      return;
    }

    (async () => {
      try {
        const { items: data } = await defaultRankAwardService.getRankAwards({
          year,
        } as RankAwardFilterModel);
        setRankAwards(data);
      } catch (error) {
        console.log('Failed to fetch rankAwards details', error);
      }
    })();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [year]);

  return rankAwards;
};
