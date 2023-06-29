import { CssBaseline } from '@mui/material';
import { StyledEngineProvider } from '@mui/material/styles';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import App from './main/app';
import { store } from './app/stores/store';

const root = createRoot(document.getElementById('root') as HTMLElement);
root.render(
  <StrictMode>
    <StyledEngineProvider injectFirst>
      <CssBaseline />
      <Provider store={store}>
        <App />
      </Provider>
    </StyledEngineProvider>
  </StrictMode>
);
