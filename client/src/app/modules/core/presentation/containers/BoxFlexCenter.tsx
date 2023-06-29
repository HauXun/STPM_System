import { Box, BoxProps } from '@mui/material';

export type Props = BoxProps & {
  direction?: 'column' | 'row',
};

export default function BoxFlexCenter({ sx, className, children, direction = 'row', justifyContent = 'space-between', justifyItems = 'initial', alignItems = 'center' }: Props) {
  return (
    <Box
      className={className}
      sx={{
        display: 'flex',
        flexDirection: direction,
        justifyContent,
        justifyItems,
        alignItems,
        ...sx,
      }}
    >
      {children}
    </Box>
  );
}
