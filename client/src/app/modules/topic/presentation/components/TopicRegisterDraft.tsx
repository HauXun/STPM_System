import { IonIcon } from '@ionic/react';
import {
  CloseRounded,
  WorkspacePremiumRounded
} from '@mui/icons-material';
import {
  Box,
  Button,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Typography
} from '@mui/material';
import { addCircleOutline, closeCircleOutline, createOutline } from 'ionicons/icons';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { CustomTableHead, CustomTableRow } from '~/app/modules/shared/presentation/components/CustomTable';
import CustomizedMenu from '~/app/modules/shared/presentation/components/CustomizedMenu';

function createData(leader: boolean, name: string, mssv: string, className: string) {
  return { leader, name, mssv, className };
}

const rows = [
  createData(true, 'Frozen yoghurt', '2011379', 'CTK44'),
  createData(false, 'Frozen yoghurt', '2011379', 'CTK45'),
  createData(false, 'Frozen yoghurt', '2011379', 'CTK47'),
];

export default function TopicRegisterDraft() {
  return (
    <Paper elevation={0} sx={{ width: 800, px: 6, py: 8, boxShadow: COMPONENT_SHADOW }}>
      <Box
        sx={{
          display: 'flex',
          width: '100%',
          justifyContent: 'space-between',
          alignItems: 'center',
        }}
      >
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
      </Box>
      <Box
        sx={{
          mt: 5,
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          maxWidth: 600,
        }}
      >
        <Typography sx={{ fontWeight: 600 }}>What the hell?</Typography>
        {/* <InputBase className="font-semibold" sx={{ ml: 1, flex: 1 }} value="What the hell?" /> */}
        <Button
          className="font-semibold text-white"
          sx={{ textTransform: 'none', height: 30, borderRadius: 2 }}
          variant="contained"
        >
          Sửa
        </Button>
      </Box>
      <Box
        sx={{
          my: 2,
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          maxWidth: 600,
        }}
      >
        <Typography sx={{ fontWeight: 600 }}>What the hell?</Typography>
        {/* <InputBase className="font-semibold" sx={{ ml: 1, flex: 1 }} value="What the hell?" /> */}
        <Button
          className="font-semibold text-white"
          sx={{ textTransform: 'none', height: 30, borderRadius: 2 }}
          variant="contained"
        >
          Sửa
        </Button>
      </Box>
      <Box>
        <Typography className="font-semibold">
          Thành viên
          <IconButton sx={{ ml: 1 }} color="info">
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
            {rows.map((row, i) => (
              <CustomTableRow key={i}>
                <TableCell align="center">
                  {row.leader ? <WorkspacePremiumRounded /> : ''}
                </TableCell>
                <TableCell component="th" sx={{ width: 250 }}>
                  {row.name}
                </TableCell>
                <TableCell align="left">{row.mssv}</TableCell>
                <TableCell align="left">{row.className}</TableCell>
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
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 4 }}>
        <CustomizedMenu preText="Xác nhận đăng ký" />
      </Box>
    </Paper>
  );
}
