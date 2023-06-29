import { IonIcon } from '@ionic/react';
import { WorkspacePremiumRounded } from '@mui/icons-material';
import {
  Avatar,
  Box,
  IconButton,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Typography,
} from '@mui/material';
import { addCircleOutline, closeCircleOutline, createOutline } from 'ionicons/icons';
import { useEffect, useState } from 'react';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import {
  CustomTableHead,
  CustomTableRow,
} from '~/app/modules/shared/presentation/components/CustomTable';
import CustomizedMenu from '~/app/modules/shared/presentation/components/CustomizedMenu';
import DropdownInputEdit from '~/app/modules/shared/presentation/components/DropdownInputEdit';
import LabelInputEdit from '~/app/modules/shared/presentation/components/LabelInputEdit';
import { Topic } from '../../domain/models/Topic';

export type TopicInfoProps = {
  topic: Topic | undefined;
};

export default function TopicInfo({ topic }: TopicInfoProps) {
  const [topicName, setTopicName] = useState('');
  const [topicRank, setTopicRank] = useState('');

  useEffect(() => {
    setTopicName(topic?.topicName ?? '');
    setTopicRank(topic?.topicRank.rankName ?? '');
  }, [topic]);

  return (
    <Paper
      sx={{
        minWidth: 700,
        height: 'fit-content',
        gridArea: 'h1',
        p: 4,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
      }}
      elevation={0}
    >
      <Stack spacing={3} sx={{ mb: 3 }}>
        <LabelInputEdit onChange={(e) => setTopicName(e.target.value)} value={topicName} />
        <DropdownInputEdit
          // onChange={(e) => setTopicRank(e.target.value)}
          value={topicRank}
          list={[
            { id: 1, value: 'Hạng mục 1' },
            { id: 2, value: 'Hạng mục 2' },
          ]}
        />
      </Stack>
      <Box>
        <Typography className="font-semibold">
          Thành viên
          <IconButton sx={{ ml: 1 }} color="info">
            <IonIcon icon={addCircleOutline} />
          </IconButton>
        </Typography>
      </Box>

      <TableContainer>
        <Table size="small" aria-label="a dense table">
          <CustomTableHead>
            <TableRow>
              <TableCell align="center">Trưởng nhóm</TableCell>
              <TableCell align="left">Sinh viên</TableCell>
              <TableCell align="left"></TableCell>
            </TableRow>
          </CustomTableHead>
          <TableBody>
            {topic?.users.map((user, i) => (
              <CustomTableRow key={i}>
                <TableCell
                  align="center"
                  sx={{
                    cursor: 'pointer',
                    px: 0,
                    '&:hover .MuiSvgIcon-root': {
                      opacity: user.id === topic.leaderId ? 1 : 0.4,
                    },
                  }}
                >
                  <WorkspacePremiumRounded
                    className="text-4xl"
                    sx={{
                      opacity: user.id === topic.leaderId ? 1 : 0,
                    }}
                  />
                </TableCell>
                <TableCell component="th">
                  <BoxFlexCenter justifyContent="initial">
                    <Avatar
                      alt={user.fullName}
                      src={user.imageUrl || 'https://picsum.photos/300/300'}
                      sx={{ width: 50, height: 50, mr: 2 }}
                    />
                    <Box>
                      <Typography className="text-gray-600">{user.fullName}</Typography>
                      <Typography className="text-base text-gray-500">
                        {user.mssv} - {user.gradeName}
                      </Typography>
                    </Box>
                  </BoxFlexCenter>
                </TableCell>
                <TableCell align="left">
                  <IconButton color="info">
                    <IonIcon icon={createOutline} />
                  </IconButton>
                  <IconButton color="error">
                    <IonIcon icon={closeCircleOutline} />
                  </IconButton>
                </TableCell>
              </CustomTableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <BoxFlexCenter justifyContent='flex-end' sx={{ mt: 4 }}>
        <CustomizedMenu preText="Lưu" />
      </BoxFlexCenter>
    </Paper>
  );
}
