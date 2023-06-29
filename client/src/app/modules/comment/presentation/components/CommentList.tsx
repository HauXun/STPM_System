import { List } from '@mui/material';
import CommentCard from './CommentCard';
import { useParams } from 'react-router-dom';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { useEffect, ChangeEvent } from 'react';
import PrimaryPagination from '~/app/modules/shared/presentation/components/Pagination';
import { CommentFilterModel } from '../../domain/models/CommentFilterModel';
import { CommentState } from '../../infrastructure/store/types';

type CommentListProps = {
  commentState: CommentState;
  fetchComments: (model: CommentFilterModel) => void;
  setFilter: (model: CommentFilterModel) => void;
};

export default function CommentList({ commentState, fetchComments, setFilter }: CommentListProps) {
  const { topicId, postId } = useParams<{ topicId: string; postId: string }>();
  const { data: comments, filter, pagination } = commentState;

  useEffect(() => {
    fetchComments(filter);
  }, [fetchComments, filter]);

  useEffect(() => {
    setFilter({ pageNumber: 1, pageSize: 10, topicId, postId } as CommentFilterModel);
  }, [postId, setFilter, topicId]);

  const handlePageChange = (e: ChangeEvent<unknown>, page: number) => {
    setFilter({ ...filter, pageNumber: page } as CommentFilterModel);
  };

  return (
    <BoxFlexCenter direction='column' className='gap-y-8'>
      <List
        sx={{
          width: '100%',
          '& .MuiListItem-root:not(:last-child)': {
            mb: 2,
          },
        }}
      >
        {comments &&
          comments.length > 0 &&
          comments.map((comment, i) => <CommentCard key={i} comment={comment} topicId={topicId} />)}
      </List>
      <PrimaryPagination
        count={Math.ceil((pagination?.totalItemCount ?? 0) / (pagination?.pageSize ?? 1))}
        page={(pagination?.pageIndex ?? 0) + 1}
        onChange={handlePageChange}
      />
    </BoxFlexCenter>
  );
}
