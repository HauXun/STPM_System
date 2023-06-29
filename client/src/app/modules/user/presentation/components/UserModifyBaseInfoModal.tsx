import { Paper, Stack, Typography } from '@mui/material';
import PrimaryButton from '~/app/modules/core/presentation/components/PrimaryButton';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';

type Props = {};

export default function UserModifyBaseInfoModal({}: Props) {
  return (
    <Paper
      elevation={5}
      sx={{
        borderRadius: 5,
        width: 'max-content',
        p: 3,
      }}
    >
      <Stack spacing={1} sx={{ mb: 3 }}>
        <Typography className="text-2xl font-bold" variant="body1">
          Thông tin thành viên
        </Typography>
        <Typography className="text-justify" variant="caption">
          Thay đổi thông tin hoặc thêm mới thành viên.
        </Typography>
      </Stack>
      <PrimaryInput className="w-64 rounded-3xl py-1" placeholder="Nhập tên thành viên" />
      <PrimaryInput className="w-32 rounded-3xl py-1" placeholder="Nhập mssv" />
      <PrimaryInput className="w-32 rounded-3xl py-1" placeholder="Nhập lớp" />
      <PrimaryButton className="rounded-3xl" text="Lưu" />
    </Paper>
  );
}
