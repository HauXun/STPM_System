import { Pagination, PaginationProps } from '@mui/material';

export default function PrimaryPagination(props: PaginationProps) {
  return (
    <Pagination
      className="text-center"
      sx={{
        mx: 'auto',
        my: 3,
        width: 'fit-content',
        '& .MuiPaginationItem-root': {
          // Hide text content
          height: 'unset',
          '& .MuiPaginationItem-page': {
            // Hide the page number
            visibility: 'hidden',
          },
          '&.Mui-selected': {
            width: 60,
          },
        },
      }}
      {...props}
      color="primary"
    />
  );
}
