import { Typography } from '@mui/material';
import { CSSProperties } from '@mui/styles';
import { useState } from 'react';
import CancelButton from '~/app/modules/core/presentation/components/CancelButton';
import PrimaryButton from '~/app/modules/core/presentation/components/PrimaryButton';
import PrimaryInput from '~/app/modules/core/presentation/components/PrimaryInput';
import BoxInput from '~/app/modules/core/presentation/containers/BoxInput';

type Props = {
  sx?: CSSProperties;
  value?: string | number;
  onClick?: (event: React.MouseEvent<unknown>) => void;
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
};

export default function LabelInputEdit({ sx, value, onChange, onClick }: Props) {
  const [isEdit, setIsEdit] = useState(false);

  return (
    <BoxInput sx={{ ...sx }}>
      {isEdit ? (
        <PrimaryInput value={value} onChange={onChange} />
      ) : (
        <Typography sx={{ fontWeight: 600 }}>{value}</Typography>
      )}
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
