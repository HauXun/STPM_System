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
  TableRow,
  TableSortLabel,
  Toolbar,
  Tooltip,
  Typography,
} from '@mui/material';
import { alpha } from '@mui/material/styles';
import { visuallyHidden } from '@mui/utils';
import { ellipsisVerticalOutline } from 'ionicons/icons';
import { ChangeEvent, useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { HeadCellWithId, Order } from '~/app/modules/shared/common/types';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import PrimaryPagination from '~/app/modules/shared/presentation/components/Pagination';
import { formatStatus } from '~/app/modules/shared/utils';
import { EnhancedTableProps, getComparator, stableSort } from '~/app/modules/shared/utils/table';
import { UserFilterModel } from '../../domain/models/UserFilterModel';
import { UserState } from '../../infrastructure/store/types';
import { UserRoutes } from '../routes';

interface Data {
  id: number;
  fullName: string;
  email: string;
  className: string;
  roleName: string;
  joinedDate: string;
  status: boolean;
}

const headCells: readonly HeadCellWithId<keyof Data>[] = [
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
    id: 'joinedDate',
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

function EnhancedTableHead(props: EnhancedTableProps<keyof Data>) {
  const { onSelectAllClick, order, orderBy, numSelected, rowCount, onRequestSort } = props;
  const createSortHandler = (property: keyof Data) => (event: React.MouseEvent<unknown>) => {
    onRequestSort(event, property);
  };

  return (
    <TableHead>
      <TableRow
        sx={{
          '& .MuiTableCell-root': {
            backgroundColor: 'white',
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

type UserListProps = {
  userState: UserState;
  fetchUsers: (model: UserFilterModel) => void;
  setFilter: (model: UserFilterModel) => void;
};

export default function UserList({ userState, fetchUsers, setFilter }: UserListProps) {
  const [order, setOrder] = useState<Order>('asc');
  const [orderBy, setOrderBy] = useState<keyof Data>('fullName');
  const [selected, setSelected] = useState<readonly string[]>([]);
  const [users, setUsers] = useState<Data[]>([]);
  const { data, filter, pagination } = userState;
  const navigate = useNavigate();

  useEffect(() => {
    const newUser = data.map(
      (u) =>
        ({
          id: u.id,
          fullName: u.fullName,
          email: u.email,
          className: u.gradeName,
          roleName: u.roles.join(', '),
          joinedDate: u.joinedDate.toLocaleString(),
          status: u.lockEnable,
        } as Data)
    );

    setUsers(newUser);
  }, [data]);

  useEffect(() => {
    fetchUsers(filter);
  }, [fetchUsers, filter]);

  useEffect(() => {
    setFilter({ pageNumber: 1, pageSize: 10 } as UserFilterModel);
  }, [setFilter]);

  const handlePageChange = (e: ChangeEvent<unknown>, page: number) => {
    setFilter({ ...filter, pageNumber: page } as UserFilterModel);
  };

  const handleRequestSort = (event: React.MouseEvent<unknown>, property: keyof Data) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(property);
  };

  const handleSelectAllClick = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.checked) {
      const newSelected = users.map((u) => u.fullName);
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

  const isSelected = (name: string) => selected.indexOf(name) !== -1;

  // Avoid a layout jump when reaching the last page with empty rows.
  const emptyRows = 0; // page > 0 ? Math.max(0, (1 + 0) * 10 - users.length) : 0;

  const visibleRows = useMemo(
    () => stableSort(users, getComparator(order, orderBy)).slice(0 * 10, 0 * 10 + 10),
    [order, orderBy, users]
  );

  return (
    <Paper sx={{ width: '100%', overflow: 'hidden', boxShadow: COMPONENT_SHADOW }} elevation={0}>
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
            rowCount={users.length}
          />
          <TableBody>
            {visibleRows.map((row, index) => {
              const isItemSelected = isSelected(row.fullName);
              const labelId = `enhanced-table-checkbox-${index}`;
              const { name, color } = formatStatus(row.status);

              return (
                <TableRow
                  hover
                  role="checkbox"
                  aria-checked={isItemSelected}
                  tabIndex={-1}
                  key={row.fullName}
                  selected={isItemSelected}
                  onClick={(e) => navigate(`/${UserRoutes.USERS}/${row.id}`)}
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
                    onClick={(event) => handleClick(event, row.fullName)}
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
                  <TableCell align="left">
                    {new Date(row.joinedDate).toLocaleDateString()}
                  </TableCell>
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
      <PrimaryPagination
        count={Math.ceil((pagination?.totalItemCount ?? 0) / (pagination?.pageSize ?? 1))}
        page={(pagination?.pageIndex ?? 0) + 1}
        onChange={handlePageChange}
      />
    </Paper>
  );
}
