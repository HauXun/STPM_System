import { IonIcon } from '@ionic/react';
import { BoxProps, Button, ButtonProps } from '@mui/material';
import { filterOutline } from 'ionicons/icons';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import SearchInput, { Props as SearchInputProps } from './SearchInput';

type Props = BoxProps &
  Pick<SearchInputProps, 'onChange' | 'placeholder'> &
  Pick<ButtonProps, 'onClick'> & {
  };

export default function SearchFilter({
  sx,
  className,
  placeholder,
  onClick,
  onChange,
}: Props) {
  return (
    <BoxFlexCenter className={className} sx={{ width: '100%', ...sx }}>
      <SearchInput placeholder={placeholder} onChange={onChange} />
      <Button
        className="border-0 bg-white font-semibold text-black"
        sx={{ textTransform: 'none', boxShadow: COMPONENT_SHADOW }}
        variant="outlined"
        endIcon={<IonIcon icon={filterOutline} />}
        onClick={onClick}
      >
        Lọc
      </Button>
    </BoxFlexCenter>
  );
}
