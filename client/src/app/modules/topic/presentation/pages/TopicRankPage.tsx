import { Box, Stack, Typography } from '@mui/material';
import { Fragment, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useRankAwardByYear } from '~/app/hooks/useRankAwardByYear';
import { RankAwardFilterModel } from '~/app/modules/rankAward/domain/models/RankAwardFilterModel';
import { RankAwardItem } from '~/app/modules/rankAward/domain/models/RankAwardItem';
import { rankAwardActions } from '~/app/modules/rankAward/infrastructure/store/rankAwardSlice';
import { defaultTopicService } from '~/app/modules/shared/common';
import { useAppDispatch } from '~/app/stores/hooks';
import { useGlobalContext } from '~/main/app';
import RankAward_Type1 from '~/main/assets/RankAward_Type1.png';
import RankAward_Type2 from '~/main/assets/RankAward_Type2.png';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import TopicRankChartContainer from '../containers/TopicRankChartContainer';
import TopicRankDetailContainer from '../containers/TopicRankDetailContainer';
import TopicRankInfoContainer from '../containers/TopicRankInfoContainer';
import TopicRankYearContainer from '../containers/TopicRankYearContainer';

const thumbnailDetailImages = [
  {
    path: RankAward_Type1,
    color: 'var(--primary-green)',
  },
  {
    path: RankAward_Type2,
    color: 'orange',
  },
];

export default function TopicRankPage() {
  const { year: yearParam } = useParams<{ year: string }>();
  const { setTitle } = useGlobalContext();

  const dispatch = useAppDispatch();
  const rankAwardsByYear = useRankAwardByYear(Number(yearParam));

  const [rankAwards, setRankAwards] = useState<RankAwardItem[]>([]);

  useEffect(() => {
    (async () => {
      try {
        if (rankAwardsByYear && rankAwardsByYear.length > 0) {
          const topicRanks = [...new Set(rankAwardsByYear.map((rank) => rank.topicRankId))];
          const rankAwards = await Promise.all(
            topicRanks.map(async (t) => {
              const { metadata } = await defaultTopicService.getTopics({
                topicRankId: t,
              } as TopicFilterModel);
              const rankAward = rankAwardsByYear.filter((r) => r.topicRankId === t);
              const topicRankName =
                rankAward.find((r) => r.topicRankId === t)?.topicRank.rankName || '';
              return {
                topicCount: metadata.totalItemCount,
                rankName: topicRankName,
                rankAward: rankAward,
              };
            })
          );
          setRankAwards(rankAwards);
        }
      } catch (error) {
        console.log('Failed to fetch rank award details', error);
      }
    })();
  }, [rankAwardsByYear]);

  useEffect(() => {
    setTitle('Hạng mục');
    dispatch(rankAwardActions.fetchRankAwards({} as RankAwardFilterModel));
  }, [setTitle, dispatch]);

  return (
    <Stack spacing={5}>
      <TopicRankYearContainer />
      <TopicRankChartContainer />
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: 'auto 1fr',
          gap: 4,
        }}
      >
        <Typography className="ml-10 text-xl font-bold" sx={{ mb: -2 }}>
          Hạng mục
        </Typography>
        <Typography className="ml-10 text-xl font-bold" sx={{ mb: -2 }}>
          Giải thưởng
        </Typography>
        {rankAwards &&
          rankAwards.length > 0 &&
          rankAwards.map((rankAward, i) => {
            const { path, color } = thumbnailDetailImages[i];
            const { topicCount, rankName, rankAward: rankAwardList } = rankAward;
            return (
              <Fragment key={i}>
                <TopicRankInfoContainer
                  topicCount={topicCount}
                  rankName={rankName}
                  thumbnailImage={path}
                  primaryColor={color}
                />
                <TopicRankDetailContainer rankAwardList={rankAwardList} primaryColor={color} />
              </Fragment>
            );
          })}
      </Box>
    </Stack>
  );
}
