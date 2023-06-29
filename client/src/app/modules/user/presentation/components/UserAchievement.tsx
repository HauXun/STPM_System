import { Box, Paper, PaperProps, SxProps, Theme, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { CustomScrollbar } from '~/app/modules/core/presentation/components/CustomScrollbar';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { RankAward } from '~/app/modules/rankAward/domain/models/RankAward';
import { defaultRankAwardService } from '~/app/modules/shared/common';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';
import { Topic } from '~/app/modules/topic/domain/models/Topic';
import { TopicFilterModel } from '~/app/modules/topic/domain/models/TopicFilterModel';
import { TopicState } from '~/app/modules/topic/infrastructure/store/types';

type Props = PaperProps & {
  userId: string;
  topicState: TopicState;
  fetchTopics: (model: TopicFilterModel) => void;
  setTopicFilter: (model: TopicFilterModel) => void;
};

export default function UserAchievement({
  sx,
  userId,
  topicState,
  fetchTopics,
  setTopicFilter,
}: Props) {
  const [awards, setAwards] = useState<
    {
      topic: Topic | undefined;
      rankAward: RankAward;
    }[]
  >([]);
  const { data: topicData, filter: topicFilter } = topicState;

  useEffect(() => {
    fetchTopics(topicFilter);
  }, [fetchTopics, topicFilter]);

  useEffect(() => {
    setTopicFilter({ userId: Number(userId) } as TopicFilterModel);
  }, [setTopicFilter, userId]);

  useEffect(() => {
    (async () => {
      try {
        if (topicData && topicData.length > 0) {
          const awardGiven = topicData.map((topic) => topic.specificAward.rankAwardId);
          const getAwards = await Promise.all(
            awardGiven.map(async (a) => {
              const rankAward = await defaultRankAwardService.getRankAwardsById(a.toString());
              const topic = topicData.find((topic) => topic.specificAward.rankAwardId === a);
              return {
                topic: topic,
                rankAward: rankAward,
              };
            })
          );

          setAwards(getAwards);
        }
      } catch (error) {
        console.log('Failed to fetch user details', error);
      }
    })();
  }, [topicData]);

  return (
    <Paper
      sx={{
        ...({
          p: 2,
          borderRadius: 2,
          boxShadow: COMPONENT_SHADOW,
          ...centerFlexItems({ direction: 'column', align: 'initial' }),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <Typography color="var(--primary-green)" className="mb-5 font-semibold">
        Thành tựu
      </Typography>
      {awards && awards.length > 0 ? (
        <CustomScrollbar
          // eslint-disable-next-line tailwindcss/no-custom-classname
          className="custom-scrollbar"
          autoHide
          autoHideTimeout={1000}
          autoHideDuration={200}
        >
          {awards.map((award, i) => {
            const { topic, rankAward } = award;
            return (
              <Box
                key={i}
                sx={{
                  display: 'grid',
                  placeItems: 'center',
                  width: 'inherit',
                  columnGap: 2,
                  gridTemplateColumns: 'auto 1fr',
                  '&:not(:first-of-type)': {
                    mt: 2,
                  },
                }}
              >
                <Box
                  component="img"
                  sx={{
                    width: 60,
                    aspectRatio: '1 / 1',
                    borderRadius: 2,
                  }}
                  src={
                    topic?.topicPhotos[0]
                      ? topic?.topicPhotos[0].imageUrl
                      : 'https://q-dance-network-images.akamaized.net/41347/1666098985-0001_20220625165639_kevin_453a5245-edit.jpg?auto=compress&fit=max?w=1200'
                  }
                />
                <BoxFlexCenter
                  direction="column"
                  alignItems="flex-start"
                  sx={{
                    width: '100%',
                    '& .MuiTypography-root': {
                      WebkitLineClamp: 1,
                      overflow: 'hidden',
                      display: '-webkit-box',
                      WebkitBoxOrient: 'vertical',
                    },
                  }}
                >
                  <Typography className="text-xl font-bold" variant="body1">
                    {topic?.topicName}
                  </Typography>
                  <Typography
                    className="text-blue-500"
                    variant="body2"
                  >{`• ${rankAward.awardName} - # ${topic?.topicRank.rankName}`}</Typography>
                </BoxFlexCenter>
              </Box>
            );
          })}
        </CustomScrollbar>
      ) : (
        <Paper
          elevation={0}
          sx={{
            display: 'grid',
            alignContent: 'center',
            justifyContent: 'center',
            backgroundColor: '#ebebeb',
            width: '100%',
            height: '100%',
          }}
        >
          <Typography className="font-semibold text-gray-400">Chưa có thành tựu nào</Typography>
        </Paper>
      )}
    </Paper>
  );
}
