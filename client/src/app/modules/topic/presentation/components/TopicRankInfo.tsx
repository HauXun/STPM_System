import NearMeRoundedIcon from '@mui/icons-material/NearMeRounded';
import { Avatar, Paper, PaperProps, SxProps, Theme, Typography } from '@mui/material';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';

type Props = PaperProps & {
  primaryColor: string;
  thumbnailImage?: string;
  topicCount: number;
  rankName: string;
};

export default function TopicRankInfo({ primaryColor, thumbnailImage, topicCount, rankName, sx }: Props) {
  return (
    <Paper
      sx={{
        ...({
          width: 300,
          borderRadius: 2,
          p: 2,
          boxShadow: COMPONENT_SHADOW,
          ...centerFlexItems({ direction: 'column', align: 'initial' }),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <BoxFlexCenter direction="column">
        <Typography className="font-bold" color={primaryColor}>
          {rankName}
        </Typography>
        <Avatar
          alt="rank-award"
          src={thumbnailImage ?? ''}
          sx={{ width: 200, height: 200, p: 5 }}
        />
      </BoxFlexCenter>
      <BoxFlexCenter>
        <BoxFlexCenter sx={{ columnGap: 2 }}>
          <NearMeRoundedIcon />
          <Typography className="text-base" variant="caption">
            Số lượng tham gia
          </Typography>
        </BoxFlexCenter>
        <Typography className="font-bold" variant="h5">
          {topicCount}
        </Typography>
      </BoxFlexCenter>
    </Paper>
  );
}
