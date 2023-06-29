import { ButtonProps, Menu, MenuItem, TextFieldProps, Typography } from '@mui/material';
import { CSSProperties } from '@mui/styles';
import { useState } from 'react';
import CancelButton from '~/app/modules/core/presentation/components/CancelButton';
import PrimaryButton from '~/app/modules/core/presentation/components/PrimaryButton';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';
import BoxInput from '~/app/modules/core/presentation/containers/BoxInput';

type Props = Pick<ButtonProps, 'onClick'> &
  Pick<TextFieldProps, 'onChange'> & {
    sx?: CSSProperties;
    value?: string;
    list?: { id: string | number; value: string | number }[];
  };

export default function DropdownInputEdit({ sx, value, list, onClick, onChange }: Props) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [isEdit, setIsEdit] = useState(false);
  const [tempValue, setTempValue] = useState('');

  const handleOpenMenu = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleCloseMenu = () => {
    setAnchorEl(null);
  };

  const handleMenuItemClick = (newValue: { id: string | number; value: string | number }) => {
    // Create a synthetic event object
    const event = {
      target: {
        id: newValue.id,
        value: newValue.value,
      },
    } as React.ChangeEvent<HTMLInputElement>;

    onChange?.(event); // Call onChange with the synthetic event

    handleCloseMenu();
  };

  return (
    <BoxInput sx={{ ...sx }}>
      {isEdit ? (
        <PrimaryInput
          value={value}
          onClick={handleOpenMenu}
          onChange={(e) => {
            if (isEdit) {
              onChange?.(e);
            }
          }}
        />
      ) : (
        <Typography sx={{ fontWeight: 600 }}>{value}</Typography>
      )}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleCloseMenu}
        PaperProps={{
          style: {
            minWidth: 200,
          },
        }}
      >
        {list &&
          list.length > 0 &&
          list.map((item, i) => (
            <MenuItem key={i} onClick={() => handleMenuItemClick(item)}>
              {item.value}
            </MenuItem>
          ))}
      </Menu>
      {isEdit ? (
        <>
          <PrimaryButton
            sx={{
              mr: 1,
            }}
            text="Lưu"
            onClick={(e) => {
              setIsEdit(!isEdit);
              onClick?.(e);
            }}
          />
          <CancelButton
            onClick={(e) => {
              setIsEdit(!isEdit);
              onChange?.({ target: { value: tempValue } } as React.ChangeEvent<HTMLInputElement>);
            }}
          />
        </>
      ) : (
        <PrimaryButton
          text="Sửa"
          onClick={(e) => {
            setIsEdit(!isEdit);
            setTempValue(value ?? '');
          }}
        />
      )}
    </BoxInput>
  );
}
