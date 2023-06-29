import { IonIcon } from '@ionic/react';
import {
  Box,
  Button,
  Card,
  CardContent,
  IconButton,
  Stack,
  SvgIcon,
  Typography,
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import { eyeOffOutline, eyeOutline } from 'ionicons/icons';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';
import {
  SvgGmail,
  SvgShape1,
  SvgShape2,
  SvgShape3,
  SvgShape4,
  SvgShape5,
  SvgShape6,
  SvgShape7,
} from '~/app/modules/shared/common/svg';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';

const useStyles = makeStyles((theme) => ({
  card: {
    background: 'rgba(255, 255, 255, 0.2)',
    boxShadow: '0px 0px 28px -8px rgba(0,0,0,0.75)',
    backdropFilter: 'blur(20px)',
    WebkitBackdropFilter: 'blur(20px)',
    borderRadius: '50%',
    border: '1px solid rgba(255, 255, 255, 0.3)',
    height: 600,
    aspectRatio: '1 / 1',
  },
  cardContainer: {
    width: 'inherit',
    height: 'inherit',
    display: 'grid',
    placeItems: 'center',
    backdropFilter: 'blur(20px)',
    WebkitBackdropFilter: 'blur(20px)',
    background: 'rgba(255, 255, 255, 0.2)',
  },
  cardContent: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
  },
}));

export default function UserSignUpForm() {
  const classes = useStyles();
  const [showPassword, setShowPassword] = useState(false);
  const [showRePassword, setShowRePassword] = useState(false);

  return (
    <Box
      sx={{
        width: '100%',
        height: '100%',
        position: 'relative',
        '& .MuiSvgIcon-root': {
          position: 'absolute',
          transform: 'translate(-50%, -50%)',
          fontSize: '30rem',
        },
      }}
    >
      <SvgIcon sx={{ top: '30%', left: '40%' }}>
        <SvgShape1 />
      </SvgIcon>
      <SvgIcon sx={{ top: '60%', left: '60%' }}>
        <SvgShape2 />
      </SvgIcon>
      <SvgIcon sx={{ top: '90%', left: '90%' }}>
        <SvgShape3 />
      </SvgIcon>
      <SvgIcon sx={{ top: '10%', left: '90%' }}>
        <SvgShape4 />
      </SvgIcon>
      <SvgIcon sx={{ top: '95%', left: '25%' }}>
        <SvgShape5 />
      </SvgIcon>
      <SvgIcon sx={{ top: '2%', left: '15%' }}>
        <SvgShape6 />
      </SvgIcon>
      <SvgIcon sx={{ top: '65%', left: '0%' }}>
        <SvgShape7 />
      </SvgIcon>
      <Box className={`${classes.cardContainer}`}>
        <Card className={`${classes.card}`}>
          <CardContent className={`h-full ${classes.cardContent}`}>
            <Typography variant="h5" component="h2" className="mb-10 font-bold text-slate-600">
              Đăng ký
            </Typography>
            <Stack
              spacing={2}
              sx={{
                '& .MuiInputBase-root': {
                  boxShadow: COMPONENT_SHADOW,
                  py: 0.5,
                  borderRadius: '12px',
                },
              }}
            >
              <PrimaryInput className="mr-0 w-80" placeholder="Tên đăng nhập" />
              <PrimaryInput className="w-80" placeholder="Nhập Email" />
              <PrimaryInput
                className="mr-0"
                endAdornment={
                  <IconButton
                    className="absolute right-0 mr-1"
                    onClick={(e) => setShowPassword(!showPassword)}
                  >
                    <IonIcon icon={showPassword ? eyeOffOutline : eyeOutline} />
                  </IconButton>
                }
                type={showPassword ? 'text' : 'password'}
                placeholder="Nhập mật khẩu"
              />
              <PrimaryInput
                className="mr-0"
                endAdornment={
                  <IconButton
                    className="absolute right-0 mr-1"
                    onClick={(e) => setShowRePassword(!showRePassword)}
                  >
                    <IonIcon icon={showRePassword ? eyeOffOutline : eyeOutline} />
                  </IconButton>
                }
                type={showPassword ? 'text' : 'password'}
                placeholder="Nhập lại mật khẩu"
              />
              <Stack className="mt-8" spacing={1}>
                <Button className="rounded-xl bg-slate-500 normal-case text-white">Đăng ký</Button>
                <Button
                  className="rounded-xl border-slate-500 bg-slate-100 normal-case text-slate-500"
                  startIcon={<SvgGmail />}
                  sx={{
                    border: '1px solid',
                  }}
                >
                  Đăng nhập bằng Gmail
                </Button>
              </Stack>
            </Stack>
            <Typography className="mt-8 text-base text-slate-500">
              Bạn đã có tài khoản?&nbsp;
              <Link className="text-slate-900" to="/sign-in">
                Đăng nhập
              </Link>
            </Typography>
          </CardContent>
        </Card>
      </Box>
    </Box>
  );
}
