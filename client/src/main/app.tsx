// eslint-disable-next-line @typescript-eslint/no-unused-vars
import AdminLayout from '~/app/modules/core/presentation/components/layouts/admin';
import styles from './app.module.scss';

import NxWelcome from './nx-welcome';
import { Box, createTheme, ThemeProvider } from '@mui/material';

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#F8FAF9',
    },
  },
  typography: {
    fontFamily: 'Raleway, Nunito, Arial, sans-serif',
    fontSize: 16,
    fontWeightLight: 300,
    fontWeightRegular: 400,
    fontWeightMedium: 500,
    fontWeightBold: 700
  },
});


export function App() {
  return (
    <ThemeProvider theme={theme}>
      <Box
        className="custom-scrollbar"
        sx={{ fontFamily: 'Raleway', width: '100vw', height: '100vh', backgroundColor: '#f8faf9', overflow: 'hidden' }}
      >
        {/* <NxWelcome title="stpm-system" /> */}
        <AdminLayout />
      </Box>
    </ThemeProvider>
  );
}

export default App;
