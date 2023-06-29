import { AppBar, Box, IconButton, Toolbar, Stack, Badge } from '@mui/material';
import { makeStyles } from '@mui/styles';
import {
  APPBAR_PADDING_SIZE,
  NAVBAR_PADDING_SIZE,
  PRIMARY_APP_WIDTH,
} from '~/app/modules/shared/constants';
import AccountMenu from '../AccountMenu';
import { NavLink } from 'react-router-dom';
import { notificationsOutline } from 'ionicons/icons';
import { IonIcon } from '@ionic/react';
import { PostRoutes } from '~/app/modules/post/presentation/routes';
import { TopicRoutes } from '~/app/modules/topic/presentation/routes';

const useStyles = makeStyles((theme) => ({
  appBar: {
    color: 'black',
    backgroundColor: theme.palette.background.default,
  },
}));

type Props = {};

export default function Header({}: Props) {
  const classes = useStyles();

  return (
    <AppBar
      elevation={0}
      className={classes.appBar}
      position="fixed"
      sx={{
        width: { sm: `100%` },
        py: `calc(${NAVBAR_PADDING_SIZE}px - ${APPBAR_PADDING_SIZE}px)`,
        '& .MuiToolbar-root': {
          width: { sm: `${PRIMARY_APP_WIDTH}px` },
          mx: 'auto',
        },
      }}
    >
      <Toolbar sx={{ justifyContent: 'space-between' }}>
        <Box
          component="nav"
          sx={{
            '& a': {
              py: 1.5,
              px: 2,
              fontSize: '1.1rem',
              fontWeight: 'bold',
              '&:not(:last-child)': {
                mr: 1,
              },
            },
          }}
        >
          <NavLink to={`/`}>Trang chủ</NavLink>
          <NavLink to={`/`}>Giới thiệu</NavLink>
          <NavLink to={`/${PostRoutes.POSTS}`}>Bài viết</NavLink>
          <NavLink to={`/${TopicRoutes.TOPICS}`}>Đề tài</NavLink>
          <NavLink to={`/`}>Hỗ trợ</NavLink>
          <NavLink to={`/`}>Tin tức</NavLink>
        </Box>
        <Stack direction="row" spacing={2}>
          <IconButton>
            <Badge badgeContent={4} color="error">
              <IonIcon icon={notificationsOutline} />
            </Badge>
          </IconButton>
          <AccountMenu />
        </Stack>
      </Toolbar>
    </AppBar>
  );
}
