import BoxFlexCenter, { Props as BoxFlexCenterProps } from './BoxFlexCenter';

type Props = BoxFlexCenterProps & {};

export default function BoxInput({ sx, className, children }: Props) {
  return (
    <BoxFlexCenter
      className={className}
      sx={{
        height: 35,
        ...sx,
      }}
    >
      {children}
    </BoxFlexCenter>
  );
}
