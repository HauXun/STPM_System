import {
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  alpha,
} from '@mui/material';
import { Fragment, useEffect, useState } from 'react';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import SearchFilter from '~/app/modules/shared/presentation/components/SearchFilter';
import { formatTopicStatus } from '~/app/modules/shared/utils';
import { useAppDispatch, useAppSelector } from '~/app/stores/hooks';
import { selectTopicFilter, selectTopics } from '../../infrastructure/store/selectors';
import { topicActions } from '../../infrastructure/store/topicSlice';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import { Link, useLocation, useNavigate } from 'react-router-dom';

interface Column {
  id: 'name' | 'rank' | 'total' | 'joinDate' | 'status';
  label: string;
  minWidth?: number | string;
  align: 'left' | 'center' | 'right' | 'justify' | 'inherit';
  color?: string;
}

const columns: readonly Column[] = [
  { id: 'name', label: 'Tên đề tài', minWidth: '30%', align: 'left' },
  { id: 'rank', label: 'Hạng mục', minWidth: '20%', align: 'center' },
  {
    id: 'total',
    label: 'Tham gia',
    minWidth: '10%',
    align: 'center',
  },
  {
    id: 'joinDate',
    label: 'Ngày tham gia',
    minWidth: '20%',
    align: 'center',
  },
  {
    id: 'status',
    label: 'Trạng thái',
    minWidth: '30%',
    align: 'center',
  },
];

export default function TopicList() {
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  const { pathname } = useLocation();
  const navigate = useNavigate();

  const topicList = useAppSelector(selectTopics);
  const filter = useAppSelector(selectTopicFilter);

  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(topicActions.fetchTopics(filter));
  }, [dispatch, filter]);

  useEffect(() => {
    dispatch(topicActions.setFilter({ pageNumber: 1, pageSize: 200 } as TopicFilterModel));
  }, []);

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  return (
    <Fragment>
      <SearchFilter
        placeholderSearch="Tìm kiếm đề tài"
        onChangeInput={(event) => console.log(event)}
      />
      <Paper
        sx={{ width: '100%', overflow: 'hidden', mt: 4, boxShadow: COMPONENT_SHADOW }}
        elevation={0}
      >
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
          <Table stickyHeader aria-label="sticky table">
            <TableHead>
              <TableRow
                sx={{
                  '& .MuiTableCell-root:first-of-type': {
                    pl: 6,
                  },
                }}
              >
                {columns.map((column) => (
                  <TableCell
                    className="py-6 font-bold"
                    key={column.id}
                    align={column.align}
                    style={{ width: column.minWidth }}
                  >
                    {column.label}
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {topicList.data
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                .map((topic, i) => {
                  const { name, color } = formatTopicStatus({
                    registered: topic.registered,
                    cancelled: topic.cancel,
                    forceLock: topic.forceLock,
                  });

                  return (
                    <TableRow
                      hover
                      component={Link}
                      to={`${pathname}/${topic.id}`}
                      role="checkbox"
                      tabIndex={-1}
                      key={i}
                      sx={{
                        cursor: 'pointer',
                        '& .MuiTableCell-root:first-of-type': {
                          pl: 6,
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
                        className="text-green-600"
                        align="left"
                        sx={{ width: columns[0].minWidth }}
                      >
                        {topic.topicName}
                      </TableCell>
                      <TableCell
                        className="text-gray-600"
                        align="center"
                        sx={{ width: columns[1].minWidth }}
                      >
                        {topic.topicRank.rankName}
                      </TableCell>
                      <TableCell
                        className="text-gray-600"
                        align="center"
                        sx={{ width: columns[2].minWidth }}
                      >
                        {topic.users.length}
                      </TableCell>
                      <TableCell
                        className="text-gray-600"
                        align="center"
                        sx={{ width: columns[3].minWidth }}
                      >
                        {new Date(topic.regisDate).toLocaleDateString()}
                      </TableCell>
                      <TableCell align="center" sx={{ width: columns[4].minWidth }}>
                        <Button
                          className={`${color} w-7/12 rounded-lg px-4 py-2 text-base font-bold normal-case`}
                        >
                          {name}
                        </Button>
                      </TableCell>
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
          count={topicList.data.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Paper>
    </Fragment>
  );
}
