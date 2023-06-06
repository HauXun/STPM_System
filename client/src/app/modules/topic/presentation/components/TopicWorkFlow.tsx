import { IonIcon } from '@ionic/react';
import { Avatar, Badge, Box, Chip, IconButton, Paper, Typography } from '@mui/material';
import { addCircleOutline, peopleOutline, personOutline } from 'ionicons/icons';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { Topic } from '../../domain/models/Topic';

export type TopicWorkFlowProps = {
  topic: Topic | undefined;
};

export default function TopicWorkFlow({ topic }: TopicWorkFlowProps) {
  return (
    <Paper
      sx={{
        width: '100%',
        px: 4,
        py: 4,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
      }}
    >
      <Typography sx={{ fontWeight: 600 }}>Tập tin liên quan</Typography>
      <Typography variant="caption">
        Lorem ipsum dolor sit amet consectetur adipisicing elit. Alias, dolore.
      </Typography>
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: '40% 1fr',
        }}
      >
        <Box sx={{ display: 'grid', gridTemplateColumns: '30% 1fr', p: 1, pr: 0 }}>
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <IonIcon className="text-2xl" icon={personOutline} />
            <Typography
              className="text-base text-gray-600"
              variant="body2"
              sx={{
                ml: 1.5,
              }}
            >
              Trưởng nhóm
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', ml: 5 }}>
            <Avatar
              alt={topic?.leader.fullName}
              src={topic?.leader.imageUrl}
              sx={{ width: 35, height: 35, mr: 2 }}
            />
            <Box>
              <Typography className="text-base text-gray-600">{topic?.leader.fullName}</Typography>
            </Box>
          </Box>
        </Box>
        <Box sx={{ display: 'grid', gridTemplateColumns: '30% 1fr', p: 1, pr: 0 }}>
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <IonIcon className="text-2xl" icon={peopleOutline} />
            <Typography
              className="text-base text-gray-600"
              variant="body2"
              sx={{
                ml: 1.5,
              }}
            >
              Chỉ định chấm thi
            </Typography>
          </Box>
          <Box
            sx={{
              display: 'flex',
              alignItems: 'center',
              ml: 5,
              '& .MuiAvatar-root:not(:first-of-type)': { ml: 0.2 },
            }}
          >
            {topic?.userTopicRatings.map((user, i) => (
              <Avatar key={i} alt={user.fullName} src={user.imageUrl} sx={{ width: 35, height: 35 }} />
            ))}
            <IconButton sx={{ ml: 1 }} color="info">
              <IonIcon icon={addCircleOutline} />
            </IconButton>
          </Box>
        </Box>
        <Box sx={{ display: 'grid', gridTemplateColumns: '30% 1fr', p: 1, pr: 0 }}>
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <IonIcon className="text-2xl" icon={peopleOutline} />
            <Typography
              className="text-base text-gray-600"
              variant="body2"
              sx={{
                ml: 1.5,
              }}
            >
              Nhóm
            </Typography>
          </Box>
          <Box
            sx={{
              display: 'flex',
              alignItems: 'center',
              ml: 5,
              '& .MuiAvatar-root:not(:first-of-type)': { ml: 0.2 },
            }}
          >
            {topic?.users.map((user, i) => (
              <Avatar key={i} alt={user.fullName} src={user.imageUrl} sx={{ width: 35, height: 35 }} />
            ))}
          </Box>
        </Box>
        <Box sx={{ display: 'grid', gridTemplateColumns: '30% 1fr', p: 1, pr: 0 }}>
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <IonIcon className="text-2xl" icon={personOutline} />
            <Typography
              className="text-base text-gray-600"
              variant="body2"
              sx={{
                ml: 1.5,
              }}
            >
              Tổng điểm
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', ml: 5 }}>
            <Chip
              className="bg-orange-100"
              sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
              icon={<Badge color="warning" variant="dot" />}
              label="Chưa đánh giá"
            />
          </Box>
        </Box>
      </Box>
    </Paper>
  );
}
