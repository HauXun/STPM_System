import { IonIcon } from '@ionic/react';
import { CloseRounded, WorkspacePremiumRounded } from '@mui/icons-material';
import {
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
import {
  addCircleOutline,
  checkmarkOutline,
  closeCircleOutline,
  createOutline,
} from 'ionicons/icons';
import { useEffect, useState } from 'react';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import {
  CustomTableHead,
  CustomTableRow,
} from '~/app/modules/shared/presentation/components/CustomTable';
import CustomizedMenu from '~/app/modules/shared/presentation/components/CustomizedMenu';
import DropdownInputEdit from '~/app/modules/shared/presentation/components/DropdownInputEdit';
import LabelInputEdit from '~/app/modules/shared/presentation/components/LabelInputEdit';
import { User } from '~/app/modules/user/domain/models/User';
import { Topic } from '../../domain/models/Topic';

function createData(leader: boolean, fullName: string, mssv: string, gradeName: string) {
  return { leader, fullName, mssv, gradeName } as TopicUserDraft;
}

type TopicUserDraft = Pick<User, 'fullName' | 'mssv' | 'gradeName'> & {
  leader: boolean;
};

const rows: TopicUserDraft[] = [
  createData(true, 'Vroeger Was Alles Beter', '2011379', 'CTK44'),
  createData(false, 'Frozen yoghurt', '2011379', 'CTK45'),
  createData(false, 'Frozen yoghurt', '2011379', 'CTK47'),
];

export type TopicDraftInfoProps = {
  topic: Topic | undefined;
};

export default function TopicRegisterDraft({ topic }: TopicDraftInfoProps) {
  const [topicName, setTopicName] = useState('');
  const [topicRank, setTopicRank] = useState('');
  const [usersDraft, setUsersDraft] = useState<TopicUserDraft[]>(rows);
  const [editUser, setEditUser] = useState<TopicUserDraft>();
  const [fullName, setFullName] = useState('');
  const [mssv, setMssv] = useState('');
  const [gradeName, setGradeName] = useState('');

  useEffect(() => {
    setTopicName(topic?.topicName ?? 'Điều khiển thông minh');
    setTopicRank(topic?.topicRank.rankName ?? 'Hạng mục 2');
  }, [topic]);

  const checkEmpty = (users: TopicUserDraft[]) => {
    return users.filter(
      (u) =>
        (u.fullName && u.mssv && u.gradeName) ||
        (u === editUser && (editUser.fullName && editUser.mssv && editUser.gradeName))
    );
  };

  const updateInfo = () => {
    let replaceIndex = -1;
    const newUsers = usersDraft.filter((u, i) => {
      if (u === editUser) {
        replaceIndex = i;
        return false;
      }

      return true;
    });

    if (replaceIndex < 0) return;

    const newUser: TopicUserDraft = {
      leader: editUser?.leader ?? false,
      fullName,
      mssv,
      gradeName,
    };
    setEditUser(undefined);
    newUsers.splice(replaceIndex, 0, newUser);
    setUsersDraft(checkEmpty(newUsers));
    resetInfo();
  };

  const addNewUserDraft = () => {
    if (usersDraft.length > 2) return;

    const newUser: TopicUserDraft = {
      leader: false,
      fullName: '',
      mssv: '',
      gradeName: '',
    };

    const newUsers = usersDraft;
    newUsers.splice(usersDraft.length - 1, 0, newUser);

    setEditUser(newUser);
    setUsersDraft(newUsers);
  };

  const resetInfo = () => {
    setEditUser(undefined);
    setFullName('');
    setMssv('');
    setGradeName('');
  };

  const updateLeaderUser = (user: TopicUserDraft) => {
    if (user.leader) return;

    const newUsers = usersDraft.map((u, i) => {
      if (u.leader) u.leader = false;

      if (u === user) {
        u.leader = true;
        return u;
      }

      return u;
    });

    setUsersDraft(newUsers);
  };

  return (
    <Paper elevation={0} sx={{ width: 'fit-content', px: 6, py: 8, boxShadow: COMPONENT_SHADOW }}>
      <BoxFlexCenter>
        <Box>
          <Typography variant="h5" className="font-semibold">
            Thông tin đăng ký
          </Typography>
          <Typography variant="subtitle2" className="mt-2 text-sm text-gray-600">
            Together we are and will always be Victorious! Enjoy Psyko Punkz' latest video clip
            "Victorious".
          </Typography>
        </Box>
        <IconButton sx={{ p: '10px', width: 50, height: 50 }}>
          <CloseRounded />
        </IconButton>
      </BoxFlexCenter>
      <Stack spacing={3} sx={{ my: 3 }}>
        <LabelInputEdit onChange={(e) => setTopicName(e.target.value)} value={topicName} />
        <DropdownInputEdit
          onChange={(e) => setTopicRank(e.target.value)}
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
          <IconButton sx={{ ml: 1 }} color="info" onClick={(e) => addNewUserDraft()}>
            <IonIcon icon={addCircleOutline} />
          </IconButton>
        </Typography>
      </Box>

      <TableContainer>
        <Table sx={{ minWidth: 650, my: 2 }} size="small" aria-label="a dense table">
          <CustomTableHead>
            <TableRow>
              <TableCell align="center">Trưởng nhóm</TableCell>
              <TableCell align="left">Họ và tên</TableCell>
              <TableCell align="left">MSSV</TableCell>
              <TableCell align="left">Lớp</TableCell>
              <TableCell align="left"></TableCell>
            </TableRow>
          </CustomTableHead>
          <TableBody>
            {usersDraft.map((user, i) => (
              <CustomTableRow key={i}>
                <TableCell
                  align="center"
                  sx={{
                    cursor: 'pointer',
                    '&:hover .MuiSvgIcon-root': {
                      opacity: user.leader ? 1 : 0.4,
                    },
                  }}
                  onClick={(e) => updateLeaderUser(user)}
                >
                  <WorkspacePremiumRounded
                    sx={{
                      opacity: user.leader ? 1 : 0,
                    }}
                  />
                </TableCell>
                <TableCell component="th">
                  {editUser === user ? (
                    <PrimaryInput
                      value={fullName}
                      className="w-64"
                      placeholder="Nhập tên thành viên"
                      onChange={(e) => setFullName(e.target.value)}
                    />
                  ) : (
                    user.fullName
                  )}
                </TableCell>
                <TableCell align="left">
                  {editUser === user ? (
                    <PrimaryInput
                      value={mssv}
                      className="w-32"
                      placeholder="Nhập mssv"
                      onChange={(e) => setMssv(e.target.value)}
                    />
                  ) : (
                    user.mssv
                  )}
                </TableCell>
                <TableCell align="left">
                  {editUser === user ? (
                    <PrimaryInput
                      value={gradeName}
                      className="w-32"
                      placeholder="Nhập lớp"
                      onChange={(e) => setGradeName(e.target.value)}
                    />
                  ) : (
                    user.gradeName
                  )}
                </TableCell>
                <TableCell align="left">
                  {editUser === user ? (
                    <IconButton
                      color="primary"
                      onClick={(e) => {
                        updateInfo();
                      }}
                    >
                      <IonIcon icon={checkmarkOutline} />
                    </IconButton>
                  ) : (
                    <IconButton
                      color="info"
                      onClick={(e) => {
                        setEditUser(user);
                        setFullName(user.fullName);
                        setMssv(user.mssv);
                        setGradeName(user.gradeName);
                      }}
                    >
                      <IonIcon icon={createOutline} />
                    </IconButton>
                  )}
                  <IconButton
                    color="error"
                    onClick={(e) => {
                      if (editUser && editUser !== user) return;

                      if (editUser) {
                        resetInfo();
                        setUsersDraft(checkEmpty(usersDraft));
                        return;
                      }

                      // remove user
                      const newUsers = usersDraft.filter((u) => u !== user);
                      resetInfo();
                      setUsersDraft(checkEmpty(newUsers));
                    }}
                  >
                    <IonIcon icon={closeCircleOutline} />
                  </IconButton>
                </TableCell>
              </CustomTableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <BoxFlexCenter justifyContent='flex-end' sx={{ mt: 4 }}>
        <CustomizedMenu preText="Xác nhận đăng ký" />
      </BoxFlexCenter>
    </Paper>
  );
}
