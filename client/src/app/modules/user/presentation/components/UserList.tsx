import { IonIcon } from '@ionic/react';
import { Delete, FilterList } from '@mui/icons-material';
import {
  Box,
  Button,
  Checkbox,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  TableSortLabel,
  Toolbar,
  Tooltip,
  Typography,
} from '@mui/material';
import { alpha } from '@mui/material/styles';
import { visuallyHidden } from '@mui/utils';
import { ellipsisVerticalOutline } from 'ionicons/icons';
import { Fragment, useMemo, useState } from 'react';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import SearchFilter from '~/app/modules/shared/presentation/components/SearchFilter';
import { formatStatus } from '~/app/modules/shared/utils';

interface Data {
  fullName: string;
  email: string;
  className: string;
  roleName: string;
  joinDate: string;
  status: boolean;
}

function createData(
  fullName: string,
  email: string,
  className: string,
  roleName: string,
  joinDate: string,
  status: boolean
): Data {
  return {
    fullName,
    email,
    className,
    roleName,
    joinDate,
    status,
  };
}

const rows = [
  createData('Hauxun', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', true),
  createData('Hauxun 1', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', false),
  createData('Hauxun 2', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', true),
  createData('Hauxun 3', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', true),
  createData('Hauxun 4', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', false),
  createData('Hauxun 5', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', false),
  createData('Hauxun 6', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', true),
  createData('Hauxun 7', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', false),
  createData('Hauxun 8', '2011379@dlu.edu.vn', 'CTK44A', 'Sinh viên', '12/12/2022', true),
];

function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

type Order = 'asc' | 'desc';

function getComparator<Key extends keyof never>(
  order: Order,
  orderBy: Key
): (
  a: { [key in Key]: number | string | boolean },
  b: { [key in Key]: number | string | boolean }
) => number {
  return order === 'desc'
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}

function stableSort<T>(array: readonly T[], comparator: (a: T, b: T) => number) {
  const stabilizedThis = array.map((el, index) => [el, index] as [T, number]);
  stabilizedThis.sort((a, b) => {
    const order = comparator(a[0], b[0]);
    if (order !== 0) {
      return order;
    }
    return a[1] - b[1];
  });
  return stabilizedThis.map((el) => el[0]);
}

interface HeadCell {
  id: keyof Data;
  label: string;
  align: 'left' | 'center' | 'right' | 'justify' | 'inherit';
  minWidth?: number | string;
}

const headCells: readonly HeadCell[] = [
  {
    id: 'fullName',
    align: 'left',
    label: 'Họ và tên',
    minWidth: '25%',
  },
  {
    id: 'className',
    align: 'left',
    label: 'Lớp',
    minWidth: '15%',
  },
  {
    id: 'roleName',
    align: 'left',
    label: 'Vai trò',
    minWidth: '15%',
  },
  {
    id: 'joinDate',
    align: 'left',
    label: 'Ngày tham gia',
    minWidth: '15%',
  },
  {
    id: 'status',
    align: 'center',
    label: 'Trạng thái',
    minWidth: '15%',
  },
];

interface EnhancedTableProps {
  numSelected: number;
  onRequestSort: (event: React.MouseEvent<unknown>, property: keyof Data) => void;
  onSelectAllClick: (event: React.ChangeEvent<HTMLInputElement>) => void;
  order: Order;
  orderBy: string;
  rowCount: number;
}

function EnhancedTableHead(props: EnhancedTableProps) {
  const { onSelectAllClick, order, orderBy, numSelected, rowCount, onRequestSort } = props;
  const createSortHandler = (property: keyof Data) => (event: React.MouseEvent<unknown>) => {
    onRequestSort(event, property);
  };

  return (
    <TableHead>
      <TableRow>
        <TableCell
          padding="checkbox"
          sx={{
            '& .MuiCheckbox-root': {
              color: 'var(--primary-green)',
              ml: 3,
            },
          }}
        >
          <Checkbox
            indeterminate={numSelected > 0 && numSelected < rowCount}
            checked={rowCount > 0 && numSelected === rowCount}
            onChange={onSelectAllClick}
            inputProps={{
              'aria-label': 'select all desserts',
            }}
          />
        </TableCell>
        {headCells.map((headCell) => (
          <TableCell
            key={headCell.id}
            align={headCell.align}
            sortDirection={orderBy === headCell.id ? order : false}
            sx={{ width: headCell.minWidth }}
          >
            <TableSortLabel
              active={orderBy === headCell.id}
              direction={orderBy === headCell.id ? order : 'asc'}
              onClick={createSortHandler(headCell.id)}
            >
              {headCell.label}
              {orderBy === headCell.id ? (
                <Box component="span" sx={visuallyHidden}>
                  {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                </Box>
              ) : null}
            </TableSortLabel>
          </TableCell>
        ))}
        <TableCell align="right"></TableCell>
      </TableRow>
    </TableHead>
  );
}

interface EnhancedTableToolbarProps {
  numSelected: number;
}

function EnhancedTableToolbar(props: EnhancedTableToolbarProps) {
  const { numSelected } = props;

  return (
    <Toolbar
      sx={{
        pl: { sm: 2 },
        pr: { xs: 1, sm: 1 },
      }}
    >
      {numSelected > 0 ? (
        <Typography sx={{ flex: '1 1 100%' }} color="inherit" variant="subtitle1" component="div">
          {numSelected} selected
        </Typography>
      ) : (
        <Typography sx={{ flex: '1 1 100%' }} variant="h6" id="tableTitle" component="div">
          Danh sách tài khoản
        </Typography>
      )}
      {numSelected > 0 ? (
        <Tooltip title="Delete">
          <IconButton>
            <Delete />
          </IconButton>
        </Tooltip>
      ) : (
        <Tooltip title="Filter list">
          <IconButton>
            <FilterList />
          </IconButton>
        </Tooltip>
      )}
    </Toolbar>
  );
}

export default function UserList() {
  const [order, setOrder] = useState<Order>('asc');
  const [orderBy, setOrderBy] = useState<keyof Data>('fullName');
  const [selected, setSelected] = useState<readonly string[]>([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const handleRequestSort = (event: React.MouseEvent<unknown>, property: keyof Data) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(property);
  };

  const handleSelectAllClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.checked) {
      const newSelected = rows.map((n) => n.fullName);
      setSelected(newSelected);
      return;
    }
    setSelected([]);
  };

  const handleClick = (event: React.MouseEvent<unknown>, name: string) => {
    const selectedIndex = selected.indexOf(name);
    let newSelected: readonly string[] = [];

    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, name);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1)
      );
    }

    setSelected(newSelected);
  };

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const isSelected = (name: string) => selected.indexOf(name) !== -1;

  // Avoid a layout jump when reaching the last page with empty rows.
  const emptyRows = page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rows.length) : 0;

  const visibleRows = useMemo(
    () =>
      stableSort(rows, getComparator(order, orderBy)).slice(
        page * rowsPerPage,
        page * rowsPerPage + rowsPerPage
      ),
    [order, orderBy, page, rowsPerPage]
  );

  return (
    <Fragment>
      <SearchFilter
        placeholderSearch="Tìm kiếm tài khoản"
        onChangeInput={(event) => console.log(event.target.value)}
      />
      <Paper
        sx={{ width: '100%', overflow: 'hidden', mt: 4, boxShadow: COMPONENT_SHADOW }}
        elevation={0}
      >
        <EnhancedTableToolbar numSelected={selected.length} />
        <TableContainer
          sx={{
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
          <Table stickyHeader sx={{ minWidth: 750 }} aria-labelledby="sticky table">
            <EnhancedTableHead
              numSelected={selected.length}
              order={order}
              orderBy={orderBy}
              onSelectAllClick={handleSelectAllClick}
              onRequestSort={handleRequestSort}
              rowCount={rows.length}
            />
            <TableBody>
              {visibleRows.map((row, index) => {
                const isItemSelected = isSelected(row.fullName);
                const labelId = `enhanced-table-checkbox-${index}`;
                const { name, color } = formatStatus(row.status);

                return (
                  <TableRow
                    hover
                    onClick={(event) => handleClick(event, row.fullName)}
                    role="checkbox"
                    aria-checked={isItemSelected}
                    tabIndex={-1}
                    key={row.fullName}
                    selected={isItemSelected}
                    sx={{
                      cursor: 'pointer',
                      height: '68px',
                      '& .MuiTableCell-root': {
                        border: 0,
                        py: 1,
                      },
                      '&.MuiTableRow-hover:hover': {
                        bgcolor: (theme) =>
                          alpha(theme.palette.primary.main, theme.palette.action.activatedOpacity),
                      },
                    }}
                  >
                    <TableCell
                      padding="checkbox"
                      sx={{
                        '& .MuiCheckbox-root': {
                          color: 'var(--primary-green)',
                          ml: 3,
                        },
                      }}
                    >
                      <Checkbox
                        checked={isItemSelected}
                        inputProps={{
                          'aria-labelledby': labelId,
                        }}
                      />
                    </TableCell>
                    <TableCell component="th" id={labelId} scope="row" padding="normal">
                      <Typography>{row.fullName}</Typography>
                      <Typography variant="caption" className="text-blue-400">
                        {row.email}
                      </Typography>
                    </TableCell>
                    <TableCell align="left">{row.className}</TableCell>
                    <TableCell align="left">{row.roleName}</TableCell>
                    <TableCell align="left">{row.joinDate}</TableCell>
                    <TableCell align="center">
                      <Button
                        className={`${color} w-10/12 rounded-lg px-4 py-2 text-base font-bold normal-case`}
                      >
                        {name}
                      </Button>
                    </TableCell>
                    <TableCell align="center">
                      <IconButton color="default">
                        <IonIcon icon={ellipsisVerticalOutline} />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                );
              })}
              {emptyRows > 0 && (
                <TableRow
                  style={{
                    height: 68 * emptyRows,
                  }}
                >
                  <TableCell
                    colSpan={7}
                    sx={{
                      border: 0,
                    }}
                  />
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
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
