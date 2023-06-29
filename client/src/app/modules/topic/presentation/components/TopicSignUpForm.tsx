import {
  Box,
  Button,
  Card,
  CardContent,
  FormControl,
  InputLabel,
  MenuItem,
  OutlinedInput,
  Select,
  SelectChangeEvent,
  Stack,
  SvgIcon,
  Typography,
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import { useState } from 'react';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';
import {
  SvgShape1,
  SvgShape2,
  SvgShape3,
  SvgShape4,
  SvgShape6,
  SvgShape7,
} from '~/app/modules/shared/common/svg';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import Character1 from '~/main/assets/Character1.png';
import TitleLogo from '~/main/assets/TitleLogo.png';
import { chevronDownOutline } from 'ionicons/icons';
import { IonIcon } from '@ionic/react';

const useStyles = makeStyles((theme) => ({
  card: {
    background: 'rgba(255, 255, 255, 0.2)',
    boxShadow: '0px 0px 28px -8px rgba(0,0,0,0.75)',
    backdropFilter: 'blur(20px)',
    WebkitBackdropFilter: 'blur(20px)',
    borderRadius: 20,
    border: '1px solid rgba(255, 255, 255, 0.3)',
    width: 'fit-content',
    height: 'fit-content',
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

export default function TopicSignUpForm() {
  const [rank, setRank] = useState('');

  const handleChange = (event: SelectChangeEvent) => {
    setRank(event.target.value);
  };

  const classes = useStyles();

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
      <SvgIcon sx={{ top: '2%', left: '15%' }}>
        <SvgShape6 />
      </SvgIcon>
      <SvgIcon sx={{ top: '65%', left: '0%' }}>
        <SvgShape7 />
      </SvgIcon>
      <Box className={`${classes.cardContainer}`}>
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: 'auto auto',
            columnGap: 10,
            height: 'fit-content',
            alignItems: 'center',
          }}
        >
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              justifyContent: 'flex-end',
              alignItems: 'center',
              height: '100%',
              '& img': {
                objectFit: 'contain',
              },
            }}
          >
            <Box component="img" src={TitleLogo} sx={{ width: 300 }} />
            <Box component="img" src={Character1} sx={{ width: 450 }} />
          </Box>
          <Card className={`px-8 py-10 ${classes.card}`}>
            <CardContent className={`h-full ${classes.cardContent}`}>
              <Typography variant="h5" component="h2" className="mb-10 font-bold text-slate-600">
                Đăng ký đề tài
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
                <FormControl sx={{ flexDirection: 'row', alignItems: 'center', columnGap: 3 }}>
                  <Typography className="text-slate-700" component="label">
                    Đề tài
                  </Typography>
                  <PrimaryInput name="username" className="mr-0 w-80" placeholder="Tên đăng nhập" />
                </FormControl>
                <Typography className="text-slate-700" component="label">
                  Thông tin tham gia
                </Typography>
                <Stack className="mt-8" spacing={1}>
                  <Box>
                    <FormControl sx={{ minWidth: 300 }} size="small">
                      <InputLabel className='text-base'>Hạng mục tham gia</InputLabel>
                      <Select
                        value={rank}
                        onChange={handleChange}
                        input={<OutlinedInput label="Hạng mục tham gia" />}
                        IconComponent={() => <IonIcon icon={chevronDownOutline} className='pointer-events-none absolute right-5' />}
                        className='bg-white py-0'
                      >
                        <MenuItem value={1}>Hạng mục 1</MenuItem>s
                        <MenuItem value={2}>Hạng mục 2</MenuItem>
                      </Select>
                    </FormControl>
                  </Box>
                  <Typography
                    className="font-semibold text-sky-950"
                    component="label"
                    variant="body2"
                  >
                    Trưởng nhóm
                  </Typography>
                  <Box sx={{ display: 'grid', gridTemplateColumns: '250px 150px 150px' }}>
                    <PrimaryInput className="rounded-xl py-1" placeholder="Nhập tên trưởng nhóm" />
                    <PrimaryInput className="rounded-xl py-1" placeholder="Nhập mssv" />
                    <PrimaryInput className="mr-0 rounded-xl py-1" placeholder="Nhập lớp" />
                  </Box>
                </Stack>
                <Stack spacing={1}>
                  <Typography
                    className="font-semibold text-sky-950"
                    component="label"
                    variant="body2"
                  >
                    Thành viên
                  </Typography>
                  <Box sx={{ display: 'grid', gridTemplateColumns: '250px 150px 150px' }}>
                    <PrimaryInput className="rounded-xl py-1" placeholder="Nhập tên thành viên" />
                    <PrimaryInput className="rounded-xl py-1" placeholder="Nhập mssv" />
                    <PrimaryInput className="mr-0 rounded-xl py-1" placeholder="Nhập lớp" />
                  </Box>
                  <Box sx={{ display: 'grid', gridTemplateColumns: '250px 150px 150px' }}>
                    <PrimaryInput className="rounded-xl py-1" placeholder="Nhập tên thành viên" />
                    <PrimaryInput className="rounded-xl py-1" placeholder="Nhập mssv" />
                    <PrimaryInput className="mr-0 rounded-xl py-1" placeholder="Nhập lớp" />
                  </Box>
                </Stack>
              </Stack>
              <Button className="mt-20 rounded-2xl bg-slate-500 px-10 font-semibold normal-case text-white">
                Gửi thông tin đăng ký
              </Button>
            </CardContent>
          </Card>
        </Box>
      </Box>
    </Box>
  );
}
