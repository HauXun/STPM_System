import { IonIcon } from '@ionic/react';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { filterOutline } from 'ionicons/icons';
import { Box, Button } from '@mui/material';
import SearchInput from './SearchInput';

type Props = {
  placeholderSearch?: string;
  onClickFilter?: (event: React.MouseEvent<unknown>) => void;
  onChangeInput?: (event: React.ChangeEvent<HTMLInputElement>) => void;
};

export default function SearchFilter({placeholderSearch, onClickFilter, onChangeInput}: Props) {
  return (
    <Box sx={{ display: 'flex', width: '100%', justifyContent: 'space-between' }}>
      <SearchInput placeholder={placeholderSearch} onChange={onChangeInput} />
      <Button
        className="border-0 bg-white font-semibold text-black"
        sx={{ textTransform: 'none', boxShadow: COMPONENT_SHADOW }}
        variant="outlined"
        endIcon={<IonIcon icon={filterOutline} />}
        onClick={onClickFilter}
      >
        Lọc
      </Button>
    </Box>
  );
}
