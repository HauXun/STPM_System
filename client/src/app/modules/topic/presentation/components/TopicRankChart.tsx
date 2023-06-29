import { Box, Paper, PaperProps, SxProps, Theme, Typography } from '@mui/material';
import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';

type Props = PaperProps & {};

const data = [
  {
    'Hạng mục 1': 4000,
    'Hạng mục 2': 2400,
  },
  {
    'Hạng mục 1': 3000,
    'Hạng mục 2': 1398,
  },
  {
    'Hạng mục 1': 2000,
    'Hạng mục 2': 9800,
  },
  {
    'Hạng mục 1': 2780,
    'Hạng mục 2': 3908,
  },
  {
    'Hạng mục 1': 1890,
    'Hạng mục 2': 4800,
  },
  {
    'Hạng mục 1': 2390,
    'Hạng mục 2': 3800,
  },
  {
    'Hạng mục 1': 3490,
    'Hạng mục 2': 4300,
  },
];

export default function TopicRankChart({ sx }: Props) {
  const legendFormatter =
    (additionalValue: string) =>
    (value: string, entry: any): string => {
      return `${additionalValue} ${value}`;
    };

  return (
    <Paper
      sx={{
        ...({
          borderRadius: 2,
          p: 4,
          width: '100%',
          boxShadow: COMPONENT_SHADOW,
          ...centerFlexItems({ direction: 'column', align: 'initial' }),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <Box>
        <Typography className="font-bold">Biểu đồ</Typography>
        <Typography variant="caption">(+21%) so với năm trước</Typography>
      </Box>
      <ResponsiveContainer width="100%" aspect={4.0 / 1.2}>
        <LineChart width={1000} height={500} data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="name" />
          <YAxis />
          <Tooltip />
          <Legend
            layout="horizontal"
            verticalAlign="top"
            align="right"
            height={40}
            iconType="circle"
          />
          <Line type="monotone" dataKey="Hạng mục 2" stroke="orange" strokeWidth={3} />
          <Line type="monotone" dataKey="Hạng mục 1" stroke="green" strokeWidth={3} />
        </LineChart>
      </ResponsiveContainer>
    </Paper>
  );
}
