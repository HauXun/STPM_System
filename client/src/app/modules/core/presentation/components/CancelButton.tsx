import { Button } from '@mui/material';

type Props = {
  onClick?: (event: React.MouseEvent<unknown>) => void;
};

export default function CancelButton({ onClick }: Props) {
  return (
    <Button
      className="h-full rounded-lg bg-gray-200 font-bold normal-case text-gray-700"
      onClick={onClick}
    >
      Huỷ
    </Button>
  );
}
