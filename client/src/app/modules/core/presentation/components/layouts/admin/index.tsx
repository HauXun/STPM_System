import { Box, Grid, Paper, Typography } from '@mui/material';
import { CustomScrollbar } from '../../CustomScrollbar';
import { APPBAR_SIZE, DRAWER_WIDTH } from '~/app/modules/shared/constans';
import Header from './Header';
import Sidebar from './Sidebar';
import { styled } from '@mui/material/styles';
import Notification from '../../Notification';
import TopicList from '../../TopicList';

type Props = {};

export default function AdminLayout({}: Props) {
  return (
    <Box sx={{ display: 'flex' }}>
      <Header title='Danh sách đăng ký' />
      <Sidebar />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          width: { sm: `calc(100% - ${DRAWER_WIDTH}px)` },
          mt: `${APPBAR_SIZE}px`,
        }}
      >
        <CustomScrollbar
          className="custom-scrollbar "
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
            <TopicList />
          </Box>
        </CustomScrollbar>
      </Box>
    </Box>
  );
}
