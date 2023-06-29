import { IonIcon } from '@ionic/react';
import { CloseRounded } from '@mui/icons-material';
import {
  Avatar,
  Badge,
  Box,
  Button,
  Checkbox,
  Chip,
  IconButton,
  ListItemIcon,
  MenuItem,
  Paper,
  Select,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TableSortLabel,
  Typography,
  SxProps,
  Theme,
} from '@mui/material';
import { alpha } from '@mui/material/styles';
import { visuallyHidden } from '@mui/utils';
import { optionsOutline } from 'ionicons/icons';
import { useMemo, useState } from 'react';
import CancelButton from '~/app/modules/core/presentation/components/CancelButton';
import PrimaryButton from '~/app/modules/core/presentation/components/PrimaryButton';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { HeadCellWithId, Order } from '~/app/modules/shared/common/types';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import SearchInput from '~/app/modules/shared/presentation/components/SearchInput';
import { centerFlexItems } from '~/app/modules/shared/utils';
import { EnhancedTableProps, getComparator, stableSort } from '~/app/modules/shared/utils/table';
import RankAward_Type1 from '~/main/assets/RankAward_Type1.png';
import RankAward_Type2 from '~/main/assets/RankAward_Type2.png';

interface Data {
  fullName: string;
  email: string;
  roleName: string;
}

function createData(fullName: string, email: string, roleName: string): Data {
  return {
    fullName,
    email,
    roleName,
  };
}

const rows = [
  createData('Hauxun', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 1', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 2', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 3', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 4', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 5', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 6', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 7', '2011379@dlu.edu.vn', 'Giám khảo'),
  createData('Hauxun 8', '2011379@dlu.edu.vn', 'Giám khảo'),
];

const headCells: readonly HeadCellWithId<keyof Data>[] = [
  {
    id: 'fullName',
    align: 'left',
    label: 'Họ và tên',
    minWidth: '65%',
  },
  {
    id: 'roleName',
    align: 'right',
    label: 'Vai trò',
    minWidth: '35%',
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
            color: (theme) => theme.palette.primary.main,
            '& .MuiButtonBase-root:hover': {
              color: 'inherit',
            },
            '& .MuiSvgIcon-root': {
              color: 'inherit',
            },
            '& .Mui-active': {
              color: 'inherit',
            },
            '&:last-child > .MuiButtonBase-root ': {
              mr: 3,
            },
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
      </TableRow>
    </TableHead>
  );
}

type Props = {};

export default function TopicSpecificRatingModal({}: Props) {
  const [order, setOrder] = useState<Order>('asc');
  const [orderBy, setOrderBy] = useState<keyof Data>('fullName');
  const [selected, setSelected] = useState<readonly string[]>([]);

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

  const isSelected = (name: string) => selected.indexOf(name) !== -1;

  const visibleRows = useMemo(
    () => stableSort(rows, getComparator(order, orderBy)),
    [order, orderBy]
  );

  const options = [
    {
      value: '1',
      label: 'Hạng mục 1',
      icon: (
        <Box
          component="img"
          sx={{
            width: 30,
            m: 1,
            ml: 2,
            objectFit: 'contain',
          }}
          src={RankAward_Type1}
        />
      ),
    },
    {
      value: '2',
      label: 'Hạng mục 2',
      icon: (
        <Box
          component="img"
          sx={{
            width: 30,
            m: 1,
            ml: 2,
            objectFit: 'contain',
          }}
          src={RankAward_Type2}
        />
      ),
    },
    // Add more options as needed
  ];

  return (
    <Paper
      elevation={5}
      sx={{
        width: 'fit-content',
        maxWidth: 600,
        borderRadius: 5,
        p: 3,
        py: 5,
        bgcolor: (theme) => theme.palette.background.default,
      }}
    >
      <Stack spacing={3}>
        <BoxFlexCenter>
          <Box>
            <Typography variant="h5" className="font-bold">
              Chỉ định chấm thi
            </Typography>
            <Typography variant="subtitle2" className="mt-2 text-sm text-gray-600">
              Together we are and will always be Victorious! Enjoy Psyko Punkz' latest video clip
              "Victorious".
            </Typography>
          </Box>
          <IconButton sx={{ p: '10px', width: 50, height: 50 }}>
            <CloseRounded />
          </IconButton>
        </BoxFlexCenter>
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: '30% auto 20%',
            columnGap: 3,
          }}
        >
          <Select
            displayEmpty
            IconComponent={() => (
              <Box
                component="img"
                sx={{
                  width: 30,
                  m: 1,
                  objectFit: 'contain',
                }}
                src={RankAward_Type1}
              />
            )}
            renderValue={() => (
              <Typography className="text-base leading-4 text-gray-500" variant="body2">
                Hạng mục 1
              </Typography>
            )}
            sx={{
              ...({
                borderRadius: 2,
                backgroundColor: 'white',
                boxShadow: COMPONENT_SHADOW,
                '& .MuiSelect-select': {
                  py: 0,
                  ...centerFlexItems({}),
                },
              } as SxProps<Theme>),
            }}
          >
            {options.map((option) => (
              <MenuItem key={option.value} value={option.value} sx={{}}>
                {option.label}
                <ListItemIcon>{option.icon}</ListItemIcon>
              </MenuItem>
            ))}
          </Select>
          <Select
            displayEmpty
            IconComponent={() => (
              <Box
                component="img"
                sx={{
                  width: 30,
                  m: 1,
                  objectFit: 'contain',
                }}
                src={RankAward_Type1}
              />
            )}
            renderValue={() => (
              <Typography className="text-base leading-4 text-gray-500" variant="body2">
                Quản trị viên
              </Typography>
            )}
            sx={{
              ...({
                borderRadius: 2,
                backgroundColor: 'white',
                boxShadow: COMPONENT_SHADOW,
                '& .MuiSelect-select': {
                  py: 0,
                  ...centerFlexItems({}),
                },
              } as SxProps<Theme>),
            }}
          >
            {options.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
                <ListItemIcon>{option.icon}</ListItemIcon>
              </MenuItem>
            ))}
          </Select>
          <Select
            displayEmpty
            IconComponent={() => (
              <IonIcon
                className="mr-2 text-5xl"
                style={{
                  transform: 'rotate(90deg)',
                }}
                icon={optionsOutline}
              />
            )}
            renderValue={() => (
              <Typography className="text-base leading-4 text-gray-500" variant="body2">
                2023
              </Typography>
            )}
            sx={{
              ...({
                borderRadius: 2,
                backgroundColor: 'white',
                boxShadow: COMPONENT_SHADOW,
                '& .MuiSelect-select': {
                  py: 0,
                  ...centerFlexItems({}),
                },
              } as SxProps<Theme>),
            }}
          >
            {options.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
                <ListItemIcon>{option.icon}</ListItemIcon>
              </MenuItem>
            ))}
          </Select>
        </Box>
        <SearchInput
          children={
            <Button
              variant="outlined"
              sx={{
                borderRadius: 50,
                py: 0,
                mr: 0.5,
                textTransform: 'none',
                border: '1px solid var(--primary-green)',
              }}
            >
              + Add
            </Button>
          }
        />
        <BoxFlexCenter justifyContent='initial' sx={{ pl: 3 }}>
          <Badge
            sx={{
              p: 0,
              '& .MuiBadge-badge': {
                backgroundColor: '#c5c5c5',
                p: 0,
                border: (theme) => `1px solid ${theme.palette.background.default}`,
                cursor: 'pointer',
                transition: 'all 150ms',
                '&:hover': {
                  backgroundColor: '#d1d1d1',
                },
                '&:active': {
                  backgroundColor: '#ababab',
                },
              },
            }}
            color="secondary"
            overlap="circular"
            badgeContent={<CloseRounded className="text-xs" />}
          >
            <Avatar
              alt="Avatar"
              src="https://picsum.photos/300/300"
              sx={{ width: 50, height: 50, mr: 1 }}
            />
          </Badge>
        </BoxFlexCenter>
        <Paper
          sx={{
            width: '100%',
            overflow: 'hidden',
            mt: 4,
            boxShadow: COMPONENT_SHADOW,
            borderRadius: 3,
          }}
          elevation={0}
        >
          <TableContainer
            sx={{
              maxHeight: 300,
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
            <Table stickyHeader aria-labelledby="sticky table">
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
                            alpha(
                              theme.palette.primary.main,
                              theme.palette.action.activatedOpacity
                            ),
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
                        <BoxFlexCenter sx={{ justifyContent: 'initial' }}>
                          <Avatar
                            alt={row.fullName}
                            src="https://picsum.photos/300/300"
                            sx={{ width: 50, height: 50, mr: 2 }}
                          />
                          <Box>
                            <Typography className="font-semibold text-gray-600">
                              {row.fullName}
                            </Typography>
                            <Typography variant="caption" className="text-sm text-blue-400">
                              {row.email}
                            </Typography>
                          </Box>
                        </BoxFlexCenter>
                      </TableCell>
                      <TableCell align="right">
                        <Chip
                          className="bg-inherit font-semibold text-orange-600"
                          sx={{
                            mr: 3,
                            '& .MuiBadge-root': { m: '10px 5px 10px 20px' },
                            '& .MuiChip-label': { pr: 0 },
                          }}
                          icon={<Badge color="warning" variant="dot" />}
                          label={row.roleName}
                        />
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: '1fr 1fr',
            columnGap: 3,
          }}
        >
          <CancelButton
            variant="outlined"
            className="bg-slate-100 text-slate-500"
            sx={{
              border: '1px solid var(--primary-green)',
            }}
          />
          <PrimaryButton text="Thêm" />
        </Box>
      </Stack>
    </Paper>
  );
}
