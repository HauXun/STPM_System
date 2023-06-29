import { IonIcon } from '@ionic/react';
import { IconButton, InputBase, Paper, PaperProps, SxProps, TextFieldProps, Theme } from '@mui/material';
import { searchOutline } from 'ionicons/icons';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '../../utils';

export type Props = PaperProps &
  Pick<TextFieldProps, 'onChange'> & {
    placeholder?: string;
  };

export default function SearchInput({
  sx,
  className,
  placeholder,
  children,
  onChange,
  ...props
}: Props) {
  return (
    <Paper
      className={className}
      component="form"
      sx={{
        ...({
          width: 560,
          borderRadius: 50,
          boxShadow: COMPONENT_SHADOW,
          p: 0.5,
          ...centerFlexItems({}),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <IconButton className="text-2xl" sx={{ p: '5px' }} aria-label="menu">
        <IonIcon icon={searchOutline} />
      </IconButton>
      <InputBase
        className="text-base"
        sx={{ ml: 1, flex: 1 }}
        placeholder={placeholder ?? 'Tìm kiếm'}
        inputProps={{ 'aria-label': 'tìm kiếm' }}
        onChange={onChange}
      />
      {children}
    </Paper>
  );
}
