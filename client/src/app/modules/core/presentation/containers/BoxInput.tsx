import { Box } from '@mui/material';
import { CSSProperties } from '@mui/styles';
import { ReactNode } from 'react';

type Props = {
  sx?: CSSProperties;
  children?: ReactNode;
};

export default function BoxInput({ sx, children }: Props) {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        height: 35,
        ...sx,
      }}
    >
      {children}
    </Box>
  );
}
