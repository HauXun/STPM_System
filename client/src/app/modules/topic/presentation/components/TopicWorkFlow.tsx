import { IonIcon } from '@ionic/react';
import { Avatar, Badge, Box, Chip, IconButton, Paper, PaperProps, Typography } from '@mui/material';
import { addCircleOutline, peopleOutline, personOutline } from 'ionicons/icons';
import { useEffect, useState } from 'react';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { defaultUserService } from '~/app/modules/shared/common';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { User } from '~/app/modules/user/domain/models/User';
import { Topic } from '../../domain/models/Topic';

export type TopicWorkFlowProps = PaperProps & {
  topic: Topic | undefined;
};

export default function TopicWorkFlow({ topic, sx }: TopicWorkFlowProps) {
  const [users, setUsers] = useState<User[]>();

  useEffect(() => {
    (async () => {
      try {
        if (topic && topic.userTopicRatings.length > 0) {
          const usersData = await Promise.all(
            topic.userTopicRatings.map((u) => defaultUserService.getUsersById(u.userId.toString()))
          );
          setUsers(usersData);
        }
      } catch (error) {
        console.log('Failed to fetch user details', error);
      }
    })();
  }, [topic]);
  
  return (
    <Paper
      sx={{
        width: '100%',
        gridArea: 'h3',
        px: 4,
        py: 4,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
        ...sx,
      }}
      elevation={0}
    >
      <Typography sx={{ fontWeight: 600 }}>Tập tin liên quan</Typography>
      <Typography variant="caption">
        Một số thông tin liên quan công tác chấm thi đối với đề tài dự thi.
      </Typography>
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: '50% 1fr',
        }}
      >
        <Box sx={{ display: 'grid', gridTemplateColumns: '32% 1fr', p: 1, pr: 0 }}>
          <BoxFlexCenter justifyContent="initial">
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
          </BoxFlexCenter>
          <BoxFlexCenter justifyContent="initial" sx={{ ml: 5 }}>
            <Avatar
              alt={topic?.leader.fullName}
              src={topic?.leader.imageUrl || 'https://picsum.photos/300/300'}
              sx={{ width: 35, height: 35, mr: 2 }}
            />
            <Box>
              <Typography className="text-base text-gray-600">{topic?.leader.fullName}</Typography>
            </Box>
          </BoxFlexCenter>
        </Box>
        <Box sx={{ display: 'grid', gridTemplateColumns: '40% 1fr', p: 1, pr: 0 }}>
          <BoxFlexCenter justifyContent="initial">
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
          </BoxFlexCenter>
          <BoxFlexCenter
            justifyContent="initial"
            sx={{
              ml: 5,
              '& .MuiAvatar-root:not(:first-of-type)': { ml: 0.2 },
            }}
          >
            {users && users.length > 0 && users.map((user, i) => (
              <Avatar
                key={i}
                alt={user.fullName}
                src={user.imageUrl || 'https://picsum.photos/300/300'}
                sx={{ width: 35, height: 35 }}
              />
            ))}
            <IconButton sx={{ ml: 1 }} color="info">
              <IonIcon icon={addCircleOutline} />
            </IconButton>
          </BoxFlexCenter>
        </Box>
        <Box sx={{ display: 'grid', gridTemplateColumns: '32% 1fr', p: 1, pr: 0 }}>
          <BoxFlexCenter justifyContent="initial">
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
          </BoxFlexCenter>
          <BoxFlexCenter
            justifyContent="initial"
            sx={{
              ml: 5,
              '& .MuiAvatar-root:not(:first-of-type)': { ml: 0.2 },
            }}
          >
            {topic?.users.map((user, i) => (
              <Avatar
                key={i}
                alt={user.fullName}
                src={user.imageUrl || 'https://picsum.photos/300/300'}
                sx={{ width: 35, height: 35 }}
              />
            ))}
          </BoxFlexCenter>
        </Box>
        <Box sx={{ display: 'grid', gridTemplateColumns: '40% 1fr', p: 1, pr: 0 }}>
          <BoxFlexCenter justifyContent="initial">
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
          </BoxFlexCenter>
          <BoxFlexCenter justifyContent="initial" sx={{ ml: 5 }}>
            <Chip
              className="bg-orange-100"
              sx={{ borderRadius: 2, '& .MuiBadge-root': { m: '10px 5px 10px 20px' } }}
              icon={<Badge color="warning" variant="dot" />}
              label="Chưa đánh giá"
            />
          </BoxFlexCenter>
        </Box>
      </Box>
    </Paper>
  );
}
