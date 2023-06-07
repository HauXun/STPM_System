import { Button } from '@mui/material';
import { CSSProperties } from '@mui/styles';

type Props = {
  sx?: CSSProperties;
  text: string;
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
};

export default function PrimaryButton({ sx, text, onClick }: Props) {
  return (
    <Button
      className="font-semibold text-white"
      sx={{ textTransform: 'none', height: '100%', borderRadius: 2, ...sx }}
      variant="contained"
      onClick={onClick}
    >
      {text}
    </Button>
  );
}
