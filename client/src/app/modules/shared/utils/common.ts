import { SxProps, Theme } from '@mui/material';

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

export const formatStatus = (status: boolean): { name: string; color: string } => {
  if (status) return { name: 'Hoạt động', color: 'bg-green-100 hover:bg-green-200 text-green-600' };

  return { name: 'Đã khoá', color: 'bg-red-100 hover:bg-red-200 text-red-600' };
};

export const darkerBoxShadow = (boxShadow: string, darkLevel: number) => {
  const rgbaRegex = /rgba\((\d+),\s*(\d+),\s*(\d+),\s*([\d.]+)\)/;
  const match = boxShadow.match(rgbaRegex);

  if (match) {
    const red = parseInt(match[1], 10);
    const green = parseInt(match[2], 10);
    const blue = parseInt(match[3], 10);
    const alpha = parseFloat(match[4]);

    const darkenedAlpha = Math.min(alpha + darkLevel, 1); // Increase alpha by darkLevel, clamped to [0, 1]

    const rgbaColor = `rgba(${red}, ${green}, ${blue}, ${darkenedAlpha})`;
    const boxShadowWithoutColor = boxShadow.replace(rgbaRegex, '');

    return `${boxShadowWithoutColor.trim()} ${rgbaColor}`;
  }

  // Return the original boxShadow if it doesn't match the expected format
  return boxShadow;
};

export const centerFlexItems = ({
  direction = 'row',
  justify = 'space-between',
  align = 'center',
}): SxProps<Theme>=> {
  return {
    display: 'flex',
    flexDirection: direction,
    justifyContent: justify,
    alignItems: align,
  };
};
