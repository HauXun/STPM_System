import { Menu, MenuItem, Typography } from '@mui/material';
import { CSSProperties } from '@mui/styles';
import { useState } from 'react';
import CancelButton from '~/app/modules/core/presentation/components/CancelButton';
import PrimaryButton from '~/app/modules/core/presentation/components/PrimaryButton';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';
import BoxInput from '~/app/modules/core/presentation/containers/BoxInput';

type Props = {
  sx?: CSSProperties;
  value?: string | number;
  list?: { id: string | number; value: string | number }[];
  onClick?: (event: React.MouseEvent<unknown>) => void;
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
};

export default function DropdownInputEdit({ sx, value, list, onClick, onChange }: Props) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [isEdit, setIsEdit] = useState(false);

  const handleOpenMenu = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleCloseMenu = () => {
    setAnchorEl(null);
  };

  const handleMenuItemClick = (newValue: string | number) => {
    // Create a synthetic event object
    const event = {
      target: {
        value: newValue,
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
            <MenuItem key={i} onClick={() => handleMenuItemClick(item.id)}>
              {item.value}
            </MenuItem>
          ))}
      </Menu>
      {isEdit ? (
        <>
          <PrimaryButton sx={{ mr: 1 }} text="Lưu" onClick={onClick} />
          <CancelButton
            onClick={(e) => {
              setIsEdit(!isEdit);
            }}
          />
        </>
      ) : (
        <PrimaryButton text="Sửa" onClick={(e) => setIsEdit(!isEdit)} />
      )}
    </BoxInput>
  );
}
