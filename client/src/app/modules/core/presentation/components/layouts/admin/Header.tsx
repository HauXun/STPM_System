import { AppBar, Toolbar, Typography } from '@mui/material';
import AccountMenu from '../../AccountMenu';
import { makeStyles } from '@mui/styles';
import { APPBAR_PADDING_SIZE, DRAWER_WIDTH } from '~/app/modules/shared/constans';

const useStyles = makeStyles((theme) => ({
  appBar: {
    '& .MuiToolbar-root': {
      paddingLeft: 0,
      // '& > *': {
      //   margin: 0,
      // },
    },
  },
}));

type Props = {
  title: string;
};

export default function Header({ title }: Props) {
  const classes = useStyles();

  return (
    <AppBar
      elevation={0}
      className={classes.appBar}
      color="primary"
      position="fixed"
      sx={{
        width: { sm: `calc(100% - ${DRAWER_WIDTH}px - ${APPBAR_PADDING_SIZE}px)` },
        ml: { sm: `calc(${DRAWER_WIDTH}px + ${APPBAR_PADDING_SIZE})` },
        fontFamily: 'Nunito',
      }}
    >
      <Toolbar sx={{ justifyContent: 'space-between' }}>
        <Typography variant="h6" noWrap component="div" sx={{ fontFamily: 'Nunito', fontWeight: 'bold' }}>
          {title}
        </Typography>
        <AccountMenu />
      </Toolbar>
    </AppBar>
  );
}
