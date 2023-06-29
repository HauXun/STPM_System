import { Paper, Typography } from '@mui/material';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { Topic } from '../../domain/models/Topic';

export type TopicAttachmentInfoProps = {
  topic: Topic | undefined;
};

export default function TopicAttachmentInfo({ topic }: TopicAttachmentInfoProps) {
  return (
    <Paper
      sx={{
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        gridArea: 'h2',
        p: 4,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
      }}
      elevation={0}
    >
      <Typography className="font-semibold">Tập tin liên quan</Typography>
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
        <Typography className="font-semibold text-gray-500">Chưa có tập tin nào</Typography>
      </Paper>
    </Paper>
  );
}
