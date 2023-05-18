import { IonIcon } from '@ionic/react';
import { IconButton, InputBase, Paper } from '@mui/material';
import { searchOutline } from 'ionicons/icons';

type Props = {
  /**
   * Injected by the documentation to work in an iframe.
   * You won't need it on your project.
   */
  placeholder?: string;
};

export default function SearchInput({ placeholder }: Props) {
  return (
    <Paper
      component="form"
      sx={{
        display: 'flex',
        alignItems: 'center',
        width: 560,
        borderRadius: 50,
        boxShadow: '0px 4px 4px 0px rgba(86,157,170,0.2)',
      }}
    >
      <IconButton className='text-2xl' sx={{ p: '10px' }} aria-label="menu">
        <IonIcon icon={searchOutline} />
      </IconButton>
      <InputBase
        className="text-base"
        sx={{ ml: 1, flex: 1 }}
        placeholder={placeholder ?? 'Tìm kiếm'}
        inputProps={{ 'aria-label': 'tìm kiếm' }}
      />
    </Paper>
  );
}
