import { Button, ButtonProps } from '@mui/material';

interface Props extends ButtonProps {
  text: string;
}

export default function PrimaryButton({ sx, text, className, ...props }: Props) {
  return (
    <Button
      className={`font-semibold text-white ${className}`}
      sx={{ textTransform: 'none', height: '100%', borderRadius: 2, ...sx }}
      variant="contained"
      {...props}
    >
      {text}
    </Button>
  );
}
