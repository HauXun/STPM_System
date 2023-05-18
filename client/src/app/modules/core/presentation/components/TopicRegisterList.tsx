import { Box, Button, IconButton, SvgIcon } from '@mui/material';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TablePagination from '@mui/material/TablePagination';
import TableRow from '@mui/material/TableRow';
import { Fragment, useState } from 'react';
import SearchInput from '~/app/modules/shared/presentation/components/SearchInput';
import { formatTopicStatus } from '~/app/modules/shared/utils';
import { filterOutline } from 'ionicons/icons';
import { IonButton, IonIcon } from '@ionic/react';
import SendIcon from '@mui/icons-material/Send';

interface Column {
  id: 'name' | 'rank' | 'total' | 'joinDate' | 'status';
  label: string;
  minWidth?: number;
  align: 'left' | 'center' | 'right' | 'justify' | 'inherit';
}

const columns: readonly Column[] = [
  { id: 'name', label: 'Tên đề tài', minWidth: 250, align: 'left' },
  { id: 'rank', label: 'Hạng mục', minWidth: 100, align: 'center' },
  {
    id: 'total',
    label: 'Tham gia',
    minWidth: 100,
    align: 'center',
  },
  {
    id: 'joinDate',
    label: 'Ngày tham gia',
    minWidth: 100,
    align: 'center',
  },
  {
    id: 'status',
    label: 'Trạng thái',
    minWidth: 100,
    align: 'center',
  },
];

interface Data {
  name: string;
  rank: string;
  total: number;
  joinDate: string;
  status: { registered: boolean; cancelled: boolean; forceLock: boolean };
}

function createData(
  name: string,
  rank: string,
  total: number,
  joinDate: string,
  status: { registered: boolean; cancelled: boolean; forceLock: boolean }
): Data {
  return { name, rank, total, joinDate, status };
}

const rows = [
  createData('Ứng dụng quản lý bãi giữ xe 1', 'Hạng mục 2', 2, '17/04/2023', {
    registered: false,
    forceLock: true,
    cancelled: false,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 2', 'Hạng mục 2', 2, '17/04/2023', {
    registered: true,
    forceLock: false,
    cancelled: false,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 3', 'Hạng mục 2', 2, '17/04/2023', {
    registered: false,
    forceLock: false,
    cancelled: false,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 4', 'Hạng mục 2', 2, '17/04/2023', {
    registered: false,
    forceLock: false,
    cancelled: true,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 5', 'Hạng mục 2', 2, '17/04/2023', {
    registered: false,
    forceLock: true,
    cancelled: false,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 6', 'Hạng mục 2', 2, '17/04/2023', {
    registered: true,
    forceLock: false,
    cancelled: false,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 7', 'Hạng mục 2', 2, '17/04/2023', {
    registered: true,
    forceLock: false,
    cancelled: true,
  }),
  createData('Ứng dụng quản lý bãi giữ xe 8', 'Hạng mục 2', 2, '17/04/2023', {
    registered: true,
    forceLock: false,
    cancelled: false,
  }),
];

export default function TopicRegisterList() {
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  return (
    <Fragment>
      <Box sx={{ display: 'flex', width: '100%', justifyContent: 'space-between' }}>
        <SearchInput />
        <Button
          className="font-semibold"
          sx={{ textTransform: 'none' }}
          variant="contained"
          endIcon={<IonIcon icon={filterOutline} />}
        >
          Lọc
        </Button>
      </Box>
      <Paper sx={{ width: '100%', overflow: 'hidden', mt: 4 }}>
        <TableContainer
          sx={{
            paddingX: 5,
            maxHeight: 440,
            '&::-webkit-scrollbar': {
              width: 6,
            },
            '&::-webkit-scrollbar-track': {
              backgroundColor: 'transparent',
            },
            '&::-webkit-scrollbar-thumb': {
              backgroundColor: 'transparent',
              borderRadius: 4,
              transition: 'background-color 0.3s ease',
            },
            '&:hover::-webkit-scrollbar-track': {
              backgroundColor: 'transparent',
            },
            '&:hover::-webkit-scrollbar-thumb': {
              backgroundColor: '#00000033',
            },
          }}
        >
          <Table stickyHeader aria-label="sticky table">
            <TableHead>
              <TableRow>
                {columns.map((column) => (
                  <TableCell
                    className="font-bold"
                    sx={{ paddingY: 3, fontFamily: 'Nunito' }}
                    key={column.id}
                    align={column.align}
                    style={{ minWidth: column.minWidth }}
                  >
                    {column.label}
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {rows.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row, i) => {
                return (
                  <TableRow hover role="checkbox" tabIndex={-1} key={i}>
                    {columns.map((column) => {
                      const value = row[column.id];
                      if (typeof value === 'object') {
                        const { name, color } = formatTopicStatus(value);
                        return (
                          <TableCell
                            key={column.id}
                            align={column.align}
                            sx={{
                              fontFamily: 'Nunito',
                            }}
                          >
                            {
                              <button
                                className={`${color} w-8/12 font-bold py-2 px-4 rounded text-sm`}
                              >
                                {name}
                              </button>
                            }
                          </TableCell>
                        );
                      }
                      return (
                        <TableCell
                          key={column.id}
                          align={column.align}
                          sx={{
                            fontFamily: 'Nunito',
                          }}
                        >
                          {value}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          className="font-medium"
          rowsPerPageOptions={[10, 25, 100]}
          component="div"
          count={rows.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Paper>
    </Fragment>
  );
}
