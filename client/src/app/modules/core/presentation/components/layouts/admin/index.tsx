import { Box } from '@mui/material';
import { ACCOUNT_MENU_SIZE, DRAWER_WIDTH, NAVBAR_PADDING_SIZE } from '~/app/modules/shared/constants';
import { CustomScrollbar } from '../../CustomScrollbar';
import Header from './Header';
import Sidebar from './Sidebar';
import { Outlet } from 'react-router-dom';

type Props = {};

export default function AdminLayout({}: Props) {
  return (
    <Box sx={{ display: 'flex' }}>
      <Header />
      <Sidebar />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          width: { sm: `calc(100% - ${DRAWER_WIDTH}px)` },
          mt: `calc(${ACCOUNT_MENU_SIZE}px + ${NAVBAR_PADDING_SIZE * 2}px)`,
        }}
      >
        <CustomScrollbar
          // eslint-disable-next-line tailwindcss/no-custom-classname
          className="custom-scrollbar"
          autoHide
          autoHideTimeout={1000}
          autoHideDuration={200}
        >
          <Box sx={{ p: 3 }}>
            {/* <Notification /> */}
            {/* <Grid container spacing={2}>
              <Grid item xs={5}>
                <Paper>
                <Typography>Lorem1000</Typography>
                xs=8
                </Paper>
              </Grid>
              <Grid item xs={7}>
                <Paper>
                <Typography>odaijsodi</Typography>
                xs=7
                </Paper>
              </Grid>
            </Grid> */}
            {/* <TopicRegisterList /> */}
            {/* <TopicRegisterDraft /> */}
            {/* <Topic /> */}
            <Outlet />
          </Box>
        </CustomScrollbar>
      </Box>
    </Box>
  );
}
