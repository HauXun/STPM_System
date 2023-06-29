import { InputBase, InputBaseProps } from "@mui/material";

export default function PrimaryInput({ sx, className, ...props}: InputBaseProps) {
  return (
    <InputBase
      className={`bg-white text-base font-semibold ${className}`}
      sx={{ flex: 1, mr: 2, border: '1px solid #ccc', borderRadius: '6px', px: 2, height: '100%', ...sx }}
      {...props}
    />
  );
}
