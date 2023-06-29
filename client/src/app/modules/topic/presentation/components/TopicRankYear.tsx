import { ArrowBackRounded, ArrowForwardRounded } from '@mui/icons-material/';
import { Button, IconButton, Paper, PaperProps, SxProps, Theme } from '@mui/material';
import { useEffect, useState } from 'react';
import { Link, redirect, useParams } from 'react-router-dom';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { RankAward } from '~/app/modules/rankAward/domain/models/RankAward';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';
import { TopicRoutes } from '../routes';

type Props = PaperProps & {
  rankAwardList: RankAward[];
};

export default function TopicRankYear({ rankAwardList, sx }: Props) {
  const { year: yearParam } = useParams<{ year: string }>();
  const [years, setYears] = useState<number[]>([]);
  
  useEffect(() => {
    const newList = [
      ...new Set(
        rankAwardList.flatMap((award) => award.specificAwards.map((sAward) => sAward.year))
      ),
    ];
    setYears(newList);
  }, [rankAwardList]);

  if (!yearParam && years && years.length > 0) {
    const currentYear = years.pop();
    return redirect(
      `/admin/${TopicRoutes.TOPICS}/${TopicRoutes.RANKS}/${currentYear ?? new Date().getFullYear()}`
    );
  }

  const handleBackClick = (event: React.MouseEvent<unknown>) => {
    console.log('handleClick', event);
  };
  const handleForwardClick = (event: React.MouseEvent<unknown>) => {
    console.log('handleClick', event);
  };
  const handleYearClick = (event: React.MouseEvent<unknown>) => {
    console.log('handleClick', event);
  };

  return (
    <Paper
      sx={{
        ...({
          width: '100%',
          borderRadius: 2,
          p: 2,
          boxShadow: COMPONENT_SHADOW,
          ...centerFlexItems({}),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <IconButton onClick={handleBackClick} size="small">
        <ArrowBackRounded />
      </IconButton>
      <BoxFlexCenter
        sx={{
          width: 'inherit',
        }}
      >
        {years &&
          years.length > 0 &&
          years.map((year, i) => (
            <Link key={i} to={`/admin/${TopicRoutes.TOPICS}/${TopicRoutes.RANKS}/${year}`}>
              <Button
                className="mx-2 w-20 font-semibold"
                sx={{ borderRadius: 2 }}
                variant={`${year === Number(yearParam) ? 'contained' : 'text'}`}
                onClick={handleYearClick}
              >
                {year}
              </Button>
            </Link>
          ))}
      </BoxFlexCenter>
      <IconButton onClick={handleForwardClick} size="small">
        <ArrowForwardRounded />
      </IconButton>
    </Paper>
  );
}
