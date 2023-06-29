import {
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  alpha,
} from '@mui/material';
import { ChangeEvent, useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { COMPONENT_SHADOW, CUSTOM_SCROLLBAR } from '~/app/modules/shared/constants';
import PrimaryPagination from '~/app/modules/shared/presentation/components/Pagination';
import { formatTopicStatus } from '~/app/modules/shared/utils';
import { TopicFilterModel } from '../../domain/models/TopicFilterModel';
import { TopicState } from '../../infrastructure/store/types';
import { TopicRoutes } from '../routes';
import Popup from '~/app/modules/shared/presentation/components/Popup';
import TopicRegisterDraft from './TopicRegisterDraft';

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

type TopicListProps = {
  topicState: TopicState;
  fetchTopics: (model: TopicFilterModel) => void;
  setFilter: (model: TopicFilterModel) => void;
};

export default function TopicList({ topicState, fetchTopics, setFilter }: TopicListProps) {
  // const [rowsPerPage, setRowsPerPage] = useState(10);
  const { data: topics, filter, pagination } = topicState;
  const [open, setOpen] = useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const navigate = useNavigate();

  useEffect(() => {
    fetchTopics(filter);
  }, [fetchTopics, filter]);

  useEffect(() => {
    setFilter({ pageNumber: 1, pageSize: 10 } as TopicFilterModel);
  }, [setFilter]);

  const handlePageChange = (e: ChangeEvent<unknown>, page: number) => {
    setFilter({ ...filter, pageNumber: page } as TopicFilterModel);
  };

  return (
    <>
      <Paper sx={{ width: '100%', overflow: 'hidden', boxShadow: COMPONENT_SHADOW }} elevation={0}>
        <TableContainer
          sx={{
            maxHeight: 440,
            '&::-webkit-scrollbar': {
              width: 6,
            },
            ...CUSTOM_SCROLLBAR,
          }}
        >
          <Table stickyHeader aria-label="sticky table">
            <TableHead>
              <TableRow
                sx={{
                  '& .MuiTableCell-root': {
                    backgroundColor: 'white',
                  },
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
              {topics.map((topic, i) => {
                const { name, color } = formatTopicStatus({
                  registered: topic.registered,
                  cancelled: topic.cancel,
                  forceLock: topic.forceLock,
                });

                return (
                  <TableRow
                    hover
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
                          alpha(theme.palette.primary.main, theme.palette.action.activatedOpacity),
                      },
                    }}
                    onClick={(e) => {
                      if (name === 'Chờ duyệt') {
                        handleOpen();
                      } else {
                        navigate(`/admin/${TopicRoutes.TOPICS}/${topic.id}`);
                      }
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
        <PrimaryPagination
          count={Math.ceil((pagination?.totalItemCount ?? 0) / (pagination?.pageSize ?? 1))}
          page={(pagination?.pageIndex ?? 0) + 1}
          onChange={handlePageChange}
        />
      </Paper>
      <Popup open={open} onClose={handleClose}>
        <TopicRegisterDraft />
      </Popup>
    </>
  );
}
