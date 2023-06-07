import { AppBar, Toolbar, Typography } from '@mui/material';
import { makeStyles } from '@mui/styles';
import { APPBAR_PADDING_SIZE, DRAWER_WIDTH, NAVBAR_PADDING_SIZE } from '~/app/modules/shared/constants';
import AccountMenu from '../../AccountMenu';
import { useGlobalContext } from '~/main/app';

const useStyles = makeStyles((theme) => ({
  appBar: {
    color: 'black',
    backgroundColor: 'transparent',
  },
}));

type Props = {};

export default function Header({ }: Props) {
  const classes = useStyles();
  const { title: adminTitle } = useGlobalContext()

  return (
    <AppBar
      elevation={0}
      className={classes.appBar}
      position="fixed"
      sx={{
        width: { sm: `calc(100% - ${DRAWER_WIDTH}px)` },
        py: `calc(${NAVBAR_PADDING_SIZE}px - ${APPBAR_PADDING_SIZE}px)`
      }}
    >
      <Toolbar sx={{ justifyContent: 'space-between' }}>
        <Typography
          variant="h6"
          noWrap
          component="div"
          sx={{ fontWeight: 'bold' }}
        >
          {adminTitle}
        </Typography>
        <AccountMenu />
      </Toolbar>
    </AppBar>
  );
}
