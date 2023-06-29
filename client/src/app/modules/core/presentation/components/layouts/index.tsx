import { Box } from '@mui/material';
import { Outlet } from 'react-router-dom';
import {
  ACCOUNT_MENU_SIZE,
  NAVBAR_PADDING_SIZE,
  PRIMARY_APP_WIDTH
} from '~/app/modules/shared/constants';
import { CustomScrollbar } from '../CustomScrollbar';
import Header from './Header';

type Props = {};

export default function AppLayout({}: Props) {
  return (
    <Box sx={{ display: 'flex' }}>
      <Header />
      <Box sx={{ height: '100vh' }} aria-label="scrollbar temp" />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          width: `100%`,
          mt: `calc(${ACCOUNT_MENU_SIZE}px + ${NAVBAR_PADDING_SIZE * 2}px)`,
        }}
      >
        <CustomScrollbar
          // eslint-disable-next-line tailwindcss/no-custom-classname
          className="custom-scrollbar"
          autoHide
          autoHideTimeout={1000}
          autoHideDuration={200}
        >
          <Box sx={{ p: 3, width: PRIMARY_APP_WIDTH, mx: 'auto' }}>
            <Outlet />
          </Box>
        </CustomScrollbar>
      </Box>
    </Box>
  );
}

// It seems that adding the <Box sx={{ height: '100vh' }} /> component before the main content is helping in displaying the content within the CustomScrollbar. This additional Box component with height: '100vh' ensures that the parent container of the CustomScrollbar has a non-zero height.

// By providing a non-zero height to the parent container, it allows the CustomScrollbar component to calculate its dimensions correctly and display the content properly.

// While it's not clear why the CustomScrollbar component requires this additional height in your specific case, it's possible that the library or the CSS styles applied to your layout have specific requirements for proper rendering.

// Using height: '100vh' sets the height of the Box component to fill the entire viewport height, ensuring that the parent container has a valid height for the CustomScrollbar to function as expected.

// Keep in mind that this solution might not be ideal for all scenarios, and it's recommended to adjust the height according to your layout requirements.

// By adding the <Box sx={{ height: '100vh' }} /> component, you should be able to display the content within the CustomScrollbar correctly in your AppLayout.