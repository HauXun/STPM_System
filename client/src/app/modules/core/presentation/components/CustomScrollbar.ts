import { styled } from '@mui/material';
import { Scrollbars } from 'react-custom-scrollbars';

export const CustomScrollbar = styled(Scrollbars)(({ theme }) => ({
  '& .trackVertical': {
    backgroundColor: theme.palette.background.paper,
  },
  '& .thumbVertical': {
    backgroundColor: theme.palette.secondary.main,
    borderRadius: 4,
  },
}));
