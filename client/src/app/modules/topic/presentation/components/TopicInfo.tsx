import { IonIcon } from '@ionic/react';
import { WorkspacePremiumRounded } from '@mui/icons-material';
import {
  Avatar,
  Box,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Typography,
} from '@mui/material';
import { addCircleOutline, closeCircleOutline, createOutline } from 'ionicons/icons';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { COMPONENT_SHADOW, TOPIC_CARD_HEIGHT } from '~/app/modules/shared/constants';
import {
  CustomTableHead,
  CustomTableRow,
} from '~/app/modules/shared/presentation/components/CustomTable';
import CustomizedMenu from '~/app/modules/shared/presentation/components/CustomizedMenu';
import DropdownInputEdit from '~/app/modules/shared/presentation/components/DropdownInputEdit';
import LabelInputEdit from '~/app/modules/shared/presentation/components/LabelInputEdit';
import { Topic } from '../../domain/models/Topic';
import { defaultTopicService } from '~/app/modules/shared/common';
import { selectTopicById } from '../../infrastructure/store/selectors';
import { useAppSelector } from '~/app/stores/hooks';

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
      <LabelInputEdit onChange={(e) => setTopicName(e.target.value)} value={topicName} />
      {/* <LabelInputEdit
        onChange={(e) => setTopicName(e.target.value)}
        value={topicName}
        onBlur={(e) => console.log(topicName)}
      /> */}
      <DropdownInputEdit
        onChange={(e) => setTopicRank(e.target.value)}
        value={topicRank}
        list={[
          { id: 1, value: 'Hạng mục 1' },
          { id: 2, value: 'Hạng mục 2' },
        ]}
      />
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
              <TableCell sx={{ px: 0, width: 120 }} align="left">
                Trưởng nhóm
              </TableCell>
              <TableCell sx={{ pl: 6 }} align="left">
                Sinh viên
              </TableCell>
              <TableCell align="left"></TableCell>
            </TableRow>
          </CustomTableHead>
          <TableBody>
            {topic?.users.map((user, i) => (
              <CustomTableRow key={i}>
                <TableCell sx={{ px: 0 }} align="center">
                  {user.id === topic.leaderId ? (
                    <WorkspacePremiumRounded className="text-4xl" />
                  ) : (
                    ''
                  )}
                </TableCell>
                <TableCell component="th" sx={{ width: 310, pl: 6 }}>
                  <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Avatar
                      alt={user.fullName}
                      src={user.imageUrl}
                      sx={{ width: 50, height: 50, mr: 2 }}
                    />
                    <Box>
                      <Typography className="text-gray-600">{user.fullName}</Typography>
                      <Typography className="text-gray-500" sx={{ fontSize: '1rem' }}>
                        {user.mssv} - {user.gradeName}
                      </Typography>
                    </Box>
                  </Box>
                </TableCell>
                <TableCell sx={{ px: 0, width: 80 }} align="left">
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
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 4 }}>
        <CustomizedMenu preText="Lưu" />
      </Box>
    </Paper>
  );
}
