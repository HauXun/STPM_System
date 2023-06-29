import { IonIcon } from '@ionic/react';
import {
  Avatar,
  Box,
  Chip,
  IconButton,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Typography,
} from '@mui/material';
import { ellipsisVerticalOutline, paperPlaneOutline, heartOutline } from 'ionicons/icons';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { Comment } from '../../domain/models/Comment';
import { useEffect, useState } from 'react';
import { defaultUserService } from '~/app/modules/shared/common';

type CommentCardProps = {
  comment: Comment;
  topicId?: string;
};

export default function CommentCard({ comment, topicId }: CommentCardProps) {
  const [markRating, setMarkRating] = useState<number>();
  const { content, user, date, modifiedDate } = comment;

  useEffect(() => {
    (async () => {
      try {
        const data = await defaultUserService.getUserTopicRating(
          user.id.toString(),
          topicId || '-1'
        );
        setMarkRating(data?.mark);
      } catch (error) {
        console.log('Failed to fetch rating details', error);
      }
    })();
  }, [topicId, user.id]);

  return (
    <ListItem
      alignItems="flex-start"
      sx={{
        bgcolor: 'background.paper',
        borderRadius: 3,
        boxShadow: COMPONENT_SHADOW,
      }}
    >
      <ListItemAvatar>
        <Avatar alt="Avatar" src={user.imageUrl || 'https://picsum.photos/300/300'} />
      </ListItemAvatar>
      <ListItemText
        primary={
          <BoxFlexCenter>
            <Typography component="span" variant="body2" className="font-bold">
              {user.fullName}
              {markRating && (
                <Chip
                  className="ml-2 h-6 text-sm"
                  label={`${markRating} điểm`}
                  color="error"
                  variant="outlined"
                />
              )}
            </Typography>
            <Box>
              <Typography variant="caption">
                {modifiedDate
                  ? new Date(modifiedDate).toLocaleString()
                  : new Date(date).toLocaleString()}
              </Typography>
              <IconButton className="text-lg">
                <IonIcon icon={ellipsisVerticalOutline} />
              </IconButton>
            </Box>
          </BoxFlexCenter>
        }
        secondary={
          <Typography component="div">
            <Typography component="span" className="text-base text-gray-500">{content}</Typography>
            <Box
              sx={{
                display: 'flex',
                columnGap: 2,
                mt: 2,
              }}
            >
              <IconButton className="rounded-3xl py-1 text-lg">
                <IonIcon icon={heartOutline} />
                <Typography component="span" className="ml-1 font-bold" variant="caption">
                  Thích
                </Typography>
              </IconButton>
              <IconButton className="rounded-3xl py-1 text-lg">
                <IonIcon icon={paperPlaneOutline} />
                <Typography component="span" className="ml-1 font-bold" variant="caption">
                  Phản hồi
                </Typography>
              </IconButton>
            </Box>
          </Typography>
        }
      />
    </ListItem>
  );
}
