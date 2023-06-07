import { InputBase } from '@mui/material'
import { CSSProperties } from '@mui/styles';

type Props = {
  sx?: CSSProperties;
  value?: string | number;
  onClick?: (event: React.MouseEvent<HTMLDivElement>) => void;
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export default function PrimaryInput({ sx, value, onClick, onChange }: Props) {
  return (
    <InputBase
        className="rounded-md text-lg font-semibold"
        sx={{ flex: 1, mr: 2, border: '1px solid #ccc', px: 2, height: '100%', ...sx }}
        value={value}
        onClick={onClick}
        onChange={onChange}
      />
  )
}