export const formatTopicStatus = ({
  registered,
  cancelled,
  forceLock,
}: {
  registered: boolean;
  cancelled: boolean;
  forceLock: boolean;
}): { name: string; color: string } => {
  if (forceLock) return { name: 'Buộc hủy', color: 'bg-gray-200 hover:bg-gray-300 text-gray-600' };

  if (cancelled) return { name: 'Đã hủy', color: 'bg-red-100 hover:bg-red-200 text-red-600' };

  if (registered)
    return { name: 'Đã đăng ký', color: 'bg-green-100 hover:bg-green-200 text-green-600' };
    
  return { name: 'Chờ duyệt', color: 'bg-yellow-100 hover:bg-amber-100 text-amber-500' };
};
