import { TableHead, TableRow, styled } from "@mui/material";

export const CustomTableHead = styled(TableHead)(({ theme }) => ({
  '&': {
    th: {
      color: theme.palette.primary.main,
      fontSize: 18,
      fontWeight: 700,
      border: 0,
    },
  },
}));

export const CustomTableRow = styled(TableRow)(({ theme }) => ({
  '&': {
    'th, td': {
      border: 0,
    },
  },
}));