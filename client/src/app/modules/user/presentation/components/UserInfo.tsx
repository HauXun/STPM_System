import {
  Avatar,
  Box,
  Button,
  Divider,
  Paper,
  PaperProps,
  SxProps,
  Theme,
  Typography,
} from '@mui/material';
import { useUserDetails } from '~/app/hooks';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';

type Props = PaperProps & {
  userId: string;
};

export default function UserInfo({ sx, userId }: Props) {
  const user = useUserDetails(userId);

  return (
    <Paper
      sx={{
        ...({
          boxShadow: COMPONENT_SHADOW,
          borderRadius: 2,
          ...centerFlexItems({ justify: 'initial' }),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <Box
        sx={{
          width: 'fit-content',
          p: 5,
          rowGap: 2,
          ...centerFlexItems({ direction: 'column' }),
        }}
      >
        <Avatar
          src={user?.imageUrl || 'https://picsum.photos/300/300'}
          sx={{
            width: 70,
            height: 70,
          }}
        />
        <Typography className="whitespace-nowrap text-2xl font-bold leading-6" variant="h6">
          {user?.fullName}
        </Typography>
        <Typography variant="body2" className="text-blue-400">
          {user?.email}
        </Typography>
        <BoxFlexCenter>
          <BoxFlexCenter direction="column" className="mx-4">
            <Typography
              color="var(--primary-green)"
              className="text-xl font-bold leading-6"
              variant="h6"
            >
              {user?.postCount}
            </Typography>
            <Typography variant="body2" className="text-slate-500">
              Bài viết
            </Typography>
          </BoxFlexCenter>
          <Divider orientation="vertical" flexItem />
          <BoxFlexCenter direction="column" className="mx-4">
            <Typography
              color="var(--primary-green)"
              className="text-xl font-bold leading-6"
              variant="h6"
            >
              {user?.commentCount}
            </Typography>
            <Typography variant="body2" className="text-slate-500">
              Bình luận
            </Typography>
          </BoxFlexCenter>
        </BoxFlexCenter>
        <Button
          className="w-full bg-gray-50 py-0.5 text-base normal-case text-gray-600"
          variant="text"
          sx={{ border: '1px solid gray' }}
        >
          Gửi tin nhắn
        </Button>
      </Box>
      <Divider orientation="vertical" flexItem />
      <Box
        sx={{
          display: 'grid',
          gridTemplate: `
                'h1 h2 h3'
                'h4 h4 h4'
                'h5 h6 h7'
                'h8 h8 h8'
                'h9 h10 h10'
              `,
          rowGap: 3,
          width: '100%',
          px: 2,
        }}
      >
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            MSSV
          </Typography>
          <Typography className="text-lg font-semibold">{user?.mssv || 'None'}</Typography>
        </Box>
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Giới tính
          </Typography>
          <Typography className="text-lg font-semibold">Empty</Typography>
        </Box>
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Lớp
          </Typography>
          <Typography className="text-lg font-semibold">{user?.gradeName || 'None'}</Typography>
        </Box>
        <Divider flexItem variant="middle" sx={{ gridArea: 'h4' }} />
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Địa chỉ
          </Typography>
          <Typography className="text-lg font-semibold">Đại học Đà Lạt</Typography>
        </Box>
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Số điện thoại
          </Typography>
          <Typography className="text-lg font-semibold">{user?.phoneNumber}</Typography>
        </Box>
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Trạng thái
          </Typography>
          <Typography className="text-lg font-semibold">
            {!user?.lockEnable ? 'Khoá' : 'Hoạt động'}
          </Typography>
        </Box>
        <Divider flexItem variant="middle" sx={{ gridArea: 'h8' }} />
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Ngày tham gia
          </Typography>
          <Typography className="text-lg font-semibold">
            {user?.joinedDate ? new Date(user.joinedDate).toLocaleDateString() : ''}
          </Typography>
        </Box>
        <Box className="mx-4" sx={{ display: 'flex', flexDirection: 'column', gridArea: 'h10' }}>
          <Typography
            color="var(--primary-green)"
            className="text-lg font-semibold leading-6"
            variant="h6"
          >
            Email
          </Typography>
          <Typography className="text-lg font-semibold">{user?.email}</Typography>
        </Box>
      </Box>
    </Paper>
  );
}
