import { Button, ButtonProps } from '@mui/material';

export default function CancelButton({ sx, className, ...props }: ButtonProps) {
  return (
    <Button
      className={`h-full rounded-lg bg-gray-200 font-bold normal-case text-gray-700 ${className}`}
      sx={{ ...sx }}
      {...props}
    >
      Huỷ
    </Button>
  );
}
