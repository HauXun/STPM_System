import { IonIcon } from '@ionic/react';
import { Avatar, IconButton, InputBase, Paper } from '@mui/material';
import { sendOutline } from 'ionicons/icons';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';

type Props = {};

export default function CommentForm({}: Props) {
  return (
    <BoxFlexCenter
      sx={{
        p: 2,
      }}
    >
      <Avatar
        alt="Remy Sharp"
        src="https://picsum.photos/300/300"
        sx={{ width: 60, height: 60, mr: 2, border: '2px solid var(--primary-green)' }}
      />
      <Paper
        component="form"
        sx={{
          ...centerFlexItems({}),
          borderRadius: 50,
          boxShadow: COMPONENT_SHADOW,
          px: 2,
          py: 0.5,
          flex: 1,
        }}
      >
        <InputBase
          className="text-base"
          sx={{ ml: 1, flex: 1 }}
          placeholder="Viết bình luận"
          inputProps={{ 'aria-label': 'bình luận' }}
        />
        <IconButton className="text-2xl" aria-label="menu">
          <IonIcon icon={sendOutline} />
        </IconButton>
      </Paper>
    </BoxFlexCenter>
  );
}
