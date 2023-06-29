// eslint-disable-next-line @typescript-eslint/no-unused-vars
import './app.module.scss';

import { Box, createTheme, ThemeProvider } from '@mui/material';
import { Theme } from '@mui/material/styles';
import { createContext, useContext, useState } from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { APPBAR_PADDING_SIZE } from '~/app/modules/shared/constants';
import { rootRoutes } from './routes';

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#479C45',
    },
    background: {
      default: '#f8faf9',
    },
  },
  typography: {
    fontFamily: 'Nunito, Raleway, Roboto, Arial, sans-serif',
    fontSize: 16,
    fontWeightLight: 300,
    fontWeightRegular: 400,
    fontWeightMedium: 500,
    fontWeightBold: 700,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          border: 0,
        },
      },
    },
    MuiToolbar: {
      styleOverrides: {
        root: {
          paddingLeft: 0,
          paddingTop: APPBAR_PADDING_SIZE,
          paddingBottom: APPBAR_PADDING_SIZE,
        },
      },
    },
    MuiAvatar: {
      styleOverrides: {
        root: {
          '& .MuiAvatar-img': { objectFit: 'contain' },
        },
      },
    },
    // MuiCssBaseline: {
    //   styleOverrides: {
    //     // Reset all base CSS styles
    //     '@global': {
    //       '*': {
    //         margin: 0,
    //         padding: 0,
    //         boxSizing: 'border-box',
    //       },
    //       html: {
    //         WebkitFontSmoothing: 'antialiased',
    //         MozOsxFontSmoothing: 'grayscale',
    //         height: '100%',
    //         width: '100%',
    //       },
    //       body: {
    //         height: '100%',
    //         width: '100%',
    //       },
    //     },
    //   },
    // }
  },
});

declare module 'react' {
  interface CSSProperties {
    '--tree-view-color'?: string;
    '--tree-view-bg-color'?: string;
    '--primary-green'?: string;
    '--primary-bg-color'?: string;
  }
}

declare global {
  interface Document {
    adminTitle: string;
  }
}

// Augment the DefaultTheme interface to include your custom properties
declare module '@mui/styles' {
  interface DefaultTheme extends Theme {
    palette: {
      primary: {
        main: string;
      };
      action: {
        activatedOpacity: number;
      };
      background: {
        default: string;
      };
    };
  }
}

export type GlobalContent = {
  title: string;
  setTitle: (title: string) => void;
};

export const GlobalContext = createContext<GlobalContent>({} as GlobalContent);

export const useGlobalContext = () => useContext(GlobalContext);

export function App() {
  const [title, setTitle] = useState('Stpm ');

  const setAppTitle = (newTitle: string) => {
    setTitle(newTitle);
  };

  const contextValue: GlobalContent = {
    title,
    setTitle: setAppTitle,
  };

  return (
    <ThemeProvider theme={theme}>
      <Box
        // eslint-disable-next-line tailwindcss/no-custom-classname
        className="custom-scrollbar"
        sx={{
          fontFamily: 'Nunito',
          width: '100vw',
          height: '100vh',
          backgroundColor: '#f8faf9',
          overflow: 'hidden',
        }}
        style={{
          '--tree-view-color': '#479C45',
          '--primary-green': '#479C45',
          '--primary-bg-color': '#f8faf9',
        }}
      >
        {/* <NxWelcome title="stpm-system" /> */}
        <GlobalContext.Provider value={contextValue}>
          <RouterProvider router={createBrowserRouter([...rootRoutes])} />
        </GlobalContext.Provider>
      </Box>
    </ThemeProvider>
  );
}

export default App;
