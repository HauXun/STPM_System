import { Paper, Typography } from '@mui/material';
import { COMPONENT_SHADOW, TOPIC_CARD_HEIGHT } from '~/app/modules/shared/constants';
import { Topic } from '../../domain/models/Topic';

export type TopicAttachmentInfoProps = {
  topic: Topic | undefined;
};

export default function TopicAttachmentInfo({ topic }: TopicAttachmentInfoProps) {
  return (
    <Paper
      sx={{
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-between',
        width: '49%',
        height: TOPIC_CARD_HEIGHT,
        px: 4,
        py: 4,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
      }}
    >
      <Typography sx={{ fontWeight: 600 }}>Tập tin liên quan</Typography>
      <Paper
        elevation={0}
        sx={{
          display: 'grid',
          alignContent: 'center',
          justifyContent: 'center',
          mt: 2,
          backgroundColor: '#ddd',
          width: '100%',
          height: '100%',
        }}
      >
        <Typography sx={{ fontWeight: 600, color: 'gray' }}>Chưa có tập tin nào</Typography>
      </Paper>
    </Paper>
  );
}
