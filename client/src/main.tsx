import { StrictMode } from 'react';
import * as ReactDOM from 'react-dom/client';
import { StyledEngineProvider } from '@mui/material/styles';
import App from './main/app';
import { BrowserRouter } from 'react-router-dom';
import { CssBaseline } from '@mui/material';

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);
root.render(
  <StrictMode>
    <StyledEngineProvider injectFirst>
      <BrowserRouter>
        <CssBaseline />
        <App />
      </BrowserRouter>
    </StyledEngineProvider>
  </StrictMode>
);
