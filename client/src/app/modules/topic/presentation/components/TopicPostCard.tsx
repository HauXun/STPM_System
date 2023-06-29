import { IonIcon } from '@ionic/react';
import { Avatar, Box, IconButton, Paper, Typography } from '@mui/material';
import { trashOutline } from 'ionicons/icons';
import { Link } from 'react-router-dom';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { UserRoutes } from '~/app/modules/user/presentation/routes';
import { Topic } from '../../domain/models/Topic';
import { TopicRoutes } from '../routes';

type Props = {
  topic: Topic;
};

export default function TopicPostCard({ topic }: Props) {
  const { id, topicName, shortDescription, leader, users, topicPhotos } = topic;

  return (
    <Paper
      elevation={0}
      sx={{
        p: 3,
        borderRadius: 3,
        boxShadow: COMPONENT_SHADOW,
      }}
    >
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: 'auto 1fr auto',
          columnGap: 3,
        }}
      >
        <Box
          component={Link}
          to={`/${TopicRoutes.TOPICS}/${id || ''}`}
          sx={{
            width: 280,
            aspectRatio: '3.2 / 2',
            borderRadius: 3,
            boxShadow: COMPONENT_SHADOW,
            overflow: 'hidden',
          }}
        >
          <Box
            component="img"
            sx={{ width: '100%', height: '100%' }}
            src={
              topicPhotos[0]
                ? topicPhotos[0].imageUrl
                : 'https://q-dance-network-images.akamaized.net/41347/1666098985-0001_20220625165639_kevin_453a5245-edit.jpg?auto=compress&fit=max?w=1200'
            }
          />
        </Box>
        <BoxFlexCenter direction="column" alignItems="initial">
          <Box component={Link} to={`/${TopicRoutes.TOPICS}/${id || ''}`}>
            <Typography className="text-xl font-bold" variant="body1">
              {topicName}
            </Typography>
            <Typography
              className="text-justify"
              variant="body2"
              sx={{
                WebkitLineClamp: 4,
                overflow: 'hidden',
                display: '-webkit-box',
                WebkitBoxOrient: 'vertical',
              }}
            >
              {shortDescription}
            </Typography>
          </Box>
          <BoxFlexCenter justifyContent="initial">
            {users &&
              users.length > 0 &&
              [...users, leader].map((user, i) => (
                <Link to={`/${UserRoutes.USERS}/${user.id}`} key={i}>
                  <Avatar
                    alt="Avatar"
                    src={user.imageUrl || 'https://picsum.photos/300/300'}
                    sx={{
                      width: 35,
                      height: 35,
                      mr: -0.8,
                      border: '2px solid white',
                      cursor: 'pointer',
                    }}
                  />
                </Link>
              ))}
          </BoxFlexCenter>
        </BoxFlexCenter>
        <Box
          sx={{
            display: 'grid', // none
            placeItems: 'center',
          }}
        >
          <IconButton color="primary">
            <IonIcon icon={trashOutline} />
          </IconButton>
        </Box>
      </Box>
    </Paper>
  );
}
