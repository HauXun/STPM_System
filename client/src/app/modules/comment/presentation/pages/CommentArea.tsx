import { IonIcon } from '@ionic/react';
import { IconButton, Paper, PaperProps, TextField, Typography } from '@mui/material';
import { createOutline, checkmarkOutline } from 'ionicons/icons';
import { useState } from 'react';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import CommentFormContainer from '../containers/CommentFormContainer';
import CommentListContainer from '../containers/CommentListContainer';

type CommentAreaProps = PaperProps & {};

export default function CommentArea({ sx }: CommentAreaProps) {
  const [mark, setMark] = useState('');
  const [isEditMark, setIsEditMark] = useState(false);

  return (
    <Paper
      elevation={3}
      sx={{
        width: 'fit-content',
        minWidth: 800,
        borderRadius: 3,
        p: 3,
        mx: 'auto !important',
        bgcolor: (theme) => theme.palette.background.default,
        ...sx,
      }}
    >
      <BoxFlexCenter>
        <Typography className="font-bold">Bình luận</Typography>
        <BoxFlexCenter>
          <Typography className="font-bold" component="span" variant="body2">
            Điểm đề tài:
          </Typography>
          {isEditMark ? (
            <TextField
              sx={{
                width: 100,
                backgroundColor: 'white',
                m: 0,
                ml: 1,
                '& .MuiInputBase-input': {
                  py: 1,
                },
              }}
              type="number"
              inputProps={{
                min: 0,
                max: 10,
              }}
              value={mark}
              onChange={(e) => setMark(e.target.value)}
            />
          ) : (
            <Typography className="ml-1" component="span" variant="body2">
              {mark === '' ? 'Chưa chấm điểm' : mark + ' điểm'}
            </Typography>
          )}
          {isEditMark ? (
            <IconButton
              className="text-2xl"
              color="primary"
              onClick={(e) => setIsEditMark(!isEditMark)}
            >
              <IonIcon icon={checkmarkOutline} />
            </IconButton>
          ) : (
            <IconButton
              className="text-2xl"
              color="info"
              onClick={(e) => setIsEditMark(!isEditMark)}
            >
              <IonIcon icon={createOutline} />
            </IconButton>
          )}
        </BoxFlexCenter>
      </BoxFlexCenter>
      <CommentFormContainer />
      <CommentListContainer />
    </Paper>
  );
}
