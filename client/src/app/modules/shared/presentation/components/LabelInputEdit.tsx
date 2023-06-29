import { ButtonProps, TextFieldProps, Typography } from '@mui/material';
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
  };

export default function LabelInputEdit({ sx, value, onChange, onClick }: Props) {
  const [isEdit, setIsEdit] = useState(false);
  const [tempValue, setTempValue] = useState('');

  return (
    <BoxInput sx={{ ...sx }}>
      {isEdit ? (
        <PrimaryInput value={value} onChange={onChange} />
      ) : (
        <Typography sx={{ fontWeight: 600 }}>{value}</Typography>
      )}
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
